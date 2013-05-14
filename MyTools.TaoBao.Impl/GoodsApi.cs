/*
 *名称：GoodsApi
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-11 05:37:44
 *修改时间：
 *备注：
 */

using System;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Utility;
using MyTools.TaoBao.DomainModule.ExceptionDef;
using MyTools.TaoBao.Interface;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Util;
using MyTools.TaoBao.DomainModule;
using Product = MyTools.TaoBao.DomainModule.Product;

namespace MyTools.TaoBao.Impl
{
    public class GoodsApi:IGoodsApi
    {

        #region Members

        ITopClient client = InstanceLocator.Current.GetInstance<ITopClient>();

        #endregion


        #region Constructor

        #endregion


        #region public Methods

        /// <summary>
        /// 发布商品
        /// </summary>
        /// <param name="product">商品</param>
        /// <returns>商品编号</returns>
        public Item PublishGoods(Product product)
        { 
            ItemAddRequest req = new ItemAddRequest();
            
            Util.CopyModel(product, req);

            TopContext tContext = InstanceLocator.Current.GetInstance<TopContext>();
            ItemAddResponse response = client.Execute(req, tContext.SessionKey);

            if (response.IsError)
                throw new TopResponseException(response.ErrCode,response.ErrMsg,response.SubErrCode,response.SubErrMsg,response.TopForbiddenFields);

            return response.Item;
        }

        /// <summary>
        /// 更新和添加销售商品图片
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <param name="properties">销售属性</param>
        /// <param name="imgPath">本地图片路径</param>
        /// <returns></returns>
        public PropImg UploadItemPropimg(long numId, string properties,string imgPath)
        {
            #region validation

            if (numId <= 0 || string.IsNullOrEmpty(properties) || string.IsNullOrEmpty(imgPath))
                throw new Exception(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty, new System.Diagnostics.StackTrace().ToString()));

            #endregion

            ItemPropimgUploadRequest req = new ItemPropimgUploadRequest();
            req.NumIid = numId;
            FileItem fItem = new FileItem(imgPath);
            req.Image = fItem;
            req.Properties = properties;
            TopContext tContext = InstanceLocator.Current.GetInstance<TopContext>();
            ItemPropimgUploadResponse response = client.Execute(req, tContext.SessionKey);

            if (response.IsError)
                throw new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode, response.SubErrMsg, response.TopForbiddenFields);

            return response.PropImg;
        }

        #endregion
         

        #region Private Methods

        #endregion

        
    }
}