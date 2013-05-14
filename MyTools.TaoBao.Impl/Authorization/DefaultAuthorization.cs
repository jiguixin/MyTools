/*
 *名称：DefaultAuthorization
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-05 05:29:00
 *修改时间：
 *备注：
 */

using System;
using System.Diagnostics;
using System.Linq;
using HtmlAgilityPack;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface.Authorization;
using Top.Api.Util;

namespace MyTools.TaoBao.Impl.Authorization
{
    /// <summary>
    ///默认认证方式的实现，用于客户端应用程序
    /// </summary>
    public class DefaultAuthorization : IAuthorization
    {
        /// <param name="authHtml">授权码</param>
        public TopContext Authorized(string authHtml)
        {
            if (string.IsNullOrWhiteSpace(authHtml))
                throw new Exception(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty, new System.Diagnostics.StackTrace().ToString()));

            HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(authHtml);
            var selectNodes =
                doc.DocumentNode.SelectNodes(Resource.SysConfig_AuthorizedCodeXPath);

            if (selectNodes == null)
                throw new Exception(Resource.Exception_NotFoundAuthorizedCode);

            return (from value in selectNodes select value.Attributes[1].Value into authCode where !string.IsNullOrEmpty(authCode) && authCode.IndexOf("TOP-") >= 0 select TopUtils.GetTopContext(authCode)).FirstOrDefault();


            /*等价于：
             * foreach (HtmlNode value in selectNodes)
            {
                string authCode = value.Attributes[1].Value;
                if (!string.IsNullOrEmpty(authCode) && authCode.IndexOf("TOP-") >= 0)
                {
                    return TopUtils.GetTopContext(authCode);
                }
            }
             */
        }

        public virtual void Dispose()
        {
        }
    }

//end DefaultAuthorization
}