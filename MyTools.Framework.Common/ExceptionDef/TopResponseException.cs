/*
 *名称：TopResponseException
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-11 05:51:25
 *修改时间：
 *备注：
 */

using System;
using Infrastructure.Crosscutting.Declaration;

namespace MyTools.Framework.Common.ExceptionDef
{
    public class TopResponseException:Exception
    {

        #region Members

        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 子错误码
        /// </summary> 
        public string SubErrCode { get; set; }

        /// <summary>
        /// 子错误信息
        /// </summary> 
        public string SubErrMsg { get; set; }

        /// <summary>
        /// 禁止访问字段
        /// </summary>
        public string TopForbiddenFields { get; set; }

        #endregion


        #region Constructor

        public TopResponseException(string errCode, string errMsg, string subErrCode, string subErrMsg, string topForbiddenFields)
        {
            this.ErrCode = errCode;
            this.ErrMsg = errMsg;
            this.SubErrCode = subErrCode;
            this.SubErrMsg = subErrMsg;
            this.TopForbiddenFields = topForbiddenFields;
        }
        #endregion


        #region Public Methods

        public override string ToString()
        { 
            return base.ToString() +
                   "   ( ErrCode->'{0}'; ErrMsg->'{1}'; SubErrCode->'{2}'; SubErrMsg->'{3}'; TopForbiddenFields->'{4}' )".StringFormat(
                       ErrCode,
                       ErrMsg,
                       SubErrCode,
                       SubErrMsg,
                       TopForbiddenFields);
        }

        #endregion


        #region Private Methods

        #endregion

         
    }
}