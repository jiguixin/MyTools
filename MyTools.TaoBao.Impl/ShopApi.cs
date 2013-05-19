///////////////////////////////////////////////////////////
//  ShopApi.cs
//  Implementation of the Class ShopApi
//  Generated by Enterprise Architect
//  Created on:      08-五月-2013 0:20:00
//  Original author: 吉桂昕
///////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Infrastructure.Crosscutting.IoC;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;

namespace MyTools.TaoBao.Impl
{
    /// <summary>
    ///     店铺API,通过调用淘宝API实现相关功能
    /// </summary>
    public class ShopApi : IShopApi
    {
        ITopClient client = InstanceLocator.Current.GetInstance<ITopClient>();
         
        public ShopApi()
        {
        }

        ////店铺API，taobao.sellercats.list.get; 获取卖家自己的产品类目
        /// <summary>
        ///店铺API，taobao.sellercats.list.get; 获取卖家自己的产品类目
        /// </summary>
        /// <param name="userNick">淘宝昵称</param>
        public List<SellerCat> GetSellercatsList(string userNick)
        {
            if (string.IsNullOrEmpty(userNick))
                throw new Exception(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty
                                                 , new System.Diagnostics.StackTrace().ToString()));

            var reqCats = new SellercatsListGetRequest {Nick = userNick};
            SellercatsListGetResponse responseCats = client.Execute(reqCats);

            return responseCats.SellerCats;
        }

        /// <summary>
        /// 获取商品所属的店铺类目列表
        /// </summary>
        /// <param name="userNick">淘宝用户名</param>
        /// <param name="parentSellCatName">店铺的父组类目</param>
        /// <param name="childSellCatsNames">子类目列表</param>
        public string GetSellerCids(string userNick, string parentSellCatName, params string[] childSellCatsNames)
        {
            List<SellerCat> sellerCats = GetSellercatsList(userNick);

            return GetSellerCids(sellerCats, parentSellCatName, childSellCatsNames);
        }

        /// <summary>
        /// 获取商品所属的店铺类目列表
        /// </summary>
        /// <param name="sellerCats">卖家的类目列表</param>
        /// <param name="parentSellCatName">店铺的父组类目</param>
        /// <param name="childSellCatsNames">子类目列表</param>
        public string GetSellerCids(List<SellerCat> sellerCats, string parentSellCatName,
                                    params string[] childSellCatsNames)
        { 
            if (string.IsNullOrEmpty(parentSellCatName))
                throw new Exception(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty, new System.Diagnostics.StackTrace().ToString()));

            // 获取父节点
            SellerCat parentSellerCat = sellerCats.FirstOrDefault(s => s.Name.ToUpper() == parentSellCatName);

            if (parentSellerCat == null)
                throw new Exception(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty, new System.Diagnostics.StackTrace().ToString()));

            //如果用户数据有误，将子节点放到了父结点中，如果发现是子节点，就直接返回CID
            if (parentSellerCat.ParentCid != 0)
                return parentSellerCat.Cid.ToString(CultureInfo.InvariantCulture);

            //如果子结点为空，直接返回父亲结点CID
            // ReSharper disable AssignNullToNotNullAttribute
            if (childSellCatsNames == null && !childSellCatsNames.Any())
                // ReSharper restore AssignNullToNotNullAttribute
            {
                return parentSellerCat.Cid.ToString(CultureInfo.InvariantCulture);
            }
              
            #region 原始版
            
            //1, 获取该parentName对应的Cid
            //            var parentCid = sellerCats.FirstOrDefault(s=>s.Name == parentSellCatName);
            //
            //            if (parentCid == null)
            //                return null;


            //2，根据父CID和各类别名查找相应的子类别用逗号分开
            //string result = null;
            //foreach (var cat in childSellCatsNames)
            //{
            //    var sellerCat = sellerCats.FirstOrDefault(s => s.ParentCid == parentCid.Cid && s.Name == cat);
            //    if (sellerCat != null)
            //    {
            //        if (!string.IsNullOrEmpty(result))
            //            result += "," + sellerCat.Cid;
            //        else
            //            result += sellerCat.Cid;
            //    }

            //}
            //return result;

            //等同于下面表达式
            #endregion
            
            string result = null;
            foreach (
                SellerCat sellerCat in
                    childSellCatsNames.Select(
                        cat => sellerCats.FirstOrDefault(s => s.ParentCid == parentSellerCat.Cid && s.Name.Contains(cat)))
                                      .Where(sellerCat => sellerCat != null))
            {
                if (!string.IsNullOrEmpty(result))
                    result += "," + sellerCat.Cid;
                else
                    result += sellerCat.Cid;
            }
            return result;
        }

        /// <summary>
        ///     得到店铺橱窗数量
        /// </summary>
        public string GetShopRemainshowcase()
        {
            return "";
        }

        ~ShopApi()
        {
        }

        public virtual void Dispose()
        {
        }
    }
}

//end ShopApi