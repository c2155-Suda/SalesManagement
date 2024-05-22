namespace SalesManagement_SysDev
{
    partial class F_MainOffice
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
            this.panelMainOffice = new System.Windows.Forms.Panel();
            this.btnSale = new System.Windows.Forms.Button();
            this.btnClient = new System.Windows.Forms.Button();
            this.btnEmployee = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panelMainOffice
            // 
            this.panelMainOffice.Location = new System.Drawing.Point(92, 156);
            this.panelMainOffice.Margin = new System.Windows.Forms.Padding(2);
            this.panelMainOffice.Name = "panelMainOffice";
            this.panelMainOffice.Size = new System.Drawing.Size(216, 48);
            this.panelMainOffice.TabIndex = 12;
            // 
            // btnSale
            // 
            this.btnSale.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSale.Location = new System.Drawing.Point(660, 595);
            this.btnSale.Margin = new System.Windows.Forms.Padding(2);
            this.btnSale.Name = "btnSale";
            this.btnSale.Size = new System.Drawing.Size(360, 100);
            this.btnSale.TabIndex = 11;
            this.btnSale.Text = "売上管理";
            this.btnSale.UseVisualStyleBackColor = true;
            this.btnSale.Click += new System.EventHandler(this.btnSale_Click);
            // 
            // btnClient
            // 
            this.btnClient.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnClient.Location = new System.Drawing.Point(660, 395);
            this.btnClient.Margin = new System.Windows.Forms.Padding(2);
            this.btnClient.Name = "btnClient";
            this.btnClient.Size = new System.Drawing.Size(360, 100);
            this.btnClient.TabIndex = 10;
            this.btnClient.Text = "顧客管理";
            this.btnClient.UseVisualStyleBackColor = true;
            this.btnClient.Click += new System.EventHandler(this.btnClient_Click);
            // 
            // btnEmployee
            // 
            this.btnEmployee.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnEmployee.Location = new System.Drawing.Point(660, 195);
            this.btnEmployee.Margin = new System.Windows.Forms.Padding(2);
            this.btnEmployee.Name = "btnEmployee";
            this.btnEmployee.Size = new System.Drawing.Size(360, 100);
            this.btnEmployee.TabIndex = 9;
            this.btnEmployee.Text = "社員管理";
            this.btnEmployee.UseVisualStyleBackColor = true;
            this.btnEmployee.Click += new System.EventHandler(this.btnEmployee_Click);
            // 
            // F_MainOffice
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1680, 890);
            this.Controls.Add(this.panelMainOffice);
            this.Controls.Add(this.btnSale);
            this.Controls.Add(this.btnClient);
            this.Controls.Add(this.btnEmployee);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_MainOffice";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.F_MainOffice_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMainOffice;
        private System.Windows.Forms.Button btnSale;
        private System.Windows.Forms.Button btnClient;
        private System.Windows.Forms.Button btnEmployee;
    }
}