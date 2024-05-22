namespace SalesManagement_SysDev
{
    partial class F_Product
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
            this.cmbProductId = new System.Windows.Forms.ComboBox();
            this.checkBoxFlag = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxHideRea = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.cmbMajor = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbSmall = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPrice = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePickerS = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbMaker = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.dateTimePickerE = new System.Windows.Forms.DateTimePicker();
            this.label12 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioBtnSea = new System.Windows.Forms.RadioButton();
            this.btnSea = new System.Windows.Forms.Button();
            this.btnReg = new System.Windows.Forms.Button();
            this.radioBtnUpd = new System.Windows.Forms.RadioButton();
            this.btnUp = new System.Windows.Forms.Button();
            this.radioBtnReg = new System.Windows.Forms.RadioButton();
            this.textBoxProductName = new System.Windows.Forms.TextBox();
            this.lblPage = new System.Windows.Forms.Label();
            this.textBoxPageNo = new System.Windows.Forms.TextBox();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnPreviousPage = new System.Windows.Forms.Button();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.dataGridViewDsp = new System.Windows.Forms.DataGridView();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxModelNumber = new System.Windows.Forms.TextBox();
            this.textBoxColor = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.numericUpDownPoint = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownQuantity = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSafetyStock = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownStock = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDsp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSafetyStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStock)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbProductId
            // 
            this.cmbProductId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductId.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbProductId.FormattingEnabled = true;
            this.cmbProductId.Location = new System.Drawing.Point(174, 205);
            this.cmbProductId.Margin = new System.Windows.Forms.Padding(2);
            this.cmbProductId.Name = "cmbProductId";
            this.cmbProductId.Size = new System.Drawing.Size(180, 35);
            this.cmbProductId.TabIndex = 170;
            this.cmbProductId.SelectedIndexChanged += new System.EventHandler(this.cmbProductId_SelectedIndexChanged);
            // 
            // checkBoxFlag
            // 
            this.checkBoxFlag.AutoSize = true;
            this.checkBoxFlag.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxFlag.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBoxFlag.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxFlag.Location = new System.Drawing.Point(761, 331);
            this.checkBoxFlag.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxFlag.Name = "checkBoxFlag";
            this.checkBoxFlag.Size = new System.Drawing.Size(181, 31);
            this.checkBoxFlag.TabIndex = 159;
            this.checkBoxFlag.Text = "非表示フラグ";
            this.checkBoxFlag.UseVisualStyleBackColor = true;
            this.checkBoxFlag.CheckedChanged += new System.EventHandler(this.checkBoxFlag_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(983, 332);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(147, 27);
            this.label6.TabIndex = 158;
            this.label6.Text = "非表示理由";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(59, 208);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 27);
            this.label1.TabIndex = 154;
            this.label1.Text = "商品ID";
            // 
            // textBoxHideRea
            // 
            this.textBoxHideRea.Enabled = false;
            this.textBoxHideRea.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxHideRea.ImeMode = System.Windows.Forms.ImeMode.On;
            this.textBoxHideRea.Location = new System.Drawing.Point(1142, 329);
            this.textBoxHideRea.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxHideRea.Name = "textBoxHideRea";
            this.textBoxHideRea.Size = new System.Drawing.Size(482, 34);
            this.textBoxHideRea.TabIndex = 153;
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnClear.Location = new System.Drawing.Point(1474, 75);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(150, 80);
            this.btnClear.TabIndex = 151;
            this.btnClear.Text = "入力クリア";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnView
            // 
            this.btnView.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnView.Location = new System.Drawing.Point(1259, 75);
            this.btnView.Margin = new System.Windows.Forms.Padding(2);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(150, 80);
            this.btnView.TabIndex = 150;
            this.btnView.Text = "一覧表示";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // cmbMajor
            // 
            this.cmbMajor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMajor.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbMajor.FormattingEnabled = true;
            this.cmbMajor.Location = new System.Drawing.Point(159, 271);
            this.cmbMajor.Margin = new System.Windows.Forms.Padding(2);
            this.cmbMajor.Name = "cmbMajor";
            this.cmbMajor.Size = new System.Drawing.Size(292, 35);
            this.cmbMajor.TabIndex = 230;
            this.cmbMajor.SelectedIndexChanged += new System.EventHandler(this.cmbMajor_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Enabled = false;
            this.label9.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label9.Location = new System.Drawing.Point(476, 274);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 27);
            this.label9.TabIndex = 229;
            this.label9.Text = "小分類";
            // 
            // cmbSmall
            // 
            this.cmbSmall.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSmall.Enabled = false;
            this.cmbSmall.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbSmall.FormattingEnabled = true;
            this.cmbSmall.Location = new System.Drawing.Point(573, 271);
            this.cmbSmall.Margin = new System.Windows.Forms.Padding(2);
            this.cmbSmall.Name = "cmbSmall";
            this.cmbSmall.Size = new System.Drawing.Size(197, 35);
            this.cmbSmall.TabIndex = 228;
            this.cmbSmall.SelectionChangeCommitted += new System.EventHandler(this.cmbSmall_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(59, 274);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 27);
            this.label3.TabIndex = 227;
            this.label3.Text = "大分類";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(983, 208);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 27);
            this.label2.TabIndex = 231;
            this.label2.Text = "価格";
            // 
            // textBoxPrice
            // 
            this.textBoxPrice.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxPrice.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxPrice.Location = new System.Drawing.Point(1053, 206);
            this.textBoxPrice.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPrice.MaxLength = 9;
            this.textBoxPrice.Name = "textBoxPrice";
            this.textBoxPrice.Size = new System.Drawing.Size(150, 34);
            this.textBoxPrice.TabIndex = 232;
            this.textBoxPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxPrice_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(787, 274);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 27);
            this.label4.TabIndex = 233;
            this.label4.Text = "型番";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(59, 332);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 27);
            this.label5.TabIndex = 236;
            this.label5.Text = "発売日";
            // 
            // dateTimePickerS
            // 
            this.dateTimePickerS.Checked = false;
            this.dateTimePickerS.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateTimePickerS.Location = new System.Drawing.Point(173, 329);
            this.dateTimePickerS.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerS.Name = "dateTimePickerS";
            this.dateTimePickerS.ShowCheckBox = true;
            this.dateTimePickerS.Size = new System.Drawing.Size(261, 34);
            this.dateTimePickerS.TabIndex = 237;
            this.dateTimePickerS.ValueChanged += new System.EventHandler(this.dateTimePickerS_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(1219, 208);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(147, 27);
            this.label7.TabIndex = 238;
            this.label7.Text = "安全在庫数";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(966, 274);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 27);
            this.label8.TabIndex = 240;
            this.label8.Text = "色";
            // 
            // cmbMaker
            // 
            this.cmbMaker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMaker.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbMaker.FormattingEnabled = true;
            this.cmbMaker.Location = new System.Drawing.Point(811, 206);
            this.cmbMaker.Margin = new System.Windows.Forms.Padding(2);
            this.cmbMaker.Name = "cmbMaker";
            this.cmbMaker.Size = new System.Drawing.Size(160, 35);
            this.cmbMaker.TabIndex = 243;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(702, 209);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(105, 27);
            this.label10.TabIndex = 242;
            this.label10.Text = "メーカー";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(367, 208);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 27);
            this.label11.TabIndex = 244;
            this.label11.Text = "商品名";
            // 
            // dateTimePickerE
            // 
            this.dateTimePickerE.Checked = false;
            this.dateTimePickerE.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.dateTimePickerE.Location = new System.Drawing.Point(481, 329);
            this.dateTimePickerE.Margin = new System.Windows.Forms.Padding(2);
            this.dateTimePickerE.Name = "dateTimePickerE";
            this.dateTimePickerE.ShowCheckBox = true;
            this.dateTimePickerE.Size = new System.Drawing.Size(261, 34);
            this.dateTimePickerE.TabIndex = 246;
            this.dateTimePickerE.ValueChanged += new System.EventHandler(this.dateTimePickerE_ValueChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(438, 332);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 27);
            this.label12.TabIndex = 247;
            this.label12.Text = "～";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioBtnSea);
            this.panel1.Controls.Add(this.btnSea);
            this.panel1.Controls.Add(this.btnReg);
            this.panel1.Controls.Add(this.radioBtnUpd);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.radioBtnReg);
            this.panel1.Location = new System.Drawing.Point(25, 38);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(697, 137);
            this.panel1.TabIndex = 248;
            // 
            // radioBtnSea
            // 
            this.radioBtnSea.AutoSize = true;
            this.radioBtnSea.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioBtnSea.Location = new System.Drawing.Point(530, 9);
            this.radioBtnSea.Name = "radioBtnSea";
            this.radioBtnSea.Size = new System.Drawing.Size(116, 25);
            this.radioBtnSea.TabIndex = 178;
            this.radioBtnSea.TabStop = true;
            this.radioBtnSea.Text = "検索処理";
            this.radioBtnSea.UseVisualStyleBackColor = true;
            this.radioBtnSea.CheckedChanged += new System.EventHandler(this.radioBtnSea_CheckedChanged);
            // 
            // btnSea
            // 
            this.btnSea.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSea.Location = new System.Drawing.Point(512, 37);
            this.btnSea.Margin = new System.Windows.Forms.Padding(2);
            this.btnSea.Name = "btnSea";
            this.btnSea.Size = new System.Drawing.Size(150, 80);
            this.btnSea.TabIndex = 147;
            this.btnSea.Text = "検索";
            this.btnSea.UseVisualStyleBackColor = true;
            this.btnSea.Click += new System.EventHandler(this.btnSea_Click);
            // 
            // btnReg
            // 
            this.btnReg.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnReg.Location = new System.Drawing.Point(39, 37);
            this.btnReg.Margin = new System.Windows.Forms.Padding(2);
            this.btnReg.Name = "btnReg";
            this.btnReg.Size = new System.Drawing.Size(150, 80);
            this.btnReg.TabIndex = 145;
            this.btnReg.Text = "登録";
            this.btnReg.UseVisualStyleBackColor = true;
            this.btnReg.Click += new System.EventHandler(this.btnReg_Click);
            // 
            // radioBtnUpd
            // 
            this.radioBtnUpd.AutoSize = true;
            this.radioBtnUpd.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioBtnUpd.Location = new System.Drawing.Point(294, 9);
            this.radioBtnUpd.Name = "radioBtnUpd";
            this.radioBtnUpd.Size = new System.Drawing.Size(116, 25);
            this.radioBtnUpd.TabIndex = 177;
            this.radioBtnUpd.TabStop = true;
            this.radioBtnUpd.Text = "更新処理";
            this.radioBtnUpd.UseVisualStyleBackColor = true;
            this.radioBtnUpd.CheckedChanged += new System.EventHandler(this.radioBtnUpd_CheckedChanged);
            // 
            // btnUp
            // 
            this.btnUp.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnUp.Location = new System.Drawing.Point(276, 37);
            this.btnUp.Margin = new System.Windows.Forms.Padding(2);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(150, 80);
            this.btnUp.TabIndex = 146;
            this.btnUp.Text = "更新";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // radioBtnReg
            // 
            this.radioBtnReg.AutoSize = true;
            this.radioBtnReg.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.radioBtnReg.Location = new System.Drawing.Point(57, 9);
            this.radioBtnReg.Name = "radioBtnReg";
            this.radioBtnReg.Size = new System.Drawing.Size(116, 25);
            this.radioBtnReg.TabIndex = 176;
            this.radioBtnReg.TabStop = true;
            this.radioBtnReg.Text = "登録処理";
            this.radioBtnReg.UseVisualStyleBackColor = true;
            this.radioBtnReg.CheckedChanged += new System.EventHandler(this.radioBtnReg_CheckedChanged);
            // 
            // textBoxProductName
            // 
            this.textBoxProductName.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxProductName.ImeMode = System.Windows.Forms.ImeMode.On;
            this.textBoxProductName.Location = new System.Drawing.Point(464, 205);
            this.textBoxProductName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxProductName.Name = "textBoxProductName";
            this.textBoxProductName.Size = new System.Drawing.Size(215, 34);
            this.textBoxProductName.TabIndex = 249;
            // 
            // lblPage
            // 
            this.lblPage.AutoSize = true;
            this.lblPage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblPage.Location = new System.Drawing.Point(1400, 843);
            this.lblPage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(65, 19);
            this.lblPage.TabIndex = 1295;
            this.lblPage.Text = "ページ";
            // 
            // textBoxPageNo
            // 
            this.textBoxPageNo.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxPageNo.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxPageNo.Location = new System.Drawing.Point(1346, 840);
            this.textBoxPageNo.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxPageNo.Name = "textBoxPageNo";
            this.textBoxPageNo.Size = new System.Drawing.Size(40, 26);
            this.textBoxPageNo.TabIndex = 1290;
            // 
            // btnLastPage
            // 
            this.btnLastPage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnLastPage.Location = new System.Drawing.Point(1594, 839);
            this.btnLastPage.Margin = new System.Windows.Forms.Padding(2);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(30, 30);
            this.btnLastPage.TabIndex = 1294;
            this.btnLastPage.Text = "▶|";
            this.btnLastPage.UseVisualStyleBackColor = true;
            // 
            // btnNextPage
            // 
            this.btnNextPage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNextPage.Location = new System.Drawing.Point(1560, 839);
            this.btnNextPage.Margin = new System.Windows.Forms.Padding(2);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(30, 30);
            this.btnNextPage.TabIndex = 1293;
            this.btnNextPage.Text = "▶";
            this.btnNextPage.UseVisualStyleBackColor = true;
            // 
            // btnPreviousPage
            // 
            this.btnPreviousPage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnPreviousPage.Location = new System.Drawing.Point(1526, 839);
            this.btnPreviousPage.Margin = new System.Windows.Forms.Padding(2);
            this.btnPreviousPage.Name = "btnPreviousPage";
            this.btnPreviousPage.Size = new System.Drawing.Size(30, 30);
            this.btnPreviousPage.TabIndex = 1292;
            this.btnPreviousPage.Text = "◀";
            this.btnPreviousPage.UseVisualStyleBackColor = true;
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnFirstPage.Location = new System.Drawing.Point(1492, 839);
            this.btnFirstPage.Margin = new System.Windows.Forms.Padding(2);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(30, 30);
            this.btnFirstPage.TabIndex = 1291;
            this.btnFirstPage.Text = "|◀";
            this.btnFirstPage.UseVisualStyleBackColor = true;
            // 
            // dataGridViewDsp
            // 
            this.dataGridViewDsp.AllowUserToAddRows = false;
            this.dataGridViewDsp.AllowUserToDeleteRows = false;
            this.dataGridViewDsp.AllowUserToResizeColumns = false;
            this.dataGridViewDsp.AllowUserToResizeRows = false;
            this.dataGridViewDsp.ColumnHeadersHeight = 25;
            this.dataGridViewDsp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridViewDsp.Location = new System.Drawing.Point(64, 386);
            this.dataGridViewDsp.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewDsp.Name = "dataGridViewDsp";
            this.dataGridViewDsp.ReadOnly = true;
            this.dataGridViewDsp.RowHeadersWidth = 62;
            this.dataGridViewDsp.RowTemplate.Height = 27;
            this.dataGridViewDsp.ShowCellToolTips = false;
            this.dataGridViewDsp.ShowEditingIcon = false;
            this.dataGridViewDsp.Size = new System.Drawing.Size(1560, 426);
            this.dataGridViewDsp.TabIndex = 1289;
            this.dataGridViewDsp.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDsp_CellClick);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(1454, 274);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(93, 27);
            this.label13.TabIndex = 1296;
            this.label13.Text = "発注量";
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.SystemColors.Window;
            this.label14.Cursor = System.Windows.Forms.Cursors.No;
            this.label14.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label14.ForeColor = System.Drawing.Color.Silver;
            this.label14.Location = new System.Drawing.Point(154, 205);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(200, 34);
            this.label14.TabIndex = 1299;
            this.label14.Text = "※自動採番です";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxModelNumber
            // 
            this.textBoxModelNumber.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxModelNumber.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBoxModelNumber.Location = new System.Drawing.Point(857, 271);
            this.textBoxModelNumber.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxModelNumber.MaxLength = 20;
            this.textBoxModelNumber.Name = "textBoxModelNumber";
            this.textBoxModelNumber.Size = new System.Drawing.Size(92, 34);
            this.textBoxModelNumber.TabIndex = 1301;
            // 
            // textBoxColor
            // 
            this.textBoxColor.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxColor.ImeMode = System.Windows.Forms.ImeMode.On;
            this.textBoxColor.Location = new System.Drawing.Point(1019, 271);
            this.textBoxColor.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxColor.MaxLength = 20;
            this.textBoxColor.Name = "textBoxColor";
            this.textBoxColor.Size = new System.Drawing.Size(154, 34);
            this.textBoxColor.TabIndex = 1302;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label15.Location = new System.Drawing.Point(1273, 274);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(93, 27);
            this.label15.TabIndex = 1303;
            this.label15.Text = "発注点";
            // 
            // numericUpDownPoint
            // 
            this.numericUpDownPoint.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDownPoint.Location = new System.Drawing.Point(1371, 272);
            this.numericUpDownPoint.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownPoint.Name = "numericUpDownPoint";
            this.numericUpDownPoint.Size = new System.Drawing.Size(72, 34);
            this.numericUpDownPoint.TabIndex = 1304;
            this.numericUpDownPoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericUpDownQuantity
            // 
            this.numericUpDownQuantity.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDownQuantity.Location = new System.Drawing.Point(1552, 272);
            this.numericUpDownQuantity.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownQuantity.Name = "numericUpDownQuantity";
            this.numericUpDownQuantity.Size = new System.Drawing.Size(72, 34);
            this.numericUpDownQuantity.TabIndex = 1305;
            this.numericUpDownQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericUpDownSafetyStock
            // 
            this.numericUpDownSafetyStock.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDownSafetyStock.Location = new System.Drawing.Point(1371, 206);
            this.numericUpDownSafetyStock.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownSafetyStock.Name = "numericUpDownSafetyStock";
            this.numericUpDownSafetyStock.Size = new System.Drawing.Size(72, 34);
            this.numericUpDownSafetyStock.TabIndex = 1306;
            this.numericUpDownSafetyStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericUpDownStock
            // 
            this.numericUpDownStock.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.numericUpDownStock.Location = new System.Drawing.Point(1552, 206);
            this.numericUpDownStock.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numericUpDownStock.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.numericUpDownStock.Name = "numericUpDownStock";
            this.numericUpDownStock.Size = new System.Drawing.Size(72, 34);
            this.numericUpDownStock.TabIndex = 1307;
            this.numericUpDownStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label16.Location = new System.Drawing.Point(1454, 208);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(93, 27);
            this.label16.TabIndex = 1308;
            this.label16.Text = "在庫数";
            // 
            // F_Product
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1680, 890);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.numericUpDownStock);
            this.Controls.Add(this.numericUpDownSafetyStock);
            this.Controls.Add(this.numericUpDownQuantity);
            this.Controls.Add(this.numericUpDownPoint);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBoxColor);
            this.Controls.Add(this.textBoxModelNumber);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lblPage);
            this.Controls.Add(this.textBoxPageNo);
            this.Controls.Add(this.btnLastPage);
            this.Controls.Add(this.btnNextPage);
            this.Controls.Add(this.btnPreviousPage);
            this.Controls.Add(this.btnFirstPage);
            this.Controls.Add(this.dataGridViewDsp);
            this.Controls.Add(this.textBoxProductName);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.dateTimePickerE);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cmbMaker);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dateTimePickerS);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxPrice);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbMajor);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cmbSmall);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbProductId);
            this.Controls.Add(this.checkBoxFlag);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxHideRea);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnView);
            this.Font = new System.Drawing.Font("ＭＳ Ｐゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "F_Product";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "F_Product";
            this.Load += new System.EventHandler(this.F_Product_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDsp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSafetyStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbProductId;
        private System.Windows.Forms.CheckBox checkBoxFlag;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxHideRea;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.ComboBox cmbMajor;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbSmall;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPrice;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimePickerS;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbMaker;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dateTimePickerE;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioBtnSea;
        private System.Windows.Forms.Button btnSea;
        private System.Windows.Forms.Button btnReg;
        private System.Windows.Forms.RadioButton radioBtnUpd;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.RadioButton radioBtnReg;
        private System.Windows.Forms.TextBox textBoxProductName;
        private System.Windows.Forms.Label lblPage;
        private System.Windows.Forms.TextBox textBoxPageNo;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnPreviousPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.DataGridView dataGridViewDsp;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxModelNumber;
        private System.Windows.Forms.TextBox textBoxColor;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numericUpDownPoint;
        private System.Windows.Forms.NumericUpDown numericUpDownQuantity;
        private System.Windows.Forms.NumericUpDown numericUpDownSafetyStock;
        private System.Windows.Forms.NumericUpDown numericUpDownStock;
        private System.Windows.Forms.Label label16;
    }
}