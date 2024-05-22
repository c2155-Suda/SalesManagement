using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    public partial class F_Product : Form
    {
        EmployeeDbConnection employee = new EmployeeDbConnection();
        ProductDbConnect product = new ProductDbConnect();
        DataAccess access = new DataAccess();
        private List<M_ProductDsp> Product;
        private List<M_ProductDsp> subProduct;
        private List<M_SmallClassificationDsp> smallClass;
        private List<M_MajorClassificationDsp> majorClass;
        private List<M_MakerDsp> Maker;
        private bool updFlg;
        private int pageSize = 12;
        public int LoginEmID;
        public bool loginUserInfoCheck = true;
        public F_Product()
        {
            InitializeComponent();            
        }

        private void F_Product_Load(object sender, EventArgs e)
        {
            if (!LockLoginUserInfo())
            {
                loginUserInfoCheck = false;
                return;
            }
            GetComboBoxData();
            GetAllData();
            radioBtnSea.Checked = true;
        }
        private bool LockLoginUserInfo()
        {
            try
            {
                if (employee.CheckEmployeeExsistence(LoginEmID))
                {
                    MessageBox.Show("ログイン中の社員IDは削除済みです", "エラー");
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        //登録
        private void btnReg_Click(object sender, EventArgs e)
        {
            try
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        
        private bool GetValidDataAtRegistration()
        {
            try
            {
                if(String.IsNullOrEmpty(textBoxProductName.Text.Trim()))
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
                if (textBoxPrice.Text.Trim().Length <= 9)
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
                if (dateTimePickerS.Checked == false)
                {
                    MessageBox.Show("発売日が未選択です", "選択エラー");
                    dateTimePickerS.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private M_Product GenerateDataAtRegistration()
        {
            try
            {
                DateTime releaseDate = DateTime.Parse(dateTimePickerS.Text);
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
                    PrSafetyStock = int.Parse(numericUpDownSafetyStock.Text.Trim()),
                    ScID = (int)cmbSmall.SelectedValue,
                    PrModelNumber = textBoxModelNumber.Text.Trim(),
                    PrColor = textBoxColor.Text.Trim(),
                    PrReleaseDate = releaseDate,
                    PrOrderPoint = (int)numericUpDownPoint.Value,
                    PrFlag = hideFlg,
                    PrHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim(),
                    PrOrderQuantity = (int)numericUpDownQuantity.Value
                };
            }
            catch
            {
                throw;
            }
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                //入力チェック
                if (!GetValidDataAtUpdate())
                    return;
                //更新情報作成
                var updProduct = GenerateProductDataAtUpdate();
                var updStock = GenerateStockDataAtUpDate();
                //更新処理
                product.UpdateProductData(updProduct, updStock);
                GetAllData();
                AllClear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private bool GetValidDataAtUpdate()
        {
            try
            {
                if (cmbProductId.SelectedIndex == -1)
                {
                    MessageBox.Show("商品IDが未選択です", "選択エラー");
                    cmbProductId.Focus();
                    return false;
                }
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
                if (textBoxPrice.Text.Trim().Length <= 9)
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
                if (dateTimePickerS.Checked == false)
                {
                    MessageBox.Show("発売日が未選択です", "選択エラー");
                    dateTimePickerS.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        private M_Product GenerateProductDataAtUpdate()
        {
            try
            {
                DateTime releaseDate = DateTime.Parse(dateTimePickerS.Text);
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
                    PrID = int.Parse(cmbProductId.SelectedValue.ToString()),
                    PrName = textBoxProductName.Text.Trim(),
                    MaID = (int)cmbMaker.SelectedValue,
                    Price = int.Parse(textBoxPrice.Text.Trim()),
                    PrSafetyStock = int.Parse(numericUpDownSafetyStock.Text.Trim()),
                    ScID = (int)cmbSmall.SelectedValue,
                    PrModelNumber = textBoxModelNumber.Text.Trim(),
                    PrColor = textBoxColor.Text.Trim(),
                    PrReleaseDate = releaseDate,
                    PrOrderPoint = (int)numericUpDownPoint.Value,
                    PrOrderQuantity = (int)numericUpDownQuantity.Value,

                    PrFlag = hideFlg,
                    PrHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim()
                };
            }
            catch
            {
                throw;
            }
        }
        private T_Stock GenerateStockDataAtUpDate()
        {
            try
            {
                return new T_Stock
                {
                    StID = product.GetStockID(int.Parse(cmbProductId.SelectedValue.ToString())),
                    StQuantity = (int)numericUpDownStock.Value
                };
            }
            catch 
            {
                throw;
            }
        }
        private void btnSea_Click(object sender, EventArgs e)
        {
            try
            {
                //データ取得
                bool selectFlg = true;
                selectFlg = GetValidDataAtSelect(selectFlg);

                //抽出
                GenerateDataAtSelect(selectFlg);

                //結果表示
                SetSelectDate();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private bool GetValidDataAtSelect(bool Flg)
        {
            try
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
            catch
            {
                throw;
            }
        }

        private void GenerateDataAtSelect(bool selectFlg)
        {
            try
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
                    subProduct = new List<M_ProductDsp>(Product);
                    subProduct.Insert(0, new M_ProductDsp { PrID = null });
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
                    subProduct = new List<M_ProductDsp>(Product);
                    subProduct.Insert(0, new M_ProductDsp { PrID = null });
                }
            }
            catch
            {
                throw;
            }
        }
        private void SetSelectDate()
        {
            try
            {
                dataGridViewDsp.DataSource = Product;

                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Product.Skip(pageSize * pageNo).Take(pageSize).ToList();

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Product.Count / (double)pageSize)) + "ページ";
                RefreshProductCombo();
                dataGridViewDsp.Refresh();
            }
            catch
            {
                throw;
            }
        }
        private void AllClear()
        {
            try
            {
                cmbProductId.SelectedIndex = -1;
                textBoxProductName.Text = "";
                cmbMaker.SelectedIndex = -1;
                textBoxPrice.Text = "";
                numericUpDownSafetyStock.Text = "0";
                numericUpDownStock.Text = "0";
                cmbMajor.SelectedIndex = -1;
                cmbSmall.SelectedIndex = -1;
                textBoxModelNumber.Text = "";
                textBoxColor.Text = "";
                numericUpDownPoint.Text = "0";
                numericUpDownQuantity.Text = "0";
                dateTimePickerS.Value = DateTime.Now;
                dateTimePickerE.Value = DateTime.Now;
                dateTimePickerS.Checked = false;
                dateTimePickerE.Checked = false;
                checkBoxFlag.Checked = false;
                textBoxHideRea.Text = "";
            }
            catch
            {
                throw;
            }
        }

        //データグリッドビュー表示用
        private bool GetAllData()
        {
            try
            {
                //全件取得
                Product = product.GetProductData();
                subProduct = new List<M_ProductDsp>(Product);
                subProduct.Insert(0, new M_ProductDsp { PrID = null });
                if (Product == null)
                    return false;
                //データグリッドビューへの設定
                SetDataGridView();
                return true;
            }
            catch
            {
                throw;
            }
        }

        private void SetDataGridView()
        {
            try
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
                dataGridViewDsp.Columns[5].Visible = false;
                ////大分類ID
                dataGridViewDsp.Columns[6].Visible = false;
                ////大分類
                dataGridViewDsp.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[7].Width = 385;
                ////小分類ID
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
                dataGridViewDsp.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[14].Width = 100;
                ////在庫数
                dataGridViewDsp.Columns[15].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[15].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[15].Width = 100;
                ////入庫予定数
                dataGridViewDsp.Columns[16].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[16].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[16].Width = 120;
                ////発注量
                dataGridViewDsp.Columns[17].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[17].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[17].Width = 100;
                ////発注点フラグ
                dataGridViewDsp.Columns[18].Visible = false;
                ////論理削除フラグ
                dataGridViewDsp.Columns[19].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[19].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[19].Width = 90;
                ////非表示理由
                dataGridViewDsp.Columns[20].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[20].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[20].Width = 460;

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Product.Count / (double)pageSize)) + "ページ";

                RefreshProductCombo();

                dataGridViewDsp.Refresh();
            }
            catch
            {
                throw;
            }
        }

        private void radioBtnReg_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnReg.Checked == false)
                {
                    return;
                }
                RefreshProductCombo();
                RefreshMakerCombo();
                RefreshSmallClassCombo();
                AllClear();
                btnReg.Enabled = true;
                btnUp.Enabled = false;
                btnSea.Enabled = false;
                label1.Enabled = false;
                label12.Visible = false;
                dateTimePickerE.Visible = false;
                label14.Visible = true;
                label15.Visible = true;
                label16.Visible = false;
                cmbProductId.Enabled = false;
                numericUpDownPoint.Visible = true;
                label13.Visible = true;
                numericUpDownQuantity.Visible = true;
                label2.Visible = true;
                label7.Visible = true;
                textBoxPrice.Visible = true;
                numericUpDownSafetyStock.Visible = true;
                numericUpDownStock.Visible = false;
                updFlg = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void radioBtnUpd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnUpd.Checked == false)
                {
                    return;
                }
                RefreshProductCombo();
                RefreshMakerCombo();
                RefreshSmallClassCombo();
                AllClear();
                btnReg.Enabled = false;
                btnUp.Enabled = true;
                btnSea.Enabled = false;
                label1.Enabled = true;
                label12.Visible = false;
                dateTimePickerE.Visible = false;
                label14.Visible = false;
                label15.Visible = true;
                label16.Visible = true;
                cmbProductId.Enabled = true;
                numericUpDownPoint.Visible = true;
                label13.Visible = true;
                numericUpDownQuantity.Visible = true;
                label2.Visible = true;
                label7.Visible = true;
                textBoxPrice.Visible = true;
                numericUpDownSafetyStock.Visible = true;
                numericUpDownStock.Visible = true;
                updFlg = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void radioBtnSea_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnSea.Checked == false)
                {
                    return;
                }
                RefreshProductCombo();
                RefreshMakerCombo();
                RefreshSmallClassCombo();
                AllClear();
                btnReg.Enabled = false;
                btnUp.Enabled = false;
                btnSea.Enabled = true;
                label1.Enabled = true;
                label12.Visible = true;
                label12.Enabled = false;
                dateTimePickerE.Visible = true;
                label14.Visible = false;
                label15.Visible = false;
                label16.Visible = false;
                cmbProductId.Enabled = true;
                numericUpDownPoint.Visible = false;
                label13.Visible = false;
                numericUpDownQuantity.Visible = false;
                label2.Visible = false;
                label7.Visible = false;
                textBoxPrice.Visible = false;
                numericUpDownSafetyStock.Visible = false;
                numericUpDownStock.Visible = false;
                updFlg = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }


        //コンボボックスデータ取得
        private void GetComboBoxData()
        {
            try
            {
                //商品IDのコンボボックスData読込
                cmbProductId.DisplayMember = "PrID";
                cmbProductId.ValueMember = "PrID";
                cmbProductId.DataSource = Product;
                cmbProductId.SelectedIndex = -1;

                //メーカーのコンボボックスData読込
                Maker = access.GetMakerDspData();
                cmbMaker.DisplayMember = "MaName";
                cmbMaker.ValueMember = "MaID";
                cmbMaker.DataSource = Maker;
                cmbMaker.SelectedIndex = -1;

                //大分類のコンボボックスData読込
                majorClass = access.GetMajorClassificationDspData();
                majorClass.Insert(0, new M_MajorClassificationDsp { McID = 0, McName = "" });
                majorClass.Add(new M_MajorClassificationDsp { McID = -1, McName = "削除済み" });
                cmbMajor.DisplayMember = "McName";
                cmbMajor.ValueMember = "McID";
                cmbMajor.DataSource = majorClass;
                cmbMajor.SelectedIndex = -1;

                //小分類のコンボボックスData読込
                label9.Enabled = true;
                cmbSmall.Enabled = true;
                smallClass = access.GetSmallClassificationDspData(0);
                cmbSmall.DisplayMember = "ScName";
                cmbSmall.ValueMember = "ScID";
                cmbSmall.DataSource = smallClass;
                cmbSmall.SelectedIndex = -1;
            }
            catch
            {
                throw;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                GetAllData();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                AllClear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void dataGridViewDsp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (updFlg == true)
                {

                    //データグリッドビューからクリックされたデータを各入力エリアへ
                    cmbProductId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
                    textBoxProductName.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[1].Value.ToString();
                    cmbMaker.SelectedIndex = Maker.FindIndex(x => x.MaID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[2].Value.ToString()));
                    textBoxPrice.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[4].Value.ToString();
                    int temp = majorClass.FindIndex(x => x.McID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[6].Value.ToString()));
                    if (temp == -1)
                    {
                        cmbMajor.SelectedIndex = majorClass.Count - 1;
                    }
                    else
                    {
                        cmbMajor.SelectedIndex = temp;
                    }
                    cmbSmall.SelectedIndex = smallClass.FindIndex(x => x.ScID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[8].Value.ToString()));
                    textBoxModelNumber.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[10].Value.ToString();
                    textBoxColor.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value.ToString();
                    dateTimePickerS.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[12].Value.ToString();

                    numericUpDownSafetyStock.Value = int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[13].Value.ToString());

                    numericUpDownPoint.Value = int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[14].Value.ToString());

                    numericUpDownStock.Value = int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[15].Value.ToString());

                    numericUpDownQuantity.Value = int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[17].Value.ToString());

                    checkBoxFlag.Checked = (bool)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[19].Value;

                    //null処理を空欄処理
                    if (dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[20].Value == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    else
                        textBoxHideRea.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[20].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private void checkBoxFlag_CheckedChanged(object sender, EventArgs e)
        {
            try
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void textBoxPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b' && (ModifierKeys & Keys.Control) != Keys.Control)
                {
                    e.Handled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void textBoxSafetyStock_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b' && (ModifierKeys & Keys.Control) != Keys.Control)
                {
                    e.Handled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbMajor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int temp = 0;
                if (cmbMajor.SelectedIndex != -1)
                {
                    temp = int.Parse(cmbMajor.SelectedValue.ToString());
                }
                smallClass = access.GetSmallClassificationDspData(temp);
                RefreshSmallClassCombo_MajorChanged();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private void RefreshSmallClassCombo()
        {
            try
            {
                if (radioBtnSea.Checked)
                {
                    smallClass.Insert(0, new M_SmallClassificationDsp { ScID = 0, ScName = "" });
                    smallClass.Add(new M_SmallClassificationDsp { ScID = -1, ScName = "削除済み" });
                    cmbSmall.DataSource = null;
                    cmbSmall.DataSource = smallClass;
                    cmbSmall.DisplayMember = "ScName";
                    cmbSmall.ValueMember = "ScID";
                }
                else
                {
                    smallClass.RemoveAll(x => x.ScID == 0 || x.ScID == -1);
                    cmbSmall.DataSource = null;
                    cmbSmall.DataSource = smallClass;
                    cmbSmall.DisplayMember = "ScName";
                    cmbSmall.ValueMember = "ScID";
                }
            }
            catch
            {
                throw;
            }
        }
        private void RefreshSmallClassCombo_MajorChanged()
        {
            try
            {
                int temp = 0;
                if (cmbSmall.SelectedIndex != -1)
                {
                    temp = int.Parse(cmbSmall.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    smallClass.Insert(0, new M_SmallClassificationDsp { ScID = 0, ScName = "" });
                    smallClass.Add(new M_SmallClassificationDsp { ScID = -1, ScName = "削除済み" });
                    cmbSmall.DataSource = null;
                    cmbSmall.DataSource = smallClass;
                    cmbSmall.DisplayMember = "ScName";
                    cmbSmall.ValueMember = "ScID";
                }
                else
                {
                    smallClass.RemoveAll(x => x.ScID == 0 || x.ScID == -1);
                    cmbSmall.DataSource = null;
                    cmbSmall.DataSource = smallClass;
                    cmbSmall.DisplayMember = "ScName";
                    cmbSmall.ValueMember = "ScID";
                }
                cmbSmall.SelectedIndex = smallClass.FindIndex(x => x.ScID == temp);
            }
            catch
            {
                throw;
            }
        }
        private void RefreshProductCombo()
        {
            try
            {
                int temp = 0;
                if (cmbProductId.SelectedValue != null)
                {
                    temp = int.Parse(cmbProductId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    cmbProductId.DataSource = subProduct;
                    cmbProductId.SelectedIndex = subProduct.FindIndex(x => x.PrID == temp);
                }
                else
                {
                    cmbProductId.DataSource = Product;
                    cmbProductId.SelectedIndex = Product.FindIndex(x => x.PrID == temp);
                }
            }
            catch
            {
                throw;
            }

        }
        private void RefreshMakerCombo()
        {
            try
            {
                if (radioBtnSea.Checked)
                {
                    Maker.Insert(0, new M_MakerDsp { MaID = 0, MaName = "" });
                    Maker.Add(new M_MakerDsp { MaID = -1, MaName = "削除済み" });
                    cmbMaker.DataSource = null;
                    cmbMaker.DataSource = Maker;
                    cmbMaker.DisplayMember = "MaName";
                    cmbMaker.ValueMember = "MaID";
                }
                else
                {
                    Maker.RemoveAll(x => x.MaID == 0 || x.MaID == -1);
                    cmbMaker.DataSource = null;
                    cmbMaker.DataSource = Maker;
                    cmbMaker.DisplayMember = "MaName";
                    cmbMaker.ValueMember = "MaID";
                }
            }
            catch
            {
                throw;
            }
        }

        private void cmbSmall_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (cmbSmall.SelectedIndex == -1)
                {
                    return;
                }
                if (radioBtnSea.Checked && (cmbSmall.SelectedIndex == 0 || cmbSmall.SelectedIndex == smallClass.Count - 1))
                {
                    return;
                }
                int temp = majorClass.FindIndex(x => x.McID == smallClass.Single(y => y.ScID == int.Parse(cmbSmall.SelectedValue.ToString())).McID);
                if (temp == -1)
                {
                    cmbMajor.SelectedIndex = majorClass.Count - 1;
                }
                else
                {
                    cmbMajor.SelectedIndex = temp;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbProductId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnUpd.Checked)
                {
                    if (cmbProductId.SelectedValue == null)
                    {
                        AllClear();
                    }
                    else
                    {
                        int index = Product.FindIndex(x => x.PrID == int.Parse(cmbProductId.SelectedValue.ToString()));
                        cmbProductId.Text = Product[index].PrID.ToString();
                        textBoxProductName.Text = Product[index].PrName.ToString();
                        cmbMaker.SelectedIndex = Maker.FindIndex(x => x.MaID == Product[index].MaID);
                        textBoxPrice.Text = Product[index].Price.ToString();
                        int temp = majorClass.FindIndex(x => x.McID == Product[index].McID);
                        if (temp == -1)
                        {
                            cmbMajor.SelectedIndex = -1;
                        }
                        else
                        {
                            cmbMajor.SelectedIndex = temp;
                        }
                        cmbSmall.SelectedIndex = smallClass.FindIndex(x => x.ScID == Product[index].ScID);
                        textBoxModelNumber.Text = Product[index].PrModelNumber.ToString();
                        textBoxColor.Text = Product[index].PrColor.ToString();
                        dateTimePickerS.Text = Product[index].PrReleaseDate.ToString();
                        numericUpDownSafetyStock.Value = Product[index].PrSafetyStock;
                        numericUpDownPoint.Value = Product[index].PrOrderPoint;
                        numericUpDownStock.Value = Product[index].StQuantity;
                        numericUpDownQuantity.Value = Product[index].PrOrderQuantity;
                        checkBoxFlag.Checked = Product[index].PrFlagBool;
                        if (Product[index].PrHidden == null)
                        {
                            textBoxHideRea.Text = "";
                        }
                        else
                        {
                            textBoxHideRea.Text = Product[index].PrHidden.ToString();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void dateTimePickerS_ValueChanged(object sender, EventArgs e)
        {
            if (radioBtnSea.Checked && (!dateTimePickerS.Checked && !dateTimePickerE.Checked))
            {
                label12.Enabled = false;
            }
            else
            {
                label12.Enabled = true;
            }
        }

        private void dateTimePickerE_ValueChanged(object sender, EventArgs e)
        {
            if (radioBtnSea.Checked && (!dateTimePickerS.Checked && !dateTimePickerE.Checked))
            {
                label12.Enabled = false;
            }
            else
            {
                label12.Enabled = true;
            }
        }
    }
}
