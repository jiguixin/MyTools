using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Logging.TraceSource;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using MyTools.Utility;
using Top.Api.Domain;

namespace MyTools
{
    public partial class FrmPublishGoods : Form
    {
        private readonly ILogger _log = InstanceLocator.Current.GetInstance<ILoggerFactory>().Create();
         
        private readonly IGoodsPublish _goodsPublish = InstanceLocator.Current.GetInstance<IGoodsPublish>(Resource.SysConfig_Banggo);

        private readonly IRequest _request = InstanceLocator.Current.GetInstance<IRequest>(Resource.SysConfig_Banggo);
         
        delegate void ChangeTextBoxValue(string str); // 新增委托代理

        private void SetRichTextBoxValue(string str)
        {
            txtLog.AppendText(str);
        }

        void SetText(string str)
        {
            try
            {
                this.BeginInvoke(new ChangeTextBoxValue(SetRichTextBoxValue), str); // 也可用 this.Invoke调用
            }
            catch (Exception)
            { 
            }
            
        }

        public FrmPublishGoods()
        {
            InitializeComponent();
             
            TextBoxTraceListener tl = new TextBoxTraceListener();
            tl.Filter = new System.Diagnostics.EventTypeFilter(SourceLevels.Information);
            tl.ChangeTextBoxValue = SetText;

            TraceSourceProvider.Source.Listeners.Add(tl);
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            if (!bgwRun.IsBusy)
            {
                bgwRun.RunWorkerAsync(txtUrls.Text);
            }
        }

        private void bgwRun_DoWork(object sender, DoWorkEventArgs e)
        {

            var inputSource = e.Argument as string;

            if (inputSource != null)
            {
                string[] source = inputSource.Split(';');

                foreach (string s in source)
                {
                    if (s.IsEmptyString())
                        continue;

                    if (s.IsUrl())
                    {
                        PublishGoods(s);
                    }
                    else //如果输入的是款号
                    {
                        var gUrl = _request.GetGoodsUrl(s);

                        if (!gUrl.IsEmptyString())
                        {
                            PublishGoods(gUrl);
                        } 
                    }

                    Thread.Sleep(1000);
                }
            }
        }

        private void PublishGoods(string url)
        {
            try
            {
                Item item = _goodsPublish.PublishGoods(url,_request);

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
            }
        }

        private void bgwRun_ProgressChanged(object sender, ProgressChangedEventArgs e)
        { 
        }

        private void bgwRun_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            txtLog.AppendText("操作结束");
        }
    }
}