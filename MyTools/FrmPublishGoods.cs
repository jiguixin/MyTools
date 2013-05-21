using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infrastructure.Crosscutting.IoC;
using MyTools.TaoBao.Interface;

namespace MyTools
{
    public partial class FrmPublishGoods : Form
    {
        private IGoodsApi goodsApi = InstanceLocator.Current.GetInstance<IGoodsApi>();

        public FrmPublishGoods()
        {
            InitializeComponent();
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            var urls = txtUrls.Text.Split(';');
            foreach (var url in urls)
            {
                goodsApi.PublishGoodsForBanggoToTaobao(url);
            } 
        }
    }
}
