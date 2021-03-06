﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Infrastructure.Crosscutting.IoC;
using MyTools.TaoBao.DomainModule;
using MyTools.TaoBao.Interface;
using Top.Api.Util;


namespace MyTools.TaoBao
{
    public partial class FrmLogin : Form
    {
        public string resultHtml;

        private string authorizeUrl;

        private ICommonApi _comApi = InstanceLocator.Current.GetInstance<ICommonApi>();
          
        public FrmLogin(string authorizeUrlParm)
        {
            this.authorizeUrl = authorizeUrlParm;
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //            resultHtml = webBrowser1.DocumentText; 
            //              
            // context = _comApi.Authorized(resultHtml); 

            var context = TopUtils.GetTopContext(txtAuthCode.Text);

            InstanceLocator.Current.RegisterInstance<AuthorizedContext>(new AuthorizedContext()
                {
                    AppKey = context.AppKey,
                    SessionKey = context.SessionKey,
                    UserId = context.UserId.ToString(),
                    UserNick = context.UserNick
                });

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
