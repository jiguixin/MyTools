namespace MyTools
{
    partial class FrmUpdateGoodsSku
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
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.lblSearchCondition = new System.Windows.Forms.Label();
            this.bgwRun = new System.ComponentModel.BackgroundWorker();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chkNotModifyPrice = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRate = new System.Windows.Forms.TextBox();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.txtNewTitle = new System.Windows.Forms.TextBox();
            this.txtOriginalTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(129, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(255, 21);
            this.txtSearch.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(406, 12);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(107, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "按搜索条件更新";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lblSearchCondition
            // 
            this.lblSearchCondition.AutoSize = true;
            this.lblSearchCondition.Location = new System.Drawing.Point(65, 22);
            this.lblSearchCondition.Name = "lblSearchCondition";
            this.lblSearchCondition.Size = new System.Drawing.Size(59, 12);
            this.lblSearchCondition.TabIndex = 2;
            this.lblSearchCondition.Text = "查询条件:";
            // 
            // bgwRun
            // 
            this.bgwRun.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwRun_DoWork);
            this.bgwRun.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwRun_ProgressChanged);
            this.bgwRun.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwRun_RunWorkerCompleted);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(943, 331);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(12, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 41);
            this.label2.TabIndex = 7;
            this.label2.Text = "多个以 \",\" 号分割";
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
            this.splitContainer1.Panel1.Controls.Add(this.txtNewTitle);
            this.splitContainer1.Panel1.Controls.Add(this.txtOriginalTitle);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.txtStock);
            this.splitContainer1.Panel1.Controls.Add(this.txtRate);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.chkNotModifyPrice);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.txtSearch);
            this.splitContainer1.Panel1.Controls.Add(this.btnOk);
            this.splitContainer1.Panel1.Controls.Add(this.lblSearchCondition);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(943, 467);
            this.splitContainer1.SplitterDistance = 132;
            this.splitContainer1.TabIndex = 8;
            // 
            // chkNotModifyPrice
            // 
            this.chkNotModifyPrice.AutoSize = true;
            this.chkNotModifyPrice.Checked = true;
            this.chkNotModifyPrice.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNotModifyPrice.Location = new System.Drawing.Point(406, 53);
            this.chkNotModifyPrice.Name = "chkNotModifyPrice";
            this.chkNotModifyPrice.Size = new System.Drawing.Size(192, 16);
            this.chkNotModifyPrice.TabIndex = 8;
            this.chkNotModifyPrice.Text = "只更新库存不批量修改产品价格";
            this.chkNotModifyPrice.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "折扣比:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(241, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "初始库存:";
            // 
            // txtRate
            // 
            this.txtRate.Location = new System.Drawing.Point(130, 48);
            this.txtRate.Name = "txtRate";
            this.txtRate.Size = new System.Drawing.Size(84, 21);
            this.txtRate.TabIndex = 11;
            this.txtRate.Text = "0.68";
            // 
            // txtStock
            // 
            this.txtStock.Location = new System.Drawing.Point(306, 48);
            this.txtStock.Name = "txtStock";
            this.txtStock.Size = new System.Drawing.Size(78, 21);
            this.txtStock.TabIndex = 12;
            this.txtStock.Text = "3";
            // 
            // txtNewTitle
            // 
            this.txtNewTitle.Location = new System.Drawing.Point(305, 83);
            this.txtNewTitle.Name = "txtNewTitle";
            this.txtNewTitle.Size = new System.Drawing.Size(78, 21);
            this.txtNewTitle.TabIndex = 16;
            this.txtNewTitle.Text = "xx";
            // 
            // txtOriginalTitle
            // 
            this.txtOriginalTitle.Location = new System.Drawing.Point(129, 83);
            this.txtOriginalTitle.Name = "txtOriginalTitle";
            this.txtOriginalTitle.Size = new System.Drawing.Size(84, 21);
            this.txtOriginalTitle.TabIndex = 15;
            this.txtOriginalTitle.Text = "xx";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(240, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "新标题:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(76, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "原标题:";
            // 
            // FrmUpdateGoodsSku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 467);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmUpdateGoodsSku";
            this.Text = "更新库存";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblSearchCondition;
        private System.ComponentModel.BackgroundWorker bgwRun;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox chkNotModifyPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.TextBox txtRate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtNewTitle;
        private System.Windows.Forms.TextBox txtOriginalTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}