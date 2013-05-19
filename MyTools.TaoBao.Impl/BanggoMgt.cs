﻿/*
 *名称：IBanggoMgt
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-12 05:35:46
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Infrastructure.Crosscutting.Declaration;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MyTools.TaoBao.Impl
{
    public class BanggoMgt : IBanggoMgt
    {
        /// <summary>
        ///     得到单个产品信息
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public BanggoProduct GetGoodsInfo(BanggoRequestModel requestModel)
        {
            var product = new BanggoProduct();
            product.GoodsSn = requestModel.GoodsSn;

            GetProductBaseInfo(product, requestModel); 

            GetProductSku(product, requestModel);

            return product;
        }

        //读取或构造单个产品的基础信息
        /// <summary>
        ///     读取或构造单个产品的基础信息。
        ///     包括：标题、价格、销量、产品描述
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        public void GetProductBaseInfo(BanggoProduct product, BanggoRequestModel requestModel)
        {
            #region 得到banggo数据

            product.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                              new StackTrace()));

            requestModel.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                   new StackTrace()));

            var restClient = new RestClient(requestModel.Referer);
            var request = new RestRequest(Method.GET);

            IRestResponse response = restClient.Execute(request);

            if (response.ErrorException != null)
                throw response.ErrorException;

            var doc = new HtmlDocument();
            doc.LoadHtml(response.Content);

            #endregion

            #region 获得品牌

            HtmlNode selectNodesForProductBrandCode =
                doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetBanggoProductBrandCodeXPath);
            selectNodesForProductBrandCode.ThrowIfNull(string.Format(Resource.Exception_XPathGetDataError,
                                                                     new StackTrace()));

            product.BrandCode = selectNodesForProductBrandCode.InnerText;

            #endregion

            #region 获取产品详细界面的类别

            HtmlNode selectNodesForProductCategory =
                doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetBanggoProductCategoryXPath);

            selectNodesForProductCategory.ThrowIfNull(string.Format(Resource.Exception_XPathGetDataError,
                                                                    new StackTrace()));

            product.Category = selectNodesForProductCategory.InnerText;

            #endregion

            #region 得到banggo的父目录，如 T恤

            var selectNodeForProductParentCatalog = doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetBanggoProductParentCatalogXPath);

            selectNodeForProductParentCatalog.ThrowIfNull(string.Format(Resource.Exception_XPathGetDataError,
                                                                   new StackTrace()));

            product.ParentCatalog = selectNodeForProductParentCatalog.InnerHtml;

            #endregion

            #region 获得产品目录

            HtmlNode selectNodesForProductCatalog =
                doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetBanggoProductCatalogXPath);

            selectNodesForProductCatalog.ThrowIfNull(string.Format(Resource.Exception_XPathGetDataError,
                                                                   new StackTrace()));

            product.Catalog = selectNodesForProductCatalog.InnerText;

            #endregion

            //todo 需要修改获取的HTML img 的SRC URL
            #region 得到产品描述

            product.Desc = doc.GetElementbyId("goods_model").OuterHtml;

            #endregion
        }

        /// <summary>
        ///     得到可售商品Sku
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        public void GetProductSku(BanggoProduct product, BanggoRequestModel requestModel)
        {
            var result = GetGoodsPriceAndColorContent(requestModel);

            JObject jObj = JObject.Parse(result);

            string data = jObj.SelectToken("data").Value<string>();

            product.ThumbUrl = jObj.SelectToken("thumb_url").Value<string>();

            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            GetPriceAndSalesVolume(product, doc);

            GetProductAndSize(product, requestModel, doc);
        }

        /// <summary>
        /// 解析产品的URL 得到款号
        /// </summary>
        /// <param name="url">产品的URL</param>
        /// <returns></returns> 
        public string ResolveProductUrl(string url)
        { 
            Regex r = new Regex(@"\d{6}",RegexOptions.Multiline);
            MatchCollection  m = r.Matches(url);

            int i = m.Count;

            if (i > 0)
            {
                string value = m[i-1].Groups[0].Value;

                return value;
            }
            return null;
        }


        #region helper

        //得到Price/getGoodsPrice(获得该商品的价格、颜色)的响应内容
        private static string GetGoodsPriceAndColorContent(BanggoRequestModel requestModel)
        {
            var cookieJar = new CookieContainer();

            string url = string.Format(
                "http://act.banggo.com/Price/getGoodsPrice?r={0}&callback=&goods_sn={1}", DateTime.Now.Ticks,
                requestModel.GoodsSn);

            var restClient = new RestClient(url) { CookieContainer = cookieJar };

            var request = new RestRequest(Method.GET);

            request.AddHeader("Host", "act.banggo.com");

            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent",
                              "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31");

            request.AddHeader("Referer", requestModel.Referer);

            request.AddHeader("Accept-Encoding", "gzip,deflate,sdch");

            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.8");
            request.AddHeader("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");

            IRestResponse response = restClient.Execute(request);

            if (response.ErrorException != null)
                throw response.ErrorException;

            string result = response.Content.TrimStart('(').TrimEnd(')');
            return result;
        }

        /// <summary>
        ///     得到可售商品大小及库存
        /// </summary>
        /// <param name="requestModel">请求模型</param>
        private List<ProductSize> GetAvailableSize(BanggoRequestModel requestModel)
        {
            var lstResult = new List<ProductSize>();

            string url = string.Format(
                "http://act.banggo.com/Ajax/cartAjax?time={0}&ajaxtype=color_size&type=color&code={1}&r_code=&goods_sn={2}",
                DateTime.Now.Ticks, requestModel.ColorCode, requestModel.GoodsSn);

            string responseContent = GetBanggoReponseContent(url, requestModel.Referer);

            responseContent.ThrowIfNullOrEmpty(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                             new StackTrace()));

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

                    lstResult.Add(new ProductSize
                    {
                        Alias = sName,
                        SizeCode = sCode,
                        SalePrice = goodsInfo.SelectToken("sale_price").Value<double>(),
                        Price = (goodsInfo.SelectToken("market_price").Value<double>() * SysConst.DiscountRatio.ToDouble()).ToInt32(),
                        AvlNum = avlNum
                    });
                }
            }

            return lstResult;
        }
          
        //生成产品颜色,得到颜色名和色码以及相应的产品图片
        /// <summary>
        ///     生成产品颜色,得到颜色名和色码以及相应的产品图片
        /// </summary>
        /// <param name="colorInfo">类别结构：newchoose('color','90','黑色组','','207728','read');changeImg'#current_img','/sources/images/goods/MB/207728/207728_90_13.jpg','http://img7.ibanggo.com/sources/images/goods/MB/207728/207728_90_13--w_498_h_498.jpg','http://img7.ibanggo.com/sources/images/goods/MB/207728/207728_90_13--w_730_h_730.jpg',1)</param>
        /// <returns></returns>
        internal ProductColor CreateProductColor(string colorInfo)
        {
            string[] colorAndImg = colorInfo.Split(';');

            string[] colorNameAndCode = colorAndImg[0].Replace("'", "").Split(',');
            string colorCode = colorNameAndCode[1];
            string colorName = colorNameAndCode[2];

            string imgUrl = colorAndImg[1].Replace("'", "").Split(',')[2];

            return new ProductColor
                {
                    Title = colorName,
                    ColorCode = colorCode,
                    ImgUrl = imgUrl,
                };
        }

        //得到banggo的响应内容 ,主要用于得到Ajax/cartAjax 的结果
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

            IRestResponse response = restClient.Execute(request);

            if (response.ErrorException != null)
                throw response.ErrorException;

            return response.Content;
        }

        //得到该颜色下和该大小下的库存大小
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

        #region GetProductSku 相关方法
         
        //得到产品的颜色和大小
        private void GetProductAndSize(BanggoProduct product, BanggoRequestModel requestModel, HtmlDocument doc)
        {
            HtmlNode htmlNodeColorList = doc.GetElementbyId("read_colorlist");

            htmlNodeColorList.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                        new StackTrace()));

            HtmlNodeCollection colors = htmlNodeColorList.SelectNodes("li/a");

            htmlNodeColorList.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                        new StackTrace()));

            product.ColorList = new List<ProductColor>();

            foreach (HtmlNode colorNode in colors)
            {
                string colorInfo = colorNode.Attributes["onclick"].Value;

                ProductColor productColor = CreateProductColor(colorInfo);

                requestModel.ColorCode = productColor.ColorCode;
                productColor.SizeList = GetAvailableSize(requestModel);
                product.ColorList.Add(productColor);
            }
        }
         
        //得到该商品的价格和销量信息
        private static void GetPriceAndSalesVolume(BanggoProduct product, HtmlDocument doc)
        {
            product.SalePrice = GetSalePrice(doc);

            product.VipPrice = GetVipPrice(doc);

            product.SvipPrice = GetSvipPrice(doc);

            product.MarketPrice = GetMarketPrice(doc);

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (product.MarketPrice == 0)
            // ReSharper restore CompareOfFloatsByEqualityOperator
            {
                product.MarketPrice = product.SalePrice;
            }

            product.SalesVolume = GetSalesVolume(doc);

        }
         
        //得到市场价
        private static double GetMarketPrice(HtmlDocument doc)
        {
            HtmlNode nodeMarketPrice = doc.DocumentNode.SelectSingleNode("div[@class='goods_price']/del");

            if (nodeMarketPrice != null)
                return nodeMarketPrice.InnerText.Remove(0, 1).ToDouble();
            else
                return 0;
        }

        //得到销量
        private static int GetSalesVolume(HtmlDocument doc)
        {
            HtmlNode nodeSalesVolume = doc.DocumentNode.SelectSingleNode("div[@class='sales']/p/strong[@class='red']/a");

            nodeSalesVolume.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                      new StackTrace()));

            return nodeSalesVolume.InnerText.ToInt32();
        }

        //得到SVIP价格
        private static double GetSvipPrice(HtmlDocument doc)
        {
            HtmlNode htmlNodeSvipPrice = doc.GetElementbyId("svip_price");

            htmlNodeSvipPrice.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                        new StackTrace()));

            return htmlNodeSvipPrice.InnerText.ToDouble();
        }

        //得到VIP价格
        private static double GetVipPrice(HtmlDocument doc)
        {
            HtmlNode htmlNodeVipPrice = doc.GetElementbyId("vip_price");

            htmlNodeVipPrice.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                       new StackTrace()));

            return htmlNodeVipPrice.InnerText.ToDouble();
        }

        //得到售价
        private static double GetSalePrice(HtmlDocument doc)
        {
            HtmlNode htmlNodeSalePrice = doc.GetElementbyId("sale_price");

            htmlNodeSalePrice.ThrowIfNull(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                        new StackTrace()));
            double salePrice = htmlNodeSalePrice.InnerText.Remove(0, 1).ToDouble();
            return salePrice;
        }
         
        #endregion

        #endregion
    }
}