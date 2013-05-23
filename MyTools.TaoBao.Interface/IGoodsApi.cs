/*
 *名称：IGoodsApi
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-11 05:12:44
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
using Top.Api.Domain;
using Top.Api.Request;
using Product = MyTools.TaoBao.DomainModule.Product;

namespace MyTools.TaoBao.Interface
{
    /// <summary>
    /// 商品API
    /// </summary>
    public interface IGoodsApi
    {
        /// <summary>
        /// 发布商品
        /// </summary>
        /// <param name="product">商品</param>
        /// <returns>商品编号</returns>
        Item PublishGoods(Product product);

        /// <summary>
        /// 从banggo上获取数据发布到淘宝
        /// </summary>
        /// <param name="banggoProductUrl"></param>
        /// <returns></returns>
        Item PublishGoodsForBanggoToTaobao(string banggoProductUrl);

        /// <summary>
        /// 更新和添加销售商品图片
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <param name="properties">销售属性</param>
        /// <param name="imgPath">本地图片路径</param>
        /// <returns></returns>
        PropImg UploadItemPropimg(long numId, string properties, string imgPath);

        /// <summary>
        /// 更新和添加销售商品图片
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <param name="properties">销售属性</param>
        /// <param name="urlImg">网上的图片地址</param>
        /// <returns></returns>
        PropImg UploadItemPropimg(long numId, string properties, Uri urlImg);



        /// <summary>
        /// 获取当前会话用户出售中的商品列表 
        /// taobao.items.onsale.get 
        /// </summary>
        /// <example>
        /// ItemsOnsaleGetRequest req = new ItemsOnsaleGetRequest();
        /// req.Fields = "num_iid,title,price";
        /// req.Q = "N97";
        /// req.Cid = 1512L;
        /// req.SellerCids = "11";
        /// req.PageNo = 10L;
        /// req.HasDiscount = true;
        /// req.HasShowcase = true;
        /// req.OrderBy = "list_time:desc";
        /// req.IsTaobao = true;
        /// req.IsEx = true;
        /// req.PageSize = 100L;
        /// DateTime dateTime = DateTime.Parse("2000-01-01 00:00:00");
        /// req.StartModified = dateTime;
        /// DateTime dateTime = DateTime.Parse("2000-01-01 00:00:00");
        /// req.EndModified = dateTime;
        /// ItemsOnsaleGetResponse response = client.Execute(req, sessionKey);
        /// </example>
        /// <param name="req">要查询传入的参数</param>
        List<Item> GetOnSaleGoods(ItemsOnsaleGetRequest req);

        /// <summary>
        /// 得到当前会话用户库存中的商品列表 
        /// taobao.items.inventory.get
        /// </summary>
        /// <example>
        /* ITopClient client = new DefaultTopClient(url, appkey, appsecret);
          ItemsInventoryGetRequest req=new ItemsInventoryGetRequest();
           req.Fields = "pic_url,num,props,valid_thru";
           req.Q = "nike";
           req.Banner = "for_shelved";
           req.Cid = 1232L;
           req.SellerCids = "12,123";
           req.PageNo = 2L;
           req.PageSize = 40L;
           req.HasDiscount = true;
           req.OrderBy = "list_time:desc";
           req.IsTaobao = true;
           req.IsEx = true;
           DateTime dateTime = DateTime.Parse("2000-01-01 00:00:00");
           req.StartModified = dateTime;
           DateTime dateTime = DateTime.Parse("2000-01-01 00:00:00");
           req.EndModified = dateTime;
           ItemsInventoryGetResponse response = client.Execute(req, sessionKey);*/
        /// </example>
        /// <param name="req"></param>
        /// <returns></returns>
        List<Item> GetInventoryGoods(ItemsInventoryGetRequest req);
    }

}