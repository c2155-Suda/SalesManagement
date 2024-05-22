namespace SalesManagement_SysDev
{
    partial class F_Logistics
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
            this.panelLogistics = new System.Windows.Forms.Panel();
            this.btnWarehousing = new System.Windows.Forms.Button();
            this.btnSyukko = new System.Windows.Forms.Button();
            this.btnHattyu = new System.Windows.Forms.Button();
            this.btnChumon = new System.Windows.Forms.Button();
            this.btnProduct = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panelLogistics
            // 
            this.panelLogistics.Location = new System.Drawing.Point(30, 67);
            this.panelLogistics.Margin = new System.Windows.Forms.Padding(2);
            this.panelLogistics.Name = "panelLogistics";
            this.panelLogistics.Size = new System.Drawing.Size(219, 56);
            this.panelLogistics.TabIndex = 20;
            // 
            // btnWarehousing
            // 
            this.btnWarehousing.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnWarehousing.Location = new System.Drawing.Point(920, 195);
            this.btnWarehousing.Margin = new System.Windows.Forms.Padding(2);
            this.btnWarehousing.Name = "btnWarehousing";
            this.btnWarehousing.Size = new System.Drawing.Size(360, 100);
            this.btnWarehousing.TabIndex = 19;
            this.btnWarehousing.Text = "入庫管理";
            this.btnWarehousing.UseVisualStyleBackColor = true;
            this.btnWarehousing.Click += new System.EventHandler(this.btnWarehousing_Click);
            // 
            // btnSyukko
            // 
            this.btnSyukko.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSyukko.Location = new System.Drawing.Point(920, 395);
            this.btnSyukko.Margin = new System.Windows.Forms.Padding(2);
            this.btnSyukko.Name = "btnSyukko";
            this.btnSyukko.Size = new System.Drawing.Size(360, 100);
            this.btnSyukko.TabIndex = 18;
            this.btnSyukko.Text = "出庫管理";
            this.btnSyukko.UseVisualStyleBackColor = true;
            this.btnSyukko.Click += new System.EventHandler(this.btnSyukko_Click);
            // 
            // btnHattyu
            // 
            this.btnHattyu.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnHattyu.Location = new System.Drawing.Point(400, 595);
            this.btnHattyu.Margin = new System.Windows.Forms.Padding(2);
            this.btnHattyu.Name = "btnHattyu";
            this.btnHattyu.Size = new System.Drawing.Size(360, 100);
            this.btnHattyu.TabIndex = 17;
            this.btnHattyu.Text = "発注管理";
            this.btnHattyu.UseVisualStyleBackColor = true;
            this.btnHattyu.Click += new System.EventHandler(this.btnHattyu_Click);
            // 
            // btnChumon
            // 
            this.btnChumon.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnChumon.Location = new System.Drawing.Point(400, 395);
            this.btnChumon.Margin = new System.Windows.Forms.Padding(2);
            this.btnChumon.Name = "btnChumon";
            this.btnChumon.Size = new System.Drawing.Size(360, 100);
            this.btnChumon.TabIndex = 16;
            this.btnChumon.Text = "注文管理";
            this.btnChumon.UseVisualStyleBackColor = true;
            this.btnChumon.Click += new System.EventHandler(this.btnChumon_Click);
            // 
            // btnProduct
            // 
            this.btnProduct.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnProduct.Location = new System.Drawing.Point(400, 195);
            this.btnProduct.Margin = new System.Windows.Forms.Padding(2);
            this.btnProduct.Name = "btnProduct";
            this.btnProduct.Size = new System.Drawing.Size(360, 100);
            this.btnProduct.TabIndex = 15;
            this.btnProduct.Text = "商品管理";
            this.btnProduct.UseVisualStyleBackColor = true;
            this.btnProduct.Click += new System.EventHandler(this.btnProduct_Click);
            // 
            // F_Logistics
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1680, 890);
            this.Controls.Add(this.panelLogistics);
            this.Controls.Add(this.btnWarehousing);
            this.Controls.Add(this.btnSyukko);
            this.Controls.Add(this.btnHattyu);
            this.Controls.Add(this.btnChumon);
            this.Controls.Add(this.btnProduct);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_Logistics";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_Logistics";
            this.Load += new System.EventHandler(this.F_Logistics_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLogistics;
        private System.Windows.Forms.Button btnWarehousing;
        private System.Windows.Forms.Button btnSyukko;
        private System.Windows.Forms.Button btnHattyu;
        private System.Windows.Forms.Button btnChumon;
        private System.Windows.Forms.Button btnProduct;
    }
}