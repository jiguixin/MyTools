﻿/*
 *名称：CommonTest
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-11 06:17:56
 *修改时间：
 *备注：
 */

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using Infrastructure.Crosscutting.Utility;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using MyTools.TaoBao.DomainModule;
using NUnit.Framework;
using RestSharp;
using Top.Api.Request;
using Product = MyTools.TaoBao.DomainModule.Product;

namespace MyTools.TaoBao.UnitTest
{
    [TestFixture]
    public class CommonTest
    {

        #region Init

        /// <summary>
        /// 为整个TestFixture初始化资源
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
        }

        /// <summary>
        /// 为整个TestFixture释放资源
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
        }

        /// <summary>
        /// 为每个Test方法创建资源
        /// </summary>
        [SetUp]
        public void Initialize()
        {
        }

        /// <summary>
        /// 为每个Test方法释放资源
        /// </summary>
        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        #region public Test

        [Test]
        public void TestCopeMode()
        {
            Product product = new Product();
            product.Desc = "asdf";
            product.Num = 22;


            ItemAddRequest req = new ItemAddRequest();

            Util.CopyModel(product, req);

            Console.WriteLine(req.Desc);
            Console.WriteLine(req.Num);
        }

        [Test]
        public void TimeTest()
        {

            Console.WriteLine(DateTime.Now.Ticks);

        }

        [Test]
        public void Test1()
        {
            string BrandCode = "Metersbonwe";
            string Category = "女装";
            string Catalog = "1234567890针织短袖恤	";
            string startTitle = string.Format("{0} {1} {2} ", BrandCode.Trim(), Category.Trim(), Catalog.Trim());

            if (startTitle.Length > 24)
            {
                int moreThanNum = startTitle.Length - 24;
                string finalCatalog = "";
                if (moreThanNum < Catalog.Length)
                {
                    finalCatalog = Catalog.Remove(0, moreThanNum);
                }

                string GoodsSn = "";
                //return string.Format("{0} {1} {2} {3}", Brand, Category, finalCatalog, GoodsSn);
                //string finalCatalog = Catalog.Remove(0, moreThanNum);



            }
        }

        [Test]
        public void GetImgByte()
        {  
            var restClient = new RestClient("http://img6.ibanggo.com/sources/images/goods/MB/209697/209697_00--w_498_h_498.jpg");
            var request = new RestRequest(Method.GET);
            IRestResponse response = restClient.Execute(request);

             var bytes = response.RawBytes;

             FileStream fs = new FileStream("209697_00.jpg", FileMode.Create, FileAccess.Write);
             fs.Write(bytes, 0, bytes.Length);
             fs.Close();

            //Guid photoID = System.Guid.NewGuid();
            // String photolocation = String.Format(@"c:\temp\{0}.jpg", Guid.NewGuid().ToString());

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
              

        }

        [Test]
        public void DownloadImg()
        { 
            Image img = PicDealHelper.DownloadImage(
                "http://img6.ibanggo.com/sources/images/goods/MB/209697/209697_00--w_498_h_498.jpg");
            img.Save("test.jpg");


        }

        [Test]
        public void ResourceTest()
        {
            ResourceManager rm = new ResourceManager(typeof(Resource).FullName,
                             typeof(Resource).Assembly);
            var s = rm.GetString("SysConfig_METERSBONWE_BrandProp");

            Console.WriteLine(s);
        }

        [Test]
        public void UrlTest()
        {

            Uri uri = new Uri("http://img3.mbanggo.com/sources/images/bgpicupload/206039/206039_29--w_438_h_710.jpg");
            int lastIndex = uri.Segments.Length - 1;
            Console.WriteLine(uri.Segments[lastIndex]);
 
        }

        #endregion


        #region Private Methods

        public void Ta(string s)
        {
            double d = 120.78;

        }

        public bool MyTe(string s)
        {
            string result = null;


            return true;
        }

        #endregion
    }
}