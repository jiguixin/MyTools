using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Logging.TraceSource;
using MyTools.TaoBao.Interface;
using MyTools.Utility;
using Top.Api.Domain;

namespace MyTools
{
    public partial class FrmPublishGoods : Form
    {
        private readonly ILogger _log = InstanceLocator.Current.GetInstance<ILoggerFactory>().Create();


        private readonly IGoodsApi _goodsApi = InstanceLocator.Current.GetInstance<IGoodsApi>();

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

            var urlResult = e.Argument as string;

            if (urlResult != null)
            {
                string[] urls = urlResult.Split(';');

                foreach (string url in urls)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(url))
                        {
                            continue;
                        }
                        Item item = _goodsApi.PublishGoodsForBanggoToTaobao(url);
                         
                        Thread.Sleep(1000);
                    }
                    catch (Exception ex)
                    {
                        _log.LogError(ex.Message, ex);
                    }
                }
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