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
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Infrastructure.Crosscutting.Declaration;
using MyTools.Framework.Common;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MyTools.TaoBao.Impl
{
    public class BanggoMgt : IBanggoMgt
    {
        private StringBuilder sbDesc = new StringBuilder();
        /// <summary>
        ///     得到单个产品信息
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public BanggoProduct GetGoodsInfo(BanggoRequestModel requestModel)
        {
            var product = new BanggoProduct {GoodsSn = requestModel.GoodsSn};

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

            product.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                              new StackTrace()));

            requestModel.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
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
            selectNodesForProductBrandCode.ThrowIfNull(Resource.Exception_XPathGetDataError.StringFormat(
                                                                     new StackTrace()));

            product.Brand = selectNodesForProductBrandCode.InnerText;

            #endregion

            #region 获取产品详细界面的类别

            HtmlNode selectNodesForProductCategory =
                doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetBanggoProductCategoryXPath);

            selectNodesForProductCategory.ThrowIfNull(Resource.Exception_XPathGetDataError.StringFormat(
                                                                    new StackTrace()));

            product.Category = selectNodesForProductCategory.InnerText;

            #endregion

            #region 得到banggo的父目录，如 T恤

            HtmlNode selectNodeForProductParentCatalog =
                doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetBanggoProductParentCatalogXPath);

            selectNodeForProductParentCatalog.ThrowIfNull(Resource.Exception_XPathGetDataError.StringFormat(
                                                                        new StackTrace()));

            product.ParentCatalog = selectNodeForProductParentCatalog.InnerHtml;

            #endregion

            #region 获得产品目录

            HtmlNode selectNodesForProductCatalog =
                doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetBanggoProductCatalogXPath);

            selectNodesForProductCatalog.ThrowIfNull(Resource.Exception_XPathGetDataError.StringFormat(
                                                                   new StackTrace()));

            product.Catalog = selectNodesForProductCatalog.InnerText;

            #endregion

            #region 得到产品描述

            //修改获取的HTML img 的SRC URL
            HtmlNodeCollection imgNodes = doc.DocumentNode.SelectNodes(Resource.SysConfig_GetGoodsModeImgGreyXPath);

            sbDesc.Clear();
            sbDesc.AppendLine(SysConst.PrefixGoodsDesc);

            
            sbDesc.AppendLine("Banggo 产品地址：<a href=\"{0}\" target=\"_blank\" title=\"{0}\">{0}</a> <br/>".StringFormat( requestModel.Referer)); 
            if (imgNodes != null)
            { 
                 sbDesc.AppendLine(GetProductDesc(requestModel, imgNodes, doc, Resource.SysConfig_GoodsDescId));
            }
            else
            {
                #region 处理请求HTML 没有grey.gif的情况，以及产品描述节点ID不为Goods_Mode的情况
                
                //有可能是没有grey.gif文件，检查是否有描述结点。如果有，直接返回

                var desNode = doc.GetElementbyId(Resource.SysConfig_GoodsDescId);

                if (desNode != null)
                {
                    sbDesc.AppendLine(desNode.OuterHtml);
                }
                else //说明是该描述id为productinfo_div
                {
                    //检查是否有grey.gif文件
                    imgNodes =
                        doc.DocumentNode.SelectNodes(
                            Resource.SysConfig_GetProductInfoImgGreyXPath);

                    if (imgNodes != null)
                    {
                        sbDesc.AppendLine(GetProductDesc(requestModel, imgNodes, doc, Resource.SysConfig_ProductInfoId));
                    }
                    else
                    {
                        //检查是否有productinfo_div结点
                        var descNode = doc.GetElementbyId(Resource.SysConfig_ProductInfoId);

                        if (descNode != null)
                        {
                            sbDesc.AppendLine( descNode.OuterHtml);
                        }
                        else
                        {  
                            sbDesc.AppendLine("详情到：" + requestModel.Referer);
                        } 
                    }
                }

                #endregion
            }

            product.Desc = sbDesc.ToString();

            #endregion
        }

        //读取Desc数据，因为有些产品描述的Id是productinfo_div
        private static string GetProductDesc( BanggoRequestModel requestModel, HtmlNodeCollection imgNodes,
                                     HtmlDocument doc, string detailId)
        {
            string desc;
            foreach (HtmlNode imgNode in imgNodes)
            {
                imgNode.SetAttributeValue("src", imgNode.GetAttributeValue("original", ""));
            }

            var desNode = doc.GetElementbyId(detailId);

            if (desNode != null)
            {
                desc = desNode.OuterHtml;
            }
            else
            {
                desc = "详情到：" + requestModel.Referer;
            }
            return desc;
        }


        /// <summary>
        ///     得到可售商品Sku
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        public void GetProductSku(BanggoProduct product, BanggoRequestModel requestModel)
        {
            string result = GetGoodsPriceAndColorContent(requestModel);

            JObject jObj = JObject.Parse(result);

            var data = jObj.SelectToken("data").Value<string>();

            product.ThumbUrl = jObj.SelectToken("thumb_url").Value<string>();

            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            GetPriceAndSalesVolume(product, doc);

            GetProductAndSize(product, requestModel, doc);
        }

        /// <summary>
        ///     解析产品的URL 得到款号
        /// </summary>
        /// <param name="url">产品的URL</param>
        /// <returns></returns>
        public string ResolveProductUrl(string url)
        {
            var r = new Regex(Resource.SysConfig_GetGoodsSnByUrlRegex, RegexOptions.Multiline);
            MatchCollection m = r.Matches(url);

            int i = m.Count;

            if (i > 0)
            {
                string value = m[i - 1].Groups[0].Value;

                return value;
            }
            return null;
        }

        #region helper

        //得到Price/getGoodsPrice(获得该商品的价格、颜色)的响应内容
        private static string GetGoodsPriceAndColorContent(BanggoRequestModel requestModel)
        {
            var cookieJar = new CookieContainer();

            string url =
                Resource.SysConfig_GetGoodsPriceByBanggoUrl.StringFormat(DateTime.Now.Ticks,
                requestModel.GoodsSn);

            var restClient = new RestClient(url) {CookieContainer = cookieJar};

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

            string url =
                Resource.SysConfig_GetProductByBanggoAvailableColorUrl.StringFormat(
                DateTime.Now.Ticks, requestModel.ColorCode, requestModel.GoodsSn);

            string responseContent = GetBanggoReponseContent(url, requestModel.Referer);

            responseContent.ThrowIfNullOrEmpty(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
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
                            SalePrice = goodsInfo.SelectToken(Resource.SysConfig_GetSalePriceId).Value<double>(),
                            Price =
                                (goodsInfo.SelectToken("market_price").Value<double>()*SysConst.DiscountRatio.ToDouble())
                                      .ToInt32(),
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
            string url =
                Resource.SysConfig_GetProductByBanggoAvailableSizeUrl.StringFormat(
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
            HtmlNode htmlNodeColorList = doc.GetElementbyId(Resource.SysConfig_ColorListId);

            htmlNodeColorList.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                                        new StackTrace()));

            HtmlNodeCollection colors = htmlNodeColorList.SelectNodes("li/a");

            colors.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                             new StackTrace()));


            HtmlNode htmlNodeSizeList = doc.GetElementbyId(Resource.SysConfig_SizeListId);
            htmlNodeSizeList.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                                       new StackTrace()));

            HtmlNodeCollection sizes = htmlNodeSizeList.SelectNodes("a");
            sizes.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                            new StackTrace()));

            product.BSizeToTSize = new Dictionary<string, string>();

            foreach (HtmlNode sizeNode in sizes)
            {
                product.BSizeToTSize.Add(sizeNode.InnerText.Trim(), null);
            }


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
            HtmlNode nodeMarketPrice = doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetMarketPriceXPath);

            if (nodeMarketPrice != null)
                return nodeMarketPrice.InnerText.Remove(0, 1).ToDouble();
            return 0;
        }

        //得到销量
        private static int GetSalesVolume(HtmlDocument doc)
        {
            HtmlNode nodeSalesVolume = doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetSalesVolumeXPath);

            nodeSalesVolume.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                                      new StackTrace()));

            return nodeSalesVolume.InnerText.ToInt32();
        }

        //得到SVIP价格
        private static double GetSvipPrice(HtmlDocument doc)
        {
            HtmlNode htmlNodeSvipPrice = doc.GetElementbyId(Resource.SysConfig_GetSvipPriceId);

            htmlNodeSvipPrice.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                                        new StackTrace()));

            return htmlNodeSvipPrice.InnerText.ToDouble();
        }

        //得到VIP价格
        private static double GetVipPrice(HtmlDocument doc)
        {
            HtmlNode htmlNodeVipPrice = doc.GetElementbyId(Resource.SysConfig_GetVipPriceId);

            htmlNodeVipPrice.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                                       new StackTrace()));

            return htmlNodeVipPrice.InnerText.ToDouble();
        }

        //得到售价
        private static double GetSalePrice(HtmlDocument doc)
        {
            HtmlNode htmlNodeSalePrice = doc.GetElementbyId(Resource.SysConfig_GetSalePriceId);

            htmlNodeSalePrice.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                                        new StackTrace()));
            double salePrice = htmlNodeSalePrice.InnerText.Remove(0, 1).ToDouble();
            return salePrice;
        }

        #endregion

        #endregion
    }
}