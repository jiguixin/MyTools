/*
 *名称：ICommonApi
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-05 05:27:28
 *修改时间：
 *备注：
 */

using Top.Api.Util;

namespace MyTools.TaoBao.Interface
{ 
    /// <summary>
    /// 通用API
    /// </summary>
    public interface ICommonApi
    {
        /// 获得taobao认证接口
        /// <param name="authHtml">授权码</param>
        TopContext Authorized(string authHtml);
    }//end ICommonApi
}