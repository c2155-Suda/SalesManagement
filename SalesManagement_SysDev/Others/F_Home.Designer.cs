namespace SalesManagement_SysDev
{
    partial class F_Home
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
            this.lblHome = new System.Windows.Forms.Label();
            this.panelBar = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnScreenLock = new System.Windows.Forms.Button();
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnLogistics = new System.Windows.Forms.Button();
            this.btnSalesOffice = new System.Windows.Forms.Button();
            this.btnMainOffice = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.lblSoName = new System.Windows.Forms.Label();
            this.lblEmName = new System.Windows.Forms.Label();
            this.lblLogHis = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelScreen = new System.Windows.Forms.Panel();
            this.panelBar.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHome
            // 
            this.lblHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.lblHome.Font = new System.Drawing.Font("Showcard Gothic", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHome.ForeColor = System.Drawing.Color.Purple;
            this.lblHome.Location = new System.Drawing.Point(0, 0);
            this.lblHome.Name = "lblHome";
            this.lblHome.Size = new System.Drawing.Size(220, 150);
            this.lblHome.TabIndex = 0;
            this.lblHome.Text = "OM21";
            this.lblHome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHome.Click += new System.EventHandler(this.lblHome_Click);
            // 
            // panelBar
            // 
            this.panelBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panelBar.Controls.Add(this.btnClose);
            this.panelBar.Controls.Add(this.btnScreenLock);
            this.panelBar.Controls.Add(this.btnHistory);
            this.panelBar.Controls.Add(this.btnLogin);
            this.panelBar.Controls.Add(this.btnLogistics);
            this.panelBar.Controls.Add(this.btnSalesOffice);
            this.panelBar.Controls.Add(this.btnMainOffice);
            this.panelBar.Location = new System.Drawing.Point(0, 150);
            this.panelBar.Name = "panelBar";
            this.panelBar.Size = new System.Drawing.Size(220, 892);
            this.panelBar.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(20, 780);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(180, 70);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "終了";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnScreenLock
            // 
            this.btnScreenLock.BackColor = System.Drawing.Color.White;
            this.btnScreenLock.Location = new System.Drawing.Point(20, 680);
            this.btnScreenLock.Name = "btnScreenLock";
            this.btnScreenLock.Size = new System.Drawing.Size(180, 70);
            this.btnScreenLock.TabIndex = 5;
            this.btnScreenLock.Text = "ロック🔒";
            this.btnScreenLock.UseVisualStyleBackColor = false;
            this.btnScreenLock.Click += new System.EventHandler(this.btnScreenLock_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.BackColor = System.Drawing.Color.White;
            this.btnHistory.Location = new System.Drawing.Point(20, 550);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(180, 100);
            this.btnHistory.TabIndex = 4;
            this.btnHistory.Text = "履歴";
            this.btnHistory.UseVisualStyleBackColor = false;
            this.btnHistory.Click += new System.EventHandler(this.btnHistory_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.White;
            this.btnLogin.Location = new System.Drawing.Point(20, 420);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(180, 100);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "ログイン";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnLogistics
            // 
            this.btnLogistics.BackColor = System.Drawing.Color.White;
            this.btnLogistics.Location = new System.Drawing.Point(20, 290);
            this.btnLogistics.Name = "btnLogistics";
            this.btnLogistics.Size = new System.Drawing.Size(180, 100);
            this.btnLogistics.TabIndex = 2;
            this.btnLogistics.Text = "物流";
            this.btnLogistics.UseVisualStyleBackColor = false;
            this.btnLogistics.Click += new System.EventHandler(this.btnLogistics_Click);
            // 
            // btnSalesOffice
            // 
            this.btnSalesOffice.BackColor = System.Drawing.Color.White;
            this.btnSalesOffice.Location = new System.Drawing.Point(20, 160);
            this.btnSalesOffice.Name = "btnSalesOffice";
            this.btnSalesOffice.Size = new System.Drawing.Size(180, 100);
            this.btnSalesOffice.TabIndex = 1;
            this.btnSalesOffice.Text = "営業所";
            this.btnSalesOffice.UseVisualStyleBackColor = false;
            this.btnSalesOffice.Click += new System.EventHandler(this.btnSalesOffice_Click);
            // 
            // btnMainOffice
            // 
            this.btnMainOffice.BackColor = System.Drawing.Color.White;
            this.btnMainOffice.Location = new System.Drawing.Point(20, 30);
            this.btnMainOffice.Name = "btnMainOffice";
            this.btnMainOffice.Size = new System.Drawing.Size(180, 100);
            this.btnMainOffice.TabIndex = 0;
            this.btnMainOffice.Text = "本社";
            this.btnMainOffice.UseVisualStyleBackColor = false;
            this.btnMainOffice.Click += new System.EventHandler(this.btnMainOffice_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Location = new System.Drawing.Point(247, 586);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(8, 8);
            this.propertyGrid1.TabIndex = 2;
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panelTitle.Controls.Add(this.lblDateTime);
            this.panelTitle.Controls.Add(this.lblSoName);
            this.panelTitle.Controls.Add(this.lblEmName);
            this.panelTitle.Controls.Add(this.lblLogHis);
            this.panelTitle.Controls.Add(this.lblTitle);
            this.panelTitle.Location = new System.Drawing.Point(220, 0);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(1687, 150);
            this.panelTitle.TabIndex = 3;
            // 
            // lblDateTime
            // 
            this.lblDateTime.AutoSize = true;
            this.lblDateTime.BackColor = System.Drawing.Color.Transparent;
            this.lblDateTime.Location = new System.Drawing.Point(1308, 117);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(180, 27);
            this.lblDateTime.TabIndex = 4;
            this.lblDateTime.Text = "ログイン日時：";
            // 
            // lblSoName
            // 
            this.lblSoName.AutoSize = true;
            this.lblSoName.BackColor = System.Drawing.Color.Transparent;
            this.lblSoName.Location = new System.Drawing.Point(1377, 90);
            this.lblSoName.Name = "lblSoName";
            this.lblSoName.Size = new System.Drawing.Size(111, 27);
            this.lblSoName.TabIndex = 3;
            this.lblSoName.Text = "営業所：";
            // 
            // lblEmName
            // 
            this.lblEmName.AutoSize = true;
            this.lblEmName.BackColor = System.Drawing.Color.Transparent;
            this.lblEmName.Location = new System.Drawing.Point(1280, 63);
            this.lblEmName.Name = "lblEmName";
            this.lblEmName.Size = new System.Drawing.Size(208, 27);
            this.lblEmName.TabIndex = 2;
            this.lblEmName.Text = "ログイン社員名：";
            // 
            // lblLogHis
            // 
            this.lblLogHis.AutoSize = true;
            this.lblLogHis.BackColor = System.Drawing.Color.Transparent;
            this.lblLogHis.Location = new System.Drawing.Point(1281, 36);
            this.lblLogHis.Name = "lblLogHis";
            this.lblLogHis.Size = new System.Drawing.Size(207, 27);
            this.lblLogHis.TabIndex = 1;
            this.lblLogHis.Text = "ログイン認証ID：";
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTitle.Location = new System.Drawing.Point(6, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1678, 127);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Home";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelScreen
            // 
            this.panelScreen.Location = new System.Drawing.Point(220, 150);
            this.panelScreen.Name = "panelScreen";
            this.panelScreen.Size = new System.Drawing.Size(1684, 892);
            this.panelScreen.TabIndex = 4;
            // 
            // F_Home
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.panelScreen);
            this.Controls.Add(this.panelTitle);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.panelBar);
            this.Controls.Add(this.lblHome);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "F_Home";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OM21システム開発演習";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.F_Home_FormClosing);
            this.Load += new System.EventHandler(this.F_Home_Load);
            this.panelBar.ResumeLayout(false);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblHome;
        private System.Windows.Forms.Panel panelBar;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button btnMainOffice;
        private System.Windows.Forms.Button btnSalesOffice;
        private System.Windows.Forms.Button btnLogistics;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnScreenLock;
        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelScreen;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label lblSoName;
        private System.Windows.Forms.Label lblEmName;
        private System.Windows.Forms.Label lblLogHis;
        private System.Windows.Forms.Button btnClose;
    }
}