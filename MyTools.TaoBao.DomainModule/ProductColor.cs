/*
 *名称：ProductColor
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-12 05:56:15
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;

namespace MyTools.TaoBao.DomainModule
{
    public class ProductColor
    {

        #region Members

        /// <summary>
        /// 颜色名字
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 色码
        /// </summary>
        public string ColorCode { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        public string ImgUrl { get; set; }

        public List<ProductSize> SizeList;

        /// <summary>
        /// 对应淘宝的props属性值, 用于更新商品的销售图片
        /// </summary>
        public string MapProps { get; set; }

        ///// <summary>
        ///// 在些处不是必须，主要是因为在更新SKU时，需要得到 销售属性与淘宝的销售属性对应，如。key: 155/80A(S); 20509:28314
        ///// </summary>
        //public Dictionary<string, string> BSizeToTSize { get; set; }

        /// <summary>
        /// 该颜色下库存
        /// </summary>
        public int AvlNumForColor { get; set; }

        #endregion


        #region Constructor

        #endregion


        #region Public Methods

        #endregion


        #region Private Methods

        #endregion


    }
}