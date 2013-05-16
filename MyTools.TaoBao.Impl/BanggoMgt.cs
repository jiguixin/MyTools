/*
 *名称：IBanggoMgt
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-12 05:35:46
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
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
           
            string responseContent = GetBanggoReponseContent(url, requestModel.Referer);

            #endregion

            #region 构造成实体对象

            string result = responseContent.TrimStart('(').TrimEnd(')');

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
            #region 得到Price/getGoodsPrice(获得该商品的价格、颜色)的响应内容

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

            #endregion

            JObject jObj = JObject.Parse(result);

            string data = jObj["data"].ToString();

            HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(data);
              
            product.SalePrice = GetSalePrice(doc);

            product.VipPrice = GetVipPrice(doc);

            product.SvipPrice = GetSvipPrice(doc);

            product.SalesVolume = GetSalesVolume(doc);

              
            #region 获得商品颜色信息

            var htmlNodeColorList = doc.GetElementbyId("read_colorlist");

            htmlNodeColorList.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                               new System.Diagnostics.StackTrace().ToString()));

            var colors = htmlNodeColorList.SelectNodes("li/a");

            htmlNodeColorList.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                              new System.Diagnostics.StackTrace().ToString()));

            product.ColorList = new List<BanggoProductColor>();
            foreach (var colorNode in colors)
            {
                string colorInfo = colorNode.Attributes["onclick"].Value;

                BanggoProductColor productColor = CreateProductColor(colorInfo);

                requestModel.ColorCode = productColor.ColorCode;
                productColor.SizeList = GetAvailableSize(requestModel);
                product.ColorList.Add(productColor);
            }

            #endregion

        }


        /// <summary>
        /// 得到可售商品大小及库存
        /// </summary> 
        /// <param name="requestModel">请求模型</param>
        public List<BanggoProductSize> GetAvailableSize(BanggoRequestModel requestModel)
        {
            List<BanggoProductSize> lstResult = new List<BanggoProductSize>();

            string url = string.Format(
               "http://act.banggo.com/Ajax/cartAjax?time={0}&ajaxtype=color_size&type=color&code={1}&r_code=&goods_sn={2}",
               DateTime.Now.Ticks, requestModel.ColorCode, requestModel.GoodsSn);

            string responseContent = GetBanggoReponseContent(url, requestModel.Referer);

            responseContent.ThrowIfNullOrEmpty(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                              new System.Diagnostics.StackTrace().ToString()));

            string result = responseContent.TrimStart('(').TrimEnd(')');

            JObject jObj = JObject.Parse(result);

            JToken goodsInfo = jObj.SelectToken("goodsInfo");

            JArray a = JArray.Parse(jObj["list"].ToString());

            foreach (JToken jToken in a)
            {
                if (jToken["available"].ToBoolean())
                {
                    string sCode = jToken["size_code"].ToString();
                    string sName = jToken["size_name"].ToString();

                    requestModel.SizeCode = sCode;

                    int avlNum = GetAvlNum(requestModel);

                    lstResult.Add(new BanggoProductSize {Alias = sName, SizeCode = sCode, SalePrice=goodsInfo.SelectToken("sale_price").Value<double>(), AvlNum = avlNum});

                }
            }

            return lstResult;

        }
         
        #region helper
         
        /// <summary>
        /// 生成产品颜色,得到颜色名和色码以及相应的产品图片  
        /// </summary>
        /// <param name="colorInfo">类别结构：newchoose('color','90','黑色组','','207728','read');changeImg'#current_img','/sources/images/goods/MB/207728/207728_90_13.jpg','http://img7.ibanggo.com/sources/images/goods/MB/207728/207728_90_13--w_498_h_498.jpg','http://img7.ibanggo.com/sources/images/goods/MB/207728/207728_90_13--w_730_h_730.jpg',1)</param>
        /// <returns></returns>
        internal BanggoProductColor CreateProductColor(string colorInfo)
        {
            var colorAndImg = colorInfo.Split(';');

            var colorNameAndCode = colorAndImg[0].Replace("'", "").Split(',');
            string colorCode = colorNameAndCode[1];
            string colorName = colorNameAndCode[2];
            

            string imgUrl = colorAndImg[1].Replace("'", "").Split(',')[2];

            return new BanggoProductColor()
            {
                Title = colorName,
                ColorCode = colorCode.ToInt32(),
                ImgUrl = imgUrl,
            };
        }

        /// <summary>
        /// 得到banggo的响应内容
        /// </summary>
        private string GetBanggoReponseContent(string url, string referer)
        {
            var restClient = new RestClient(url);
            var request = new RestRequest(Method.GET);

            request.AddHeader("Host", "act.banggo.com");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent",
                              "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31");
            request.AddHeader("Referer", referer);
            request.AddHeader("Accept-Encoding", "gzip,deflate,sdch");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.8");
            request.AddHeader("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");

            var response = restClient.Execute(request);

            if (response.ErrorException != null)
                throw response.ErrorException;

            return response.Content;
        }

        /// <summary>
        /// 得到该颜色下和该大小下的库存大小
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        private int GetAvlNum(BanggoRequestModel requestModel)
        {
            string url = string.Format(
                "http://act.banggo.com/Ajax/cartAjax?time={0}&ajaxtype=color_size&type=size&code={1}&r_code={2}&goods_sn={3}",
                DateTime.Now.Ticks, requestModel.SizeCode, requestModel.ColorCode, requestModel.GoodsSn);

            string responseContent = GetBanggoReponseContent(url, requestModel.Referer);

            string result = responseContent.TrimStart('(').TrimEnd(')');

            JObject jObj = JObject.Parse(result);

            return jObj["avl_num"].ToInt32();
        }


        private int GetSalesVolume(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

        private double GetSvipPrice(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

        private double GetVipPrice(HtmlDocument doc)
        {
            throw new NotImplementedException();
        }

        //得到售价
        private static double GetSalePrice(HtmlDocument doc)
        {
            var htmlNodeSalePrice = doc.GetElementbyId("sale_price");

            htmlNodeSalePrice.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                        new System.Diagnostics.StackTrace().ToString()));
            double salePrice = htmlNodeSalePrice.InnerText.Remove(0, 1).ToDouble();
            return salePrice;
        }

        #endregion
    }
}