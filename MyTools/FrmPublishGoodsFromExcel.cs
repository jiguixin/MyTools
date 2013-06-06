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
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.Logging.TraceSource;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using MyTools.TaoBao.Interface;
using MyTools.Utility; 


namespace MyTools
{
    public partial class FrmPublishGoodsFromExcel : Form
    {
        IGoodsApi _goodsApi = InstanceLocator.Current.GetInstance<IGoodsApi>();

        delegate void ChangeTextBoxValue(string str); // 新增委托代理

        private void SetRichTextBoxValue(string str)
        {
            richTextBox1.AppendText(str);
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

        public FrmPublishGoodsFromExcel()
        {
            InitializeComponent();

            TextBoxTraceListener tl = new TextBoxTraceListener();
            tl.Filter = new System.Diagnostics.EventTypeFilter(SourceLevels.Information);
            tl.ChangeTextBoxValue = SetText;

            TraceSourceProvider.Source.Listeners.Add(tl);
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files(*.xls,*.xlsx)|*.xls;*.xlsx";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
                if (!bgwRun.IsBusy)
                {
                    bgwRun.RunWorkerAsync(ofd.FileName);
                }
            } 
        }

        private void bgwRun_DoWork(object sender, DoWorkEventArgs e)
        {
            if (e.Argument.IsNull())
                return;

            var fileName = e.Argument.ToString();

            _goodsApi.PublishGoodsFromExcel(fileName);
        }

        private void bgwRun_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            richTextBox1.AppendText("完成!");
        }
    }
}
