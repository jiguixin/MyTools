using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using RestSharp;

namespace MyTools
{
    public partial class FrmInputVerifyCode : Form
    {
        private RestClient client;

        public FrmInputVerifyCode(RestClient client)
        {
            this.client = client;
            InitializeComponent();
        }

        public string VerifyCode { get; private set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.VerifyCode = txtInputCode.Text.Trim();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void Reload()
        {
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "image/png,image/*;q=0.8,*/*;q=0.5");
            request.AddHeader("Accept-Encoding", "gzip, deflate");
            request.AddHeader("Host", "passport.banggo.com");
            request.AddHeader("Referer",
                              "https://passport.banggo.com/CASServer/login?service=http%3A%2F%2Fact.banggo.com%2FUser%2Flogin.shtml%3Freturn_url%3Dhttp%253A%252F%252Fwww.banggo.com%252F");
            request.AddHeader("User-Agent",
                              "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31");
            request.AddHeader("Accept-Language", "zh-cn,zh;q=0.8,en-us;q=0.5,en;q=0.3");
            client.BaseUrl = "https://passport.banggo.com/CASServer/ImageServlet";
            var response = client.Execute(request);
            byte[] imageBytes = response.RawBytes;

            var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            pictureBox1.Image = Image.FromStream(ms);

            ms.Close();
        }

        private void FrmInputVerifyCode_Load(object sender, EventArgs e)
        {
            Reload();
        }

    }
}
