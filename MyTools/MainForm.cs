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
using MyTools.TaoBao;
using MyTools.TaoBao.Impl;
using MyTools.TaoBao.Impl.Authorization;
using MyTools.TaoBao.Impl.NinjectModuleConfig;
using MyTools.TaoBao.Interface;
using MyTools.TaoBao.Interface.Authorization;
using Ninject;
using Top.Api;
using Top.Api.Util;
using MyTools.TaoBao.DomainModule;


namespace MyTools
{
    public partial class MainForm : Form
    {
        private int childFormNumber = 0;

        public MainForm()
        {
            InitializeComponent();
        }

        #region MyRegion
         
        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "窗口 " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
         
        #endregion

        #region var

        //App Key与App Secret在"应用证书"得到 
        private ITopClient client = InstanceLocator.Current.GetInstance<ITopClient>();

        private TopContext context;

        private IShopApi shopApi = InstanceLocator.Current.GetInstance<IShopApi>();

        private string authorizeUrl;
         
        private IAuthorization auth = InstanceLocator.Current.GetInstance<IAuthorization>();

        ILogger log = InstanceLocator.Current.GetInstance<ILoggerFactory>().Create();
           
        #endregion
          
        private void btnAuthorization_Click(object sender, EventArgs e)
        {
            log.LogInfo("正在执行验证方法-{0}", "btnAuthorization_Click");

            FrmLogin login = new FrmLogin(authorizeUrl);
            if (login.ShowDialog() == DialogResult.OK)
            {
                log.LogInfo("数据获取完成，结果为：{0}",login.resultHtml); 

                context = auth.Authorized(login.resultHtml);
                  
                InstanceLocator.Current.RegisterInstance<TopContext>(context);  
            }
        }

        private void btnGetCats_Click(object sender, EventArgs e)
        {
            var sellCatsList = shopApi.GetSellercatsList(context.UserNick);
            log.LogInfo("数据获取完成，卖家自定列表个数：{0}", sellCatsList.Count); 
        }

        private void MainForm_Load(object sender, EventArgs e)
        { 
            authorizeUrl = string.Format(Resource.SysConfig_AuthorizeUrl, SysConst.AppKey);
              
        }
    }
}
