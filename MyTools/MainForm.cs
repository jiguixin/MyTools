using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infrastructure.Crosscutting.Declaration;
using Infrastructure.Crosscutting.IoC;
using Infrastructure.Crosscutting.Logging;
using Infrastructure.Crosscutting.Utility;
using Infrastructure.Crosscutting.Utility.CommomHelper;
using MyTools.Framework.Common;
using MyTools.TaoBao;
using MyTools.TaoBao.Impl;
using MyTools.TaoBao.Impl.NinjectModuleConfig;
using MyTools.TaoBao.Interface;
using Newtonsoft.Json;
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


        private readonly IGoodsApi _goodsApi = InstanceLocator.Current.GetInstance<IGoodsApi>();

        private IShopApi shopApi = InstanceLocator.Current.GetInstance<IShopApi>();
         
        private string authorizeUrl;
         
        

        ILogger _log = InstanceLocator.Current.GetInstance<ILoggerFactory>().Create();

        private ISell _sell = InstanceLocator.Current.GetInstance<ISell>();

        private IBanggoMgt _banggoMgt = InstanceLocator.Current.GetInstance<IBanggoMgt>();
        #endregion
          
        private void btnAuthorization_Click(object sender, EventArgs e)
        {
            _log.LogInfo("正在执行验证方法-{0}", "btnAuthorization_Click");

            FrmLogin login = new FrmLogin(authorizeUrl);
            login.MdiParent = this;
            login.Show(); 
        }
         

        private void MainForm_Load(object sender, EventArgs e)
        {
            authorizeUrl = Resource.SysConfig_AuthorizeUrl.StringFormat(SysConst.AppKey);

            this.Text  += " [{0}] ".StringFormat(SysConst.AppLoginUser);
        }

        private void btnPublishProduct_Click(object sender, EventArgs e)
        {
            FrmPublishGoods frm = new FrmPublishGoods();
            frm.MdiParent = this; 
            frm.Show(); 
        }

        private void btnSetAlpha_Click(object sender, EventArgs e)
        {
            WindowsApiHelper.SetWindowsOpacity("Shell_TrayWnd"); //Shell_TrayWnd 任务栏 
            WindowsApiHelper.SetWindowsOpacity("StandardWindow"); //工作台
            WindowsApiHelper.SetWindowsOpacity("StandardFrame");   //主窗口
            WindowsApiHelper.SetWindowsOpacity(null, "系统消息");//系统提示消息 
        }

        private void btnRestoreAlpha_Click(object sender, EventArgs e)
        {
            WindowsApiHelper.RestoreOpacity("StandardWindow");//恢复工作台
            WindowsApiHelper.RestoreOpacity("StandardFrame");//恢复主窗口
            WindowsApiHelper.RestoreOpacity("Shell_TrayWnd"); //恢复任务栏 
            WindowsApiHelper.RestoreOpacity(null, "系统消息");
        }

        private void btnPublishGoodsFromExcel_Click(object sender, EventArgs e)
        {  
            var frm = new FrmPublishGoodsFromExcel();
            frm.MdiParent = this;
            frm.Show();
        }

        private void btnUpdateGoodsFormOnSale_Click(object sender, EventArgs e)
        {
            var frm = new FrmUpdateGoodsFromOnSale();
            frm.MdiParent = this; 
            frm.Show();
        }

        private void btnExportSellDetail_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel Files(*.xls,*.xlsx)|*.xls;*.xlsx";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _sell.ExportSellDetail(ofd.FileName);
            }
        }

        private void btnExportRivalSaleDetail_Click(object sender, EventArgs e)
        {
            FrmExportBanggoAndTaobaoGoodsInfo frm = new FrmExportBanggoAndTaobaoGoodsInfo();
            frm.MdiParent = this;
            frm.Show();
        }

        private void bntSingIn_Click(object sender, EventArgs e)
        { 
            string fileName = "SingInUser.txt";

            if (!File.Exists(fileName))
            {
                var settings = new JsonSerializerSettings();

                var lst = InitBanggoUsers();

                string result = JsonConvert.SerializeObject(lst, Formatting.Indented, settings);

                File.WriteAllText(fileName, result);
            }

            var lstDes = JsonConvert.DeserializeObject<List<BanggoUser>>(File.ReadAllText(fileName));


            if (!bgwRunSingIn.IsBusy)
            {
                bgwRunSingIn.RunWorkerAsync(lstDes);
            } 
        }


        #region 签到多线程

        private void bgwRunSingIn_DoWork(object sender, DoWorkEventArgs e)
        {
            var lstDes = (List<BanggoUser>)e.Argument;

            if (lstDes == null)
                return;

            foreach (var banggoUser in lstDes)
            {
                _banggoMgt.SingIn(banggoUser.UserName, banggoUser.Password);
                bgwRunSingIn.ReportProgress(100, "{0}->正在签到!".StringFormat(banggoUser.UserName));
                System.Threading.Thread.Sleep(10000);
            }
        }

        private void bgwRunSingIn_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is string)
            {
                toolStripStatusLabel.Text = e.UserState.ToString();
            }
        }

        private void bgwRunSingIn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabel.Text = "签到结束";
        }

        #endregion

        private static List<BanggoUser> InitBanggoUsers()
        {
            var lst = new List<BanggoUser>();
            lst.Add(new BanggoUser {UserName = "a00620u3783", Password = "c15881169733"});
            lst.Add(new BanggoUser() {UserName = "娟娟猪", Password = "040192"});
            lst.Add(new BanggoUser() {UserName = "佳由小翻", Password = "fan1921"});
            lst.Add(new BanggoUser() {UserName = "尹秋菊", Password = "131375asd"});
            lst.Add(new BanggoUser() {UserName = "张梅zm", Password = "zhangmei"});
            lst.Add(new BanggoUser() {UserName = "魏华", Password = "weihua"});
            lst.Add(new BanggoUser() {UserName = "张均翠", Password = "jiguixin"});
            lst.Add(new BanggoUser() {UserName = "CDMB付家秀", Password = "198911"});
            lst.Add(new BanggoUser() { UserName = "段东梅", Password = "157160" });
            lst.Add(new BanggoUser() { UserName = "廖小梅1", Password = "118205" });
            lst.Add(new BanggoUser() { UserName = "朱微", Password = "ZHUWEI1" });
            return lst;
        }

        private void btnJfExchange_Click(object sender, EventArgs e)
        {
            string fileName = "JfExchangeUser.txt";

            if (!File.Exists(fileName))
            {
                var settings = new JsonSerializerSettings();

                var lst = InitBanggoUsers();
                 
                string result = JsonConvert.SerializeObject(lst, Formatting.Indented, settings);

                File.WriteAllText(fileName, result);
            }

            var lstDes = JsonConvert.DeserializeObject<List<BanggoUser>>(File.ReadAllText(fileName));

            foreach (var user in lstDes)
            {
                _banggoMgt.JfExchange(user.UserName, user.Password);
                System.Threading.Thread.Sleep(2000); 
            }

        }

    }
}
