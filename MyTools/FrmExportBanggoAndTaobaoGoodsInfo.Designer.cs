namespace MyTools
{
    partial class FrmExportBanggoAndTaobaoGoodsInfo
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
            this.txtInput = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.bgwRun = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.txtInput);
            this.splitContainer1.Panel1.Controls.Add(this.btnExport);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(852, 420);
            this.splitContainer1.SplitterDistance = 65;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(214, 13);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(300, 21);
            this.txtInput.TabIndex = 1;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(523, 11);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 0;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(852, 351);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // bgwRun
            // 
            this.bgwRun.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwRun_DoWork);
            this.bgwRun.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwRun_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(240, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "输入要导出的条件，多个条件用“,”号分割。";
            // 
            // FrmExportBanggoAndTaobaoGoodsInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 420);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmExportBanggoAndTaobaoGoodsInfo";
            this.Text = "FrmExportBanggoAndTaobaoGoodsInfo";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.ComponentModel.BackgroundWorker bgwRun;
        private System.Windows.Forms.Label label1;
    }
}