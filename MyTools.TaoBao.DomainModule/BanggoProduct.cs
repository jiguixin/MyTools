﻿/*
 *名称：BanggoProduct
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-12 05:37:24
 *修改时间：
 *备注：
 */

using Infrastructure.Crosscutting.Declaration;
using MyTools.Framework.Common;

namespace MyTools.TaoBao.DomainModule
{
    public class BanggoProduct : Product
    {
        #region Members
      
        private string _title;

        /// <summary>
        ///     产品标题
        /// </summary>
        public override string Title
        {
            get
            {
                //从banggo正向填充标题
                if (!Brand.IsNullOrEmpty() && !ParentCatalog.IsNullOrEmpty() && !GoodsSn.IsNullOrEmpty() && MarketPrice > 0)
                {
                    string startTitle = null;
                    if (ParentCatalog == "外套")
                    {
                        startTitle = "{0} {1} {2} {3} {4} 原价:{5}".StringFormat(SysConst.PrefixTitle, Brand, Category,
                                                                    Catalog, GoodsSn, MarketPrice);
                    }
                    else
                    {
                        startTitle = "{0} {1} {2} {3} {4} 原价:{5}".StringFormat(SysConst.PrefixTitle, Brand, Category,
                                                                    ParentCatalog, GoodsSn, MarketPrice);
                    }

          
                    //标题字符不能大于60满足款号所以长度只能是54个字符
                    if (startTitle.Length > 60)
                    {
                        int moreThanNum = startTitle.Length - 60;

                        return startTitle.Remove(0, moreThanNum);
                    } 
                    return startTitle;    
                }

                //用于从淘宝中读取商品标题
                return _title; 
            }
            set { _title = value; }
        }

        /// <summary>
        ///     VIP价
        /// </summary>
        public double VipPrice { get; set; }

        /// <summary>
        ///     VIP价
        /// </summary>
        public double SvipPrice { get; set; }

        /// <summary>
        ///     折扣
        /// </summary>
        public string GoodsDiscount { get; set; }

        /// <summary>
        ///     类别，如男装、女装
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///     得到banggo的父目录，如 T恤
        /// </summary>
        public string ParentCatalog { get; set; }

        /// <summary>
        ///     商品目录，如 针织短袖恤
        /// </summary>
        public string Catalog { get; set; }

        #endregion

        #region Constructor
         
        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}