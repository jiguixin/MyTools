using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Logging;
using MyTools.TaoBao.Interface;
using Top.Api.Domain;

namespace MyTools
{
    public partial class FrmPublishGoods : Form
    {
        private readonly ILogger _log = InstanceLocator.Current.GetInstance<ILoggerFactory>().Create();


        private readonly IGoodsApi _goodsApi = InstanceLocator.Current.GetInstance<IGoodsApi>();

        public FrmPublishGoods()
        {
            InitializeComponent();
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

                        bgwRun.ReportProgress(100, item);

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
            var item = e.UserState as Item;
            if (item != null) txtLog.AppendText("{0} 已发布成功->{1}".StringFormat(item.Title, item.NumIid));
        }

        private void bgwRun_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            txtLog.AppendText("操作结束");
        }
    }
}