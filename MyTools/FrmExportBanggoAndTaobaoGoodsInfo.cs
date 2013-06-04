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
using Infrastructure.Crosscutting.Logging.TraceSource;
using MyTools.TaoBao.Interface;
using MyTools.Utility;

namespace MyTools
{
    public partial class FrmExportBanggoAndTaobaoGoodsInfo : Form
    {
        IAnalysis _analysis = InstanceLocator.Current.GetInstance<IAnalysis>();

        delegate void ChangeTextBoxValue(string str); // 新增委托代理

        private void SetRichTextBoxValue(string str)
        {
            richTextBox1.AppendText(str);
        }

        void SetText(string str)
        {
            this.BeginInvoke(new ChangeTextBoxValue(SetRichTextBoxValue), str); // 也可用 this.Invoke调用
        }

        public FrmExportBanggoAndTaobaoGoodsInfo()
        {
            InitializeComponent();

            TextBoxTraceListener tl = new TextBoxTraceListener();
            tl.Filter = new System.Diagnostics.EventTypeFilter(SourceLevels.Information);
            tl.ChangeTextBoxValue = SetText;

            TraceSourceProvider.Source.Listeners.Add(tl);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!bgwRun.IsBusy)
            {
                bgwRun.RunWorkerAsync(txtInput.Text);
            }
        }

        private void bgwRun_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = e.Argument.ToString();

            _analysis.ExportBanggoAndTaobaoGoodsInfoBySearch(result); 
        }

        private void bgwRun_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            richTextBox1.AppendText("完成!");
        }
    }
}
