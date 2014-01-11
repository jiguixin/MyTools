/*
 *名称：IRequest
 *功能：
 *创建人：吉桂昕
 *创建时间：2014-01-11 03:04:47
 *修改时间：
 *备注：
 */

using System;
using MyTools.TaoBao.DomainModule;

namespace MyTools.TaoBao.Interface
{
    public interface IRequest
    {
        /// <summary>
        /// 解析产品的URL 得到款号
        /// </summary>
        /// <param name="url">产品的URL</param>
        /// <returns></returns>
        string GetGoodsSn(string url);

        /// <summary>
        /// 得到单个产品信息,在线数据
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        Product GetGoodsInfo(RequestModel requestModel);

        /// <summary>
        /// 通过款号搜索得到该产品的URL
        /// </summary>
        /// <param name="goodsSn">款号</param>
        /// <returns></returns>
        string GetGoodsUrl(string goodsSn);

        /// <summary>
        /// 得到可售商品Sku
        /// </summary>
        /// <param name="product">产品</param>
        /// <param name="requestModel">请求模型</param>
        void GetProductSku(Product product, RequestModel requestModel);

        /// <summary>
        /// 设计在淘宝上的类目和设计店铺的类目
        /// </summary>
        /// <param name="product"></param>
        void SetCidAndSellerCids(Product product);
    }
}