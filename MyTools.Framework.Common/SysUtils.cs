/*
 *名称：SysUtils
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-21 11:27:44
 *修改时间：
 *备注：
 */

using System;
using System.Data;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using RestSharp;

namespace MyTools.Framework.Common
{
    public static class SysUtils
    {
        public static byte[] GetImgByte(string imgUrl)
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
        ///     在自定义的集合中找是否有相关配置
        /// </summary>
        /// <param name="parentCatalog">父目录，如 男装</param>
        /// <param name="childCatalog">子目录，如T恤</param>
        /// <returns>没有找到者返回 null </returns>
        public static string GetCustomCidMap(string parentCatalog, string childCatalog)
        {
            string result = null;
            switch (parentCatalog)
            {
                case "男装":
                    if (!SysConst.ManCatalogBanggoToTaobaoCid.TryGetValue(childCatalog, out result))
                    {
                        throw new Exception("ManCatalogBanggoToTaobaoCid中没有配置{0}->{1}，请配置!".StringFormat(parentCatalog,
                                                                                                         childCatalog));
                    }
                    break;
                case "女装":
                    if (!SysConst.WomenCatalogBanggoToTaobaoCid.TryGetValue(childCatalog, out result))
                    {
                        throw new Exception("WomenCatalogBanggoToTaobaoCid中没有配置{0}->{1}，请配置!".StringFormat(
                            parentCatalog, childCatalog));
                    }
                    break;
                case "男童":
                case "女童":
                    if (!SysConst.ChildCatalogBanggoToTaobaoCid.TryGetValue(childCatalog, out result))
                    {
                        throw new Exception("ChildCatalogBanggoToTaobaoCid中没有配置{0}->{1}，请配置!".StringFormat(
                            parentCatalog, childCatalog));
                    }
                    break;
            }
            return result;
        }
         
        // <summary>
        /// 在自定义类别集合中找是否有相关配置
        /// <param name="parentCatalog">父目录，如 男童</param>
        /// <returns>没有找到者返回 null </returns>
        public static string GetCustomCategoryMap(string parentCatalog)
        {
            string result;
            if (!SysConst.CategoryBanggoToTaobaoMap.TryGetValue(parentCatalog, out result))
            {
                throw new Exception("CategoryBanggoToTaobaoMap中没有配置{0}，请配置!".StringFormat(parentCatalog));
            }

            return result;
        }

        /// <summary>
        /// 通过调用RestRequest Get方式得到网页中的内容
        /// </summary>
        /// <param name="searchUrl"></param>
        /// <returns></returns>
        public static HtmlDocument GetHtmlDocumentByHttpGet(string searchUrl)
        {
            var restClient = new RestClient(searchUrl);
            restClient.CookieContainer = new System.Net.CookieContainer();
            var request = new RestRequest(Method.GET); 
            IRestResponse response = restClient.Execute(request);
             
            if (response.ErrorException != null)
                throw response.ErrorException;
            
            var doc = new HtmlDocument();

            doc.LoadHtml(Encoding.Default.GetString(response.RawBytes));

            return doc;
        }

        /// <summary>
        /// 通过调用RestRequest Get方式得到网页中的内容
        /// </summary>
        /// <param name="searchUrl"></param>
        /// <returns></returns>
        public static string GetHtmlByHttpGet(string searchUrl)
        {
            var restClient = new RestClient(searchUrl);
            restClient.CookieContainer = new System.Net.CookieContainer();
            var request = new RestRequest(Method.GET);
            IRestResponse response = restClient.Execute(request);

            if (response.ErrorException != null)
                throw response.ErrorException;
             
            return  Encoding.Default.GetString(response.RawBytes);
        }


        //检查该EXCEL是否有该表存在
        /// <summary>
        /// 检查该EXCEL是否有该表存在
        /// </summary>
        /// <param name="excel"></param>
        /// <param name="shellName"></param>
        /// <returns></returns>
        public static bool CheckTableExsit(ExcelHelper excel, string shellName)
        {
            DataTable dtSchema = excel.GetSchema();

            return (from DataRow dr in dtSchema.Rows select dr["TABLE_NAME"].ToString()).Select(name => name == shellName).FirstOrDefault();
        }
    }
}