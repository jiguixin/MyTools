/*
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

        /// <summary>
        ///     品牌
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 产品地址
        /// </summary>
        public string GoodsUrl { get; set; }

        /// <summary>
        ///     款号
        /// </summary>
        public string GoodsSn { get; set; }

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

        /// <summary>
        ///     商品主图 url
        /// </summary>
        public string ThumbUrl { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 是否是添加产品，因为添加需要设置一些默认值，但是修改就不需要这些默认值
        /// </summary>
        /// <param name="isAdd">新增为true</param>
        public BanggoProduct(bool isAdd = true)
        {
            if (isAdd)
            {
                FreightPayer = "buyer";
                Type = "fixed";
                StuffStatus = "new";
                LocationState = SysConst.LocationState;
                LocationCity = SysConst.LocationCity;
                SubStock = 2;
                ValidThru = 14;   
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}