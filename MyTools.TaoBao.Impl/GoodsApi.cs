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
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Logging;
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
        private readonly IShop _shop = InstanceLocator.Current.GetInstance<IShop>(Resource.SysConfig_GetDataWay);

        private readonly ILogger _log = InstanceLocator.Current.GetInstance<ILoggerFactory>().Create();
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
            var tContext = InstanceLocator.Current.GetInstance<TopContext>();
            ItemAddResponse response = _client.Execute(product, tContext.SessionKey);

            if (response.IsError)
            {
                throw new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                               response.SubErrMsg, response.TopForbiddenFields);
            }

            Item item = response.Item;

            _log.LogInfo(Resource.Log_PublishGoodsSuccess, product.Title, item.NumIid);

            return item;
        }

        /// <summary>
        ///     从banggo上获取数据发布到淘宝
        /// </summary>
        /// <param name="banggoProductUrl"></param>
        /// <returns></returns>
        public Item PublishGoodsForBanggoToTaobao(string banggoProductUrl)
        {
            string goodsSn = _banggoMgt.ResolveProductUrl(banggoProductUrl);

            goodsSn.ThrowIfNullOrEmpty(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(new StackTrace()));
            
            if (VerifyGoodsExist(goodsSn))
            {
                _log.LogWarning(Resource.Log_GoodsAlreadyExist.StringFormat(" PublishGoodsForBanggoToTaobao"));
                return null; 
            }

            BanggoProduct banggoProduct =
                _banggoMgt.GetGoodsInfo(new BanggoRequestModel {GoodsSn = goodsSn, Referer = banggoProductUrl});
              
            StuffProductInfo(banggoProduct);

            Item item = PublishGoods(banggoProduct);

            foreach (ProductColor pColor in banggoProduct.ColorList)
            {
                PropImg img = UploadItemPropimg(item.NumIid, pColor.MapProps, new Uri(pColor.ImgUrl));

                if (img != null && img.Id > 0)
                    _log.LogInfo(Resource.Log_PublishSaleImgSuccess, img.Id, img.Url);
                else
                    _log.LogInfo(Resource.Log_PublishSaleImgFailure);
            }

            return item;
        }

        public bool VerifyGoodsExist(string goodsSn)
        {
            var req = new ItemsOnsaleGetRequest();
            req.Fields = "num_iid,title";
            req.Q = goodsSn;
            req.PageSize = 10;
            var onSaleGoods = GetOnSaleGoods(req);

            if (onSaleGoods != null && onSaleGoods.Count > 0)
            {
                return true;
            }
            return false;
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
        /// 得到单个商品信息
        /// taobao.item.get
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
        /// 通过商品编号得到常用的Item数据
        /// 调用的GetGoods(ItemGetRequest req)
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <returns>商品详情</returns>
        public Item GetGoods(string numId)
        {
            numId.ThrowIfNullOrEmpty(
                Resource.ExceptionTemplate_MethedParameterIsNullorEmpty.StringFormat(new StackTrace()));

            var req = new ItemGetRequest();
            req.Fields = "num_iid,title,nick,outer_id,price,num,location,post_fee,express_fee,ems_fee,sku,props_name,props,input_pids,input_str,pic_url,property_alias,item_weight,item_size,created,has_showcase,item_img,prop_img,desc";

            req.NumIid = numId.ToLong();

            return GetGoods(req);
        }

        /// <summary>
        /// 获取当前会话用户出售中的商品列表 
        /// taobao.items.onsale.get 
        /// </summary> 
        /// <param name="req">要查询传入的参数</param>
        public List<Item> GetOnSaleGoods(ItemsOnsaleGetRequest req)
        {
            var tContext = InstanceLocator.Current.GetInstance<TopContext>();

            ItemsOnsaleGetResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                throw new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                               response.SubErrMsg, response.TopForbiddenFields);
            }

            return response.Items; 
        }

        //得到卖家仓库中的商品
        public List<Item> GetInventoryGoods(ItemsInventoryGetRequest req)
        {
            var tContext = InstanceLocator.Current.GetInstance<TopContext>();

            ItemsInventoryGetResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                throw new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                               response.SubErrMsg, response.TopForbiddenFields);
            }

            return response.Items; 
               
        }

        #endregion

        #region Private Methods


        //填充产品信息，将banggo的数据填充进相应的请求模型中
        private void StuffProductInfo(BanggoProduct bProduct)
        {
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
        }

        //包括设置品牌、货号
        private void SetOptionalProps(BanggoProduct bProduct)
        {
            var rm = new ResourceManager(typeof(Resource).FullName,
                                         typeof(Resource).Assembly);
            string brandProp = rm.GetString("SysConfig_{0}_BrandProp".StringFormat(bProduct.Brand));

            if (!brandProp.IsNullOrEmpty())
            {
                bProduct.Props += brandProp;
            }


            bProduct.InputPids = Resource.SysConfig_ProductCodeProp;
            bProduct.InputStr = bProduct.GoodsSn;
        }


        private PropImg UploadItemPropimgInternal(long numId, string properties, FileItem fItem)
        {
            var req = new ItemPropimgUploadRequest {NumIid = numId, Image = fItem, Properties = properties};

            var tContext = InstanceLocator.Current.GetInstance<TopContext>();
            ItemPropimgUploadResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
                throw new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                               response.SubErrMsg, response.TopForbiddenFields);

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

                sbSkuToProps.AppendFormat("{0}{1}", pColor, CommomConst.SEMI);
                lstSkuAlias.Add("{0}{1}{2}({3}色){4}".StringFormat(pColor, CommomConst.COLON, bColor.Title,
                                              bColor.ColorCode,
                                              CommomConst.SEMI));

                //读取尺码
                int sizeCount = bColor.SizeList.Count;
                for (int j = 0; j < sizeCount; j++)
                {
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
                    num += bSize.AvlNum;

                    if (bProduct.Price.IsNullOrEmpty())
                        bProduct.Price = bSize.Price.ToString(CultureInfo.InvariantCulture);

                    lstSkuPrices.Add(bSize.Price.ToString(CultureInfo.InvariantCulture));
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