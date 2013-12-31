/*
 *名称：Sell
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-06-04 16:22:33
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.Utility;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using MyTools.Framework.Common;
using MyTools.TaoBao.Interface;
using Newtonsoft.Json.Linq;

namespace MyTools.TaoBao.Impl
{
    using Infrastructure.Crosscutting.IoC;
    using Infrastructure.Crosscutting.Logging;

    public class Sell:ISell
    {
        private readonly ILogger _log = InstanceLocator.Current.GetInstance<ILoggerFactory>().Create();

        public void ExportSellDetail(string sourcePath)
        {
            var ds = ExcelHelper.GetExcelDataSet(sourcePath);

            var dtOrderList = ds.Tables["ExportOrderList"];

            var dtOrderDetailList = ds.Tables["ExportOrderDetailList"];
             
            var query = from l in dtOrderList.AsEnumerable()
                        join d in dtOrderDetailList.AsEnumerable() on l["订单编号"] equals d["订单编号"]
                        where d["订单状态"].ToString() != "交易关闭" && d["订单状态"].ToString() != "等待买家付款"
                        select new
                        {
                            OrderNo = l["订单编号"],
                            CreateTime = l["订单创建时间"],
                            GoodsSn = d["外部系统编号"],
                            Props = d["商品属性"],
                            SalePrice = l["买家应付货款"],
                            Postage = l["买家应付邮费"],
                            TotalPrice = l["总金额"],
                            OrderState = l["订单状态"],
                            UserName = l["买家会员名"],
                            Remark = l["订单备注"],
                            Price = d["价格"],
                            Count = d["购买数量"],
                            Title = d["标题"]
                        };

            string sheetName = "销售记录";
            var excel = CreateExcelForSell(sheetName);
            DataTable dt = excel.ReadTable(sheetName);

            #region 排除重复数据，找出拍了2件及以上的客户
             
            var repeat = query.GroupBy(i => i.OrderNo).Where(g => g.Count() > 1);
             
            var notRepeat = query.ToList();
            foreach (var rep in repeat)
            {                 
                var lstRep = rep.ToList();

                var repCount = lstRep.Sum(order => order.Count.ToType<int>());   //购买的产品实际总数,不只是订单数

                var singleAvgPrice = lstRep[0].TotalPrice.ToType<double>() / repCount;

                //获取他的备注信息，检查是否有';'号分割不同的来源
                var sources = GetOtherSource(lstRep);

                for (int index = 0; index < lstRep.Count; index++)
                {
                    var c = lstRep[index];
                    notRepeat.Remove(c);

                    //todo :对一个组合订单下的某个产品包含购买了几件情况

                    for (int j = 0; j < c.Count.ToType<int>(); j++)
                    {
                        var exportModel = new ExportModel(c);

                        exportModel.TotalPrice = singleAvgPrice;

                        this.AddDataRow(
                            exportModel,
                            dt,
                            excel,
                            sources == null ? repCount : 1,
                            //如果压货来源不是同一个来源那么付款金额就是每个备注中的价格
                            sources == null ? null : sources[index+j]);
                    }
                }
            }
             
            #endregion
             
            //只购买了一个订单的产品，有可能一个订单下买了几件相同的产品
            foreach (var q in notRepeat)
            {
                if (string.IsNullOrEmpty(q.Remark.ToString()))
                {
                    continue;
                }

                var lstRep = new[]{ q };
                var sources = GetOtherSource(lstRep); 
                //todo:需要要修改
                for (int i = 0; i < q.Count.ToType<int>(); i++)
                {
                    var exportModel = new ExportModel(q);
                    exportModel.Count = 1;
                    exportModel.TotalPrice = q.TotalPrice.ToType<double>() / q.Count.ToType<int>();
                    AddDataRow(q, dt, excel, 1, sources == null ? null : sources[i]);
                } 
            }

            excel.Dispose();
            Process.Start("Sell");
        }

        //获取他的备注信息，检查是否有';'号分割不同的来源 
        private static string[] GetOtherSource(IEnumerable<dynamic> lstRep)
        {
            var firstItem = lstRep.First();
            var remarks = firstItem.Remark.ToString();
            string[] sources = null;
            if (remarks.Contains("};{")) //注：如果要指定不同来源，那么就必须每个产品都要加来源
            {
                sources = remarks.Split(';');
            }
            return sources;
        }

        private void AddDataRow(dynamic q, DataTable dt, ExcelHelper excel,int repOrder =0,string stuffRemark = null)
        {
            //如果是多个来源，那就要用单独的来源
            var jRemark =
                TextHelper.ToEngInterpunction(
                    stuffRemark ?? TextHelper.ToDBC(q.Remark.ToString().Trim('\'')));
              jRemark = jRemark.Trim('\'');
            if (!VerifyRemark(jRemark))
            {
                _log.LogError("订单号：{0},的来源信息不合法.", q.OrderNo);
                return;
            }

            DataRow dr = dt.NewRow();
            dr["订单编号"] = q.OrderNo;
            dr["卖出时间"] = ObjectExtendMethod.ToDateTime(q.CreateTime).ToString("yyyy/MM/dd");
            dr["款号"] = q.GoodsSn;
            dr["购买人"] = q.UserName;
            dr["商品属性"] = q.Props;
            dr["销售金额"] = q.TotalPrice;
            dr["买家应付邮费"] = q.Postage;
            dr["单件售价"] = q.Price;
            dr["购买数量"] = q.Count;
            //                dr["结帐情况"]
            //                dr["结帐时间"]
             
            JObject jObj = JObject.Parse(jRemark);
            if (jObj != null)
            {
                var source = jObj.SelectToken("来源");

                if (source != null) dr["货源"] = source.Value<string>();

                var costPrice = jObj.SelectToken("成本价");

                if (repOrder > 0)
                {
                    dr["付款金额"] = costPrice != null ? (object)(decimal.Round(costPrice.Value<decimal>()/repOrder,2)) : 0;
                }
                else
                {
                    dr["付款金额"] = costPrice != null ? (object)costPrice.Value<string>() : 0;    
                }
                

                dr["原价"] = ObjectExtendMethod.GetNumberInt(q.Title.ToString());

                var pastage = jObj.SelectToken("邮费");
                dr["支出邮费"] = pastage != null ? (object) costPrice.Value<string>() : 0;

                var remark = jObj.SelectToken("备注");
                dr["备注"] = remark != null ? (object) remark.Value<string>() : "";

                var refund = jObj.SelectToken("退款金额");
                dr["退款金额"] = refund != null ? (object) refund.Value<string>() : 0;
            }
            else
            {
                dr["付款金额"] = 0;
                dr["原价"] = 0;
                dr["支出邮费"] = 0;
                dr["退款金额"] = 0;
            }

            GetColorAndSize(dr, q.Props.ToString());

            excel.AddNewRow(dr);
        }

        private bool VerifyRemark(string jRemark)
        {
            try
            {
                JObject jObj = JObject.Parse(jRemark);
                return true;
            }
            catch (Exception)
            { 
                return false; 
            } 
        }

        #region helper

        //创建ExcelHelper 该EXCEL是用于存放SKU数据
        private ExcelHelper CreateExcelForSell(string sheetName)
        {
            FileHelper.CreateDirectory("Sell");

            string filePath = @"Sell\{0} 销售.xls".StringFormat(DateTime.Now.ToString("yyyy-MM-dd"));

            var excel = new ExcelHelper(filePath) { Imex = "0", Hdr = "YES" };

            if (File.Exists(filePath))
            {
                if (SysUtils.CheckTableExsit(excel, sheetName))
                {
                    return excel;
                }
            }
            var dic = new Dictionary<string, string>
                {
                    {"订单编号", "varchar(255)"},
                    {"卖出时间", "varchar(255)"},
                    {"货源", "varchar(255)"},
                    {"购买人", "varchar(255)"}, 
                    {"款号", "Double"},
                    {"颜色", "varchar(255)"},
                    {"尺码", "varchar(255)"},
                    {"商品属性", "varchar(255)"},
                    {"原价", "Double"}, 
                    {"买家应付邮费", "Double"},
                    {"单件售价", "Double"},
                    {"购买数量", "Double"},
                    {"支出邮费", "Double"},
                    {"销售金额", "Double"},
                    {"付款金额", "Double"},
                    {"退款金额", "Double"},
                    {"利润", "varchar(255)"},
                    {"结帐情况", "varchar(255)"},
                    {"结帐时间", "varchar(255)"},
                    {"购买帐号", "varchar(255)"},
                    {"备注", "varchar(255)"},
                };

            excel.WriteTable(sheetName, dic);
            return excel;
        }

        //得到颜色和尺码的数值
        private void GetColorAndSize(DataRow dr, string source)
        {
            var props = source.Split(';');

            string cStr = string.Empty;
            string sStr = string.Empty;

            foreach (var prop in props)
            {
                if (prop.Contains("颜色"))
                    cStr = prop.GetNumberStr();
                else if (prop.Contains("尺码"))
                {
                    sStr = prop.GetNumberInt().ToString(CultureInfo.InvariantCulture);
                    if (sStr.Length == 5)
                    {
                        sStr = "1" + sStr.Substring(3, 2);
                    }
                }
            }
            dr["颜色"] = cStr;
            dr["尺码"] = sStr;
        }

        #endregion
    }

    public class ExportModel
    {
        public ExportModel(dynamic dyn)
        {
            OrderNo = dyn.OrderNo;
            CreateTime = dyn.CreateTime;
            GoodsSn = dyn.GoodsSn;
            Props = dyn.Props;
            SalePrice = dyn.SalePrice;
            Postage = dyn.Postage;
            TotalPrice = dyn.TotalPrice;
            OrderState = dyn.OrderState;
            UserName = dyn.UserName;
            Remark = dyn.Remark;
            Price = dyn.Price;
            Count = dyn.Count;
            Title = dyn.Title; 
        }

        public double OrderNo { get; set; }
        public DateTime CreateTime { get; set; }
        public double GoodsSn { get; set; }
        public string Props { get; set; }
        public double SalePrice { get; set; }
        public double Postage { get; set; }
        public double TotalPrice { get; set; }
        public string OrderState { get; set; }
        public string UserName { get; set; }
        public string Remark { get; set; }
        public double Price { get; set; }
        public double Count { get; set; }
        public string Title { get; set; }
    }
}