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
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using MyTools.Framework.Common;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace MyTools.TaoBao.Impl
{
    public class BanggoMgt : IBanggoMgt
    {
        #region Var

        private readonly ILogger _log = InstanceLocator.Current.GetInstance<ILoggerFactory>().Create(); 

        private StringBuilder sbDesc = new StringBuilder();
         
        #endregion

        #region Public Method

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
             
            var doc = SysUtils.GetHtmlDocumentByHttpGet(requestModel.Referer,Encoding.UTF8); 
              
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


            sbDesc.AppendLine(
                "Banggo 产品地址：<a href=\"{0}\" target=\"_blank\" title=\"{0}\">{0}</a> <br/>".StringFormat(
                    requestModel.Referer));
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
                            sbDesc.AppendLine(descNode.OuterHtml);
                        }
                        else
                        {
                            sbDesc.AppendLine("详情到：" + requestModel.Referer);
                        }
                    }
                }

                #endregion
            }
              
            product.Desc = ModifyGoodsDetailsCss(sbDesc.ToString()); ;

            #endregion
        }


        /// <summary>
        ///     得到可售商品Sku
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        public void GetProductSku(BanggoProduct product, BanggoRequestModel requestModel)
        {
            var doc = GetProductSkuBase(product, requestModel);
             
            product.ColorList = GetProductColorByOnline(requestModel, doc);
             
        }

        /// <summary>
        /// 得到SKU基本信息不包括，颜色和尺码, 主要用于手动发布产品功能
        /// </summary>
        /// <param name="product"></param>
        /// <param name="requestModel"></param>
        public HtmlDocument GetProductSkuBase(BanggoProduct product, BanggoRequestModel requestModel)
        {
            string result = GetGoodsPriceAndColorContent(requestModel);

            JObject jObj = JObject.Parse(result);

            var data = jObj.SelectToken("data").Value<string>();

            var doc = new HtmlDocument();
            doc.LoadHtml(data);

            product.ThumbUrl = jObj.SelectToken("thumb_url").Value<string>();
             
            GetPriceAndSalesVolume(product, doc);

            product.BSizeToTSize = GetBSizeToTSize(doc);

            return doc;

        }
         
        /// <summary>
        ///     解析产品的URL 得到款号
        /// </summary>
        /// <param name="url">产品的URL</param>
        /// <returns></returns>
        public string ResolveProductUrlRetGoodsSn(string url)
        {
            CheckGoodsUrl(url);

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

        /// <summary>
        /// 解析产品的URL 得到品牌
        /// </summary>
        /// <param name="url">产品的URL</param>
        /// <returns></returns>
        public string ResolveProductUrlRetBrand(string url)
        {
            CheckGoodsUrl(url);

            return url.Replace(@"http://", "").Split('.')[0]; 
        }

        /// <summary>
        /// 得到产品的颜色和大小数据，通过在线读取
        /// 该方法主要用于为手动干预价格，提供SKU数据
        /// </summary>
        /// <param name="requestModel">Referer\GoodsSn 必须传入</param>
        /// <returns></returns>
        public List<ProductColor> GetProductColorByOnline(BanggoRequestModel requestModel)
        {
            var doc = GetGoodsDetialElementData(requestModel);

            return GetProductColorByOnline(requestModel, doc);
        }

        //得到产品的颜色和大小数据，通过在线读取，该方法主要用于以自动无干预自动上产品
        public List<ProductColor> GetProductColorByOnline(BanggoRequestModel requestModel, HtmlDocument doc)
        {
            HtmlNode htmlNodeColorList = doc.GetElementbyId(Resource.SysConfig_ColorListId);

            if (htmlNodeColorList.IsNull())
                return null;
            /*
                        htmlNodeColorList.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                            new StackTrace()));*/

            HtmlNodeCollection colors = htmlNodeColorList.SelectNodes("li/a");

            if (colors.IsNull())
                return null;
            /*
                        colors.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                            new StackTrace()));*/


            var colorList = new List<ProductColor>();

            foreach (HtmlNode colorNode in colors)
            {
                string colorInfo = colorNode.Attributes["onclick"].Value;

                ProductColor productColor = CreateProductColor(colorInfo);

                requestModel.ColorCode = productColor.ColorCode;
                productColor.SizeList = GetAvailableSize(requestModel);

                foreach (var size in productColor.SizeList)
                {
                    productColor.AvlNumForColor += size.AvlNum;
                }

                colorList.Add(productColor);
            }

            return colorList;
        }

        //得到产品详细界面的Data元素数据
        public HtmlDocument GetGoodsDetialElementData(BanggoRequestModel requestModel)
        {
            //得到产品详情界面数据
            string result = GetGoodsPriceAndColorContent(requestModel);

            JObject jObj = JObject.Parse(result);

            var data = jObj.SelectToken("data").Value<string>();

            var doc = new HtmlDocument();
            doc.LoadHtml(data);
            return doc;
        }

        //得到banggo上的尺码，主要用于和taobao上的尺码建立对应关系
        public Dictionary<string, string> GetBSizeToTSize(HtmlDocument doc)
        {
            HtmlNode htmlNodeSizeList = doc.GetElementbyId(Resource.SysConfig_SizeListId);
            htmlNodeSizeList.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                new StackTrace()));

            HtmlNodeCollection sizes = htmlNodeSizeList.SelectNodes("a");
            sizes.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                new StackTrace()));

            return sizes.ToDictionary<HtmlNode, string, string>(sizeNode => sizeNode.InnerText.Trim(), sizeNode => null);
        }

        /// <summary>
        /// 将该产品的SKU数据导出为EXCEL
        /// </summary>
        /// <param name="productUrl"></param>
        public List<ProductColor> ExportProductColorForExcel(string productUrl)
        {
            if (productUrl.IsNullOrEmpty())
                throw new Exception(Resource.Exception_NotFoundAuthorizedCode.StringFormat(new StackTrace()));
            string goodsSn = ResolveProductUrlRetGoodsSn(productUrl);

            _log.LogInfo(Resource.Log_ExportProductColorForExceling.StringFormat(goodsSn));

            //const 
            string sheetName = Resource.SysConfig_Sku;

            var excel = CreateExcelForBanggoSku(sheetName);

            DataTable dt = excel.ReadTable(sheetName);

            var request = new BanggoRequestModel {GoodsSn = goodsSn, Referer = productUrl};

            var lstProductColor = GetProductColorByOnline(request);

            DataRow drNew = dt.NewRow();

            drNew["产品地址"] = productUrl;
            drNew["款号"] = goodsSn;
            drNew["售价"] = 0;

            CheckStock(lstProductColor, drNew);

            excel.AddNewRow(drNew);

            _log.LogInfo(Resource.Log_ExportProductColorForExcelSuccess.StringFormat(goodsSn));

            return lstProductColor;
        }

        /// <summary>
        /// 将多个产品的SKU数据导出为EXCEL
        /// </summary>
        /// <param name="productUrls"></param>
        public void ExportProductColorsForExcel(params string[] productUrls)
        {
            if (productUrls.IsNullOrEmpty())
                throw new Exception(Resource.Exception_NotFoundAuthorizedCode.StringFormat(new StackTrace()));
            string sheetName = Resource.SysConfig_Sku;

            var excel = CreateExcelForBanggoSku(sheetName);

            DataTable dt = excel.ReadTable(sheetName);

            foreach (var productUrl in productUrls)
            {
                string goodsSn = ResolveProductUrlRetGoodsSn(productUrl);

                var request = new BanggoRequestModel { GoodsSn = goodsSn, Referer = productUrl };

                var lstProductColor = GetProductColorByOnline(request);
                  
                DataRow drNew = dt.NewRow();

                drNew["产品地址"] = productUrl;
                drNew["款号"] = goodsSn;
                drNew["售价"] = 0;

                CheckStock(lstProductColor, drNew);
                 
                excel.AddNewRow(drNew);
            }
        }

        /// <summary>
        /// 通过款号搜索得到该产品的URL
        /// </summary>
        /// <param name="goodsSn">款号</param>
        /// <returns>要是邦购上已经售完就返回NULL</returns>
        public string GetGoodsUrl(string goodsSn)
        {
            string searchUrl = "http://search.banggo.com/Search/a_a.shtml?clickType=1&word={0}";

            var htmlDocument = new HtmlDocument();
             
            string result = HttpHelper.GETDataToUrl(searchUrl.StringFormat(goodsSn), Encoding.UTF8); ;

            htmlDocument.LoadHtml(result);

            var searchResultNode =
                htmlDocument.DocumentNode.SelectSingleNode("/html/body/div[@class='wrapper']/div[@class='contentWrapper']/div[@id='content']/div[@class='search_list ']/ul/li/a");

            if (searchResultNode.IsNull())
            {
                _log.LogWarning("GoodsSn:{0}->在邦购上没有找到其URL，可能是产品已售完",goodsSn);
                return null;
            } 

            return searchResultNode.Attributes["href"].Value.Trim();

        }

        /// <summary>
        /// 邦购上的用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="call">回调函数，用于界面输入验证码</param>
        /// <returns></returns>
        public RestClient Login(string userName, string password, Func<RestClient, string> call)
        {
            _log.LogInfo(Resource.Log_Logining.StringFormat(userName));
              
            string url =
                "https://passport.banggo.com/CASServer/login?service=http%3A%2F%2Fact.banggo.com%2FUser%2Flogin.shtml%3Freturn_url%3Dhttp%253A%252F%252Fwww.banggo.com%252F";

            var request = new RestRequest(Method.GET);
            var client = new RestClient(url);
            request.AddHeader("Accept", "text/html, application/xhtml+xml, */*");
            request.AddHeader("Referer", "http://www.banggo.com/");
            request.AddHeader("Accept-Language", "zh-CN");
            request.AddHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            request.AddHeader("Accept-Encoding", "gzip, deflate"); 
            request.AddHeader("Host", "passport.banggo.com"); 
            request.AddHeader("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");
            client.CookieContainer = new CookieContainer();
            var response = client.Execute(request);
            string vCode = null;
            Func<RestClient, string> fun = call;

           // fun.Method.Invoke(null,null);

             vCode = call(client);

            if (String.IsNullOrEmpty(vCode))
            {
                throw new Exception("请输入验证码");
            }
            //todo :将验证码POST到服务器

            client.BaseUrl =
                "https://passport.banggo.com/CASServer/login?service=http%3A%2F%2Fact.banggo.com%2FUser%2Flogin.shtml%3Freturn_url%3Dhttp%253A%252F%252Fwww.banggo.com%252F";

            request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "text/html, application/xhtml+xml, */*");
            request.AddHeader("Referer", "https://passport.banggo.com/CASServer/login?service=http%3A%2F%2Fact.banggo.com%2FUser%2Flogin.shtml%3Freturn_url%3Dhttp%253A%252F%252Fwww.banggo.com%252F");
            request.AddHeader("Accept-Language", "zh-CN");
            request.AddHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "passport.banggo.com");
            //没有加Content-Length
            request.AddHeader("Cache-Control", "no-cache");

            request.AddParameter("returnurl", "");
            request.AddParameter("username", userName.Trim());
            request.AddParameter("password", password.Trim());
            request.AddParameter("vcode", vCode.Trim());
            request.AddParameter("rememberUsername", "on");
            request.AddParameter("lt", "e2s1");
            request.AddParameter("_eventId", "submit");
            request.AddParameter("loginType", "0");
           // request.AddParameter("lastIp", "171.221.114.139");

            response = client.Execute(request);

            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                _log.LogInfo(Resource.Log_LoginSuccess.StringFormat(userName));
            }
            else
            {
                string errorInfo = Resource.Log_LoginFailure.StringFormat(userName);
                _log.LogError(errorInfo);

                throw new Exception(errorInfo);
            }

            return client;
             
        }

        /// <summary>
        /// 注：必须先调用Login方法，进行登录
        /// 签到
        /// </summary>  
        public void SingIn(RestClient client)
        {   
            _log.LogInfo(Resource.Log_SingIning);

            client.BaseUrl = string.Format("http://act.banggo.com/Ajax/sing_in/?callback=jsonp{0}", DateTime.Now.Ticks);

            var request = new RestRequest(Method.GET);
            request.AddHeader("Host", "act.banggo.com");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent",
                              "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31");
            request.AddHeader("Referer", "http://member.banggo.com/Member/index.shtml");
            request.AddHeader("Accept-Encoding", "gzip,deflate,sdch");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.8");
            request.AddHeader("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");
            
            

            var response = client.Execute(request);

            if (response.ResponseStatus == ResponseStatus.Completed)
            { 
                _log.LogInfo(Resource.Log_SingInSuccess, TextHelper.ToGb2312NotRemove(Encoding.UTF8.GetString(response.RawBytes)));
            }
            else
            {
                _log.LogError(Resource.Log_SingInFailure);
            }
        }

        /// <summary>
        /// 注：必须先调用Login方法，进行登录
        /// 积分兑换
        /// </summary> 
        public void JfExchange(RestClient client)
        {  
            _log.LogInfo(Resource.Log_JfExchangeing);

            #region 兑换积分

            //todo 这就报错了。
            client.BaseUrl = "http://jifen.banggo.com/";
            var request = new RestRequest(Method.GET);
            request.AddHeader("Host", "jifen.banggo.com");
            request.AddHeader("Accept", "text/html, application/xhtml+xml, */*");
            request.AddHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            request.AddHeader("Referer", "http://www.banggo.com/");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Accept-Language", "zh-CN");
            request.AddHeader("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");
           var response = client.Execute(request);

            client.BaseUrl = "http://jifen.banggo.com/Index/Index/checkLogin?callback=jsonp1373128617202";
            request = new RestRequest(Method.GET);
            request.AddHeader("Host", "jifen.banggo.com");
            request.AddHeader("Accept", "text/javascript, application/javascript, */*");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            request.AddHeader("Referer", "http://jifen.banggo.com/");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Accept-Language", "zh-cn"); 
            response = client.Execute(request);

            client.BaseUrl = "http://act.banggo.com/Cart/getCartInfo?callback=jsonp1373128617203";
            request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/javascript, */*;q=0.8");
            request.AddHeader("Referer", "http://jifen.banggo.com/");
            request.AddHeader("Accept-Language", "zh-CN");
            request.AddHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            request.AddHeader("Accept-Encoding", "gzip, deflate"); 
            request.AddHeader("Host", "act.banggo.com");
            
            response = client.Execute(request);

            client.BaseUrl = "http://jifen.banggo.com/Index/Index/User";
            request = new RestRequest(Method.GET);
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("Accept-Language", "zh-cn");
            request.AddHeader("Referer", "http://jifen.banggo.com/");
            request.AddHeader("Accept", "application/json, text/javascript, */*");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            request.AddHeader("Host", "jifen.banggo.com"); 

            response = client.Execute(request);

            //todo: 如果不成功还要加：http://stat.banggo.com/logs.stat?callback=jsonp1375756465623&json={ 请求

            client.BaseUrl = "http://jifen.banggo.com/Index/Store/Check?good_id={0}".StringFormat(SysConst.BanggoJfGoods);
            request = new RestRequest(Method.GET);
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("Accept-Language", "zh-cn");
            request.AddHeader("Referer", "http://jifen.banggo.com/");
            request.AddHeader("Accept", "application/json, text/javascript, */*");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("User-Agent", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)");
            request.AddHeader("Host", "jifen.banggo.com"); 

            response = client.Execute(request);

            client.BaseUrl = "http://jifen.banggo.com/Index/Store/Exchange?good_id={0}".StringFormat(SysConst.BanggoJfGoods);
            //request = new RestRequest(Method.GET);
             
            response = client.Execute(request);

            if (response.ResponseStatus == ResponseStatus.Completed)
            {
                _log.LogInfo(Resource.Log_JfExchangeSuccess, "", TextHelper.ToGb2312NotRemove(Encoding.Default.GetString(response.RawBytes))); 
            }
            else
            {
                _log.LogError(Resource.Log_JfExchangeFailure,"");
            }
             
            #endregion
        }

        #endregion

        #region helper

        //检查库存信息
        private static void CheckStock(IEnumerable<ProductColor> lstProductColor, DataRow drNew)
        {
            if (!lstProductColor.IsNullOrEmpty())
            {
                var settings = new JsonSerializerSettings();

                string result = JsonConvert.SerializeObject(lstProductColor, Formatting.Indented, settings);
                //需要注意的是，如果返回的是一个集合，那么还要在它的上面再封装一个类。否则客户端收到会出错的。 
                //转回为对象
                //var pcList = JsonConvert.DeserializeObject<List<ProductColor>>(result);

                drNew["SKU"] = result;
                drNew["库存"] = lstProductColor.Sum(productColor => productColor.AvlNumForColor);
                drNew["售完"] = 0;
            }
            else
            {
                drNew["售完"] = 1;
                drNew["库存"] = 0;
            }
        }
         
        //检查产品的URL是否正确
        private static void CheckGoodsUrl(string url)
        {
            url.ThrowIfNullOrEmpty(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(new StackTrace()));

            if (!url.IsUrl())
            {
                throw new Exception(Resource.Exception_UrlInvalid);
            }
        }
         
        //创建ExcelHelper 该EXCEL是用于存放SKU数据
        private ExcelHelper CreateExcelForBanggoSku(string sheetName)
        {
            FileHelper.CreateDirectory(Resource.SysConfig_Sku); 

            string filePath = @"Sku\{0} banggoSku.xls".StringFormat(DateTime.Now.ToString("yyyy-MM-dd"));

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
                    {"产品地址", "varchar(255)"},
                    {"款号", "Double"},
                    {"售价", "Double"},
                    {"库存","Double"},
                    {"SKU", "text"},
                    {"售完","Double"}
                };

            excel.WriteTable(sheetName, dic);
            return excel;
        }
         
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

            string result = ReplaceBracket(response.Content);
            return result;
        }

        //得到可售商品大小及库存
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

            string result = ReplaceBracket(responseContent);

            JObject jObj = JObject.Parse(result);

            JToken goodsInfo = jObj.SelectToken("goodsInfo");

            JArray a = JArray.Parse(jObj["list"].ToString());

            foreach (JToken jToken in a)
            {
                if (jToken["available"].ToType<bool>())
                {
                    string sCode = jToken["size_code"].ToString();
                    string sName = jToken["size_name"].ToString();

                    requestModel.SizeCode = sCode;

                    int avlNum = GetAvlNum(requestModel);

                    lstResult.Add(new ProductSize
                        {
                            Alias = sName,
                            SizeCode = sCode,
                            //此处提取市场价，在将导出EXCEL时，需要根据市场价来进行分析，因为分析banggo的市场价无意义
                            MarketPrice = goodsInfo.SelectToken(Resource.SysConfig_GetMarketPriceId).Value<double>(),
                            MySalePrice =
                                (goodsInfo.SelectToken(Resource.SysConfig_GetMarketPriceId).Value<double>()*SysConst.DiscountRatio)
                                      .ToType<int>(),
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


        //读取Desc数据，因为有些产品描述的Id是productinfo_div
        private static string GetProductDesc(BanggoRequestModel requestModel, HtmlNodeCollection imgNodes,
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

        //得到该颜色下和该大小下的库存大小
        private int GetAvlNum(BanggoRequestModel requestModel)
        {
            string url =
                Resource.SysConfig_GetProductByBanggoAvailableSizeUrl.StringFormat(
                    DateTime.Now.Ticks, requestModel.SizeCode, requestModel.ColorCode, requestModel.GoodsSn);

            string responseContent = GetBanggoReponseContent(url, requestModel.Referer);

            string result = ReplaceBracket(responseContent);

            JObject jObj = JObject.Parse(result);

            return jObj["avl_num"].ToType<int>();
        }

        //替换通过AJAX得到的数据替换空格、换行符和括号
        private static string ReplaceBracket(string source)
        {
            return TextHelper.TrimLf(source).TrimStart('(').TrimEnd(')');
        }

        #region GetProductSku 相关方法
         
       


        //得到产品的颜色和大小 （todo: 暂时没有用该方法）
        private void GetProductAndSize(BanggoProduct product, BanggoRequestModel requestModel, HtmlDocument doc)
        {
            HtmlNode htmlNodeColorList = doc.GetElementbyId(Resource.SysConfig_ColorListId);

            htmlNodeColorList.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                new StackTrace()));

            HtmlNodeCollection colors = htmlNodeColorList.SelectNodes("li/a");

            colors.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                new StackTrace()));


            //HtmlNode htmlNodeSizeList = doc.GetElementbyId(Resource.SysConfig_SizeListId);
            //htmlNodeSizeList.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
            //                                           new StackTrace()));

            //HtmlNodeCollection sizes = htmlNodeSizeList.SelectNodes("a");
            //sizes.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
            //                                new StackTrace()));

            //product.BSizeToTSize = new Dictionary<string, string>();

            //foreach (HtmlNode sizeNode in sizes)
            //{
            //    product.BSizeToTSize.Add(sizeNode.InnerText.Trim(), null);
            //}


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
                return nodeMarketPrice.InnerText.GetNumberDouble();
            return 0;
        }

        //得到销量
        private static int GetSalesVolume(HtmlDocument doc)
        {
            HtmlNode nodeSalesVolume = doc.DocumentNode.SelectSingleNode(Resource.SysConfig_GetSalesVolumeXPath);

            nodeSalesVolume.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                                      new StackTrace()));

            return nodeSalesVolume.InnerText.ToType<int>();
        }

        //得到SVIP价格
        private static double GetSvipPrice(HtmlDocument doc)
        {
            HtmlNode htmlNodeSvipPrice = doc.GetElementbyId(Resource.SysConfig_GetSvipPriceId);

            htmlNodeSvipPrice.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                                        new StackTrace()));

            return htmlNodeSvipPrice.InnerText.ToType<double>();
        }

        //得到VIP价格
        private static double GetVipPrice(HtmlDocument doc)
        {
            HtmlNode htmlNodeVipPrice = doc.GetElementbyId(Resource.SysConfig_GetVipPriceId);

            htmlNodeVipPrice.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                                       new StackTrace()));

            return htmlNodeVipPrice.InnerText.ToType<double>();
        }

        //得到售价
        private static double GetSalePrice(HtmlDocument doc)
        {
            HtmlNode htmlNodeSalePrice = doc.GetElementbyId(Resource.SysConfig_GetSalePriceId);

            htmlNodeSalePrice.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                                                        new StackTrace()));
            double salePrice = htmlNodeSalePrice.InnerText.GetNumberDouble();
            return salePrice;
        }

        #endregion

        #region 修改产品描述的CSS相关

        private string ModifyGoodsDetailsCss(string sourceDesc)
        {
            try
            {
                #region var

                string reg = @"[\.\#]?\w+[^{]+\{[^}]*\}";

                #endregion

                #region GetContent

                string htmlContent = sourceDesc;
                string cssContent = SysConst.GoodsDetailTemplate;

                #endregion

                var doc = new HtmlDocument();

                doc.LoadHtml(htmlContent);
                doc.OptionDefaultStreamEncoding = Encoding.UTF8;

                var lstCss = TextHelper.GetCssContent(cssContent);

                var sb = new StringBuilder();

                foreach (var css in lstCss)
                {
                    #region Get Xpath

                    var l = css.IndexOf("{", System.StringComparison.Ordinal);

                    var token = css.Substring(0, l);
                    var sytle = css.Substring(l + 1, css.Length - l - 2);

                    sb.Clear();

                    var m5 = "mt5 mb10";
                    if (token.Contains(m5))
                    {
                        sb.Append("//*[@id=\"goods_model\"]/div[@class=\"{0}\"]".StringFormat(m5));
                    }
                    else
                    {
                        var singleElement = token.Split(' ');

                        foreach (var s in singleElement)
                        {
                            ConstructXpath(s, sb);
                        }
                    }

                    #endregion

                    var nodes = doc.DocumentNode.SelectNodes(sb.ToString());

                    if (nodes.IsNull())
                        continue;

                    foreach (var node in nodes)
                    {
                        node.Attributes.Add("style", sytle);
                    }
                }

                return doc.DocumentNode.OuterHtml;
            }
            catch (Exception ex)
            {
                _log.LogError(Resource.Log_ModifyGoodsDetailTempFailure, ex);
                return sourceDesc;
            }
        }

        private static void ConstructXpath(string s, StringBuilder sb)
        {
            if (s.IsNullOrEmpty())
                return;

            if (s.Contains("#"))
            {
                var ele = s.Replace("#", "");

                sb.Append("//*[@id=\"{0}\"]".StringFormat(ele));

                return;
            }

            if (s.Contains("."))
            {
                var ele = s.Replace(".", "");
                sb.Append("//*[@class=\"{0}\"]".StringFormat(ele));
                return;
            }

            if (sb.Length > 0)
                sb.Append("/");
            sb.Append(s);
        }

        #endregion

        #endregion
    }
}