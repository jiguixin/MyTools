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

        void UpdateGoodsSkuInfo(IRequest req,
            IEnumerable<string> lstSearch,
            double discountRatio = 0.68,
            int stock = 3,
            string originalTitle = "xx",
            string newTitle = "xx",
            bool isModifyPrice = true);
    }

}