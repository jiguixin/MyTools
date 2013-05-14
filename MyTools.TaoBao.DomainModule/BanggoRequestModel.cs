/*
 *名称：BanggoRequestModel
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-13 11:24:47
 *修改时间：
 *备注：
 */

using System;

namespace MyTools.TaoBao.DomainModule
{
    public class BanggoRequestModel
    {

        #region Members

        /// <summary>
        /// 尺码，5位数，前3位是款号前3位，后2位是大小后2位如：23852
        /// </summary>
        public string SizeCode { get; set; }

        /// <summary>
        /// 色码
        /// </summary>
        public string ColorCode { get; set; }

        /// <summary>
        /// 款号
        /// </summary>
        public string GoodsSn { get; set; }

        /// <summary>
        /// 引用网站,该产品地址
        /// </summary>
        public string Referer { get; set; }

        #endregion


        #region Constructor

        #endregion


        #region Public Methods

        #endregion


        #region Private Methods

        #endregion

         
    }
}