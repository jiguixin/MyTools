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
using System.Security.Policy;
using System.Text;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Utility;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.DomainModule.ExceptionDef;
using MyTools.TaoBao.Interface;
using RestSharp;
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
        public List<SellerCat> SellercatsList;

        ILogger log = InstanceLocator.Current.GetInstance<ILoggerFactory>().Create();

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
            var req = new ItemAddRequest();

            Util.CopyModel(product, req);

            var tContext = InstanceLocator.Current.GetInstance<TopContext>();
            ItemAddResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
            {
                throw new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                               response.SubErrMsg, response.TopForbiddenFields);
            }

            var item = response.Item;

            log.LogInfo(Resource.Log_PublishGoodsSuccess, product.Title, item.NumIid);
             
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

            BanggoProduct banggoProduct =
                _banggoMgt.GetGoodsInfo(new BanggoRequestModel {GoodsSn = goodsSn, Referer = banggoProductUrl});

            var taobaoProduct = new Product();

            MapBanggoToTaobaoProduct(banggoProduct, taobaoProduct);
           
            var item = PublishGoods(taobaoProduct);

            foreach (var pColor in banggoProduct.ColorList)
            {
               var img = UploadItemPropimg(item.NumIid, pColor.MapProps, new Uri(pColor.ImgUrl));

                if (img != null && img.Id > 0)
                    log.LogInfo(Resource.Log_PublishSaleImgSuccess, img.Id, img.Url);
                else
                    log.LogInfo(Resource.Log_PublishSaleImgFailure); 
            }
             
            return item;
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
                throw new Exception(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                                  new StackTrace()));

            #endregion

            var fItem = new FileItem(imgPath);

            return UploadItemPropimgInternal(numId, properties, fItem);
        }

        /// <summary>
        /// 更新和添加销售商品图片
        /// </summary>
        /// <param name="numId">商品编号</param>
        /// <param name="properties">销售属性</param>
        /// <param name="urlImg">网上的图片地址</param>
        /// <returns></returns>
        public PropImg UploadItemPropimg(long numId, string properties, Uri urlImg)
        {
              int len = urlImg.Segments.Length;
            var fileName = len > 0
                               ? urlImg.Segments[len - 1]
                               : string.Format("{0}-{1}.jpg", numId.ToString(), properties);

            var fItem = new FileItem(fileName, GetImgByte(urlImg.ToString()));
           ;
           return UploadItemPropimgInternal(numId, properties, fItem);
        }


        private void MapBanggoToTaobaoProduct(BanggoProduct bProduct, Product tProduct)
        {
            tProduct.Title = bProduct.ProductTitle;
            tProduct.OuterId = bProduct.GoodsSn;

            tProduct.Cid = _catalog.GetCid(bProduct.Category, bProduct.ParentCatalog).ToLong();

            tProduct.Desc = bProduct.Desc;

            tProduct.Image = new FileItem(bProduct.GoodsSn + ".jpg", GetImgByte(bProduct.ThumbUrl));

            var tContext = InstanceLocator.Current.GetInstance<TopContext>();

            tProduct.SellerCids = _shop.GetSellerCids(tContext.UserNick,
                                                     string.Format("{0} - {1}", bProduct.BrandCode, bProduct.Category),
                                                     bProduct.ParentCatalog);

            //得到运费模版
            string deliveryTemplateId = _delivery.GetDeliveryTemplateId(Resource.SysConfig_DeliveryTemplateName);

            if (deliveryTemplateId == null)
            {
                SetDeliveryFee(tProduct);
            }
            else
            {
                tProduct.PostageId = deliveryTemplateId.ToLong();
                tProduct.ItemWeight = Resource.SysConfig_ItemWeight;
            }

            string itemProps = _catalog.GetItemProps(tProduct.Cid.ToString());
            tProduct.Props = itemProps; //只先提取必填项

            SetOptionalProps(bProduct, tProduct);

            SetSkuInfo(bProduct, tProduct);

            #region 暂时先不用

            /*

            tProduct.Title = bProduct.ProductTitle; 
            tProduct.OuterId = bProduct.GoodsSn;

            long cid = itemCatsApi.GetCid(bProduct.Category, bProduct.ParentCatalog).ToLong();
            tProduct.Cid = cid;
            tProduct.Desc = bProduct.Desc;
             
            tProduct.Image = new FileItem(bProduct.GoodsSn + ".jpg", GetImgByte(bProduct.ThumbUrl));

            var propsList = itemCatsApi.GetPropsByCid(cid);

            //获得该目录下必备的参数
            var mustItemProps = propsList.FindAll(p => p.Must);

            StringBuilder sbProps = new StringBuilder();

            foreach (ItemProp mustItemProp in mustItemProps)
            {
                sbProps.AppendFormat("{0}:{1};", mustItemProp.Pid, mustItemProp.PropValues[0].Vid);
            }

            var parentSellCat = from cat in SellercatsList where cat.Name.ToUpper().Contains(bProduct.BrandCode.ToUpper()) & cat.Name.Contains(bProduct.Category) select cat;

            parentSellCat.ThrowIfNullOrEmpty(string.Format(Resource.ExceptionTemplate_MethedParameterIsNullorEmpty,
                                              new StackTrace()));

           var  childSellCat = from cat in parentSellCat where cat.Name.Contains(bProduct.Catalog) select cat.Cid;

            if (childSellCat.IsNullOrEmpty()) 
            {
                childSellCat = from cat in parentSellCat where cat.Name.Contains(bProduct.ParentCatalog) select cat.Cid;
            }

            tProduct.SellerCids = parentSellCat.ToColumnString();

            //得到运费模版
            if (DeliveryTemplateList.Count > 0)
            {  
                var dTmp = DeliveryTemplateList.Find(f => f.Name == Resource.SysConfig_DeliveryTemplateName);

                if (dTmp == null)
                {
                    SetDeliveryFee(tProduct);
                }
                else
                {
                    tProduct.PostageId = dTmp.TemplateId;
                    tProduct.ItemWeight = Resource.SysConfig_ItemWeight;    
                }
            }
            else
            {
                SetDeliveryFee(tProduct);
            }
             

           // tProduct

  


            tProduct.Props = "";

*/

            #endregion
        }

        //包括设置品牌、货号
        private void SetOptionalProps(BanggoProduct bProduct, Product tProduct)
        {
            var rm = new ResourceManager(typeof (Resource).FullName,
                                         typeof (Resource).Assembly);
            string brandProp = rm.GetString(string.Format("SysConfig_{0}_BrandProp", bProduct.BrandCode));

            if (!brandProp.IsNullOrEmpty())
            {
                tProduct.Props += brandProp;
            }


            tProduct.InputPids = Resource.SysConfig_ProductCodeProp;
            tProduct.InputStr = bProduct.GoodsSn;
        }

        #endregion

        #region Private Methods


        private PropImg UploadItemPropimgInternal(long numId, string properties, FileItem fItem)
        {
            var req = new ItemPropimgUploadRequest { NumIid = numId };

            req.Image = fItem;
            req.Properties = properties;
            var tContext = InstanceLocator.Current.GetInstance<TopContext>();
            ItemPropimgUploadResponse response = _client.Execute(req, tContext.SessionKey);

            if (response.IsError)
                throw new TopResponseException(response.ErrCode, response.ErrMsg, response.SubErrCode,
                                               response.SubErrMsg, response.TopForbiddenFields);

            return response.PropImg;
        }
         
        private void SetSkuInfo(BanggoProduct bProduct, Product tProduct)
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

            List<string> propColors = _catalog.GetSaleProp(true, tProduct.Cid.ToString());

            List<string> propSize = _catalog.GetSaleProp(false, tProduct.Cid.ToString());
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
                lstSkuAlias.Add(string.Format("{0}{1}{2}({3}色){4}", pColor, CommomConst.COLON, bColor.Title,
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
                        lstSkuAlias.Add(string.Format("{0}{1}{2}({3}){4}", pSize, CommomConst.COLON, bSize.Alias,
                                                      bSize.SizeCode, CommomConst.SEMI));
                     
                    lstSkuQuantities.Add(bSize.AvlNum.ToString(CultureInfo.InvariantCulture));
                    num += bSize.AvlNum;

                    if (tProduct.Price.IsNullOrEmpty())
                        tProduct.Price = bSize.Price.ToString(CultureInfo.InvariantCulture);

                    lstSkuPrices.Add(bSize.Price.ToString(CultureInfo.InvariantCulture));
                }
            }

            tProduct.Num = num;

            tProduct.Props += sbSkuToProps.ToString();
            tProduct.PropertyAlias = lstSkuAlias.ToColumnString("");
            tProduct.SkuProperties = sbSku.ToString().TrimEnd(',');

            tProduct.SkuQuantities = lstSkuQuantities.ToColumnString();
            tProduct.SkuPrices = lstSkuPrices.ToColumnString();
            tProduct.SkuOuterIds = sbSkuOuterIds.ToString();
        }

        private static void SetDeliveryFee(Product tProduct)
        {
            tProduct.PostFee = Resource.SysConfig_PostFee;
            tProduct.ExpressFee = Resource.SysConfig_ExpressFee;
            tProduct.EmsFee = Resource.SysConfig_EmsFee;
        }

        private byte[] GetImgByte(string imgUrl)
        {
            var restClient = new RestClient(imgUrl);
            var request = new RestRequest(Method.GET);
            IRestResponse response = restClient.Execute(request);

            /*
                        IRestResponse response = restClient.ExecuteAsync(
                    request,
                    Response =>
                    {
                        if (Response != null)
                        {
                            byte[] imageBytes = Response.RawBytes;
                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = new MemoryStream(imageBytes);
                            bitmapImage.CreateOptions = BitmapCreateOptions.None;
                            bitmapImage.CacheOption = BitmapCacheOption.Default;
                            bitmapImage.EndInit();

                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            Guid photoID = System.Guid.NewGuid();
                            String photolocation = String.Format(@"c:\temp\{0}.jpg", Guid.NewGuid().ToString());
                            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                            using (var filestream = new FileStream(photolocation, FileMode.Create))
                            encoder.Save(filestream);

                            this.Dispatcher.Invoke((Action)(() => { img.Source = bitmapImage; }));
                            ;
                        }
                    });*/

            return response.RawBytes;
        }

        #endregion
    }
}