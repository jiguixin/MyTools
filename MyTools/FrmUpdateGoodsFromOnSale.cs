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

namespace MyTools
{
    public partial class FrmUpdateGoodsFromOnSale : Form
    {  
        private readonly IGoodsApi _goodsApi = InstanceLocator.Current.GetInstance<IGoodsApi>();

        delegate void ChangeTextBoxValue(string str); // 新增委托代理

        public FrmUpdateGoodsFromOnSale()
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
            this.BeginInvoke(new ChangeTextBoxValue(SetRichTextBoxValue), str); // 也可用 this.Invoke调用
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

            if (IsSearch)
                _goodsApi.UpdateGoodsFromOnSale(lstSearchs, !chkNotModifyPrice.Checked);
            else
                _goodsApi.UpdateGoodsByAssign(result,!chkNotModifyPrice.Checked);
        }

        private void bgwRun_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            richTextBox1.AppendText("更新完成!");
        }

        private void bgwRun_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           
        }

        private void btnAssignUpdate_Click(object sender, EventArgs e)
        {
            IsSearch = false;
            if (!bgwRun.IsBusy)
            {
                bgwRun.RunWorkerAsync(txtNumIds.Text);
            }
        }
    }
}
