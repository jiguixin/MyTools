using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Logging;
using MyTools.TaoBao.Interface;
using Top.Api.Domain;

namespace MyTools
{
    public partial class FrmPublishGoods : Form
    {
        ILogger _log = InstanceLocator.Current.GetInstance<ILoggerFactory>().Create();


        private IGoodsApi goodsApi = InstanceLocator.Current.GetInstance<IGoodsApi>();

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
            try
            {
                var txtUrls = e.Argument as string;

                if (txtUrls != null)
                {
                    var urls = txtUrls.Split(';');

                    foreach (var url in urls)
                    {
                        if (string.IsNullOrWhiteSpace(url))
                        {
                            continue;
                        }
                        var item = goodsApi.PublishGoodsForBanggoToTaobao(url);

                        this.bgwRun.ReportProgress(100, item);

                        Thread.Sleep(1000);

                    }
                }
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message, ex);
            }
        }

        private void bgwRun_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var item = e.UserState as Item;
            if (item != null) txtLog.AppendText(string.Format("{0} 已发布成功->{1}", item.Title, item.NumIid));
        }

        private void bgwRun_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            txtLog.AppendText("发布成功");
        }

    }
}
