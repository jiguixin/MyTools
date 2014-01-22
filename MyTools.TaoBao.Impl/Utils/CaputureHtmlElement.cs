/*
 *名称：CaputureHtmlElement
 *功能：
 *创建人：吉桂昕
 *创建时间：2014-01-20 10:18:52
 *修改时间：
 *备注：
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using MyTools.TaoBao.DomainModule;
using mshtml;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace MyTools.TaoBao.Impl.Utils
{
    public class CaputureHtmlElement
    {
        #region Members

        #endregion

        #region Constructor

        #endregion

        #region Public Methods

        /// <summary>
        /// 截取整个网站，不能用多线程
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Bitmap Capture(string url)
        {
            WebBrowser wb = CreateWebBrowser(url);

            return CaptureFullUrl(wb);
        }

        /// <summary>
        /// 截取指定元素，不能用多线程
        /// </summary>
        /// <param name="url"></param>
        public static Bitmap Capture(string url, string elementId)
        {
            return CaptureElementBase(url, elementId, CreateWebBrowser);
        }

        /// <summary>
        /// 截取邦购产品描述图片
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Bitmap CaptureBanggo(string url)
        {
            try
            {
                // download each page and dump the content
                var task = MessageLoopWorker.Run(DoWorkAsync, url);
                task.Wait();
                return (Bitmap) task.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("DoWorkAsync failed: " + ex.Message);

                return null;
            }
        }

        #endregion

        #region private Methods

        private static WebBrowser CreateWebBrowser(string url)
        {
            Rectangle screen = Screen.PrimaryScreen.Bounds;

            var wb = new WebBrowser();
            wb.Width = screen.Width;
            wb.Height = screen.Height;
            wb.ScriptErrorsSuppressed = true;
            wb.ScrollBarsEnabled = false;

            Console.WriteLine("正在加载页面...");
            wb.Navigate(url);

            //等待页面加载完毕
            while (wb.ReadyState != WebBrowserReadyState.Complete)
            {
                Application.DoEvents();
            }
            Console.WriteLine("页面加载完成");

            //计算网页Document的宽和高
            Rectangle body = wb.Document.Body.ScrollRectangle;
            var size = new Size(body.Width > screen.Width ? body.Width : screen.Width,
                                body.Height > screen.Height ? body.Height : screen.Height);
            wb.Width = size.Width;
            wb.Height = size.Height;


            return wb;
        }

        private static Bitmap CaptureFullUrl(WebBrowser wb)
        {
            var imageRect = new Rectangle(0, 0, wb.Width, wb.Height);
            var bitmap = new Bitmap(imageRect.Width, imageRect.Height);

            var ivo = wb.Document.DomDocument as IViewObject;
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                IntPtr hdc = g.GetHdc();
                ivo.Draw(1, -1, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, hdc, ref imageRect, ref imageRect, IntPtr.Zero, 0);
                g.ReleaseHdc(hdc);
            }

            return bitmap;
        }

        /*//通过指定元素的elementId，获取元素的Rectange，使用脚本注册的方式
        private Rectangle GetElementRectangeByInject(WebBrowser wb, string elementId)
        {
            HtmlElement head = wb.Document.GetElementsByTagName("head")[0];

            HtmlElement script = wb.Document.CreateElement("script");
            var script2 = (IHTMLScriptElement) script.DomElement;

            //注入的js脚本,暂时没有处理找不到指定元素ElementId的情况
            script2.text = "function getElementRectange(){var obj =  document.getElementById('" + elementId +
                           "');var rect=obj.getBoundingClientRect();var x=rect.left;var y=rect.top;var width=rect.right-rect.left;var height=rect.bottom-rect.top;return x.toString()+','+y.toString()+','+width.toString()+','+height.toString();}";
            head.AppendChild(script);
            object result = wb.Document.InvokeScript("getElementRectange");
            string[] values = result.ToString().Split(',');
            var rect = new Rectangle(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]),
                                     int.Parse(values[3]));
            return rect;
        }
*/
        //通过指定元素的elementId，获取元素的Rectange，使用脚本注册的方式
        private static Rectangle GetElementRectangeByInject(WebBrowser wb, string elementId)
        {
            HtmlElement head = wb.Document.GetElementsByTagName("head")[0];

            HtmlElement script = wb.Document.CreateElement("script");
            var script2 = (IHTMLScriptElement) script.DomElement;

            //注入的js脚本,暂时没有处理找不到指定元素ElementId的情况
            script2.text = "function getElementRectange(){var obj =  document.getElementById('" + elementId +
                           "');var rect=obj.getBoundingClientRect();var x=rect.left;var y=rect.top;var width=rect.right-rect.left;var height=rect.bottom-rect.top;return x.toString()+','+y.toString()+','+width.toString()+','+height.toString();}";
            head.AppendChild(script);
            object result = wb.Document.InvokeScript("getElementRectange");
            string[] values = result.ToString().Split(',');
            var rect = new Rectangle(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]),
                                     int.Parse(values[3]));
            return rect;
        }

        private static Bitmap CaptureElementBase(string url, string elementId,
                                                 Func<string, WebBrowser> CreateWebBrowserFunc)
        {
            WebBrowser wb = CreateWebBrowserFunc(url);

            Bitmap map = CaptureFullUrl(wb);

            //获取元素elementId的Rectange
            Rectangle rect = GetElementRectangeByInject(wb, elementId);
            //从整个网页上截取指定元素elementId的范围
            Bitmap newMap = map.Clone(rect, PixelFormat.Format32bppRgb);
            return newMap;
        }


        #endregion

        private static async Task<object> DoWorkAsync(IEnumerable<object> args)
        {
            Rectangle screen = Screen.PrimaryScreen.Bounds;
            var wb = new WebBrowser();
            wb.Width = screen.Width;
            wb.Height = screen.Height;
            wb.ScriptErrorsSuppressed = true;
            wb.ScrollBarsEnabled = false;

            TaskCompletionSource<bool> tcs = null;
            WebBrowserDocumentCompletedEventHandler documentCompletedHandler = (s, e) =>
                                                                               tcs.TrySetResult(true);
            //todo:不能等到图片加载完成
            // navigate to each URL in the list
            foreach (var url in args)
            {
                tcs = new TaskCompletionSource<bool>();
                wb.DocumentCompleted += documentCompletedHandler;
                try
                {
                    wb.Navigate(url.ToString());
                    // await for DocumentCompleted
                    await tcs.Task;
                }
                finally
                {
                    wb.DocumentCompleted -= documentCompletedHandler;
                }

                var doc = new HtmlDocument();

                doc.LoadHtml(wb.DocumentText);
                HtmlNodeCollection imgNodes =
                    doc.DocumentNode.SelectNodes(Resource.SysConfig_GetGoodsModeImgGreyXPath);
                foreach (HtmlNode imgNode in imgNodes)
                {
                    imgNode.SetAttributeValue("src", imgNode.GetAttributeValue("original", ""));
                }
                wb.DocumentText = "";

                wb.Document.Write(doc.DocumentNode.InnerHtml);
                 

                //计算网页Document的宽和高
                Rectangle body = wb.Document.Body.ScrollRectangle;
                var size = new Size(body.Width > screen.Width ? body.Width : screen.Width,
                                    body.Height > screen.Height ? body.Height : screen.Height);
                wb.Width = size.Width;
                wb.Height = size.Height;

                Bitmap map = CaptureFullUrl(wb);

                //获取元素elementId的Rectange
                Rectangle rect = GetElementRectangeByInject(wb, Resource.SysConfig_GoodsDescId);
                //从整个网页上截取指定元素elementId的范围
                Bitmap newMap = map.Clone(rect, PixelFormat.Format32bppRgb);
                wb.Dispose();
                return newMap;

            }

            return null;
        }

    }

    [ComVisible(true), ComImport]
    [Guid("0000010d-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IViewObject
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Draw(
            [MarshalAs(UnmanagedType.U4)] UInt32 dwDrawAspect,
            int lindex,
            IntPtr pvAspect,
            [In] IntPtr ptd,
            IntPtr hdcTargetDev,
            IntPtr hdcDraw,
            [MarshalAs(UnmanagedType.Struct)] ref Rectangle lprcBounds,
            [MarshalAs(UnmanagedType.Struct)] ref Rectangle lprcWBounds,
            IntPtr pfnContinue,
            [MarshalAs(UnmanagedType.U4)] UInt32 dwContinue);

        [PreserveSig]
        int GetColorSet([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
                        int lindex, IntPtr pvAspect, [In] IntPtr ptd,
                        IntPtr hicTargetDev, [Out] IntPtr ppColorSet);

        [PreserveSig]
        int Freeze([In, MarshalAs(UnmanagedType.U4)] int dwDrawAspect,
                   int lindex, IntPtr pvAspect, [Out] IntPtr pdwFreeze);

        [PreserveSig]
        int Unfreeze([In, MarshalAs(UnmanagedType.U4)] int dwFreeze);

        void SetAdvise([In, MarshalAs(UnmanagedType.U4)] int aspects,
                       [In, MarshalAs(UnmanagedType.U4)] int advf,
                       [In, MarshalAs(UnmanagedType.Interface)] IAdviseSink pAdvSink);

        void GetAdvise([In, Out, MarshalAs(UnmanagedType.LPArray)] int[] paspects,
                       [In, Out, MarshalAs(UnmanagedType.LPArray)] int[] advf,
                       [In, Out, MarshalAs(UnmanagedType.LPArray)] IAdviseSink[] pAdvSink);
    }
}

    
 