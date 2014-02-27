/*
 *名称：AuthorizedContext
 *功能：
 *创建人：吉桂昕
 *创建时间：2014-02-27 10:19:38
 *修改时间：
 *备注：
 */

using System;

namespace MyTools.TaoBao.DomainModule
{
    public class AuthorizedContext
    {

        #region Members

        /// <summary>
        /// 应用编号
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 授权码
        /// </summary>
        public string SessionKey { get; set; }

        public string RefreshToken { get; set; }

        /// <summary>
        /// 淘宝用户昵称
        /// </summary>
        public string UserNick { get; set; }

        /// <summary>
        /// 淘宝用户编号
        /// </summary>
        public string UserId { get; set; }

        #endregion


        #region Constructor

        #endregion


        #region Public Methods

        #endregion


        #region Private Methods

        #endregion

         
    }
}