namespace MyTools
{
    partial class FrmUpdateGoodsFromOnSale
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
            this.label1 = new System.Windows.Forms.Label();
            this.btnAssignUpdate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(166, 8);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(255, 21);
            this.txtSearch.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(445, 5);
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
            this.lblSearchCondition.Location = new System.Drawing.Point(101, 14);
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
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.Location = new System.Drawing.Point(0, 122);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(943, 345);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // txtNumIds
            // 
            this.txtNumIds.Location = new System.Drawing.Point(166, 35);
            this.txtNumIds.Multiline = true;
            this.txtNumIds.Name = "txtNumIds";
            this.txtNumIds.Size = new System.Drawing.Size(255, 81);
            this.txtNumIds.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(101, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "商品编号:";
            // 
            // btnAssignUpdate
            // 
            this.btnAssignUpdate.Location = new System.Drawing.Point(445, 64);
            this.btnAssignUpdate.Name = "btnAssignUpdate";
            this.btnAssignUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnAssignUpdate.TabIndex = 6;
            this.btnAssignUpdate.Text = "指定更新";
            this.btnAssignUpdate.UseVisualStyleBackColor = true;
            this.btnAssignUpdate.Click += new System.EventHandler(this.btnAssignUpdate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(101, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "以\",\"分割";
            // 
            // FrmUpdateGoodsFromOnSale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 467);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAssignUpdate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNumIds);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.lblSearchCondition);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtSearch);
            this.Name = "FrmUpdateGoodsFromOnSale";
            this.Text = "更新在线商品";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label lblSearchCondition;
        private System.ComponentModel.BackgroundWorker bgwRun;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox txtNumIds;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAssignUpdate;
        private System.Windows.Forms.Label label2;
    }
}