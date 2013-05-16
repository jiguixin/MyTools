/*
 *名称：IBanggoMgt
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-12 05:35:46
 *修改时间：
 *备注：
 */

using System;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Infrastructure.Crosscutting.Declaration;


namespace MyTools.TaoBao.Impl
{
    public class BanggoMgt:IBanggoMgt
    {
        /// <summary>
        /// 得到单个产品信息
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public BanggoProduct GetGoodsInfo(BanggoRequestModel requestModel)
        {
            BanggoProduct product = new BanggoProduct();
            GetProductBaseInfo(product, requestModel);


            #region 得到banggo数据

            requestModel.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                  new System.Diagnostics.StackTrace().ToString()));

            string url = string.Format(
                "http://act.banggo.com/Ajax/cartAjax?time={0}&ajaxtype=color_size&type=size&code={1}&r_code={2}&goods_sn={3}",
                DateTime.Now.Ticks, requestModel.SizeCode, requestModel.ColorCode, requestModel.GoodsSn);
            var restClient = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.AddHeader("Host", "act.banggo.com");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent",
                              "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31");
            request.AddHeader("Referer", requestModel.Referer);
            request.AddHeader("Accept-Encoding", "gzip,deflate,sdch");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.8");
            request.AddHeader("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");


            var response = restClient.Execute(request);


            if (response.ErrorException != null)
                throw response.ErrorException;

            #endregion

            #region 构造成实体对象

            string result = response.Content.TrimStart('(').TrimEnd(')');

            JObject jObj = JObject.Parse(result);

            BanggoProduct bProduct = new BanggoProduct();

            JToken goodsInfo = jObj.SelectToken("goodsInfo");
             
            bProduct.BrandCode = goodsInfo.SelectToken("brand_code").ToString();
            bProduct.GoodsSn = goodsInfo.SelectToken("goods_sn").ToString();
            bProduct.MarketPrice = goodsInfo.SelectToken("market_price").Value<double>();
            bProduct.VipPrice = goodsInfo.SelectToken("vip").Value<double>();
            bProduct.SvipPrice = goodsInfo.SelectToken("svip").Value<double>(); 
            bProduct.SalePrice = goodsInfo.SelectToken("sale_price").Value<double>();
            bProduct.SalesVolume = goodsInfo.SelectToken("sale_count").Value<int>();
             
           //bProduct.MarketPrice = 

             

            //             jObj.SelectToken("avl_num").Value<int>

            JToken jtAvl = jObj["avl_num"];


            JArray a = JArray.Parse(jObj["list"].ToString());

            foreach (JToken jToken in a)
            {
                string cCode = jToken["color_code"].ToString();
            }
             

            #endregion

            return null;

        }

        /// <summary>
        /// 读取或构造单个产品的基础信息。
        /// 包括：标题、价格、销量、产品描述        
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        public void GetProductBaseInfo(BanggoProduct product, BanggoRequestModel requestModel)
        {
            #region 得到banggo数据

            product.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                              new System.Diagnostics.StackTrace().ToString()));

            requestModel.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                              new System.Diagnostics.StackTrace().ToString()));

            var restClient = new RestClient(requestModel.Referer);
            var request = new RestRequest(Method.GET);

            var response = restClient.Execute(request);

            if (response.ErrorException != null)
                throw response.ErrorException;

            HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(response.Content);

            #region 获得品牌
             
            var selectNodesForProductBrandCode =
                doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetBanggoProductBrandCodeXPath);
             selectNodesForProductBrandCode.ThrowIfNull(string.Format(Resource.Exception_XPathGetDataError,
                                                  new System.Diagnostics.StackTrace().ToString()));

            product.BrandCode = selectNodesForProductBrandCode.InnerText;

            #endregion

            #region 获取产品详细界面的类别

            var selectNodesForProductCategory =
                doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetBanggoProductCategoryXPath);

            selectNodesForProductCategory.ThrowIfNull(string.Format(Resource.Exception_XPathGetDataError,
                                                  new System.Diagnostics.StackTrace().ToString()));

            product.Category = selectNodesForProductCategory.InnerText;

            #endregion

            #region 获得产品目录

            var selectNodesForProductCatalog =
                doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetBanggoProductCatalogXPath);

            selectNodesForProductCatalog.ThrowIfNull(string.Format(Resource.Exception_XPathGetDataError,
                                                                   new System.Diagnostics.StackTrace().ToString()));


            product.Catalog = selectNodesForProductCatalog.InnerText;
             
            #endregion


            #region 得到售价 
             /*   
            var selectNodesForProductSalePrice =
               doc.DocumentNode.SelectSingleNode("//*span[@id='sale_price']/span");
             
            selectNodesForProductSalePrice.ThrowIfNull(string.Format(Resource.Exception_XPathGetDataError,new System.Diagnostics.StackTrace().ToString()));


            product.SalePrice = selectNodesForProductSalePrice.InnerText.ToDouble();
*/
            GetAvailableColor(product, requestModel);


            #endregion

            #endregion


        }
         

        /// <summary>
        /// 得到可售商品颜色
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        public void GetAvailableColor(BanggoProduct product, BanggoRequestModel requestModel)
        {
            #region 得到售价

            CookieContainer cookieJar = new CookieContainer();

            string url = string.Format(
                "http://act.banggo.com/Price/getGoodsPrice?r={0}&callback=&goods_sn={1}", DateTime.Now.Ticks, requestModel.GoodsSn);

            var restClient = new RestClient(url);
            restClient.CookieContainer = cookieJar;

            var request = new RestRequest(Method.GET);

            request.AddHeader("Host", "act.banggo.com");

            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31");
             
            request.AddHeader("Referer", requestModel.Referer);
             
            request.AddHeader("Accept-Encoding", "gzip,deflate,sdch");

            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.8");
            request.AddHeader("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3"); 
             
            var response = restClient.Execute(request);

            if (response.ErrorException != null)
                throw response.ErrorException;
             
            string result = response.Content.TrimStart('(').TrimEnd(')');

            JObject jObj = JObject.Parse(result);

            string data = jObj["data"].ToString();
             
            HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(data);
            var htmlNodeSalePrice = doc.GetElementbyId("sale_price");

            htmlNodeSalePrice.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                               new System.Diagnostics.StackTrace().ToString()));

            product.SalePrice = htmlNodeSalePrice.InnerText.Remove(0, 1).ToDouble();
              
            #endregion

            #region 获得商品颜色信息

            var htmlNodeColorList = doc.GetElementbyId("read_colorlist");

            htmlNodeColorList.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                               new System.Diagnostics.StackTrace().ToString()));

            var colors = htmlNodeColorList.SelectNodes("li/a");

            htmlNodeColorList.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                              new System.Diagnostics.StackTrace().ToString()));
            foreach (var colorNode in colors)
            {
                string colorInfo= colorNode.Attributes["onclick"].Value;
                
                //todo: 解析color的信息。

            }
             
            #endregion


            throw new NotImplementedException();
        }

        /// <summary>
        /// 得到可售商品颜色
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        public void GetAvailableSize(BanggoProduct product, BanggoRequestModel requestModel)
        {
            throw new NotImplementedException();
        }
    }
}