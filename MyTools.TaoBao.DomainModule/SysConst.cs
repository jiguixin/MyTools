/*
 *名称：SysConst
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-10 10:54:48
 *修改时间：
 *备注：
 */

using System;
using System.Configuration;

namespace MyTools.TaoBao.DomainModule
{
    public class SysConst
    {
        public static readonly string AppKey = ConfigurationManager.AppSettings["appKey"];
        public static readonly string AppSecret = ConfigurationManager.AppSettings["appSecret"];
        public static readonly string PostageId = ConfigurationManager.AppSettings["PostageId"];
        
    }
}