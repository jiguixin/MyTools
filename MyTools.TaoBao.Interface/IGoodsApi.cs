/*
 *名称：IGoodsApi
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-11 05:12:44
 *修改时间：
 *备注：
 */

using System;
using Top.Api.Domain;
using Product = MyTools.TaoBao.DomainModule.Product;

namespace MyTools.TaoBao.Interface
{
    /// <summary>
    /// 商品API
    /// </summary>
    public interface IGoodsApi
    {
        /// <summary>
        /// 发布商品
        /// </summary>
        /// <param name="product">商品</param>
        /// <returns>商品编号</returns>
        Item PublishGoods(Product product);

        /// <summary>
        /// 更新和添加销售商品图片
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <param name="properties">销售属性</param>
        /// <param name="imgPath">本地图片路径</param>
        /// <returns></returns>
        PropImg UploadItemPropimg(long numId, string properties,string imgPath);



    }
     
}