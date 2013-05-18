/*
 *名称：IDelivery
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-18 12:34:04
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
using Top.Api.Domain;

namespace MyTools.TaoBao.Interface
{
    public interface IDeliveryApi:IDelivery
    {

        /// <summary>
        /// 物流API taobao.delivery.templates.get
        /// 获取用户下所有模板
        /// </summary> 
        List<DeliveryTemplate> GetDeliveryTemplates(string fields = "template_id,template_name");
    }
}