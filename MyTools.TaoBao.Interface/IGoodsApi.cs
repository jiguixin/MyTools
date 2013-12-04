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
        /// 从在售商品中更新库存
        /// </summary>
        /// <param name="search">搜索在线商品条件</param>
        /// <param name="isModifyPrice">是否要修改商品价格,true是要修改，false是只更新库存不修改以前的价格</param>
        void UpdateGoodsFromOnSale(string search = null, bool isModifyPrice = true);

        /// <summary>
        /// 从在售商品中更新库存
        /// </summary>
        /// <param name="lstSearch">多个搜索在线商品条件</param>
        /// <param name="isModifyPrice">是否要修改商品价格,true是要修改，false是只更新库存不修改以前的价格</param>
        void UpdateGoodsFromOnSale(IEnumerable<string> lstSearch, bool isModifyPrice = true);

        /// <summary>
        /// 更新产品库存和价格信息，包括修改标题和发货地址
        /// </summary>
        /// <param name="lstSearch"></param>
        /// <param name="isModifyPrice"></param>
        void UpdateGoodsSkuInfo(IEnumerable<string> lstSearch, double discountRatio = 0.68, int stock = 3, string originalTitle = "xx", string newTitle = "xx", bool isModifyPrice = true);

        /// <summary>
        /// 通过指定部分没有更新成功的商品重新更新
        /// </summary> 
        /// <param name="nulIds">多个产品以“，”号分割</param>
        /// <param name="isModifyPrice">是否要修改商品价格,true是要修改，false是只更新库存不修改以前的价格</param>
        void UpdateGoodsByAssign(string nulIds, bool isModifyPrice = true);

        /// <summary>
        /// 从banggo上获取数据发布到淘宝
        /// </summary>
        /// <param name="banggoProductUrl"></param>
        /// <returns></returns>
        Item PublishGoodsForBanggoToTaobao(string banggoProductUrl);

        /// <summary>
        /// 从EXCEL读取产品信息并发布
        /// </summary>
        /// <param name="filePath"></param>
        void PublishGoodsFromExcel(string filePath);

        //taobao.item.sku.delete 删除SKU 
        /// <summary>
        /// 删除单个SKU
        /// </summary>
        /// <param name="numId"></param>
        /// <param name="properties"></param>
        void DeleteGoodsSku(long numId, string properties,string goodSn = "");

        /// <summary>
        /// taobao.item.sku.update 更新SKU信息
        /// </summary>
        /// <returns></returns>
        Sku UpdateSku(ItemSkuUpdateRequest req);

         //taobao.item.skus.get 根据商品ID列表获取SKU信息 
        /// <summary>
        /// 根据商品ID列表获取SKU信息 
        /// </summary>
        /// <param name="numIds">支持多个商品，用“，”号分割</param>
        /// <returns></returns>
        IEnumerable<Sku> GetSkusByNumId(string numIds);

        /// <summary>
        /// 检查该商品是否已经发布
        /// </summary>
        /// <param name="goodsSn">款号</param>
        /// <returns>已经发布返回true</returns>
        Item VerifyGoodsExist(string goodsSn);

        /// <summary>
        /// 更新和添加销售商品图片
        /// taobao.item.propimg.upload 添加或修改属性图片 
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <param name="properties">销售属性</param>
        /// <param name="imgPath">本地图片路径</param>
        /// <returns></returns>
        PropImg UploadItemPropimg(long numId, string properties, string imgPath);
          
        /// <summary>
        /// 更新和添加销售商品图片
        /// taobao.item.propimg.upload 添加或修改属性图片 
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <param name="properties">销售属性</param>
        /// <param name="urlImg">网上的图片地址</param>
        /// <returns></returns>
        PropImg UploadItemPropimg(long numId, string properties, Uri urlImg);

        /// <summary>
        /// 删除该商品的销售图片
        /// taobao.item.propimg.delete 删除属性图片
        /// </summary>
        /// <param name="imgId">图片ID</param>
        /// <param name="numId">商品编号</param>
        /// <returns></returns>
        PropImg DeleteItemPropimg(long imgId, long numId,string goodsSn="");

        /// <summary>
        /// taobao.item.update.delisting 商品下架
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <returns></returns>
        Item GoodsDelisting(long numId);

        /// <summary>
        /// 按指定条件下架该产品
        /// </summary>
        /// <param name="lstSearch"></param>
        void GoodsDelistingFromOnSale(IEnumerable<string> lstSearch);

        /// <summary>
        /// 得到单个商品信息
        /// taobao.item.get
        /// </summary>
        /// <param name="req"></param>
        /// <returns>商品详情</returns>
        Item GetGoods(ItemGetRequest req);

        /// <summary>
        /// 通过商品编号得到常用的Item数据
        /// 调用的GetGoods(ItemGetRequest req)
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <returns>商品详情</returns>
        Item GetGoods(string numId);

        /// <summary>
        /// 得到产品列表
        /// </summary>
        /// <param name="numIds"></param>
        /// <returns></returns>
        List<Item> GetGoodsList(string numIds);

        /// <summary>
        /// 获取当前会话用户出售中的商品列表 
        /// taobao.items.onsale.get 
        /// </summary>
        /// <example>
        /* example
        ItemsOnsaleGetRequest req = new ItemsOnsaleGetRequest();
        req.Fields = "num_iid,title,price";
        req.Q = "N97";
        req.Cid = 1512L;
        req.SellerCids = "11";
        req.PageNo = 10L;
        req.HasDiscount = true;
        req.HasShowcase = true;
        req.OrderBy = "list_time:desc";
        req.IsTaobao = true;
        req.IsEx = true;
        req.PageSize = 100L;
        DateTime dateTime = DateTime.Parse("2000-01-01 00:00:00");
        req.StartModified = dateTime;
        DateTime dateTime = DateTime.Parse("2000-01-01 00:00:00");
        req.EndModified = dateTime;
        ItemsOnsaleGetResponse response = client.Execute(req, sessionKey);
        */ 
        /// </example>
        /// <param name="req">要查询传入的参数</param>
        List<Item> GetOnSaleGoods(ItemsOnsaleGetRequest req);

        /// <summary>
        /// 得到当前在售商品，最多个数为199
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        List<Item> GetOnSaleGoods(string search = null);

        /// <summary>
        /// 得到当前会话用户库存中的商品列表 
        /// taobao.items.inventory.get
        /// </summary>
        /// <example>
        /* example
           ITopClient client = new DefaultTopClient(url, appkey, appsecret);
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