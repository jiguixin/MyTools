namespace MyTools
{
    partial class FrmGoodsDelisting
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
            this.txtNumIds = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAssignDelisting = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
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
            this.btnOk.Location = new System.Drawing.Point(408, 12);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(107, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "按搜索条件下架";
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
            this.bgwRun.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwRun_RunWorkerCompleted);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(904, 345);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // txtNumIds
            // 
            this.txtNumIds.Location = new System.Drawing.Point(129, 42);
            this.txtNumIds.Multiline = true;
            this.txtNumIds.Name = "txtNumIds";
            this.txtNumIds.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtNumIds.Size = new System.Drawing.Size(255, 81);
            this.txtNumIds.TabIndex = 4;
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
            // btnAssignDelisting
            // 
            this.btnAssignDelisting.Location = new System.Drawing.Point(408, 71);
            this.btnAssignDelisting.Name = "btnAssignDelisting";
            this.btnAssignDelisting.Size = new System.Drawing.Size(75, 23);
            this.btnAssignDelisting.TabIndex = 6;
            this.btnAssignDelisting.Text = "指定下架";
            this.btnAssignDelisting.UseVisualStyleBackColor = true;
            this.btnAssignDelisting.Click += new System.EventHandler(this.btnAssignDelisting_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(65, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "商品编号:";
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
            this.splitContainer1.Panel1.Controls.Add(this.txtNumIds);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.txtSearch);
            this.splitContainer1.Panel1.Controls.Add(this.btnAssignDelisting);
            this.splitContainer1.Panel1.Controls.Add(this.btnOk);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.lblSearchCondition);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer1.Size = new System.Drawing.Size(904, 481);
            this.splitContainer1.SplitterDistance = 132;
            this.splitContainer1.TabIndex = 9;
            // 
            // FrmGoodsDelisting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 481);
            this.Controls.Add(this.splitContainer1);
            this.Name = "FrmGoodsDelisting";
            this.Text = "产品下架";
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
        private System.Windows.Forms.TextBox txtNumIds;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAssignDelisting;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}