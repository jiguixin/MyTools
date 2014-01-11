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
using HtmlAgilityPack;
using MyTools.TaoBao.DomainModule;
using RestSharp;

namespace MyTools.TaoBao.Interface
{
    public interface IBanggoMgt : IRequest
    { 
         /// <summary>
        /// 读取或构造单个产品的基础信息。
        /// 包括：标题、价格、销量、产品描述        
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        //void GetProductBaseInfo(BanggoProduct product, RequestModel requestModel);
        
        /// <summary>
        /// 得到SKU基本信息不包括，颜色和尺码, 主要用于手动发布产品功能
        /// </summary>
        /// <param name="product"></param>
        /// <param name="requestModel"></param>
        HtmlDocument GetProductSkuBase(BanggoProduct product, RequestModel requestModel);
         
        /// <summary>
        /// 解析产品的URL 得到品牌
        /// </summary>
        /// <param name="url">产品的URL</param>
        /// <returns></returns>
        string ResolveProductUrlRetBrand(string url);
         
        /// <summary>
        /// 得到产品的颜色和大小数据，通过在线读取
        /// 该方法主要用于为手动干预价格，提供SKU数据
        /// </summary>
        /// <param name="requestModel">Referer\GoodsSn 必须传入</param>
        /// <returns></returns>
        List<ProductColor> GetProductColorByOnline(RequestModel requestModel);

        /// <summary>
        /// 得到产品的颜色和大小数据，通过在线读取
        /// 
        /// </summary>
        /// <param name="requestModel">Referer\GoodsSn 必须传入</param>
        /// <param name="doc">调用GetGoodsDetialElementData方法获得</param>
        /// <returns></returns>
        List<ProductColor> GetProductColorByOnline(RequestModel requestModel, HtmlDocument doc);

        /// <summary>
        /// 得到banggo上的尺码，主要用于和taobao上的尺码建立对应关系
        /// </summary>
        /// <param name="doc">调用GetGoodsDetialElementData方法获得</param>
        /// <returns></returns>
        Dictionary<string, string> GetBSizeToTSize(HtmlDocument doc);

        /// <summary>
        /// 得到产品详细界面的Data元素数据
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        HtmlDocument GetGoodsDetialElementData(RequestModel requestModel);

        /// <summary>
        /// 将该产品的SKU数据导出为EXCEL
        /// </summary>
        /// <param name="productUrl"></param>
        List<ProductColor> ExportProductColorForExcel(string productUrl);

        /// <summary>
        /// 将多个产品的SKU数据导出为EXCEL
        /// </summary>
        /// <param name="productUrls"></param>
        void ExportProductColorsForExcel(params string[] productUrls);
         
        /// <summary>
        /// 邦购上的用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="call">回调函数，用于界面输入验证码</param>
        /// <returns></returns>
        RestClient Login(string userName, string password,Func<RestClient,string> call);

        /// <summary>
        /// 注：必须先调用Login方法，进行登录
        /// 签到
        /// </summary>  
        void SingIn(RestClient client);

        /// <summary>
        /// 注：必须先调用Login方法，进行登录
        /// 积分兑换
        /// </summary> 
        void JfExchange(RestClient client);
    }
}