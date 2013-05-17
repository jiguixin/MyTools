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
using System.IO;
using System.Linq;
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

        private IItemCats itemCats = InstanceLocator.Current.GetInstance<IItemCats>();

        private IShopApi shopApi = InstanceLocator.Current.GetInstance<IShopApi>();


        public List<SellerCat> SellercatsList;

        public GoodsApi()
        {
            SellercatsList = shopApi.GetSellercatsList("mbgou");
        }

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

            throw new NotImplementedException();
        }

        private void MapBanggoToTaobaoProduct(BanggoProduct bProduct, Product tProduct)
        {

            tProduct.Title = bProduct.ProductTitle; 
            tProduct.OuterId = bProduct.GoodsSn;

            long cid = itemCats.GetCid(bProduct.Category, bProduct.ParentCatalog).ToLong();
            tProduct.Cid = cid;
            tProduct.Desc = bProduct.Desc;
             
            tProduct.Image = new FileItem(bProduct.GoodsSn + ".jpg", GetImgByte(bProduct.ThumbUrl));

            var propsList = itemCats.GetPropsByCid(cid);

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

            //todo 实现sku的map
           // tProduct

  


            tProduct.Props = "";


             

            throw new NotImplementedException();
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