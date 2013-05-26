/*
 *名称：IBanggoMgt
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-12 05:35:46
 *修改时间：
 *备注：
 */

using System.Collections.Generic;
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
        /// 解析产品的URL 得到款号
        /// </summary>
        /// <param name="url">产品的URL</param>
        /// <returns></returns>
        string ResolveProductUrl(string url);
         
        /// <summary>
        /// 得到产品的颜色和大小数据，通过在线读取
        /// 该方法主要用于为手动干预价格，提供SKU数据
        /// </summary>
        /// <param name="requestModel">Referer\GoodsSn 必须传入</param>
        /// <returns></returns>
        List<ProductColor> GetProductColorByOnline(BanggoRequestModel requestModel);

        /// <summary>
        /// 将该产品的SKU数据导出为EXCEL
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="productUrl"></param>
        void ExportProductColorForExcel(string fileName, string productUrl);

    }
}