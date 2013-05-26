/*
 *名称：TaoBaoAnalysis
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-24 07:46:08
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using MyTools.Framework.Common;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Infrastructure.Crosscutting.Declaration;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MyTools.TaoBao.Impl
{
    public class TaoBaoAnalysis : IAnalysis
    {

        #region Members

        private static StringBuilder sbRivalSkuData = new StringBuilder();

        #endregion
 
        #region Constructor

        #endregion

        #region Public Methods

        /// <summary>
        /// 通过分析淘宝竞争对手得到最后的价格
        /// </summary>
        /// <param name="query">查询条件</param>
        /// <returns></returns>
        public double GetAnalysisPrice(string query)
        {
            ExportRivalGoodsInfo(query);

            return 0;
        }
       
        // 导出竞争对手的产品信息
        /// <summary>
        /// 导出竞争对手的产品信息
        /// </summary>
        /// <param name="query">在淘宝中输入的产品信息</param>
        /// <param name="marketPrice">该产品的市场价</param>
        /// <param name="salePrice">我的售价</param>
        public void ExportRivalGoodsInfo(string query, double marketPrice = 0, double salePrice = 0)
        {
            var searchUrl = SysConst.TaoBaoSearchUrl.StringFormat(query);

            var docSearchGoodsList = SysUtils.GetHtmlDocumentByHttpGet(searchUrl);

            var itemBoxs =
                docSearchGoodsList.DocumentNode.SelectNodes(
                    "//*[@class='col item icon-datalink']/div[@class='item-box']");

            itemBoxs.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(new StackTrace()));

            //            string sheetName = "价格数据{0}".StringFormat(DateTime.Now.ToString("HHmm"));
            string sheetName = "价格数据{0}".StringFormat("");

            var excel = CreateExcelForRivalPrice(sheetName);

            DataTable dt = excel.ReadTable(sheetName);

            DataRow drNew = dt.NewRow();

            //遍历搜索出来的产品列表
            foreach (var item in itemBoxs)
            {
                GetRivalDetails(item, excel, sheetName, drNew);
                drNew["款号"] = query.GetNumberStr();
                if (marketPrice > 0)
                {
                    drNew["成本价"] = (marketPrice * SysConst.CostRatio).ToInt32() + SysConst.CostExtraPrice;
                }
                drNew["售价"] = salePrice;
                excel.AddNewRow(drNew);
            }
        }
         
        #endregion

        #region Private Methods

        // 得到对手的详细销售信息
        /// <summary>
        /// 得到对手的详细销售信息
        /// </summary>
        /// <param name="item">某个对手HTML结点</param>
        /// <param name="excel">excel</param>
        /// <param name="shellName">工作表名</param>
        /// <returns></returns>
        private static void GetRivalDetails(HtmlNode item, ExcelHelper excel, string shellName, DataRow dr)
        {
            #region 得到taobao该对手的价格数据 并添加到excel中

            var salePrice = item.SelectSingleNode("div[@class='row row-focus']/div[1]").InnerText.GetNumber();
            var postage = item.SelectSingleNode("div[@class='row row-focus']/div[2]").InnerText.GetNumber();
            var salesVolume = item.SelectSingleNode("div[@class='row']/div[1]").InnerText.GetNumber();
            var evaluate = item.SelectSingleNode("div[@class='row']/div[2]").InnerText.GetNumber();
            var url = item.SelectSingleNode("h3[@class='summary']/a").Attributes["href"].Value.Trim();
            var title = XmlHelper.XmlDecode(item.SelectSingleNode("h3[@class='summary']/a/@title").InnerText.Trim());
            var rivalName = item.SelectSingleNode("div[@class='row']/div[@class='col seller']/a").InnerText.Trim();
            var rivalLocation =
                item.SelectSingleNode("div[@class='row']/div[@class='col end loc']").InnerText.Trim();

            dr["标题"] = title;
            dr["总价"] = salePrice + postage;
            dr["价格"] = salePrice;
            dr["邮费"] = postage;
            dr["销量"] = salesVolume;
            dr["评价数"] = evaluate;
            dr["用户名"] = rivalName;
            dr["地点"] = rivalLocation;
            dr["网址"] = url;

            #endregion

            //如果有销售情况，就提取他的销售详情
            if (salesVolume > 0)
            {
                var saleDetail = SysUtils.GetHtmlDocumentByHttpGet(url);

                #region 获取SKU数据

                sbRivalSkuData.Clear();

                var nodeColors = saleDetail.DocumentNode.SelectNodes("//*[@id=\"J_isku\"]/div/dl[2]/dd/ul/li");
                if (nodeColors != null)
                {
                    foreach (var nColor in nodeColors)
                    {
                        var color =
                            "颜色:{0}".StringFormat(nColor.InnerText.Trim().Replace("已选中", "").Replace("\\t", "").Replace("\\r\\n", "").Trim());
                        if (!color.IsNullOrEmpty())
                            sbRivalSkuData.AppendLine(color);
                    }
                }

                var nodeSizes = saleDetail.DocumentNode.SelectNodes("//*[@id=\"J_isku\"]/div/dl[1]/dd/ul/li");
                if (nodeSizes != null)
                {
                    foreach (var nSize in nodeSizes)
                    {
                        var size =
                            "尺码:{0}".StringFormat(
                                nSize.InnerText.Trim().Replace("已选中", "").Replace("\\t", "").Replace("\\r\\n", "").Trim());
                        if (!size.IsNullOrEmpty())
                            sbRivalSkuData.AppendLine(size);
                    }
                }

                var nodeStock = saleDetail.DocumentNode.SelectSingleNode("//*[@id=\"J_SpanStock\"]");

                if (nodeStock != null)
                {
                    sbRivalSkuData.AppendLine("库存:{0}".StringFormat(nodeStock.InnerText));
                }

                dr["SKU"] = sbRivalSkuData.ToString();

                sbRivalSkuData.Clear();

                #endregion


                #region 提取详细销售记录数据

                var saleRecordUrl =
                    saleDetail.GetElementbyId("J_listBuyerOnView").Attributes["detail:params"].Value.Replace(
                        ",showBuyerList", "&t={0}&callback=Hub.data.records_reload".StringFormat(DateTime.Now.Ticks));

                var saleDetailHtml =
                    SysUtils.GetHtmlByHttpGet(saleRecordUrl)
                            .Trim()
                            .Replace("Hub.data.records_reload(", "")
                            .TrimEnd(')');

                if (saleDetailHtml.StartsWith("{html:"))
                {
                    JObject jObj = JObject.Parse(saleDetailHtml);

                    saleDetail.LoadHtml(jObj.SelectToken("html").Value<string>());

                    var nodes =
                        saleDetail.DocumentNode.SelectNodes("/table/tbody/tr");
                    var sbContent = new StringBuilder();

                    foreach (var node in nodes)
                    {
                        sbContent.AppendLine(XmlHelper.XmlDecode(node.InnerText.Trim()));
                    }

                    dr["成交记录"] = sbContent.ToString();
                }

                #endregion
            }
        }

        //创建ExcelHelper
        private static ExcelHelper CreateExcelForRivalPrice(string sheetName)
        {
            string filePath = @"{0} 分析.xls".StringFormat(DateTime.Now.ToString("yyyy-MM-dd"));

            var excel = new ExcelHelper(filePath);
            excel.Imex = "0";
            excel.Hdr = "YES";

            if (File.Exists(filePath))
            {
                if (SysUtils.CheckTableExsit(excel, sheetName))
                {
                    return excel;
                }
            }


            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("款号", "varchar(255)");
            dic.Add("成本价", "varchar(255)");
            dic.Add("售价", "varchar(255)");
            dic.Add("标题", "varchar(255)");
            dic.Add("价格", "varchar(255)");
            dic.Add("邮费", "varchar(255)");
            dic.Add("总价", "varchar(255)");
            dic.Add("销量", "varchar(255)");
            dic.Add("评价数", "varchar(255)");
            dic.Add("用户名", "varchar(255)");
            dic.Add("地点", "varchar(255)");
            dic.Add("网址", "varchar(255)");
            dic.Add("SKU", "varchar(255)");
            dic.Add("成交记录", "text");
             
            excel.WriteTable(sheetName, dic);
            return excel;
        }


        #endregion

    }
}