namespace MyTools
{
    partial class FrmPublishGoods
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnPublish = new System.Windows.Forms.Button();
            this.txtUrls = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.bgwRun = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnPublish);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtLog);
            this.splitContainer1.Panel2.Controls.Add(this.txtUrls);
            this.splitContainer1.Size = new System.Drawing.Size(874, 467);
            this.splitContainer1.SplitterDistance = 28;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnPublish
            // 
            this.btnPublish.Location = new System.Drawing.Point(316, 3);
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(75, 23);
            this.btnPublish.TabIndex = 0;
            this.btnPublish.Text = "button1";
            this.btnPublish.UseVisualStyleBackColor = true;
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // txtUrls
            // 
            this.txtUrls.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtUrls.Location = new System.Drawing.Point(0, 0);
            this.txtUrls.Multiline = true;
            this.txtUrls.Name = "txtUrls";
            this.txtUrls.Size = new System.Drawing.Size(874, 185);
            this.txtUrls.TabIndex = 0;
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtLog.Location = new System.Drawing.Point(0, 191);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(874, 244);
            this.txtLog.TabIndex = 1;
            // 
            // bgwRun
            // 
            this.bgwRun.WorkerReportsProgress = true;
            this.bgwRun.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwRun_DoWork);
            this.bgwRun.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwRun_ProgressChanged);
            this.bgwRun.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwRun_RunWorkerCompleted);
            // 
            // FrmPublishGoods
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 467);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmPublishGoods";
            this.Text = "FrmPublishGoods";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnPublish;
        private System.Windows.Forms.TextBox txtUrls;
        private System.Windows.Forms.TextBox txtLog;
        private System.ComponentModel.BackgroundWorker bgwRun;
    }
}