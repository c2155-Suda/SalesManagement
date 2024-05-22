namespace SalesManagement_SysDev
{
    partial class F_SalesOffice
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
            this.panelSalesOffice = new System.Windows.Forms.Panel();
            this.btnShipment = new System.Windows.Forms.Button();
            this.btnArrival = new System.Windows.Forms.Button();
            this.btnOrder = new System.Windows.Forms.Button();
            this.btnChumon = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panelSalesOffice
            // 
            this.panelSalesOffice.Location = new System.Drawing.Point(106, 149);
            this.panelSalesOffice.Margin = new System.Windows.Forms.Padding(2);
            this.panelSalesOffice.Name = "panelSalesOffice";
            this.panelSalesOffice.Size = new System.Drawing.Size(227, 98);
            this.panelSalesOffice.TabIndex = 15;
            // 
            // btnShipment
            // 
            this.btnShipment.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnShipment.Location = new System.Drawing.Point(660, 680);
            this.btnShipment.Margin = new System.Windows.Forms.Padding(2);
            this.btnShipment.Name = "btnShipment";
            this.btnShipment.Size = new System.Drawing.Size(360, 100);
            this.btnShipment.TabIndex = 14;
            this.btnShipment.Text = "出荷管理";
            this.btnShipment.UseVisualStyleBackColor = true;
            this.btnShipment.Click += new System.EventHandler(this.btnShipment_Click);
            // 
            // btnArrival
            // 
            this.btnArrival.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnArrival.Location = new System.Drawing.Point(660, 500);
            this.btnArrival.Margin = new System.Windows.Forms.Padding(2);
            this.btnArrival.Name = "btnArrival";
            this.btnArrival.Size = new System.Drawing.Size(360, 100);
            this.btnArrival.TabIndex = 13;
            this.btnArrival.Text = "入荷管理";
            this.btnArrival.UseVisualStyleBackColor = true;
            this.btnArrival.Click += new System.EventHandler(this.btnArrival_Click);
            // 
            // btnOrder
            // 
            this.btnOrder.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnOrder.Location = new System.Drawing.Point(660, 140);
            this.btnOrder.Margin = new System.Windows.Forms.Padding(2);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(360, 100);
            this.btnOrder.TabIndex = 12;
            this.btnOrder.Text = "受注管理";
            this.btnOrder.UseVisualStyleBackColor = true;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // btnChumon
            // 
            this.btnChumon.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnChumon.Location = new System.Drawing.Point(660, 320);
            this.btnChumon.Margin = new System.Windows.Forms.Padding(2);
            this.btnChumon.Name = "btnChumon";
            this.btnChumon.Size = new System.Drawing.Size(360, 100);
            this.btnChumon.TabIndex = 18;
            this.btnChumon.Text = "注文管理";
            this.btnChumon.UseVisualStyleBackColor = true;
            this.btnChumon.Click += new System.EventHandler(this.btnChumon_Click);
            // 
            // F_SalesOffice
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1680, 890);
            this.Controls.Add(this.panelSalesOffice);
            this.Controls.Add(this.btnShipment);
            this.Controls.Add(this.btnArrival);
            this.Controls.Add(this.btnChumon);
            this.Controls.Add(this.btnOrder);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_SalesOffice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.F_SalesOffice_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelSalesOffice;
        private System.Windows.Forms.Button btnShipment;
        private System.Windows.Forms.Button btnArrival;
        private System.Windows.Forms.Button btnOrder;
        private System.Windows.Forms.Button btnChumon;
    }
}