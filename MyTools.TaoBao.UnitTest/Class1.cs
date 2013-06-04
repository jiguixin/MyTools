using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using NUnit.Framework;

namespace MyTools.TaoBao.UnitTest
{
    [TestFixture]
    public class Class1
    {
        //sdfsdf
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


        [Test]
        public void Foo1()
        {
             
            char c1 = (char)33;
            char c = (char)(8186 + 33);

            string strSBC = "＂，”“”“”中华１25９ｔｅｓｔ ";

            string result = TextHelper.ToEngInterpunction(TextHelper.ToDBC(strSBC));
           // Console.WriteLine(ToSBC("\""));
            Console.WriteLine(result);
             

            char a1 = '"';
            char a2 = '“';
             
            Console.WriteLine(a2 - a1);

        }

        public void Foo(bool? b = null)
        {

        }
         
    } 
}
