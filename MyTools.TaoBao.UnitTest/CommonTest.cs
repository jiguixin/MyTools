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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using Infrastructure.Crosscutting.Utility;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using MyTools.Framework.Common;
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
            var restClient =
                new RestClient("http://img6.ibanggo.com/sources/images/goods/MB/209697/209697_00--w_498_h_498.jpg");
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
            ResourceManager rm = new ResourceManager(typeof (Resource).FullName,
                                                     typeof (Resource).Assembly);
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
            string s = "this is {0}  My Name is {1}".StringFormat("pen", "jim ji");

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
            ExcelHelper excel =
                new ExcelHelper(
                    @"C:\Users\Administrator\Desktop\{0}分析.xls".StringFormat(DateTime.Now.ToString("yyyy-MM-dd HH-MM")));

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
                new ExcelHelper(
                    @"C:\Users\Administrator\Desktop\{0} 分析.xls".StringFormat(DateTime.Now.ToString("yyyy-MM-dd")));

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
        public void GetExcel()
        {
            DataTable dt = ExcelHelper.GetExcelData(@"Sku\2013-05-29 banggoSku.xls", "Sku");


            foreach (DataRow dr in dt.Rows)
            {
                PublishGoods pg = new PublishGoods(dr);


            }
        }


        [Test]
        public void ConvertType()
        {
            Console.WriteLine(CommonTest.FromType<string>(23.20));

            object o = new object();

            o.ToLong();

        }


        public static T FromType<T>(object text)
        {
            try
            {
                return (T) Convert.ChangeType(text, typeof (T), CultureInfo.InvariantCulture);
            }
            catch
            {
                return default(T);
            }
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

        [Test]
        public void CopyModel()
        {
            BanggoProduct product = new BanggoProduct();
            product.NumIid = 18623029094;
            product.PropertyAlias =
                "1627207:3232483:粉紫色组(70色);20509:28381:155/80A(S)(24242);20509:28313:160/84A(M)(24244);20509:28314:170/92A(XL)(24248);";
            product.Props = "1627207:3232483;20509:28381;20509:28313;20509:28314;";
            product.SkuOuterIds = "242591,242591,242591";
            product.SkuPrices = "178,178,178";
            product.SkuProperties =
                "1627207:3232483;20509:28381,1627207:3232483;20509:28313,1627207:3232483;20509:28314";
            product.SkuQuantities = "2,5,1";

            ItemUpdateRequest req = new ItemUpdateRequest();

            Util.CopyModel(product, req);

            FileHelper.WriteText("ItemUpdateRequestTest.html", req.DumpProperties(), Encoding.UTF8);

            FileHelper.WriteText("ProductTest.html", product.DumpProperties(), Encoding.UTF8);

        }

        [Test]
        public void StringFormtTest()
        {
            Console.WriteLine(Resource.Log_UpdateGoodsFailure);


        }

        #endregion

        #region Excel

        [Test]
        public void GetExcelData()
        {
            var ds = ExcelHelper.GetExcelDataSet(@"C:\Users\Administrator\Desktop\SaleOrder.xlsx");

            var dtOrderList = ds.Tables["ExportOrderList"];

            var dtOrderDetailList = ds.Tables["ExportOrderDetailList"];


            var query = from l in dtOrderList.AsEnumerable()
                        join d in dtOrderDetailList.AsEnumerable() on l["订单编号"] equals d["订单编号"]
                        where d["订单状态"].ToString() != "交易关闭" && d["订单状态"].ToString() != "等待买家付款"
                        select new
                            {
                                OrderNo = l["订单编号"],
                                CreateTime = l["订单创建时间"],
                                GoodsSn = d["外部系统编号"],
                                Props = d["商品属性"],
                                SalePrice = l["买家应付货款"],
                                Postage = l["买家应付邮费"],
                                TotalPrice = l["总金额"],
                                OrderState = l["订单状态"],
                                UserName = l["买家会员名"],
                                Remark = l["订单备注"],
                                Price = d["价格"],
                                Count = d["购买数量"],
                                Title = d["标题"]
                            };

            string sheetName = "销售记录";
            var excel = CreateExcelForSell(sheetName);
            DataTable dt = excel.ReadTable(sheetName);

            foreach (var q in query)
            {

                var jRemark = TextHelper.ChangeStrToDBC(q.Remark.ToString().Trim('\''));

                DataRow dr = dt.NewRow();
                dr["订单编号"] = q.OrderNo;
                dr["卖出时间"] = q.CreateTime;
                dr["款号"] = q.GoodsSn;
                dr["商品属性"] = q.Props;
                dr["销售金额"] = q.TotalPrice;
                dr["买家应付邮费"] = q.Postage;
                dr["单件售价"] = q.Price;
                dr["购买数量"] = q.Count;
                //                dr["结帐情况"]
                //                dr["结帐时间"]

                JObject jObj = JObject.Parse(jRemark);
                if (jObj != null)
                {
                    var source = jObj.SelectToken("来源");

                    if (source != null) dr["货源"] = source.Value<string>();

                    var costPrice = jObj.SelectToken("成本价");
                    dr["付款金额"] = costPrice != null ? (object) costPrice.Value<string>() : 0;

                    dr["原价"] = q.Title.ToString().GetNumberInt();
                    var pastage = jObj.SelectToken("邮费");

                    dr["支出邮费"] = pastage != null ? (object) costPrice.Value<string>() : 0;
                }
                else
                {
                    dr["付款金额"] = 0;
                    dr["原价"] = 0;
                    dr["支出邮费"] = 0;
                }

                excel.AddNewRow(dr);
            }
        }

        //创建ExcelHelper 该EXCEL是用于存放SKU数据
        private ExcelHelper CreateExcelForSell(string sheetName)
        {
            FileHelper.CreateDirectory("Sell");

            string filePath = @"Sell\{0} 销售.xls".StringFormat(DateTime.Now.ToString("yyyy-MM-dd"));

            var excel = new ExcelHelper(filePath) {Imex = "0", Hdr = "YES"};

            if (File.Exists(filePath))
            {
                if (SysUtils.CheckTableExsit(excel, sheetName))
                {
                    return excel;
                }
            }
            var dic = new Dictionary<string, string>
                {
                    {"订单编号", "varchar(255)"},
                    {"卖出时间", "varchar(255)"},
                    {"货源", "varchar(255)"},
                    {"款号", "Double"},
                    {"商品属性", "varchar(255)"},
                    {"原价", "Double"},
                    {"销售金额", "Double"},
                    {"买家应付邮费", "Double"},
                    {"单件售价", "Double"},
                    {"购买数量", "Double"},
                    {"支出邮费", "Double"},
                    {"付款金额", "Double"},
                    {"结帐情况", "varchar(255)"},
                    {"结帐时间", "varchar(255)"},
                    {"购买帐号", "varchar(255)"},
                    {"备注", "varchar(255)"},
                };

            excel.WriteTable(sheetName, dic);
            return excel;
        }


        [Test]
        public void Foo1()
        {
            //Console.WriteLine(CharConverter("{ \"来源\": \"CDMB付家秀\", \"成本价\": 109.5, “退款金额”:139}"));
        }

        #endregion

        #region Json Net Demo

        [Test]
        public void JsonNetTest()
        {
            var pc = ConstructModel();


            var settings = new JsonSerializerSettings();

            string result = JsonConvert.SerializeObject(pc, Formatting.Indented, settings);
                //需要注意的是，如果返回的是一个集合，那么还要在它的上面再封装一个类。否则客户端收到会出错的。


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
         
        public class SellDetial
        {
            //public SellDetial(DataRow dr)
            //{
            //    OrderNo = Util.Get<string>(dr, "订单编号");
            //    CreateTime = Util.Get<string>(dr, "卖出时间");
            //    Source = Util.Get<string>(dr, "货源");
            //    GoodsSn = Util.Get<string>(dr, "款号");
            //    Props = Util.Get<string>(dr, "商品属性");

            //}
            public string OrderNo { get; set; }
            public string CreateTime { get; set; }
            public string Source { get; set; }
            public string GoodsSn { get; set; }
            public string Props { get; set; }
            public double MarketPrice { get; set; }
            public double SaleTotalPrice { get; set; }
            public double HePayPostage { get; set; }
            public double SinglePrice { get; set; }
            public double Count { get; set; }
            public double MyPayPostage { get; set; }
            public double CostPrice { get; set; }
            public string PayStatus { get; set; }
            public string PayTime { get; set; }
            public string Account { get; set; }
            public string Remark { get; set; }


        }
    }
}