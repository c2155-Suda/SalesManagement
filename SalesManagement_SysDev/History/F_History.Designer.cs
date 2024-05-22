namespace SalesManagement_SysDev
{
    partial class F_History
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
            this.panelHisSelect = new System.Windows.Forms.Panel();
            this.btnOpeHis = new System.Windows.Forms.Button();
            this.btnLoginHis = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelHisSelect
            // 
            this.panelHisSelect.Location = new System.Drawing.Point(152, 137);
            this.panelHisSelect.Margin = new System.Windows.Forms.Padding(2);
            this.panelHisSelect.Name = "panelHisSelect";
            this.panelHisSelect.Size = new System.Drawing.Size(186, 64);
            this.panelHisSelect.TabIndex = 14;
            // 
            // btnOpeHis
            // 
            this.btnOpeHis.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.btnOpeHis.Enabled = false;
            this.btnOpeHis.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnOpeHis.Location = new System.Drawing.Point(620, 515);
            this.btnOpeHis.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpeHis.Name = "btnOpeHis";
            this.btnOpeHis.Size = new System.Drawing.Size(450, 100);
            this.btnOpeHis.TabIndex = 13;
            this.btnOpeHis.Text = "操作履歴";
            this.btnOpeHis.UseVisualStyleBackColor = false;
            this.btnOpeHis.Click += new System.EventHandler(this.btnOpeHis_Click);
            // 
            // btnLoginHis
            // 
            this.btnLoginHis.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnLoginHis.Location = new System.Drawing.Point(620, 295);
            this.btnLoginHis.Margin = new System.Windows.Forms.Padding(2);
            this.btnLoginHis.Name = "btnLoginHis";
            this.btnLoginHis.Size = new System.Drawing.Size(450, 100);
            this.btnLoginHis.TabIndex = 12;
            this.btnLoginHis.Text = "ログイン履歴";
            this.btnLoginHis.UseVisualStyleBackColor = true;
            this.btnLoginHis.Click += new System.EventHandler(this.btnLoginHis_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20F);
            this.label1.Location = new System.Drawing.Point(1075, 577);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 27);
            this.label1.TabIndex = 15;
            this.label1.Text = "※開発中";
            // 
            // F_History
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1680, 890);
            this.Controls.Add(this.panelHisSelect);
            this.Controls.Add(this.btnOpeHis);
            this.Controls.Add(this.btnLoginHis);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_History";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_History";
            this.Load += new System.EventHandler(this.F_History_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelHisSelect;
        private System.Windows.Forms.Button btnOpeHis;
        private System.Windows.Forms.Button btnLoginHis;
        private System.Windows.Forms.Label label1;
    }
}