namespace SalesManagement_SysDev
{
    partial class F_LoginHistory
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
            this.cmbSalesOfficeId = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewDsp = new System.Windows.Forms.DataGridView();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.cmbLogHisId = new System.Windows.Forms.ComboBox();
            this.cmbPosition = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePickerS = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerE = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSea = new System.Windows.Forms.Button();
            this.lblPage = new System.Windows.Forms.Label();
            this.textBoxPageNo = new System.Windows.Forms.TextBox();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPreviousPage = new System.Windows.Forms.Button();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.cmbEmployeeId = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDsp)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbSalesOfficeId
            // 
            this.cmbSalesOfficeId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSalesOfficeId.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbSalesOfficeId.FormattingEnabled = true;
            this.cmbSalesOfficeId.Location = new System.Drawing.Point(752, 269);
            this.cmbSalesOfficeId.Margin = new System.Windows.Forms.Padding(2);
            this.cmbSalesOfficeId.Name = "cmbSalesOfficeId";
            this.cmbSalesOfficeId.Size = new System.Drawing.Size(315, 35);
            this.cmbSalesOfficeId.TabIndex = 160;
            this.cmbSalesOfficeId.SelectedIndexChanged += new System.EventHandler(this.cmbSalesOfficeId_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(150, 272);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 27);
            this.label4.TabIndex = 157;
            this.label4.Text = "社員名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(628, 272);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 27);
            this.label3.TabIndex = 156;
            this.label3.Text = "営業所名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(1159, 272);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 27);
            this.label2.TabIndex = 155;
            this.label2.Text = "役職";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(59, 208);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 27);
            this.label1.TabIndex = 154;
            this.label1.Text = "ログイン履歴ID";
            // 
            // dataGridViewDsp
            // 
            this.dataGridViewDsp.AllowUserToAddRows = false;
            this.dataGridViewDsp.AllowUserToDeleteRows = false;
            this.dataGridViewDsp.AllowUserToResizeColumns = false;
            this.dataGridViewDsp.AllowUserToResizeRows = false;
            this.dataGridViewDsp.ColumnHeadersHeight = 25;
            this.dataGridViewDsp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewDsp.Location = new System.Drawing.Point(64, 380);
            this.dataGridViewDsp.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewDsp.Name = "dataGridViewDsp";
            this.dataGridViewDsp.ReadOnly = true;
            this.dataGridViewDsp.RowHeadersWidth = 62;
            this.dataGridViewDsp.RowTemplate.Height = 27;
            this.dataGridViewDsp.ShowCellToolTips = false;
            this.dataGridViewDsp.ShowEditingIcon = false;
            this.dataGridViewDsp.Size = new System.Drawing.Size(1560, 426);
            this.dataGridViewDsp.TabIndex = 150;
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnClear.Location = new System.Drawing.Point(1474, 74);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(150, 80);
            this.btnClear.TabIndex = 149;
            this.btnClear.Text = "入力クリア";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnView
            // 
            this.btnView.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnView.Location = new System.Drawing.Point(1229, 74);
            this.btnView.Margin = new System.Windows.Forms.Padding(2);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(150, 80);
            this.btnView.TabIndex = 148;
            this.btnView.Text = "一覧表示";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // cmbLogHisId
            // 
            this.cmbLogHisId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogHisId.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbLogHisId.FormattingEnabled = true;
            this.cmbLogHisId.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmbLogHisId.Location = new System.Drawing.Point(247, 204);
            this.cmbLogHisId.Margin = new System.Windows.Forms.Padding(2);
            this.cmbLogHisId.MaxLength = 6;
            this.cmbLogHisId.Name = "cmbLogHisId";
            this.cmbLogHisId.Size = new System.Drawing.Size(315, 35);
            this.cmbLogHisId.TabIndex = 169;
            // 
            // cmbPosition
            // 
            this.cmbPosition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPosition.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbPosition.FormattingEnabled = true;
            this.cmbPosition.Location = new System.Drawing.Point(1229, 269);
            this.cmbPosition.Margin = new System.Windows.Forms.Padding(2);
            this.cmbPosition.Name = "cmbPosition";
            this.cmbPosition.Size = new System.Drawing.Size(250, 35);
            this.cmbPosition.TabIndex = 170;
            this.cmbPosition.SelectedIndexChanged += new System.EventHandler(this.cmbPosition_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(59, 334);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(147, 27);
            this.label5.TabIndex = 171;
            this.label5.Text = "入社年月日";
            // 
            // dateTimePickerS
            // 
            this.dateTimePickerS.CalendarFont = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateTimePickerS.Checked = false;
            this.dateTimePickerS.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateTimePickerS.Location = new System.Drawing.Point(235, 328);
            this.dateTimePickerS.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerS.Name = "dateTimePickerS";
            this.dateTimePickerS.ShowCheckBox = true;
            this.dateTimePickerS.Size = new System.Drawing.Size(283, 34);
            this.dateTimePickerS.TabIndex = 172;
            this.dateTimePickerS.ValueChanged += new System.EventHandler(this.dateTimePickerS_ValueChanged);
            // 
            // dateTimePickerE
            // 
            this.dateTimePickerE.CalendarFont = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateTimePickerE.Checked = false;
            this.dateTimePickerE.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateTimePickerE.Location = new System.Drawing.Point(645, 328);
            this.dateTimePickerE.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerE.Name = "dateTimePickerE";
            this.dateTimePickerE.ShowCheckBox = true;
            this.dateTimePickerE.Size = new System.Drawing.Size(283, 34);
            this.dateTimePickerE.TabIndex = 174;
            this.dateTimePickerE.ValueChanged += new System.EventHandler(this.dateTimePickerE_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(559, 328);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(39, 27);
            this.label9.TabIndex = 175;
            this.label9.Text = "～";
            // 
            // btnSea
            // 
            this.btnSea.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSea.Location = new System.Drawing.Point(64, 74);
            this.btnSea.Margin = new System.Windows.Forms.Padding(2);
            this.btnSea.Name = "btnSea";
            this.btnSea.Size = new System.Drawing.Size(150, 80);
            this.btnSea.TabIndex = 147;
            this.btnSea.Text = "検索";
            this.btnSea.UseVisualStyleBackColor = true;
            this.btnSea.Click += new System.EventHandler(this.btnSea_Click);
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPage.Location = new System.Drawing.Point(1400, 837);
            this.lblPage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(65, 19);
            this.lblPage.TabIndex = 1288;
            this.lblPage.Text = "ページ";
            // 
            // textBoxPageNo
            // 
            this.textBoxPageNo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxPageNo.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxPageNo.Location = new System.Drawing.Point(1346, 834);
            this.textBoxPageNo.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPageNo.Name = "textBoxPageNo";
            this.textBoxPageNo.Size = new System.Drawing.Size(40, 26);
            this.textBoxPageNo.TabIndex = 1283;
            // 
            // btnLastPage
            // 
            this.btnLastPage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnLastPage.Location = new System.Drawing.Point(1594, 833);
            this.btnLastPage.Margin = new System.Windows.Forms.Padding(2);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(30, 30);
            this.btnLastPage.TabIndex = 1287;
            this.btnLastPage.Text = "▶|";
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNextPage.Location = new System.Drawing.Point(1560, 833);
            this.btnNextPage.Margin = new System.Windows.Forms.Padding(2);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(30, 30);
            this.btnNextPage.TabIndex = 1286;
            this.btnNextPage.Text = "▶";
            this.btnNextPage.UseVisualStyleBackColor = true;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnPreviousPage
            // 
            this.btnPreviousPage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnPreviousPage.Location = new System.Drawing.Point(1526, 833);
            this.btnPreviousPage.Margin = new System.Windows.Forms.Padding(2);
            this.btnPreviousPage.Name = "btnPreviousPage";
            this.btnPreviousPage.Size = new System.Drawing.Size(30, 30);
            this.btnPreviousPage.TabIndex = 1285;
            this.btnPreviousPage.Text = "◀";
            this.btnPreviousPage.UseVisualStyleBackColor = true;
            this.btnPreviousPage.Click += new System.EventHandler(this.btnPreviousPage_Click);
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnFirstPage.Location = new System.Drawing.Point(1492, 833);
            this.btnFirstPage.Margin = new System.Windows.Forms.Padding(2);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(30, 30);
            this.btnFirstPage.TabIndex = 1284;
            this.btnFirstPage.Text = "|◀";
            this.btnFirstPage.UseVisualStyleBackColor = true;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // cmbEmployeeId
            // 
            this.cmbEmployeeId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEmployeeId.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbEmployeeId.FormattingEnabled = true;
            this.cmbEmployeeId.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.cmbEmployeeId.Location = new System.Drawing.Point(247, 269);
            this.cmbEmployeeId.Margin = new System.Windows.Forms.Padding(2);
            this.cmbEmployeeId.MaxLength = 6;
            this.cmbEmployeeId.Name = "cmbEmployeeId";
            this.cmbEmployeeId.Size = new System.Drawing.Size(315, 35);
            this.cmbEmployeeId.TabIndex = 1289;
            this.cmbEmployeeId.SelectionChangeCommitted += new System.EventHandler(this.cmbEmployeeId_SelectionChangeCommitted);
            // 
            // F_LoginHistory
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1680, 890);
            this.Controls.Add(this.cmbEmployeeId);
            this.Controls.Add(this.btnSea);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.textBoxPageNo);
            this.Controls.Add(this.btnLastPage);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPreviousPage);
            this.Controls.Add(this.btnFirstPage);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dateTimePickerE);
            this.Controls.Add(this.dateTimePickerS);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cmbPosition);
            this.Controls.Add(this.cmbLogHisId);
            this.Controls.Add(this.cmbSalesOfficeId);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridViewDsp);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnView);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_LoginHistory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_LoginHistory";
            this.Load += new System.EventHandler(this.F_Employee_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDsp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbSalesOfficeId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewDsp;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ComboBox cmbLogHisId;
        private System.Windows.Forms.ComboBox cmbPosition;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePickerS;
        private System.Windows.Forms.DateTimePicker dateTimePickerE;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnSea;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.TextBox textBoxPageNo;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPreviousPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.ComboBox cmbEmployeeId;
    }
}