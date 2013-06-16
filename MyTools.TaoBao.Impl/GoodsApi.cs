/*
 *名称：GoodsApi
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-11 05:37:44
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Utility;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using MyTools.Framework.Common;
using MyTools.Framework.Common.ExceptionDef;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Util;
using Product = MyTools.TaoBao.DomainModule.Product;

namespace MyTools.TaoBao.Impl
{
    public class GoodsApi : IGoodsApi
    {
        #region Members

        private readonly IBanggoMgt _banggoMgt = InstanceLocator.Current.GetInstance<IBanggoMgt>();

        private readonly ICatalog _catalog = InstanceLocator.Current.GetInstance<ICatalog>(Resource.SysConfig_GetDataWay);
        private readonly ITopClient _client = InstanceLocator.Current.GetInstance<ITopClient>();


        private readonly IDelivery _delivery =
            InstanceLocator.Current.GetInstance<IDelivery>(Resource.SysConfig_GetDataWay);

        private readonly Dictionary<string, string> _dicColorMap = new Dictionary<string, string>();

        private readonly ILogger _log = InstanceLocator.Current.GetInstance<ILoggerFactory>().Create();
        private readonly IShop _shop = InstanceLocator.Current.GetInstance<IShop>(Resource.SysConfig_GetDataWay);
        public List<SellerCat> SellercatsList;

        #endregion

        #region Constructor

        #endregion

        #region public Methods

        /// <summary>
        ///     发布商品
        /// </summary>
        /// <param name="product">商品</param>
        /// <returns>商品编号</returns>
        public Item PublishGoods(Product product)
        {
            _log.LogInfo(Resource.Log_PublishGoodsing.StringFormat(product.Title));

            var req = new ItemAddRequest();

            Util.CopyModel(product, req);

            var tContext = InstanceLocator.Current.GetInstance<TopContext>();
            ItemAddResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                var ex = new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                                  response.SubErrMsg, response.TopForbiddenFields);
                _log.LogError(Resource.Log_PublishGoodsFailure, product.Title, ex);
                throw ex;
            }

            Item item = response.Item;

            _log.LogInfo(Resource.Log_PublishGoodsSuccess, product.Title, item.NumIid);

            return item;
        }

        /// <summary>
        ///     从在售商品中更新库存
        /// </summary>
        public void UpdateGoodsFromOnSale(string search = null,bool isModifyPrice = true)
        { 
            List<Item> lstItem = GetOnSaleGoods(search);

            UpdateGoodsInternal(lstItem, isModifyPrice);
        }

        /// <summary>
        /// 从在售商品中更新库存
        /// </summary>
        /// <param name="lstSearch">多个搜索在线商品条件</param>
        /// <param name="isModifyPrice">是否要修改商品价格,true是要修改，false是只更新库存不修改以前的价格</param>
        public void UpdateGoodsFromOnSale(IEnumerable<string> lstSearch, bool isModifyPrice = true)
        {
            foreach (var search in lstSearch)
            {
                if (string.IsNullOrWhiteSpace(search))
                    continue;
                UpdateGoodsFromOnSale(search, isModifyPrice);
            }
        }

        /// <summary>
        ///     通过指定部分没有更新成功的商品重新更新
        /// </summary>
        /// <param name="numIds">多个产品以“，”号分割</param>
        public void UpdateGoodsByAssign(string numIds,bool isModifyPrice = true)
        {
            List<Item> lstItem = GetGoodsList(numIds);

            UpdateGoodsInternal(lstItem, isModifyPrice);
        }
         

        /// <summary>
        ///     从banggo上获取数据发布到淘宝
        /// </summary>
        /// <param name="banggoProductUrl"></param>
        /// <returns></returns>
        public Item PublishGoodsForBanggoToTaobao(string banggoProductUrl)
        {
            string goodsSn = _banggoMgt.ResolveProductUrlRetGoodsSn(banggoProductUrl);

            Item item = VerifyGoodsExist(goodsSn);
            if (item.IsNotNull())
            {
                try
                {
                    var banggoProductEdit = new BanggoProduct(false)
                    {
                        ColorList = _banggoMgt.GetProductColorByOnline(new BanggoRequestModel(){ GoodsSn=item.OuterId, Referer = banggoProductUrl}),
                        GoodsSn = item.OuterId,
                        GoodsUrl = banggoProductUrl,
                        Cid = item.Cid,
                        NumIid = item.NumIid,
                        //替换原来的产品标题
                        Title = item.Title.Replace(SysConst.OriginalTitle, SysConst.NewTitle)
                    };
                      
                    DeleteSkuOnlyOne(item);

                    UpdateGoodsAndUploadPic(banggoProductEdit);
                }
                catch(Exception ex)
                {
                    _log.LogError(Resource.Log_UpdateGoodsFailure.StringFormat(item.NumIid, item.OuterId), ex);
                }
                return item;
            }

            BanggoProduct banggoProduct =
                _banggoMgt.GetGoodsInfo(new BanggoRequestModel {GoodsSn = goodsSn, Referer = banggoProductUrl});

            if (banggoProduct.ColorList.IsNullOrEmpty())
                return null;

            return PublishGoodsAndUploadPic(banggoProduct);
        }

        public void PublishGoodsFromExcel(string filePath)
        {
            IEnumerable<PublishGoods> lstPublishGoods = GetPublishGoodsFromExcel(filePath);

            foreach (PublishGoods pgModel in lstPublishGoods)
            {
                Thread.Sleep(500);

                Item item = VerifyGoodsExist(pgModel.GoodsSn);
                if (item.IsNotNull())
                {
                    #region 更新现有商品

                    try
                    {
                        var banggoProduct = new BanggoProduct(false)
                            {
                                ColorList = pgModel.ProductColors,
                                GoodsSn = item.OuterId,
                                GoodsUrl = pgModel.Url,
                                Cid = item.Cid,
                                NumIid = item.NumIid,
                                //替换原来的产品标题
                                Title = item.Title.Replace(SysConst.OriginalTitle, SysConst.NewTitle)
                            };
                        //  Util.CopyModel(item, banggoProduct); node: 不能在这赋值，这样就会造成有些为NULL的给赋成了默认值 

                        DeleteSkuOnlyOne(item);

                        UpdateGoodsAndUploadPic(banggoProduct);
                    }
                    catch(Exception ex)
                    {
                        _log.LogError(Resource.Log_UpdateGoodsFailure.StringFormat(item.NumIid, item.OuterId), ex);
                        continue;
                    }

                    #endregion
                }
                else
                {
                    #region 发布商品 

                    try
                    {
                        var product = new BanggoProduct {GoodsSn = pgModel.GoodsSn};

                        var requestModel = new BanggoRequestModel {Referer = pgModel.Url, GoodsSn = pgModel.GoodsSn};

                        _banggoMgt.GetProductBaseInfo(product, requestModel);

                        _banggoMgt.GetProductSkuBase(product, requestModel);

                        product.ColorList = pgModel.ProductColors;

                        PublishGoodsAndUploadPic(product);
                    }
                    catch(Exception ex)
                    {
                        _log.LogError(Resource.Log_PublishGoodsFailure.StringFormat(pgModel.GoodsSn), ex);
                        continue;
                    }

                    #endregion
                }
                Thread.Sleep(500);
            }
        }

        //taobao.item.sku.delete 删除SKU 
        /// <summary>
        ///     删除单个SKU
        /// </summary>
        /// <param name="numId"></param>
        /// <param name="properties"></param>
        public void DeleteGoodsSku(long numId, string properties)
        {
            _log.LogInfo(Resource.Log_DeleteGoodsSkuing.StringFormat(numId, properties));
            var tContext = InstanceLocator.Current.GetInstance<TopContext>();
            var req = new ItemSkuDeleteRequest {NumIid = numId, Properties = properties};

            ItemSkuDeleteResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                var ex = new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                                  response.SubErrMsg, response.TopForbiddenFields);
                _log.LogError(Resource.Log_DeleteGoodsSkuFailure.StringFormat(numId, properties), ex);
                throw ex;
            }
            _log.LogInfo(Resource.Log_DeleteGoodsSkuSuccess.StringFormat(numId, properties));
        }

        /// <summary>
        /// taobao.item.sku.update 更新SKU信息
        /// </summary>
        /// <returns></returns>
        public Sku UpdateSku(ItemSkuUpdateRequest req)
        { 
            _log.LogInfo(Resource.Log_UpdateSkuing.StringFormat(req.NumIid, req.Properties));
            
            req.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(new StackTrace()));

            var tContext = InstanceLocator.Current.GetInstance<TopContext>();

            var response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                var ex = new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                               response.SubErrMsg, response.TopForbiddenFields);
                _log.LogError(Resource.Log_UpdateSkuFailure.StringFormat(req.NumIid, req.Properties), ex);
                throw ex;
            }

            _log.LogInfo(Resource.Log_UpdateSkuSuccess.StringFormat(req.NumIid, req.Properties));
            
            return response.Sku; 
        }

        //taobao.item.skus.get 根据商品ID列表获取SKU信息 
        /// <summary>
        ///     根据商品ID列表获取SKU信息
        /// </summary>
        /// <param name="numIds">支持多个商品，用“，”号分割</param>
        /// <returns></returns>
        public IEnumerable<Sku> GetSkusByNumId(string numIds)
        {
            _log.LogInfo(Resource.Log_GetSkusing.StringFormat(numIds));
            var tContext = InstanceLocator.Current.GetInstance<TopContext>();

            var req = new ItemSkusGetRequest
                {
                    Fields = "properties_name,sku_id,iid,num_iid,properties,quantity,price,outer_id",
                    NumIids = numIds
                };

            ItemSkusGetResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                var ex = new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                                  response.SubErrMsg, response.TopForbiddenFields);
                _log.LogError(Resource.Log_GetSkusFailure.StringFormat(numIds), ex);
                throw ex;
            }
            _log.LogInfo(Resource.Log_GetSkusSuccess.StringFormat(numIds));

            return response.Skus.OrderBy(f=>f.SkuId).ToList();
        }

        public Item VerifyGoodsExist(string goodsSn)
        {
            goodsSn.ThrowIfNullOrEmpty(
                Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(new StackTrace()));

            var req = new ItemsOnsaleGetRequest {Fields = "num_iid,num,cid,title,outer_id", Q = goodsSn, PageSize = 10};
            List<Item> onSaleGoods = GetOnSaleGoods(req);

            if (onSaleGoods != null && onSaleGoods.Count > 0)
            {
                _log.LogWarning(Resource.Log_GoodsAlreadyExist, goodsSn);
                return onSaleGoods[0];
            }

            return null;
        }

        /// <summary>
        ///     更新和添加销售商品图片
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <param name="properties">销售属性</param>
        /// <param name="imgPath">本地图片路径</param>
        /// <returns></returns>
        public PropImg UploadItemPropimg(long numId, string properties, string imgPath)
        {
            #region validation

            if (numId <= 0 || string.IsNullOrEmpty(properties) || string.IsNullOrEmpty(imgPath))
                throw new Exception((Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(
                    new StackTrace())));

            #endregion

            var fItem = new FileItem(imgPath);

            return UploadItemPropimgInternal(numId, properties, fItem);
        }

        /// <summary>
        ///     更新和添加销售商品图片
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <param name="properties">销售属性</param>
        /// <param name="urlImg">网上的图片地址</param>
        /// <returns></returns>
        public PropImg UploadItemPropimg(long numId, string properties, Uri urlImg)
        {
            int len = urlImg.Segments.Length;
            string fileName = len > 0
                                  ? urlImg.Segments[len - 1]
                                  : "{0}-{1}.jpg".StringFormat(numId.ToString(CultureInfo.InvariantCulture), properties);

            var fItem = new FileItem(fileName, SysUtils.GetImgByte(urlImg.ToString()));
            return UploadItemPropimgInternal(numId, properties, fItem);
        }

        /// <summary>
        ///     taobao.item.update.delisting 商品下架
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <returns></returns>
        public Item GoodsDelisting(long numId)
        {
            _log.LogInfo(Resource.Log_GoodsDelisting, numId);

            var req = new ItemUpdateDelistingRequest {NumIid = numId};
            var tContext = InstanceLocator.Current.GetInstance<TopContext>();
            ItemUpdateDelistingResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                var ex = new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                                  response.SubErrMsg, response.TopForbiddenFields);

                _log.LogError(Resource.Log_GoodsDelistingFailure.StringFormat(numId), ex);
            }

            _log.LogInfo(Resource.Log_GoodsDelistingSuccess, numId);

            return response.Item;
        }

        /// <summary>
        ///     得到单个商品信息
        ///     taobao.item.get
        /// </summary>
        /// <param name="req"></param>
        /// <returns>商品详情</returns>
        public Item GetGoods(ItemGetRequest req)
        {
            req.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(new StackTrace()));

            var tContext = InstanceLocator.Current.GetInstance<TopContext>();

            ItemGetResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
                throw new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                               response.SubErrMsg, response.TopForbiddenFields);

            return response.Item;
        }

        /// <summary>
        ///     通过商品编号得到常用的Item数据
        ///     调用的GetGoods(ItemGetRequest req)
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <returns>商品详情</returns>
        public Item GetGoods(string numId)
        {
            numId.ThrowIfNullOrEmpty(
                Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(new StackTrace()));

            var req = new ItemGetRequest
                {
                    Fields =
                        "num_iid,title,nick,outer_id,price,num,location,post_fee,express_fee,ems_fee,sku,props_name,props,input_pids,input_str,pic_url,property_alias,item_weight,item_size,created,has_showcase,item_img,prop_img,desc",
                    NumIid = numId.ToLong()
                };

            return GetGoods(req);
        }

        /// <summary>
        ///     得到产品列表
        /// </summary>
        /// <param name="numIds">各ID以","号分割</param>
        /// <returns></returns>
        public List<Item> GetGoodsList(string numIds)
        {
            var tContext = InstanceLocator.Current.GetInstance<TopContext>();

            var req = new ItemsListGetRequest {Fields = "num_iid,cid,num,sku,title,price,outer_id", NumIids = numIds};

            ItemsListGetResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                var ex = new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                                  response.SubErrMsg, response.TopForbiddenFields);
                _log.LogError(Resource.Log_GetGoodsListFailure, ex);
                throw ex;
            }

            return response.Items;
        }

        /// <summary>
        ///     获取当前会话用户出售中的商品列表
        ///     taobao.items.onsale.get
        /// </summary>
        /// <param name="req">要查询传入的参数</param>
        public List<Item> GetOnSaleGoods(ItemsOnsaleGetRequest req)
        {
            _log.LogInfo(Resource.Log_GetOnSaleGoodsing.StringFormat(req.Q));
            var tContext = InstanceLocator.Current.GetInstance<TopContext>();

            ItemsOnsaleGetResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                var ex = new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                                  response.SubErrMsg, response.TopForbiddenFields);

                _log.LogError(Resource.Log_GetOnSaleGoodsFailure, ex);

                throw ex;
            }

            _log.LogInfo(Resource.Log_GetOnSaleGoodsSuccess.StringFormat(req.Q));
            return response.Items;
        }

        /// <summary>
        /// 得到当前在售商品，最多个数为199
        /// </summary>
        /// <param name="search">查询条件</param>
        /// <returns></returns>
        public List<Item> GetOnSaleGoods(string search = null)
        {
            //得到当前用户的在售商品列表 
            var req = new ItemsOnsaleGetRequest { Fields = "num_iid,num,cid,title,price,outer_id", PageSize = 199 };
            if (!search.IsNullOrEmpty())
                req.Q = search;

            return GetOnSaleGoods(req); 
        }

        //得到卖家仓库中的商品
        public List<Item> GetInventoryGoods(ItemsInventoryGetRequest req)
        {
            var tContext = InstanceLocator.Current.GetInstance<TopContext>();

            ItemsInventoryGetResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                var ex = new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                                  response.SubErrMsg, response.TopForbiddenFields);

                _log.LogError(Resource.Log_GetInventoryGoodsFailure, ex);

                throw ex;
            }

            return response.Items;
        }

        public Item UpdateGoods(Product product)
        {
            _log.LogInfo(Resource.Log_UpdateGoodsing.StringFormat(product.NumIid, product.OuterId));

            var req = new ItemUpdateRequest();

            Util.CopyModel(product, req);

            var tContext = InstanceLocator.Current.GetInstance<TopContext>();
            ItemUpdateResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                var ex = new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                                  response.SubErrMsg, response.TopForbiddenFields);
                _log.LogError(Resource.Log_UpdateGoodsFailure.StringFormat(product.NumIid, product.OuterId), ex);
                throw ex;
            }

            Item item = response.Item;

            _log.LogInfo(Resource.Log_UpdateGoodsSuccess, product.Title, item.NumIid);

            return item;
        }

        /// <summary>
        ///     更新商品信息包括SKU信息
        /// </summary>
        public void UpdateGoodsInfo(BanggoProduct banggoProduct)
        {
            //1，填充必填项到props
            string itemProps = _catalog.GetItemProps(banggoProduct.Cid.ToString());
            banggoProduct.Props = itemProps; //只先提取必填项

            //2，读取banggo上现在还有那些尺码填充到，BanggoProduct-> BSizeToTSize

            var req = new BanggoRequestModel {GoodsSn = banggoProduct.GoodsSn, Referer = banggoProduct.GoodsUrl};

            banggoProduct.BSizeToTSize = _banggoMgt.GetBSizeToTSize(_banggoMgt.GetGoodsDetialElementData(req));
            //3，SetSkuInfo 
            SetSkuInfo(banggoProduct);
            Thread.Sleep(200);

            UpdateGoods(banggoProduct);
        }

        #endregion

        #region Private Methods

        //更新商品的内部方法
        private void UpdateGoodsInternal(IEnumerable<Item> lstItem, bool isModifyPrice = true)
        {
            //遍历在售商品列表中的商品，通过outerid去查询banggo上的该产品信息
            foreach (Item item in lstItem)
            {
                Thread.Sleep(1000);
                try
                {
                    //通过款号查询如果没有得到产品的URL或得不到库存，就将该产品进行下架。
                    string goodsUrl = _banggoMgt.GetGoodsUrl(item.OuterId);

                    if (goodsUrl.IsNullOrEmpty())
                    {
                        GoodsDelisting(item.NumIid);

                        continue;
                    }

                    //如果邦购上该产品还在售，就获取他的SKU信息。 
                    var banggoProduct = new BanggoProduct(false)
                    {
                        ColorList = _banggoMgt.GetProductColorByOnline(new BanggoRequestModel
                        {
                            GoodsSn = item.OuterId,
                            Referer = goodsUrl
                        })
                    };

                    #region 如果没有强制更新者 判断邦购数据是否以淘宝现在的库存数量一样，如果一样就取消更新

                    if (!SysConst.IsEnforceUpdate)
                    {
                        if (item.Num == banggoProduct.ColorList.Sum(p => p.AvlNumForColor))
                        {
                            _log.LogInfo(Resource.Log_StockEqualNotUpdate.StringFormat(item.NumIid, item.OuterId));
                            continue;
                        }
                    }

                    #endregion

                    #region 删除SKU只保留1个可用SKU

                    DeleteSkuOnlyOne(item);

                    #endregion

                    #region 如果是不修改价格（IsModifyPrice = false），则读取Item的price 填充到MySalePrice 中

                    if (!isModifyPrice)
                    {
                        foreach (var size in banggoProduct.ColorList.SelectMany(color => color.SizeList))
                        {
                            size.MySalePrice = item.Price.ToDouble();
                        }
                    }

                    #endregion
                     
                    //替换原来的产品标题
                    banggoProduct.Title = item.Title.Replace(SysConst.OriginalTitle, SysConst.NewTitle);

                    banggoProduct.GoodsSn = item.OuterId;
                    banggoProduct.GoodsUrl = goodsUrl;
                    banggoProduct.Cid = item.Cid;
                    banggoProduct.NumIid = item.NumIid;
                    banggoProduct.OuterId = item.OuterId;

                    UpdateGoodsAndUploadPic(banggoProduct);
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {
                }
            }
        }

        //删除SKU只保留1个可用SKU
        private void DeleteSkuOnlyOne(Item item)
        {
            if (item.Skus.Count == 0 || item.Skus[0].Properties.IsNullOrEmpty())
            {
                //因为读在售没办法得到SKU所以只有单独取 
                try
                {
                    item.Skus = (List<Sku>)GetSkusByNumId(item.NumIid.ToString(CultureInfo.InvariantCulture));
                    Thread.Sleep(200);
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {
                }
            } 

            //不用排序，默认取最开始那个
            //List<Sku> skus = item.Skus.OrderByDescending(f => f.Quantity).ToList();
            List<Sku> skus = item.Skus;

            if (skus == null)
                return;

             //判断sku的第一个库存是不是为0如果为0者修改该SKU将其设置为1，因为淘宝不允许SKU总数为0
            if (skus.Count > 0)
            {
                var modifySku = skus[0];

                if (modifySku.Quantity == 0)
                {
                     UpdateSku(new ItemSkuUpdateRequest()
                        {
                            NumIid = item.NumIid,
                            Properties = modifySku.Properties,
                            Quantity = 1
                        }); 
                }
            }

            for (int i = 1; i < skus.Count; i++)
            {
                Sku sku = skus[i];

                DeleteGoodsSku(item.NumIid, sku.Properties);

                Thread.Sleep(500);
            }
        }
         
        //更新商品并上传相应的销售图片
        private void UpdateGoodsAndUploadPic(BanggoProduct banggoProduct)
        {
            UpdateGoodsInfo(banggoProduct);

            foreach (ProductColor pColor in banggoProduct.ColorList)
            {
                if (banggoProduct.NumIid != null)
                    UploadItemPropimg(banggoProduct.NumIid.Value, pColor.MapProps, new Uri(pColor.ImgUrl));

                Thread.Sleep(500);
            }
        }

        //发布商品并上传图片
        private Item PublishGoodsAndUploadPic(BanggoProduct banggoProduct)
        {
            StuffProductInfo(banggoProduct);

            Item item = PublishGoods(banggoProduct);

            foreach (ProductColor pColor in banggoProduct.ColorList)
            {
                UploadItemPropimg(item.NumIid, pColor.MapProps, new Uri(pColor.ImgUrl));
                Thread.Sleep(500);
            }
            return item;
        }

        //重EXCEL中读取要发布的数据用于发布或更新商品SKU
        private static IEnumerable<PublishGoods> GetPublishGoodsFromExcel(string filePath)
        {
            filePath.ThrowIfNullOrEmpty(
                Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(new StackTrace()));

            DataTable dtSource = ExcelHelper.GetExcelData(filePath, Resource.SysConfig_Sku);

            dtSource.ThrowIfNull(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(new StackTrace()));
            return (from DataRow dr in dtSource.Rows select new PublishGoods(dr)).ToList();
        }

        //填充产品信息，将banggo的数据填充进相应的请求模型中
        private void StuffProductInfo(BanggoProduct bProduct)
        {
            _log.LogInfo(Resource.Log_StuffProductInfoing.StringFormat(bProduct.GoodsSn));
            bProduct.OuterId = bProduct.GoodsSn;

            bProduct.Cid = _catalog.GetCid(bProduct.Category, bProduct.ParentCatalog).ToLong();

            bProduct.Image = new FileItem(bProduct.GoodsSn + ".jpg", SysUtils.GetImgByte(bProduct.ThumbUrl));

            var tContext = InstanceLocator.Current.GetInstance<TopContext>();

            bProduct.SellerCids = _shop.GetSellerCids(tContext.UserNick,
                                                      "{0} - {1}".StringFormat(bProduct.Brand, bProduct.Category),
                                                      bProduct.ParentCatalog);

            //得到运费模版
            string deliveryTemplateId = _delivery.GetDeliveryTemplateId(Resource.SysConfig_DeliveryTemplateName);

            if (deliveryTemplateId == null)
            {
                SetDeliveryFee(bProduct);
            }
            else
            {
                bProduct.PostageId = deliveryTemplateId.ToLong();
                bProduct.ItemWeight = Resource.SysConfig_ItemWeight;
            }

            string itemProps = _catalog.GetItemProps(bProduct.Cid.ToString());
            bProduct.Props = itemProps; //只先提取必填项

            SetOptionalProps(bProduct);

            SetSkuInfo(bProduct);

            _log.LogInfo(Resource.Log_StuffProductInfoSuccess.StringFormat(bProduct.GoodsSn));
        }

        //包括设置品牌、货号
        private void SetOptionalProps(BanggoProduct bProduct)
        {
            //取消在Props中增加品牌，因为在更新SKU没办法得到Brand
            /* var rm = new ResourceManager(typeof(Resource).FullName,
                                         typeof(Resource).Assembly);
            string brandProp = rm.GetString("SysConfig_{0}_BrandProp".StringFormat(bProduct.Brand));

            if (!brandProp.IsNullOrEmpty())
            {
                bProduct.Props += brandProp;
            }*/

            bProduct.InputPids = "{0},{1}".StringFormat(Resource.SysConfig_ProductCodeProp, "20000");
            bProduct.InputStr = "{0},{1}".StringFormat(bProduct.GoodsSn, bProduct.Brand);
        }

        private PropImg UploadItemPropimgInternal(long numId, string properties, FileItem fItem)
        {
            _log.LogInfo(Resource.Log_PublishSaleImging, numId);

            var req = new ItemPropimgUploadRequest {NumIid = numId, Image = fItem, Properties = properties};

            var tContext = InstanceLocator.Current.GetInstance<TopContext>();
            ItemPropimgUploadResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                var ex = new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                                  response.SubErrMsg, response.TopForbiddenFields);
                _log.LogError(Resource.Log_PublishSaleImgFailure, ex);
                throw ex;
            }

            _log.LogInfo(Resource.Log_PublishSaleImgSuccess, response.PropImg.Id, response.PropImg.Url);
            return response.PropImg;
        }

        private void SetSkuInfo(BanggoProduct bProduct)
        {
            #region var

            var sbSku = new StringBuilder();
            var sbSkuToProps = new StringBuilder();
            var lstSkuAlias = new List<string>();
            var lstSkuQuantities = new List<string>();
            var lstSkuPrices = new List<string>();
            var sbSkuOuterIds = new StringBuilder();

            int num = 0; //商品数量

            #endregion

            List<string> propColors = _catalog.GetSaleProp(true, bProduct.Cid.ToString());

            List<string> propSize = _catalog.GetSaleProp(false, bProduct.Cid.ToString());
            int colorCount = bProduct.ColorList.Count;

            //清空现有的色码与淘宝的属性映射
            _dicColorMap.Clear();

            List<string> keys = bProduct.BSizeToTSize.Keys.ToList();
            bProduct.BSizeToTSize.Clear();
            for (int k = 0; k < keys.Count; k++)
            {
                bProduct.BSizeToTSize.Add(keys[k], propSize[k]);
            }

            for (int i = 0; i < colorCount; i++)
            {
                string pColor = propColors[i];

                ProductColor bColor = bProduct.ColorList[i];

                bColor.MapProps = pColor;

                _dicColorMap.Add(bColor.ColorCode, pColor);

                num += bColor.AvlNumForColor;

                sbSkuToProps.AppendFormat("{0}{1}", pColor, CommomConst.SEMI);
                lstSkuAlias.Add("{0}{1}{2}({3}色){4}".StringFormat(pColor, CommomConst.COLON, bColor.Title,
                                                                  bColor.ColorCode,
                                                                  CommomConst.SEMI));

                //读取尺码
                int sizeCount = bColor.SizeList.Count;
                for (int j = 0; j < sizeCount; j++)
                {
                    #region 构造尺码

                    ProductSize bSize = bColor.SizeList[j];
                    string pSize;
                    if (!bProduct.BSizeToTSize.TryGetValue(bSize.Alias, out pSize))
                        continue;

                    sbSku.AppendFormat("{0}{1}", pColor, CommomConst.SEMI);
                    sbSku.AppendFormat("{0}{1}", pSize, CommomConst.COMMA);

                    if (sbSkuOuterIds.Length > 0)
                    {
                        sbSkuOuterIds.AppendFormat(",{0}", bProduct.GoodsSn);
                    }
                    else
                    {
                        sbSkuOuterIds.Append(bProduct.GoodsSn);
                    }

                    sbSkuToProps.AppendFormat("{0}{1}", pSize, CommomConst.SEMI);

                    // 不用为每个尺码都加别名
                    if (lstSkuAlias.Find(a => a.Contains(pSize)) == null)
                        lstSkuAlias.Add("{0}{1}{2}({3}){4}".StringFormat(pSize, CommomConst.COLON, bSize.Alias,
                                                                         bSize.SizeCode, CommomConst.SEMI));

                    lstSkuQuantities.Add(bSize.AvlNum.ToString(CultureInfo.InvariantCulture));
                    //num += bSize.AvlNum; 通过在颜色中去读取有效库存

                    if (bProduct.Price.IsNullOrEmpty())
                        bProduct.Price = bSize.MySalePrice.ToString(CultureInfo.InvariantCulture);

                    lstSkuPrices.Add(bSize.MySalePrice.ToString(CultureInfo.InvariantCulture));

                    #endregion
                }
            }

            bProduct.Num = num;

            bProduct.Props += sbSkuToProps.ToString();
            bProduct.PropertyAlias = lstSkuAlias.ToColumnString("");
            bProduct.SkuProperties = sbSku.ToString().TrimEnd(',');

            bProduct.SkuQuantities = lstSkuQuantities.ToColumnString();
            bProduct.SkuPrices = lstSkuPrices.ToColumnString();
            bProduct.SkuOuterIds = sbSkuOuterIds.ToString();
        }

        private static void SetDeliveryFee(Product tProduct)
        {
            tProduct.PostFee = Resource.SysConfig_PostFee;
            tProduct.ExpressFee = Resource.SysConfig_ExpressFee;
            tProduct.EmsFee = Resource.SysConfig_EmsFee;
        }

        #endregion
    }
}