using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using NUnit.Framework;
using System.Text.RegularExpressions;
using Infrastructure.Crosscutting.Declaration;

namespace UnitTest
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


        [Test]
        public void FooTest()
        {
            #region var

            string reg = @"[\.\#]?\w+[^{]+\{[^}]*\}";

            #endregion

            #region GetContent

            string htmlContent = FileHelper.ReadFileContent(@"C:\Users\Administrator\Desktop\abc.html");
            string cssContent = FileHelper.ReadFileContent(@"C:\Users\Administrator\Desktop\abc.css");

            #endregion

            HtmlDocument doc = new HtmlDocument();
            
            doc.LoadHtml(htmlContent);
            doc.OptionDefaultStreamEncoding = Encoding.UTF8;

            var lstCss = GetCssContent(cssContent);

            StringBuilder sb = new StringBuilder();

            foreach (var css in lstCss)
            {
                #region Get Xpath
                   
                var l = css.IndexOf("{", System.StringComparison.Ordinal);

                var token = css.Substring(0, l);
                var sytle = css.Substring(l + 1, css.Length - l - 2);
                 
                sb.Clear();

                var m5 = "mt5 mb10";
                if (token.Contains(m5))
                {
                    sb.Append("//*[@id=\"goods_model\"]/div[@class=\"{0}\"]".StringFormat(m5));
                }
                else
                { 
                    var singleElement = token.Split(' ');

                    foreach (var s in singleElement)
                    {
                        ConstructXpath(s, sb);
                    } 
                }

                #endregion
                 
               var nodes =  doc.DocumentNode.SelectNodes(sb.ToString());

                if (nodes.IsNull())
                    continue;

                foreach (var node in nodes)
                {
                    node.Attributes.Add("style", sytle); 
                }
            }

            Console.WriteLine(doc.DocumentNode.OuterHtml);

        }

        private static void ConstructXpath(string s, StringBuilder sb)
        {
            if (s.IsNullOrEmpty())
                return;

            if (s.Contains("#"))
            {
                var ele = s.Replace("#", "");
                 
                sb.Append("//*[@id=\"{0}\"]".StringFormat(ele));
                 
                return;
            }

            if (s.Contains("."))
            {
                var ele = s.Replace(".", "");
                sb.Append("//*[@class=\"{0}\"]".StringFormat(ele));
                return;
            }

            if (sb.Length > 0)
                sb.Append("/");
            sb.Append(s);
        }

        [Test]
        public void UncodeTest()
        {
            string s = null;

            using (FileStream fs = new FileStream("json.txt", FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                {
                    s = sr.ReadToEnd();
                }
            }
            Console.WriteLine(TextHelper.ToGB2312(s));
            Console.WriteLine(TextHelper.ToGb2312NotRemove(s));
        }

        [Test]
        public void name()
        {

            DateTime dt = DateTime.Parse("2013-10-22").AddDays(98);
            Console.WriteLine(dt.ToString());
        }


        #region Helper

        public static List<string> GetCssContent(string source)
        {
            //匹配CSS文件中的CSS样式
            var reg = @"[\.\#]?\w+[^{]+\{[^}]*\}";
            var lstResult = new List<string>();
            if (!string.IsNullOrEmpty(source))
            {
                var r = new Regex(reg); //定义一个Regex对象实例
                var mc = r.Matches(source);
                for (var i = 0; i < mc.Count; i++) //在输入字符串中找到所有匹配
                {
                    string val = mc[i].Value;

                    if (val.IsNullOrEmpty())
                        continue;

                    lstResult.Add(val.Trim()); 
                }
            }
            return lstResult;
        }

        #endregion


        #region 父类与子类的转换

        [Test]
        public void SupperToChild()
        {
            Childer c = new Childer() {SName = "父类", CName = "子类"};
            SToC(c);
        }

        private void SToC(Supper s)
        {
            Childer c = (Childer) s;
            Console.WriteLine(c.CName);

        }


        public class Supper
        {
            public string SName { get; set; }
        }

        public class Childer : Supper
        {
            public string CName { get; set; } 
        }

        #endregion
          

    } 
}
