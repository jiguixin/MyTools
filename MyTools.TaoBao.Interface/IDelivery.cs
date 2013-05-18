/*
 *名称：IDelivery
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-18 10:57:23
 *修改时间：
 *备注：
 */

using System;

namespace MyTools.TaoBao.Interface
{ 
    public interface IDelivery
    {
        /// <summary>
        /// 通过模版名得到模版ID
        /// </summary>
        /// <param name="tempName"></param>
        /// <returns></returns>
        string GetDeliveryTemplateId(string tempName);

    }
}