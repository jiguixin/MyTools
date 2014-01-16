/*
 *名称：IGoodsPublish
 *功能：
 *创建人：吉桂昕
 *创建时间：2014-01-16 03:24:13
 *修改时间：
 *备注：
 */

using System;

namespace MyTools.TaoBao.Interface
{
    using System.Collections.Generic;

    using Top.Api.Domain;

    public interface IGoodsPublish
    {
        /// 
        /// <param name="sUrl">在线产品URL</param>
        /// <param name="requestSource">数据源操作类，需要传最具的实现类，如邦购源的实现，淘宝源的实现</param>
        Item PublishGoods(string sUrl, IRequest requestSource);

        /// 
        /// <param name="requestSource"></param>
        /// <param name="isModifyPrice "></param>
        /// <param name="lstSearch"></param>
        void UpdateGoodsFromOnSale(IRequest requestSource, IEnumerable<string> lstSearch, bool isModifyPrice = true);
        /// <summary>
        /// 通过指定部分没有更新成功的商品重新更新
        /// </summary> 
        /// <param name="nulIds">多个产品以“，”号分割</param>
        /// <param name="isModifyPrice">是否要修改商品价格,true是要修改，false是只更新库存不修改以前的价格</param>
        void UpdateGoodsByAssign(IRequest req,string nulIds, bool isModifyPrice = true);

        void UpdateGoodsSkuInfo(IRequest req,
            IEnumerable<string> lstSearch,
            double discountRatio = 0.68,
            int stock = 3,
            string originalTitle = "xx",
            string newTitle = "xx",
            bool isModifyPrice = true);
    }

}