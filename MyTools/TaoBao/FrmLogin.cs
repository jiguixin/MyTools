using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace MyTools.TaoBao
{
    public partial class FrmLogin : Form
    {
        public string resultHtml;

        private string authorizeUrl;

        public FrmLogin(string authorizeUrlParm)
        {
            this.authorizeUrl = authorizeUrlParm;
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            resultHtml = webBrowser1.DocumentText;
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            webBrowser1.Navigate(authorizeUrl);
        }
    }
}
