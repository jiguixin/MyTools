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
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Utility;
using MyTools.TaoBao.DomainModule.ExceptionDef;
using MyTools.TaoBao.Interface;
using RestSharp;
using Top.Api;
using Top.Api.Domain;
using Top.Api.Request;
using Top.Api.Response;
using Top.Api.Util;
using MyTools.TaoBao.DomainModule;
using Product = MyTools.TaoBao.DomainModule.Product;
using Infrastructure.Crosscutting.Declaration;

namespace MyTools.TaoBao.Impl
{
    public class GoodsApi:IGoodsApi
    {

        #region Members

        private ITopClient client = InstanceLocator.Current.GetInstance<ITopClient>();
        private IBanggoMgt banggoMgt = InstanceLocator.Current.GetInstance<IBanggoMgt>();

        private IItemCatsApi itemCatsApi = InstanceLocator.Current.GetInstance<IItemCatsApi>();

        private ICatalog catalog = InstanceLocator.Current.GetInstance<ICatalog>(Resource.SysConfig_GetDataWay);


        private IShopApi shopApi = InstanceLocator.Current.GetInstance<IShopApi>();

        private IShop shop = InstanceLocator.Current.GetInstance<IShop>(Resource.SysConfig_GetDataWay);

        private IDelivery delivery = InstanceLocator.Current.GetInstance<IDelivery>(Resource.SysConfig_GetDataWay);

        public List<SellerCat> SellercatsList;

        private Dictionary<string, string> dicColorMap = new Dictionary<string, string>();

        #endregion


        #region Constructor


        public GoodsApi()
        {
            //TopContext tContext = InstanceLocator.Current.GetInstance<TopContext>();

            //SellercatsList = shopApi.GetSellercatsList(tContext.UserNick);


        }


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
            {
                throw new TopResponseException(response.ErrCode,response.ErrMsg,response.SubErrCode,response.SubErrMsg,response.TopForbiddenFields);
            }
               
            return response.Item;
        }

        /// <summary>
        /// 从banggo上获取数据发布到淘宝
        /// </summary>
        /// <param name="banggoProductUrl"></param>
        /// <returns></returns>
        public Item PublishGoodsForBanggoToTaobao(string banggoProductUrl)
        {
            var goodsSn = banggoMgt.ResolveProductUrl(banggoProductUrl);
            
            var banggoProduct = banggoMgt.GetGoodsInfo(new BanggoRequestModel() {GoodsSn = goodsSn, Referer = banggoProductUrl});

            var taobaoProduct = new Product();

            MapBanggoToTaobaoProduct(banggoProduct, taobaoProduct);

            return PublishGoods(taobaoProduct);
        }

        private void MapBanggoToTaobaoProduct(BanggoProduct bProduct, Product tProduct)
        {
            tProduct.Title = bProduct.ProductTitle;
            tProduct.OuterId = bProduct.GoodsSn;
              
            tProduct.Cid = catalog.GetCid(bProduct.Category,bProduct.ParentCatalog).ToLong();

            tProduct.Desc = bProduct.Desc;

            tProduct.Image = new FileItem(bProduct.GoodsSn + ".jpg", GetImgByte(bProduct.ThumbUrl));

            TopContext tContext = InstanceLocator.Current.GetInstance<TopContext>();

            tProduct.SellerCids = shop.GetSellerCids(tContext.UserNick,string.Format("{0} - {1}",bProduct.BrandCode,bProduct.Category),bProduct.ParentCatalog);

            //得到运费模版
            string deliveryTemplateId = delivery.GetDeliveryTemplateId(Resource.SysConfig_DeliveryTemplateName);

            if (deliveryTemplateId == null)
            {
                SetDeliveryFee(tProduct);
            }
            else
            {
                tProduct.PostageId = deliveryTemplateId.ToLong();
                tProduct.ItemWeight = Resource.SysConfig_ItemWeight;
            }
              
            string itemProps = catalog.GetItemProps(tProduct.Cid.ToString());
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
             

            //todo 实现sku的map
           // tProduct

  


            tProduct.Props = "";

*/

            #endregion
             
        }

        //包括设置品牌、货号
        private void SetOptionalProps(BanggoProduct bProduct, Product tProduct)
        { 
            ResourceManager rm = new ResourceManager(typeof(Resource).FullName,
                             typeof(Resource).Assembly);
            var brandProp = rm.GetString(string.Format("SysConfig_{0}_BrandProp",bProduct.BrandCode));

            if (!brandProp.IsNullOrEmpty())
            {
                tProduct.Props += brandProp;
            }


            tProduct.InputPids = Resource.SysConfig_ProductCodeProp;
            tProduct.InputStr = bProduct.GoodsSn;

             
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


        private void SetSkuInfo(BanggoProduct bProduct, Product tProduct)
        {
            var sbSku = new StringBuilder();
            var sbSkuToProps = new StringBuilder();
            var sbSkuAlias = new StringBuilder();
            var lstSkuQuantities = new List<string>();
            var lstSkuPrices = new List<string>();
            var sbSkuOuterIds = new StringBuilder();

            var propColors = catalog.GetSkuProps("颜色", tProduct.Cid.ToString());

            var propSize = catalog.GetSkuProps("尺码", tProduct.Cid.ToString());
            int colorCount = bProduct.ColorList.Count;

            //清空现有的色码与淘宝的属性映射
            dicColorMap.Clear();

            int num = 0;

            for (int i = 0; i < colorCount; i++)
            {
                var pColor = propColors[i];

                var bColor = bProduct.ColorList[i];

                dicColorMap.Add(bColor.ColorCode.ToString(), pColor);

                sbSkuToProps.AppendFormat("{0}{1}", pColor, CommomConst.SEMI);
                sbSkuAlias.AppendFormat("{0}{1}{2}({3}色){4}", pColor, CommomConst.COLON, bColor.Title, bColor.ColorCode,
                                        CommomConst.SEMI);
                
                //读取尺码
                int sizeCount = bColor.SizeList.Count;
                for (int j = 0; j < sizeCount; j++)
                {
                    var pSize = propSize[j];

                    var bSize = bColor.SizeList[j];

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

                    //todo 此处有bug 不用为每个尺码都加别名
                    sbSkuAlias.AppendFormat("{0}{1}{2}({3}){4}", pSize, CommomConst.COLON, bSize.Alias, bSize.SizeCode,
                                            CommomConst.SEMI);

                    lstSkuQuantities.Add(bSize.AvlNum.ToString(CultureInfo.InvariantCulture));
                    num += bSize.AvlNum;

                    if (tProduct.Price.IsNullOrEmpty())
                        tProduct.Price = bSize.Price.ToString();

                    lstSkuPrices.Add(bSize.Price.ToString(CultureInfo.InvariantCulture));
                }
            }

            tProduct.Num = num;

            tProduct.Props += sbSkuToProps.ToString();
            tProduct.PropertyAlias = sbSkuAlias.ToString();
            tProduct.SkuProperties = sbSku.ToString().TrimEnd(','); 

            //tProduct.Props =
            //    "18066474:145656297;20511:105255;2000:29504;1627207:3232483;20509:28383;20509:28381;1627207:3232484;20509:28383;";
            //tProduct.PropertyAlias =
            //    "1627207:3232483:黑色(99色);20509:28383:155/80A(S)(21042);20509:28381:160/84A(M)(21044);1627207:3232484:灰蓝(45色);20509:28383:155/80A(S)(21042)";

            //tProduct.SkuProperties =
            //    "1627207:3232483;20509:28383,1627207:3232483;20509:28381,1627207:3232484;20509:28383";

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