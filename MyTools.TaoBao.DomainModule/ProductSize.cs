/*
 *名称：ProductSize
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-12 06:10:31
 *修改时间：
 *备注：
 */

using System;

namespace MyTools.TaoBao.DomainModule
{
    public class ProductSize
    {

        #region Members

        /// <summary>
        /// 大小编码
        /// </summary>
        public string SizeCode { get; set; }

        /// <summary>
        /// 大小别名
        /// </summary>
        public string Alias{get;set;}

        /// <summary>
        /// 库存
        /// </summary>
        public int AvlNum { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public double SalePrice { get; set; }

        #endregion


        #region Constructor

        #endregion


        #region Public Methods

        #endregion


        #region Private Methods

        #endregion

         
    }
}