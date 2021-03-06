﻿/*
 *名称：PublishGoodsTestClass
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-05 04:47:11
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Specialized;
using System.Text;
using Infrastructure.CrossCutting.IoC.Ninject;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Impl;
using MyTools.TaoBao.Interface;
using NUnit.Framework;
using RestSharp;

namespace MyTools.TaoBao.UnitTest
{
    [TestFixture]
    public class PgTestClass
    {
        /// <summary>
        ///     为每个Test方法创建资源
        /// </summary>
        [SetUp]
        public void Initialize()
        {
            InstanceLocator.SetLocator(
                new NinjectContainer().WireDependenciesInAssemblies(typeof (ItemCatsApi).Assembly.FullName).Locator);
        }

        /// <summary>
        ///     为每个Test方法释放资源
        /// </summary>
        [TearDown]
        public void TearDown()
        {
        }

        /// <summary>
        ///     为整个TestFixture初始化资源
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
        }

        /// <summary>
        ///     为整个TestFixture释放资源
        /// </summary>
        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
        }

        #region BanggoMgt
         
        [Test]
        public void BanggoMgt_GetGoodsInfo()
        {
            var mgt = InstanceLocator.Current.GetInstance<IBanggoMgt>();

            var brm = new BanggoRequestModel();
            brm.Referer = "http://metersbonwe.banggo.com/Goods/238395.shtml";
            // brm.SizeCode = "23852";
            //brm.ColorCode = 91;
            brm.GoodsSn = "238395";

            BanggoProduct goodsModel = mgt.GetGoodsInfo(brm);
            Console.WriteLine(goodsModel.MarketPrice);
        }

        [Test]
        public void BanggoMgt_ResolveProductUrl()
        {
            var mgt = InstanceLocator.Current.GetInstance<IBanggoMgt>();
            Console.WriteLine(mgt.ResolveProductUrlRetGoodsSn("http://235589metersbonwe.banggo.com/Goods/238395.shtml"));
        }

        [Test]
        public void GetGoodsUrl_Test()
        {
            var mgt = InstanceLocator.Current.GetInstance<IBanggoMgt>();
            Console.WriteLine(mgt.GetGoodsUrl("238286"));
            Console.WriteLine(mgt.GetGoodsUrl("242591"));
        }

        [Test]
        public void GetBanggoData()
        {
            var nc =
                new NameValueCollection();
            nc.Add("Host", "act.banggo.com");
            nc.Add("Connection", "keep-alive");
            nc.Add("Accept", "*/*");
            nc.Add("User-Agent",
                   "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31");
            nc.Add("Referer", "http://metersbonwe.banggo.com/Goods/238395.shtml");
            nc.Add("Accept-Encoding", "gzip,deflate,sdch");
            nc.Add("Accept-Language", "zh-CN,zh;q=0.8");
            nc.Add("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");
            nc.Add("Cookie",
                   "stati_client_mark_code=1364310301869; PHPSESSID=fb3bc93284e0c7213c6711f91a38c60a; bg_uid=33a7f71a44e72aa2efad64724317ce56; bg_user_ip=8%7C118.113.147.218%7C%E5%9B%9B%E5%B7%9D%7C0%7C%E6%88%90%E9%83%BD%7C101270101%7C%E6%88%90%E9%83%BD%7C101270101; banggo_think_language=zh-CN; bg_time=1368356787; __utma=4343212.1532128941.1364310301.1368256305.1368351505.11; __utmb=4343212.11.10.1368351505; __utmc=4343212; __utmz=4343212.1368351505.11.3.utmcsr=search.banggo.com|utmccn=(referral)|utmcmd=referral|utmcct=/Search/a_22_a_a_a_a_a_a_a_a_a_a_a_a.shtml; NSC_58.215.174.167*80=ffffffff0958156645525d5f4f58455e445a4a423660");

            string result = HttpHelper.GETDataToUrl(
                "http://act.banggo.com/Ajax/cartAjax?time=0.4682253187056631&callback=jsonp1368356786991&ajaxtype=color_size&type=size&code=23852&r_code=91&goods_sn=238395",
                nc, Encoding.UTF8);

            Console.WriteLine(result);
        }

        [Test]
        public void GetBanggoDataByRest()
        {
            var restClient =
                new RestClient(
                    "http://act.banggo.com/Ajax/cartAjax?time=0.4682253187056631&callback=jsonp1368356786991&ajaxtype=color_size&type=size&code=23852&r_code=91&goods_sn=238395");
            var request = new RestRequest(Method.GET);

            request.AddHeader("Host", "act.banggo.com");
            //request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent",
                              "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31");
            request.AddHeader("Referer", "http://metersbonwe.banggo.com/Goods/238395.shtml");
            request.AddHeader("Accept-Encoding", "gzip,deflate,sdch");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.8");
            request.AddHeader("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");
            //request.AddCookie("Cookie",
            //       "stati_client_mark_code=1364310301869; PHPSESSID=fb3bc93284e0c7213c6711f91a38c60a; bg_uid=33a7f71a44e72aa2efad64724317ce56; bg_user_ip=8%7C118.113.147.218%7C%E5%9B%9B%E5%B7%9D%7C0%7C%E6%88%90%E9%83%BD%7C101270101%7C%E6%88%90%E9%83%BD%7C101270101; banggo_think_language=zh-CN; bg_time=1368356787; __utma=4343212.1532128941.1364310301.1368256305.1368351505.11; __utmb=4343212.11.10.1368351505; __utmc=4343212; __utmz=4343212.1368351505.11.3.utmcsr=search.banggo.com|utmccn=(referral)|utmcmd=referral|utmcct=/Search/a_22_a_a_a_a_a_a_a_a_a_a_a_a.shtml; NSC_58.215.174.167*80=ffffffff0958156645525d5f4f58455e445a4a423660");

            IRestResponse response = restClient.Execute(request);
            Console.WriteLine(response.Content);
        }

        [Test]
        public void GetBanggoDataByRest1()
        {
            string url =
                "http://act.banggo.com/Ajax/cartAjax?time={0}&ajaxtype=color_size&type=size&code={1}&r_code={2}&goods_sn={3}"
                    .StringFormat(
                        DateTime.Now.Ticks, 23852, 91, 238395);
            var restClient = new RestClient(url);
            var request = new RestRequest(Method.GET);


            request.AddHeader("Host", "act.banggo.com");
            //request.AddHeader("Connection", "keep-alive");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent",
                              "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31");
            request.AddHeader("Referer", "http://metersbonwe.banggo.com/Goods/238395.shtml");
            request.AddHeader("Accept-Encoding", "gzip,deflate,sdch");
            request.AddHeader("Accept-Language", "zh-CN,zh;q=0.8");
            request.AddHeader("Accept-Charset", "GBK,utf-8;q=0.7,*;q=0.3");
            //request.AddCookie("Cookie",
            //       "stati_client_mark_code=1364310301869; PHPSESSID=fb3bc93284e0c7213c6711f91a38c60a; bg_uid=33a7f71a44e72aa2efad64724317ce56; bg_user_ip=8%7C118.113.147.218%7C%E5%9B%9B%E5%B7%9D%7C0%7C%E6%88%90%E9%83%BD%7C101270101%7C%E6%88%90%E9%83%BD%7C101270101; banggo_think_language=zh-CN; bg_time=1368356787; __utma=4343212.1532128941.1364310301.1368256305.1368351505.11; __utmb=4343212.11.10.1368351505; __utmc=4343212; __utmz=4343212.1368351505.11.3.utmcsr=search.banggo.com|utmccn=(referral)|utmcmd=referral|utmcct=/Search/a_22_a_a_a_a_a_a_a_a_a_a_a_a.shtml; NSC_58.215.174.167*80=ffffffff0958156645525d5f4f58455e445a4a423660");

            IRestResponse response = restClient.Execute(request);
            Console.WriteLine(response.Content);
        }

        [Test]
        public void ExportProductColorForExcelTest()
        {
            var mgt = InstanceLocator.Current.GetInstance<IBanggoMgt>();
            mgt.ExportProductColorForExcel("http://metersbonwe.banggo.com/Goods/242591.shtml");
        }

        [Test]
        public void ExportProductColorsForExcelTest()
        { 
            var mgt = InstanceLocator.Current.GetInstance<IBanggoMgt>();
            mgt.ExportProductColorsForExcel("http://me-city.banggo.com/Goods/510960.shtml", "http://metersbonwe.banggo.com/Goods/207707.shtml", "http://metersbonwe.banggo.com/Goods/238286.shtml"); 
        }

        [Test]
        public void GetProductColorByOnline_Test()
        {
            var mgt = InstanceLocator.Current.GetInstance<IBanggoMgt>();
            mgt.GetProductColorByOnline(new BanggoRequestModel
                {
                    GoodsSn = "238286",
                    Referer = "http://metersbonwe.banggo.com/Goods/238286.shtml"
                });
        }


        #endregion

        [Test]
        public void GetSellerCid()
        {
            string sellerCid = "T恤 - 短袖T恤";

            var client = InstanceLocator.Current.GetInstance<IShopApi>();

            string s = client.GetSellerCids("mbgou", "Metersbonwe - 女装", "T恤 - 短袖T恤", "T恤 - 长袖T恤");

            Console.WriteLine(s);
        }

        [Test]
        public void TestGetCid()
        {
            var client = InstanceLocator.Current.GetInstance<IItemCatsApi>();

            string parentCid = client.GetCid("T恤", "女装");
            Console.WriteLine(parentCid);

            string parentCid1 = client.GetCid("T恤", "女装");
            Console.WriteLine(parentCid);
        }

        [Test]
        public void PublishGoodsFromExcelTest()
        {


        }
        // "Sku/2013-05-29 banggoSku.xls"

        #region IAnalysisTest

        [Test]
        public void GetAnalysisPriceTest()
        {
            var client = InstanceLocator.Current.GetInstance<IAnalysis>();
            client.ExportRivalGoodsInfo("me city 510957", 249, 174);
             
            //client.ExportRivalGoodsInfo("me city 510960", 249, 174);
            //client.ExportRivalGoodsInfo("metersbonwe 207707", 179, 125);
            //client.ExportRivalGoodsInfo("metersbonwe 254010", 199, 139);

        }

        [Test]
        public void ExportBanggoAndTaobaoGoodsInfoTest()
        {
            var client = InstanceLocator.Current.GetInstance<IAnalysis>();
            client.ExportBanggoAndTaobaoGoodsInfo("http://metersbonwe.banggo.com/Goods/242591.shtml");
            
        }
         

        #endregion
    }
}