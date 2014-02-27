/*
 *名称：DeliveryApi
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-18 12:37:35
 *修改时间：
 *备注：
 */

using System.Collections.Generic;
using System.Globalization;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;
using MyTools.Framework.Common.ExceptionDef;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Util;

namespace MyTools.TaoBao.Impl
{
    public class DeliveryApi : IDeliveryApi
    {
        #region Members

        private readonly ITopClient _client = InstanceLocator.Current.GetInstance<ITopClient>();

        #endregion

        #region Constructor

        #endregion

        #region Public Methods

        /// <summary>
        ///     通过模版名得到模版ID，如果通过模版名没有找到，者返回第一个模版ID
        ///     如果用户没有设置运费模版者返回NULL
        /// </summary>
        /// <param name="tempName"></param>
        /// <returns></returns>
        public string GetDeliveryTemplateId(string tempName)
        {
            List<DeliveryTemplate> deliveryTemplateList = GetDeliveryTemplates();

            if (deliveryTemplateList.IsNullOrEmpty())
                return null;

            DeliveryTemplate dTmp = deliveryTemplateList.Find(f => f.Name == Resource.SysConfig_DeliveryTemplateName);

            return dTmp.IsNotNull()
                       ? dTmp.TemplateId.ToString(CultureInfo.InvariantCulture)
                       : deliveryTemplateList[0].TemplateId.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     物流API taobao.delivery.templates.get
        ///     获取用户下所有模板
        /// </summary>
        public List<DeliveryTemplate> GetDeliveryTemplates(string fields = "template_id,template_name")
        {
            var req = new DeliveryTemplatesGetRequest {Fields = fields};

            var tContext = InstanceLocator.Current.GetInstance<AuthorizedContext>();

            DeliveryTemplatesGetResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
                throw new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                               response.SubErrMsg, response.TopForbiddenFields);

            return response.DeliveryTemplates;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}