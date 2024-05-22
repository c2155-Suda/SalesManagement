namespace SalesManagement_SysDev
{
    partial class F_OrderDetail
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioBtnReg = new System.Windows.Forms.RadioButton();
            this.radioBtnSea = new System.Windows.Forms.RadioButton();
            this.btnSea = new System.Windows.Forms.Button();
            this.btnReg = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridViewDsp = new System.Windows.Forms.DataGridView();
            this.cmbOrderDetailId = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.domainUpDownCount = new System.Windows.Forms.DomainUpDown();
            this.comboBoxProductName = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.comboBoxMajor = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxMinor = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxOrderId = new System.Windows.Forms.TextBox();
            this.textBoxState = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDsp)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.textBoxState);
            this.panel1.Controls.Add(this.textBoxOrderId);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dataGridViewDsp);
            this.panel1.Controls.Add(this.cmbOrderDetailId);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.domainUpDownCount);
            this.panel1.Controls.Add(this.comboBoxProductName);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.comboBoxMajor);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.comboBoxMinor);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(976, 676);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioBtnReg);
            this.panel2.Controls.Add(this.radioBtnSea);
            this.panel2.Controls.Add(this.btnSea);
            this.panel2.Controls.Add(this.btnReg);
            this.panel2.Location = new System.Drawing.Point(26, 83);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(304, 112);
            this.panel2.TabIndex = 312;
            // 
            // radioBtnReg
            // 
            this.radioBtnReg.AutoSize = true;
            this.radioBtnReg.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioBtnReg.Location = new System.Drawing.Point(18, 9);
            this.radioBtnReg.Name = "radioBtnReg";
            this.radioBtnReg.Size = new System.Drawing.Size(116, 25);
            this.radioBtnReg.TabIndex = 314;
            this.radioBtnReg.TabStop = true;
            this.radioBtnReg.Text = "登録処理";
            this.radioBtnReg.UseVisualStyleBackColor = true;
            // 
            // radioBtnSea
            // 
            this.radioBtnSea.AutoSize = true;
            this.radioBtnSea.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioBtnSea.Location = new System.Drawing.Point(174, 9);
            this.radioBtnSea.Name = "radioBtnSea";
            this.radioBtnSea.Size = new System.Drawing.Size(116, 25);
            this.radioBtnSea.TabIndex = 313;
            this.radioBtnSea.TabStop = true;
            this.radioBtnSea.Text = "検索処理";
            this.radioBtnSea.UseVisualStyleBackColor = true;
            // 
            // btnSea
            // 
            this.btnSea.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSea.Location = new System.Drawing.Point(170, 39);
            this.btnSea.Margin = new System.Windows.Forms.Padding(2);
            this.btnSea.Name = "btnSea";
            this.btnSea.Size = new System.Drawing.Size(120, 60);
            this.btnSea.TabIndex = 306;
            this.btnSea.Text = "検索";
            this.btnSea.UseVisualStyleBackColor = true;
            // 
            // btnReg
            // 
            this.btnReg.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnReg.Location = new System.Drawing.Point(14, 39);
            this.btnReg.Margin = new System.Windows.Forms.Padding(2);
            this.btnReg.Name = "btnReg";
            this.btnReg.Size = new System.Drawing.Size(120, 60);
            this.btnReg.TabIndex = 305;
            this.btnReg.Text = "登録";
            this.btnReg.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.LightBlue;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(976, 80);
            this.label4.TabIndex = 311;
            this.label4.Text = "受注詳細";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnClose.Location = new System.Drawing.Point(808, 122);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 60);
            this.btnClose.TabIndex = 310;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(375, 138);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 27);
            this.label2.TabIndex = 308;
            this.label2.Text = "受注ID";
            // 
            // dataGridViewDsp
            // 
            this.dataGridViewDsp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDsp.Location = new System.Drawing.Point(40, 362);
            this.dataGridViewDsp.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewDsp.Name = "dataGridViewDsp";
            this.dataGridViewDsp.RowHeadersWidth = 62;
            this.dataGridViewDsp.RowTemplate.Height = 27;
            this.dataGridViewDsp.Size = new System.Drawing.Size(888, 282);
            this.dataGridViewDsp.TabIndex = 307;
            // 
            // cmbOrderDetailId
            // 
            this.cmbOrderDetailId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOrderDetailId.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbOrderDetailId.FormattingEnabled = true;
            this.cmbOrderDetailId.Location = new System.Drawing.Point(207, 200);
            this.cmbOrderDetailId.Margin = new System.Windows.Forms.Padding(2);
            this.cmbOrderDetailId.Name = "cmbOrderDetailId";
            this.cmbOrderDetailId.Size = new System.Drawing.Size(190, 35);
            this.cmbOrderDetailId.TabIndex = 304;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(35, 203);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 27);
            this.label1.TabIndex = 303;
            this.label1.Text = "受注詳細ID";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(400, 256);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 27);
            this.label8.TabIndex = 302;
            this.label8.Text = "数量";
            // 
            // domainUpDownCount
            // 
            this.domainUpDownCount.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.domainUpDownCount.Location = new System.Drawing.Point(496, 256);
            this.domainUpDownCount.Margin = new System.Windows.Forms.Padding(2);
            this.domainUpDownCount.Name = "domainUpDownCount";
            this.domainUpDownCount.Size = new System.Drawing.Size(80, 31);
            this.domainUpDownCount.TabIndex = 301;
            this.domainUpDownCount.Text = "0";
            // 
            // comboBoxProductName
            // 
            this.comboBoxProductName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProductName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBoxProductName.FormattingEnabled = true;
            this.comboBoxProductName.Location = new System.Drawing.Point(161, 253);
            this.comboBoxProductName.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxProductName.Name = "comboBoxProductName";
            this.comboBoxProductName.Size = new System.Drawing.Size(190, 35);
            this.comboBoxProductName.TabIndex = 300;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(35, 256);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 27);
            this.label11.TabIndex = 299;
            this.label11.Text = "商品名";
            // 
            // comboBoxMajor
            // 
            this.comboBoxMajor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMajor.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBoxMajor.FormattingEnabled = true;
            this.comboBoxMajor.Location = new System.Drawing.Point(161, 307);
            this.comboBoxMajor.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxMajor.Name = "comboBoxMajor";
            this.comboBoxMajor.Size = new System.Drawing.Size(190, 35);
            this.comboBoxMajor.TabIndex = 298;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(400, 310);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 27);
            this.label9.TabIndex = 297;
            this.label9.Text = "小分類";
            // 
            // comboBoxMinor
            // 
            this.comboBoxMinor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxMinor.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBoxMinor.FormattingEnabled = true;
            this.comboBoxMinor.Location = new System.Drawing.Point(530, 307);
            this.comboBoxMinor.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxMinor.Name = "comboBoxMinor";
            this.comboBoxMinor.Size = new System.Drawing.Size(190, 35);
            this.comboBoxMinor.TabIndex = 296;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(35, 310);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 27);
            this.label3.TabIndex = 295;
            this.label3.Text = "大分類";
            // 
            // textBoxOrderId
            // 
            this.textBoxOrderId.BackColor = System.Drawing.Color.White;
            this.textBoxOrderId.Cursor = System.Windows.Forms.Cursors.No;
            this.textBoxOrderId.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxOrderId.Location = new System.Drawing.Point(471, 135);
            this.textBoxOrderId.MaxLength = 6;
            this.textBoxOrderId.Name = "textBoxOrderId";
            this.textBoxOrderId.ReadOnly = true;
            this.textBoxOrderId.ShortcutsEnabled = false;
            this.textBoxOrderId.Size = new System.Drawing.Size(137, 34);
            this.textBoxOrderId.TabIndex = 313;
            this.textBoxOrderId.TabStop = false;
            this.textBoxOrderId.Text = "ID";
            this.textBoxOrderId.WordWrap = false;
            // 
            // textBoxState
            // 
            this.textBoxState.BackColor = System.Drawing.Color.White;
            this.textBoxState.Cursor = System.Windows.Forms.Cursors.No;
            this.textBoxState.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxState.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBoxState.Location = new System.Drawing.Point(639, 135);
            this.textBoxState.MaxLength = 6;
            this.textBoxState.Name = "textBoxState";
            this.textBoxState.ReadOnly = true;
            this.textBoxState.ShortcutsEnabled = false;
            this.textBoxState.Size = new System.Drawing.Size(137, 34);
            this.textBoxState.TabIndex = 314;
            this.textBoxState.TabStop = false;
            this.textBoxState.Text = "State";
            this.textBoxState.WordWrap = false;
            // 
            // F_OrderDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_OrderDetail";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_OrderDetail";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDsp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioBtnReg;
        private System.Windows.Forms.RadioButton radioBtnSea;
        private System.Windows.Forms.Button btnSea;
        private System.Windows.Forms.Button btnReg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridViewDsp;
        private System.Windows.Forms.ComboBox cmbOrderDetailId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DomainUpDown domainUpDownCount;
        private System.Windows.Forms.ComboBox comboBoxProductName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox comboBoxMajor;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxMinor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxOrderId;
        private System.Windows.Forms.TextBox textBoxState;
    }
}