/*
 *名称：CommonTest
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-11 06:17:56
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using Infrastructure.Crosscutting.Utility;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using MyTools.TaoBao.DomainModule;
using NUnit.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Top.Api.Request;
using Product = MyTools.TaoBao.DomainModule.Product;
using Infrastructure.Crosscutting.Declaration;

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
            string startTitle = "{0} {1} {2} ".StringFormat(BrandCode.Trim(), Category.Trim(), Catalog.Trim());

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

        [Test]
        public void FormatTest()
        {
            string s = "this is {0}  My Name is {1}".StringFormat("pen","jim ji"); 

            Console.WriteLine(s);

            
        }

        [Test]
        public void XmlEncode()
        {
            string s = XmlHelper.XmlEncode(
                "http://s.taobao.com/search?spm=a230r.1.8.6.jyjgfa&promote=0&sort=price-asc&initiative_id=staobaoz_20130524&tab=all&q=233722&stats_click=search_radio_all%3A1#J_relative");

            string ds = XmlHelper.XmlDecode(s);

             s = XmlHelper.XmlEncodeAttribute(
               "http://s.taobao.com/search?spm=a230r.1.8.6.jyjgfa&promote=0&sort=price-asc&initiative_id=staobaoz_20130524&tab=all&q=233722&stats_click=search_radio_all%3A1#J_relative");

            string ds1 = XmlHelper.XmlDecode(s);
        }

        [Test]
        public void ConverPrice()
        {
//            string s = "运费：15.50";
            string s = "运费：15 元：125 /r/n 15.20";
            
            Console.WriteLine(s.GetNumberInt()); 
            Console.WriteLine(s.GetNumberDouble());
            Console.WriteLine(s.GetNumber());
            Console.WriteLine(s.GetNumberStr());

        }


        [Test]
        public void CreateExcel()
        {
            ExcelHelper excel = new ExcelHelper(@"C:\Users\Administrator\Desktop\{0}分析.xls".StringFormat(DateTime.Now.ToString("yyyy-MM-dd HH-MM")));

            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("款号", "varchar(255)");
            dic.Add("成本价", "varchar(255)");
            dic.Add("我的售价", "varchar(255)");
            dic.Add("标题", "varchar(255)");
            dic.Add("销量", "varchar(255)");
            dic.Add("价格", "varchar(255)");
            dic.Add("成交记录", "varchar(255)");
            dic.Add("评价数", "varchar(255)");
            dic.Add("最近几天成交记录", "varchar(255)"); 

            excel.WriteTable("shell", dic);

        }

        [Test]
        public void SwichRow()
        {
//            string s = "adfasdf \r\nsafdasdf";
            string s = "价格数据{0}".StringFormat(DateTime.Now.ToString("HHmm"));
            Console.WriteLine(s);
        }

        [Test]
        public bool ExcelHelperTest()
        {
            ExcelHelper excel =
               new ExcelHelper(@"C:\Users\Administrator\Desktop\{0} 分析.xls".StringFormat(DateTime.Now.ToString("yyyy-MM-dd")));

            string tName = "";

            DataTable dtSchema = excel.GetSchema();

            foreach (var name in from DataRow dr in dtSchema.Rows select dr["TABLE_NAME"].ToString())
            {
                if (name == tName)
                    return true;
                else
                {
                    return false;
                }
            }

            return false;
        }

        [Test]
        public void StringBuilderTest()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 10; i++)
            {
                sb.AppendLine("test:{0}".StringFormat(i));
            }
            Console.WriteLine(sb.ToString().TrimEnd('\n'));


        }
        #endregion

        #region Json Net Demo

        [Test]
        public void JsonNetTest()
        {
            var pc = ConstructModel();


            var settings = new JsonSerializerSettings();

            string result = JsonConvert.SerializeObject(pc, Formatting.Indented, settings);//需要注意的是，如果返回的是一个集合，那么还要在它的上面再封装一个类。否则客户端收到会出错的。


            var pcList = JsonConvert.DeserializeObject<List<ProductColor>>(result);


            //JObject jo = JObject.Parse(result);



            


            Console.WriteLine(result);

        }

      

        #region helper

        private List<ProductColor> ConstructModel()
        {
            var pcList = new List<ProductColor>();

            var pc = new ProductColor
                {
                    ColorCode = "90",
                    ImgUrl = "http://www.baidu.com",
                    MapProps = "abc",
                    Title = "fooTitle",
                    SizeList = new List<ProductSize>
                        {
                            new ProductSize()
                                {
                                    Alias = "S",
                                    AvlNum = 11,
                                    SizeCode = "144",
                                    MySalePrice = 222,
                                    MarketPrice = 34.3
                                },
                            new ProductSize()
                                {
                                    Alias = "M",
                                    AvlNum = 22,
                                    SizeCode = "146",
                                    MySalePrice = 333,
                                    MarketPrice = 33.3
                                },
                            new ProductSize()
                                {
                                    Alias = "L",
                                    AvlNum = 33,
                                    SizeCode = "148",
                                    MySalePrice = 444,
                                    MarketPrice = 44.4
                                }
                        }
                };

            #region size

            #endregion

            pcList.Add(pc);

            pc = new ProductColor();
            pc.ColorCode = "80";
            pc.ImgUrl = "http://www.baidu1.com";
            pc.MapProps = "abc12";
            pc.Title = "fooTitl123";

            #region size

            pc.SizeList = new List<ProductSize>();
            pc.SizeList.Add(new ProductSize()
            {
                Alias = "S",
                AvlNum = 11,
                SizeCode = "144",
                MySalePrice = 111,
                MarketPrice = 11
            });
            pc.SizeList.Add(new ProductSize()
            {
                Alias = "M",
                AvlNum = 22,
                SizeCode = "146",
                MySalePrice = 222,
                MarketPrice = 22
            });
            pc.SizeList.Add(new ProductSize()
            {
                Alias = "L",
                AvlNum = 33,
                SizeCode = "148",
                MySalePrice = 333,
                MarketPrice = 33
            });

            #endregion
             
            pcList.Add(pc);

            return pcList; 
        }
         
        #endregion


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