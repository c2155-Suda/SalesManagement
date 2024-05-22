using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    public partial class F_Product : Form
    {
        ProductDbConnect product = new ProductDbConnect();
        DataAccess access = new DataAccess();
        private List<M_ProductDsp> Product;
        private bool updFlg;
        private int pageSize = 12;

        public F_Product()
        {
            InitializeComponent();            
        }

        private void F_Product_Load(object sender, EventArgs e)
        {
            Product = product.GetProductData();
            GetAllData();
            radioBtnReg.Checked = true;
        }

        //登録
        private void btnReg_Click(object sender, EventArgs e)
        {
            //入力チェック
            if (!GetValidDataAtRegistration())
                return;
            //登録情報作成
            var regProduct = GenerateDataAtRegistration();
            //登録処理
            product.RegistProductData(regProduct);
            GetAllData();
            AllClear();
        }

        
        private bool GetValidDataAtRegistration()
        {
            if (String.IsNullOrEmpty(textBoxProductName.Text.Trim()))
            {
                MessageBox.Show("商品名が未入力です", "入力エラー");
                textBoxProductName.Focus();
                return false;
            }
            if (cmbMaker.SelectedIndex == -1)
            {
                MessageBox.Show("メーカー名が未選択です", "選択エラー");
                cmbMaker.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(textBoxPrice.Text.Trim()))
            {
                MessageBox.Show("価格が未入力です", "入力エラー");
                textBoxPrice.Focus();
                return false;
            }           
            if (textBoxPrice.Text.Trim().Length <= NumericRange.TotalPrice)
            {
                Regex regex = new Regex("^[0-9]+$");
                if (!regex.IsMatch(textBoxPrice.Text.Trim()))
                {
                    MessageBox.Show("価格は数字のみです", "入力エラー");
                    textBoxPrice.Focus();
                    return false;
                }                    
            }
            else
            {
                MessageBox.Show("価格は9桁です", "入力エラー");
                textBoxPrice.Focus();
                return false;
            }            
            if (String.IsNullOrEmpty(textBoxSafetyStock.Text.Trim()))
            {
                MessageBox.Show("安全在庫数が未入力です", "入力エラー");
                textBoxSafetyStock.Focus();
                return false;
            }
            if (textBoxSafetyStock.Text.Trim().Length <= NumericRange.Quant)
            {
                Regex regex = new Regex("^[0-9]+$");
                if (!regex.IsMatch(textBoxPrice.Text.Trim()))
                {
                    MessageBox.Show("安全在庫数は数字のみです", "入力エラー");
                    textBoxSafetyStock.Focus();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("安全在庫数は4桁です", "入力エラー");
                textBoxPrice.Focus();
                return false;
            }
            if (cmbMajor.SelectedIndex == -1)
            {
                MessageBox.Show("大分類が未選択です", "選択エラー");
                cmbMajor.Focus();
                return false;
            }
            if (cmbSmall.SelectedIndex == -1)
            {
                MessageBox.Show("小分類が未選択です", "選択エラー");
                cmbSmall.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(textBoxModelNumber.Text.Trim()))
            {
                MessageBox.Show("型番が未入力です", "入力エラー");
                textBoxModelNumber.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(textBoxColor.Text.Trim()))
            {
                MessageBox.Show("色が未入力です", "入力エラー");
                textBoxColor.Focus();
                return false;
            }
            return true;
        }
        private M_Product GenerateDataAtRegistration()
        {
            DateTime releaseDate = DateTime.Now;
            if (dateTimePickerS.Checked == false)
            {
                MessageBox.Show("発売日が未選択です", "選択エラー");
                dateTimePickerS.Focus();
                return null;
            }
            else if(dateTimePickerS.Checked == true )
            {
                releaseDate = DateTime.Parse(dateTimePickerS.Text);
            }
            int hattyuFlg;
            if (checkBoxFlag.Checked == false)
            {
                hattyuFlg = 0;
            }
            else
                hattyuFlg = 2;

            int hideFlg;
            if (checkBoxFlag.Checked == false)
            {
                hideFlg = 0;
            }
            else
                hideFlg = 2;
            
            //登録データセット
            return new M_Product
            {
                PrName = textBoxProductName.Text.Trim(),
                MaID = (int)cmbMaker.SelectedValue,
                Price = int.Parse(textBoxPrice.Text.Trim()),
                PrSafetyStock = int.Parse(textBoxSafetyStock.Text.Trim()),
                ScID = (int)cmbSmall.SelectedValue,
                PrModelNumber = textBoxModelNumber.Text.Trim(),
                PrColor = textBoxColor.Text.Trim(),
                PrReleaseDate = releaseDate,
                PrOrderPoint = hattyuFlg,
                PrFlag = hideFlg,
                PrHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim()
            };
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            //入力チェック
            if (!GetValidDataAtUpdate())
                return;
            //更新情報作成
            var updProduct = GenerateDataAtUpdate();
            //更新処理
            //product.UpdateProductData(updProduct,);
            GetAllData();
            AllClear();
        }
        private bool GetValidDataAtUpdate()
        {
            if (String.IsNullOrEmpty(textBoxProductName.Text.Trim()))
            {
                MessageBox.Show("商品名が未入力です", "入力エラー");
                textBoxProductName.Focus();
                return false;
            }
            if (cmbMaker.SelectedIndex == -1)
            {
                MessageBox.Show("メーカー名が未選択です", "選択エラー");
                cmbMaker.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(textBoxPrice.Text.Trim()))
            {
                MessageBox.Show("価格が未入力です", "入力エラー");
                textBoxPrice.Focus();
                return false;
            }
            if (textBoxPrice.Text.Trim().Length <= NumericRange.TotalPrice)
            {
                Regex regex = new Regex("^[0-9]+$");
                if (!regex.IsMatch(textBoxPrice.Text.Trim()))
                {
                    MessageBox.Show("価格は数字のみです", "入力エラー");
                    textBoxPrice.Focus();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("価格は9桁です", "入力エラー");
                textBoxPrice.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(textBoxSafetyStock.Text.Trim()))
            {
                MessageBox.Show("安全在庫数が未入力です", "入力エラー");
                textBoxSafetyStock.Focus();
                return false;
            }
            if (textBoxSafetyStock.Text.Trim().Length <= NumericRange.Quant)
            {
                Regex regex = new Regex("^[0-9]+$");
                if (!regex.IsMatch(textBoxPrice.Text.Trim()))
                {
                    MessageBox.Show("安全在庫数は数字のみです", "入力エラー");
                    textBoxSafetyStock.Focus();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("安全在庫数は4桁です", "入力エラー");
                textBoxPrice.Focus();
                return false;
            }
            if (cmbMajor.SelectedIndex == -1)
            {
                MessageBox.Show("大分類が未選択です", "選択エラー");
                cmbMajor.Focus();
                return false;
            }
            if (cmbSmall.SelectedIndex == -1)
            {
                MessageBox.Show("小分類が未選択です", "選択エラー");
                cmbSmall.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(textBoxModelNumber.Text.Trim()))
            {
                MessageBox.Show("型番が未入力です", "入力エラー");
                textBoxModelNumber.Focus();
                return false;
            }
            if (String.IsNullOrEmpty(textBoxColor.Text.Trim()))
            {
                MessageBox.Show("色が未入力です", "入力エラー");
                textBoxColor.Focus();
                return false;
            }
            return true;
        }

        private M_Product GenerateDataAtUpdate()
        {
            DateTime releaseDate = DateTime.Now;
            if (dateTimePickerS.Checked == false)
            {
                MessageBox.Show("発売日が未選択です", "選択エラー");
                dateTimePickerS.Focus();
                return null;
            }
            else if (dateTimePickerS.Checked == true)
            {
                releaseDate = DateTime.Parse(dateTimePickerS.Text);
            }
            int hattyuFlg;
            if (checkBoxFlag.Checked == false)
            {
                hattyuFlg = 0;
            }
            else
                hattyuFlg = 2;

            int hideFlg;
            if (checkBoxFlag.Checked == false)
            {
                hideFlg = 0;
            }
            else
                hideFlg = 2;


            // 更新データセット
            return new M_Product
            {
                PrName = textBoxProductName.Text.Trim(),
                MaID = (int)cmbMaker.SelectedValue,
                Price = int.Parse(textBoxPrice.Text.Trim()),
                PrSafetyStock = int.Parse(textBoxSafetyStock.Text.Trim()),
                ScID = (int)cmbSmall.SelectedValue,
                PrModelNumber = textBoxModelNumber.Text.Trim(),
                PrColor = textBoxColor.Text.Trim(),
                PrReleaseDate = releaseDate,
                PrOrderPoint = hattyuFlg,
                PrFlag = hideFlg,
                PrHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim()
            };
        }
        private void btnSea_Click(object sender, EventArgs e)
        {
            //データ取得
            bool selectFlg = true;
            selectFlg = GetValidDataAtSelect(selectFlg);

            //抽出
            GenerateDataAtSelect(selectFlg);

            //結果表示
            SetSelectDate();
        }
        private bool GetValidDataAtSelect(bool Flg)
        {            
            if (dateTimePickerS.Checked == true && dateTimePickerE.Checked == true)
            {
                if (dateTimePickerE.Value < dateTimePickerS.Value)
                {
                    DateTime date = DateTime.Now;
                    date = dateTimePickerS.Value;
                    dateTimePickerS.Value = dateTimePickerE.Value;
                    dateTimePickerE.Value = date;
                }
            }
            else if (dateTimePickerS.Checked == false && dateTimePickerE.Checked == false)
            {
                Flg = false;
            }

            return Flg;
        }

        private void GenerateDataAtSelect(bool selectFlg)
        {
            int prID = 0;
            if (cmbProductId.Text != "")
            {
                prID = int.Parse(cmbProductId.Text);
            }

            int maID = 0;
            if (cmbMaker.SelectedIndex != -1)
            {
                maID = (int)cmbMaker.SelectedValue;
            }

            int mcID = 0;
            if (cmbMajor.SelectedIndex != -1)
            {
                mcID = (int)cmbMajor.SelectedValue;
            }

            int scID = 0;
            if (cmbSmall.SelectedIndex != -1)
            {
                scID = (int)cmbSmall.SelectedValue;
            }

            int hattyuFlg;
            if (checkBoxFlag.Checked == false)
            {
                hattyuFlg = 0;
            }
            else
                hattyuFlg = 2;

            int hideFlg;
            if (checkBoxFlag.Checked == false)
            {
                hideFlg = 0;
            }
            else
                hideFlg = 2;


            if (selectFlg == false)
            {
                // 検索条件のセット
                M_ProductDsp selectCondition = new M_ProductDsp()
                {
                    PrID = prID,
                    PrName = textBoxProductName.Text.Trim(),
                    MaID = maID,
                    McID = mcID,
                    ScID = scID,
                    PrModelNumber = textBoxModelNumber.Text.Trim(),
                    PrColor = textBoxColor.Text.Trim(),
                    PrOrderPoint = hattyuFlg,
                    PrFlag = hideFlg,
                    PrHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim()
                };
                // データの抽出
                Product = product.GetProductData(selectCondition);
            }
            else if (selectFlg == true)
            {
                DateTime? sdate = DateTime.Now, edate = DateTime.Now;
                if (dateTimePickerS.Checked == false)
                {
                    sdate = null;
                    edate = dateTimePickerE.Value;
                }
                else if (dateTimePickerE.Checked == false)
                {
                    sdate = dateTimePickerS.Value;
                    edate = null;
                }
                else
                {
                    sdate = dateTimePickerS.Value;
                    edate = dateTimePickerE.Value;
                }

                // 検索条件のセット
                M_ProductDsp selectCondition = new M_ProductDsp()
                {
                    PrID = (int)cmbProductId.SelectedValue,
                    PrName = textBoxProductName.Text.Trim(),
                    MaID = (int)cmbMaker.SelectedValue,
                    ScID = (int)cmbSmall.SelectedValue,
                    PrModelNumber = textBoxModelNumber.Text.Trim(),
                    PrColor = textBoxColor.Text.Trim(),
                    PrOrderPoint = hattyuFlg,
                    PrFlag = hideFlg,
                    PrHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim()
                };
                // データの抽出
                Product = product.GetProductData(selectCondition, sdate, edate);
            }

        }
        private void SetSelectDate()
        {
            dataGridViewDsp.DataSource = Product;

            //dataGridViewのページ番号指定
            textBoxPageNo.Text = "1";
            int pageNo = int.Parse(textBoxPageNo.Text) - 1;
            dataGridViewDsp.DataSource = Product.Skip(pageSize * pageNo).Take(pageSize).ToList();

            //dataGridViewの総ページ数
            lblPage.Text = "/" + ((int)Math.Ceiling(Product.Count / (double)pageSize)) + "ページ";

            GetComboBoxData();

            dataGridViewDsp.Refresh();
        }
        private void AllClear()
        {
            cmbProductId.SelectedIndex = -1;
            textBoxProductName.Text = "";
            cmbMaker.SelectedIndex = -1;
            textBoxPrice.Text = "";
            textBoxSafetyStock.Text = "";
            cmbMajor.SelectedIndex = -1;
            cmbSmall.SelectedIndex = -1;
            textBoxModelNumber.Text = "";
            textBoxColor.Text = "";
            checkBoxHattyu.Checked = false;
            domainUpDownCount.Text = "0";
            dateTimePickerS.Value = DateTime.Now;
            dateTimePickerE.Value = DateTime.Now;
            dateTimePickerS.Checked = false;
            dateTimePickerE.Checked = false;
            checkBoxFlag.Checked = false;
            textBoxHideRea.Text = "";
        }

        //データグリッドビュー表示用
        private bool GetAllData()
        {
            //全件取得
            Product = product.GetProductData();
            if (Product == null)
                return false;
            //データグリッドビューへの設定
            SetDataGridView();
            return true;
        }

        private void SetDataGridView()
        {
            dataGridViewDsp.DataSource = Product;
            //dataGridViewのページ番号指定
            textBoxPageNo.Text = "1";
            int pageNo = int.Parse(textBoxPageNo.Text) - 1;
            dataGridViewDsp.DataSource = Product.Skip(pageSize * pageNo).Take(pageSize).ToList();
            
            //列幅自動設定解除
            dataGridViewDsp.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            //textsize
            this.dataGridViewDsp.DefaultCellStyle.Font = new Font("ＭＳ Ｐゴシック", 20);
            this.dataGridViewDsp.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ Ｐゴシック", 20);
            //ヘッダーの高さ
            dataGridViewDsp.ColumnHeadersHeight = 100;            
            //ヘッダーの折り返し表示
            dataGridViewDsp.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewDsp.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //行単位選択
            dataGridViewDsp.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //複数指定無効処理
            dataGridViewDsp.MultiSelect = false;

            //ヘッダー文字位置、セル文字位置、列幅の設定
            ////商品ID
            dataGridViewDsp.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[0].Width = 120;
            ////商品名
            dataGridViewDsp.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[1].Width = 330;
            ////メーカーID
            dataGridViewDsp.Columns[2].Width = 130;
            dataGridViewDsp.Columns[2].Visible = false;            
            ////メーカー名
            dataGridViewDsp.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[3].Width = 220;            
            ////価格
            dataGridViewDsp.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[4].Width = 220;
            ////JANコード
            dataGridViewDsp.Columns[5].Width = 130;
            dataGridViewDsp.Columns[5].Visible = false;
            ////大分類ID
            dataGridViewDsp.Columns[6].Width = 130;
            dataGridViewDsp.Columns[6].Visible = false;
            ////大分類
            dataGridViewDsp.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[7].Width = 385;
            ////小分類ID
            dataGridViewDsp.Columns[8].Width = 130;
            dataGridViewDsp.Columns[8].Visible = false;
            ////小分類
            dataGridViewDsp.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[9].Width = 220;
            ////型番
            dataGridViewDsp.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[10].Width = 90;
            ////色
            dataGridViewDsp.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[11].Width = 135;
            ////発売日
            dataGridViewDsp.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[12].Width = 200;
            ////安全在庫数
            dataGridViewDsp.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[13].Width = 100;
            ////発注点
            dataGridViewDsp.Columns[14].Width = 130;
            dataGridViewDsp.Columns[14].Visible = false;
            ////発注点フラグ
            dataGridViewDsp.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[15].Width = 100;
            ////在庫数
            dataGridViewDsp.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[16].Width = 100;
            ////入庫予定数
            dataGridViewDsp.Columns[17].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[17].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[17].Width = 120;
            ////発注量
            dataGridViewDsp.Columns[18].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[18].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[18].Width = 100;
            ////boolフラグ
            dataGridViewDsp.Columns[19].Width = 130;
            dataGridViewDsp.Columns[19].Visible = false;
            ////論理削除フラグ
            dataGridViewDsp.Columns[20].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[20].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[20].Width = 90;
            ////非表示理由
            dataGridViewDsp.Columns[21].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[21].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[21].Width = 460;

            //dataGridViewの総ページ数
            lblPage.Text = "/" + ((int)Math.Ceiling(Product.Count / (double)pageSize)) + "ページ";

            GetComboBoxData();

            dataGridViewDsp.Refresh();
        }

        private void radioBtnReg_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnReg.Checked == false)
            {
                return;
            }
            btnReg.Enabled = true;
            btnUp.Enabled = false;
            btnSea.Enabled = false;
            label1.Enabled = false;
            label12.Visible = false;
            dateTimePickerE.Visible = false;
            label14.Visible = true;
            cmbProductId.Enabled = false;
            checkBoxHattyu.Visible = true;
            label13.Visible = true;
            domainUpDownCount.Visible = true;
            label2.Visible = true;
            label7.Visible = true;
            textBoxPrice.Visible = true;
            textBoxSafetyStock.Visible = true;
            updFlg = false;
        }

        private void radioBtnUpd_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnUpd.Checked == false)
            {
                return;
            }
            btnReg.Enabled = false;
            btnUp.Enabled = true;
            btnSea.Enabled = false;
            label1.Enabled = true;
            label12.Visible = false;
            dateTimePickerE.Visible = false;
            label14.Visible = false;
            cmbProductId.Enabled = true;
            checkBoxHattyu.Visible = true;
            label13.Visible = true;
            domainUpDownCount.Visible = true;
            label2.Visible = true;
            label7.Visible = true;
            textBoxPrice.Visible = true;
            textBoxSafetyStock.Visible = true;
            updFlg = true;
        }

        private void radioBtnSea_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnSea.Checked == false)
            {
                return;
            }
            btnReg.Enabled = false;
            btnUp.Enabled = false;
            btnSea.Enabled = true;
            label1.Enabled = true;
            label12.Visible = true;
            dateTimePickerE.Visible = true;
            label14.Visible = false;
            cmbProductId.Enabled = true;
            checkBoxHattyu.Visible = false;
            label13.Visible = false;
            domainUpDownCount.Visible = false;
            label2.Visible = false;
            label7.Visible = false;
            textBoxPrice.Visible = false;
            textBoxSafetyStock.Visible = false;
            updFlg = false;
        }


        //コンボボックスデータ取得
        private void GetComboBoxData()
        {
            //商品IDのコンボボックスData読込
            var listProduct = Product;
            cmbProductId.DataSource = listProduct;
            cmbProductId.DisplayMember = "PrID";
            cmbProductId.ValueMember = "PrID";
            cmbProductId.SelectedIndex = -1;
               
            //メーカーのコンボボックスData読込
            var listMaker = access.GetMakerDspData();
            cmbMaker.DataSource = listMaker;
            cmbMaker.DisplayMember = "MaName";
            cmbMaker.ValueMember = "MaID";
            cmbMaker.SelectedIndex = -1;

            //大分類のコンボボックスData読込
            var listMajor = access.GetMajorClassificationDspData();
            cmbMajor.DataSource = listMajor;
            cmbMajor.DisplayMember = "McName";
            cmbMajor.ValueMember = "McID";
            cmbMajor.SelectedIndex = -1;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            AllClear();
            GetAllData();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            AllClear();
            GetAllData();
        }

        private void dataGridViewDsp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (updFlg == true)
            {
                
                //データグリッドビューからクリックされたデータを各入力エリアへ
                cmbProductId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
                textBoxProductName.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[1].Value.ToString();
                cmbMaker.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[3].Value.ToString();
                textBoxPrice.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[4].Value.ToString();
                cmbMajor.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[7].Value.ToString();
                cmbSmall.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[9].Value.ToString();
                textBoxModelNumber.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[10].Value.ToString();
                textBoxColor.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value.ToString();
                dateTimePickerS.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[12].Value.ToString();
                textBoxSafetyStock.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[13].Value.ToString();
                domainUpDownCount.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[17].Value.ToString();

                checkBoxHattyu.Checked = (bool)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[15].Value;
                checkBoxFlag.Checked = (bool)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[20].Value;

                //null処理を空欄処理
                if(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[21].Value == null)
                {
                    textBoxHideRea.Text = "";
                }
                else
                    textBoxHideRea.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[21].Value.ToString();
            }
        }
        private void checkBoxFlag_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFlag.Checked == true)
            {
                label6.Enabled = true;
                textBoxHideRea.Enabled = true;
            }
            if (checkBoxFlag.Checked == false)
            {
                label6.Enabled = false;
                textBoxHideRea.Enabled = false;
            }
        }

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b' && (ModifierKeys & Keys.Control) != Keys.Control)
            {
                e.Handled = true;
            }
        }

        private void textBoxSafetyStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b' && (ModifierKeys & Keys.Control) != Keys.Control)
            {
                e.Handled = true;
            }
        }

        private void cmbMajor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listSmall = access.GetSmallClassificationDspData();
            
            if(cmbMajor.SelectedIndex == -1)
            {                
                label9.Enabled = false;
                cmbSmall.Enabled = false;
            }
            else if(cmbMajor.SelectedIndex != -1)
            {
                if (int.TryParse(cmbMajor.SelectedValue.ToString(),out int mcID))
                {
                    listSmall = access.GetSmallClassificationDspData(mcID);
                } 
                label9.Enabled = true;
                cmbSmall.Enabled = true;
                //小分類のコンボボックスData読込
                cmbSmall.DataSource = listSmall;
                cmbSmall.DisplayMember = "ScName";
                cmbSmall.ValueMember = "ScID";
                cmbSmall.SelectedIndex = -1;
            }            
        }

    }
}
