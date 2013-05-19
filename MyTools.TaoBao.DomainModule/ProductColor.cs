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


        #endregion


        #region Constructor

        #endregion


        #region Public Methods

        #endregion


        #region Private Methods

        #endregion


    }
}