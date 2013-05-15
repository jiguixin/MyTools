/*
 *名称：BanggoProduct
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-12 05:37:24
 *修改时间：
 *备注：
 */

using System;

namespace MyTools.TaoBao.DomainModule
{
    public class BanggoProduct
    {

        #region Members

        /// <summary>
        /// 品牌
        /// </summary>
        public string BrandCode { get; set; }

        /// <summary>
        /// 款号
        /// </summary>
        public string GoodsSn { get; set; }

        /// <summary>
        /// 产品标题
        /// </summary>
        public string ProductTitle { get;set;}

        /// <summary>
        /// 市场价
        /// </summary>
        public double MarketPrice { get; set; }

        /// <summary>
        /// 总售价
        /// </summary>
        public double SalePrice { get; set; }

        /// <summary>
        /// VIP价
        /// </summary>
        public double VipPrice { get; set; }

        /// <summary>
        ///VIP价
        /// </summary>
        public double SvipPrice { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int SalesVolume { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public string GoodsDiscount { get; set; }

        /// <summary>
        /// 类别，如男装、女装
        /// </summary>
        public string Category { get; set; }
         

        #endregion


        #region Constructor

        #endregion


        #region Public Methods

        #endregion


        #region Private Methods

        #endregion


    }


}