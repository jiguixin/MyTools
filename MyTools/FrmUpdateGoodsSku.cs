using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Logging.TraceSource;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using MyTools.TaoBao.Interface;
using MyTools.Utility;
using Infrastructure.Crosscutting.Declaration;

namespace MyTools
{
    public partial class FrmUpdateGoodsSku : Form
    {  
        private readonly IGoodsApi _goodsApi = InstanceLocator.Current.GetInstance<IGoodsApi>();

        delegate void ChangeTextBoxValue(string str); // 新增委托代理

        public FrmUpdateGoodsSku()
        {
            InitializeComponent();
              
            TextBoxTraceListener tl = new TextBoxTraceListener();
            tl.Filter = new System.Diagnostics.EventTypeFilter(SourceLevels.Information);
            tl.ChangeTextBoxValue = SetText;

            TraceSourceProvider.Source.Listeners.Add(tl); 
        }

        private void SetRichTextBoxValue(string str)
        {
            richTextBox1.AppendText(str); 
        }

        private bool IsSearch = true;

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
         
        private void btnOk_Click(object sender, EventArgs e)
        {
            IsSearch = true;
            if (!bgwRun.IsBusy)
            { 
                bgwRun.RunWorkerAsync(txtSearch.Text);
            }
        }

        private void bgwRun_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = e.Argument.ToString();

            IEnumerable<string> lstSearchs = TextHelper.StringToArray<string>(result);

            foreach (var search in lstSearchs)
            {
                _goodsApi.UpdateGoodsSkuInfo(search, discountRatio: txtRate.Text.ToType<double>(), stock: txtStock.Text.ToType<int>(), originalTitle: txtOriginalTitle.Text, newTitle: txtNewTitle.Text, isModifyPrice: !chkNotModifyPrice.Checked); 
            }
           
               
        }

        private void bgwRun_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            richTextBox1.AppendText("更新完成!");
        }

        private void bgwRun_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
        }

       
    }
}
