/*
 *名称：IBanggoMgt
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-12 05:35:46
 *修改时间：
 *备注：
 */

using System.Collections.Generic;
using HtmlAgilityPack;
using MyTools.TaoBao.DomainModule;

namespace MyTools.TaoBao.Interface
{
    public interface IBanggoMgt
    {
        /// <summary>
        /// 得到单个产品信息
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        BanggoProduct GetGoodsInfo(BanggoRequestModel requestModel);
         
         /// <summary>
        /// 读取或构造单个产品的基础信息。
        /// 包括：标题、价格、销量、产品描述        
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        void GetProductBaseInfo(BanggoProduct product, BanggoRequestModel requestModel);
        
        /// <summary>
        /// 得到可售商品Sku
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        void GetProductSku(BanggoProduct product, BanggoRequestModel requestModel);

        /// <summary>
        /// 得到SKU基本信息不包括，颜色和尺码, 主要用于手动发布产品功能
        /// </summary>
        /// <param name="product"></param>
        /// <param name="requestModel"></param>
        HtmlDocument GetProductSkuBase(BanggoProduct product, BanggoRequestModel requestModel);

        /// <summary>
        /// 解析产品的URL 得到款号
        /// </summary>
        /// <param name="url">产品的URL</param>
        /// <returns></returns>
        string ResolveProductUrlRetGoodsSn(string url);

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
        List<ProductColor> GetProductColorByOnline(BanggoRequestModel requestModel);

        /// <summary>
        /// 得到产品的颜色和大小数据，通过在线读取
        /// 
        /// </summary>
        /// <param name="requestModel">Referer\GoodsSn 必须传入</param>
        /// <param name="doc">调用GetGoodsDetialElementData方法获得</param>
        /// <returns></returns>
        List<ProductColor> GetProductColorByOnline(BanggoRequestModel requestModel, HtmlDocument doc);

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
        HtmlDocument GetGoodsDetialElementData(BanggoRequestModel requestModel);

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
        /// 通过款号搜索得到该产品的URL
        /// </summary>
        /// <param name="goodsSn">款号</param>
        /// <returns></returns>
        string GetGoodsUrl(string goodsSn);
    }
}