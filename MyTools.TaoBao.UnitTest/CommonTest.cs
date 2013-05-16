/*
 *名称：CommonTest
 *功能：
 *创建人：吉桂昕
 *创建时间：2013-05-11 06:17:56
 *修改时间：
 *备注：
 */

using System;
using Infrastructure.Crosscutting.Utility;
using MyTools.TaoBao.DomainModule;
using NUnit.Framework;
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
                //return string.Format("{0} {1} {2} {3}", BrandCode, Category, finalCatalog, GoodsSn);
                //string finalCatalog = Catalog.Remove(0, moreThanNum);



            }
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