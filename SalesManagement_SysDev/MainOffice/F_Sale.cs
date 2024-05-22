using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SalesManagement_SysDev
{
    public partial class F_Sale : Form
    {
        SaleDbConnection sale = new SaleDbConnection();
        F_SaleDetail f_Detail;
        EmployeeDbConnection employee = new EmployeeDbConnection();
        ClientDbConnection client = new ClientDbConnection();
        ProductDbConnect product = new ProductDbConnect();
        DataAccess access = new DataAccess();
        InputFormCheck icheck = new InputFormCheck();
        private List<T_SaleDsp> Sale;
        private List<T_SaleDsp> subSale;
        private List<M_SalesOfficeDsp> SalesOffice;
        private List<M_ClientDsp> Client;
        private List<M_EmployeeDsp> Employee;
        private List<M_ProductDsp> Product;
        private List<M_MajorClassificationDsp> majorClass;
        private List<M_SmallClassificationDsp> smallClass;
        private int pageSize = 12;
        private bool updFlg;
        public int LoginEmID;
        public int LoginSoID;
        public bool loginUserInfoCheck = true;
        public F_Sale()
        {
            InitializeComponent();
        }
        private void F_Sale_Load(object sender, EventArgs e)
        {
            if (!LockLoginUserInfo())
            {
                loginUserInfoCheck = false;
                return;
            }
            GetAllData();
            radioBtnSea.Checked = true;

        }
        private bool LockLoginUserInfo()
        {
            try
            {
                SalesOffice = access.GetSalesOfficeDspData();
                Employee = employee.GetEmployeeData();
                Client = client.GetClientData();
                if (!Employee.Any(x => x.EmID == LoginEmID))
                {
                    MessageBox.Show("ログイン中の社員IDは削除済みです", "エラー");
                    return false;
                }
                GetComboBoxData();
                return true;
            }
            catch
            {
                throw;
            }
        }
        private void GetComboBoxData()
        {
            try
            {
                //注文IDのコンボボックスData読込
                cmbSaleId.DisplayMember = "SaID";
                cmbSaleId.ValueMember = "SaID";
                cmbSaleId.DataSource = Sale;
                cmbSaleId.SelectedIndex = -1;

                //大分類のコンボボックスData読込
                majorClass = access.GetMajorClassificationDspData();
                majorClass.Insert(0, new M_MajorClassificationDsp { McID = 0, McName = "" });
                majorClass.Add(new M_MajorClassificationDsp { McID = -1, McName = "削除済み" });
                cmbMajor.DisplayMember = "McName";
                cmbMajor.ValueMember = "McID";
                cmbMajor.DataSource = majorClass;
                cmbMajor.SelectedIndex = -1;
            }
            catch
            {
                throw;
            }
        }

        private void cmbSalesOfficeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int temp = 0;
                if (cmbSalesOfficeId.SelectedIndex != -1)
                {
                    temp = int.Parse(cmbSalesOfficeId.SelectedValue.ToString());
                }
                Client = access.GetClientDspData(temp);
                Employee = access.GetEmployeeDspData(temp);
                RefreshClientCombo();
                RefreshEmployeeCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void textBoxOrderId_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b' && (ModifierKeys & Keys.Control) != Keys.Control)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSaleId.SelectedIndex == -1)
                {
                    MessageBox.Show("売上IDを選択してください。", "選択エラー");
                    cmbSaleId.Focus();
                    return;
                }
                f_Detail = new F_SaleDetail();
                int saID = (int)cmbSaleId.SelectedValue;
                f_Detail.saID = saID;
                f_Detail.orID = int.Parse(textBoxOrderId.Text);
                f_Detail.logEmID = LoginEmID;
                f_Detail.ShowDialog();
                var saleData = Sale.Single(x => x.SaID == saID);
                var updatedSaleData = sale.GetSaleData(new T_SaleRead { SaID = saID, SaFlag = -1 }).Single();
                dataGridViewDsp.Refresh();
                checkBoxFlag.Checked = updatedSaleData.SaBoolFlag;
                if (updatedSaleData.SaHidden == null)
                {
                    textBoxHideRea.Text = "";
                }
                f_Detail.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private bool GetValidDataAtSelect()
        {
            try
            {
                if (!string.IsNullOrEmpty(textBoxOrderId.Text.Trim()))
                {
                    if(int.TryParse(textBoxOrderId.Text.Trim(),out int orID) && orID > 0 && orID <= NumericRange.ID)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("受注IDは1~999999の整数のみです。", "入力エラー");
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }

        }
        private bool GetSelectMode()
        {
            try
            {
                bool Flg = true;
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
                int saID = 0;
                if (cmbSaleId.Text != "")
                {
                    saID = int.Parse(cmbSaleId.Text);
                }
                int orID = 0;
                if (!string.IsNullOrEmpty(textBoxOrderId.Text.Trim()))
                {
                    orID = int.Parse(textBoxOrderId.Text.Trim());
                }
                int clID = 0;
                if (cmbClientId.Text != "")
                {
                    clID = (int)cmbClientId.SelectedValue;
                }
                int soID = 0;
                if (cmbSalesOfficeId.Text != "")
                {
                    soID = (int)cmbSalesOfficeId.SelectedValue;
                }
                int emID = 0;
                if (cmbEmployeeId.Text != "")
                {
                    emID = (int)cmbEmployeeId.SelectedValue;
                }
                int prID = 0;
                if (cmbProductId.Text != "")
                {
                    prID = (int)cmbProductId.SelectedValue;
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
                    T_SaleRead selectCondition = new T_SaleRead()
                    {
                        SaID=saID,
                        OrID = orID,
                        SoID = soID,
                        EmID = emID,
                        ClID = clID,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        SaFlag = hideFlg,
                        SaHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Sale = sale.GetSaleData(selectCondition);
                    subSale = new List<T_SaleDsp>(Sale);
                    subSale.Insert(0, new T_SaleDsp { SaID = null });
                }
                else
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
                    T_SaleRead selectCondition = new T_SaleRead()
                    {
                        SaID=saID,
                        OrID = orID,
                        SoID = soID,
                        EmID = emID,
                        ClID = clID,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        SaFlag = hideFlg,
                        SaHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Sale = sale.GetSaleData(selectCondition, sdate, edate);
                    subSale = new List<T_SaleDsp>(Sale);
                    subSale.Insert(0, new T_SaleDsp { SaID = null });
                }
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
                if (!GetValidDataAtSelect())
                {
                    return;
                }
                bool selectModeFlg = GetSelectMode();

                //抽出
                GenerateDataAtSelect(selectModeFlg);

                //結果表示
                SetSelectDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void btnHidden_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSaleId.SelectedIndex == -1)
                {
                    MessageBox.Show("売上IDを選択してください。", "選択エラー");
                    cmbSaleId.Focus();
                    return;
                }
                int saID = (int)cmbSaleId.SelectedValue;
                int iFlg = 0;
                if (checkBoxFlag.Checked == false)
                {
                    textBoxHideRea.Text = "";
                }
                else
                {
                    iFlg = 2;
                }
                string orhidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim();
                sale.UpdateSaleHidden(saID, iFlg, orhidden);
                GetAllData();
                AllClear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                GetAllData();
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private void RefreshSalesOfficeCombo()
        {
            try
            {
                cmbSalesOfficeId.SelectedIndexChanged -= cmbSalesOfficeId_SelectedIndexChanged;
                int temp = 0;
                if (cmbSalesOfficeId.SelectedIndex != -1)
                {
                    temp = int.Parse(cmbSalesOfficeId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    SalesOffice.Insert(0, new M_SalesOfficeDsp { SoID = 0, SoName = "" });
                    SalesOffice.Add(new M_SalesOfficeDsp { SoID = -1, SoName = "削除済み" });
                }
                else
                {
                    SalesOffice.RemoveAll(x => x.SoID == 0 || x.SoID == -1);
                }
                cmbSalesOfficeId.DataSource = null;
                cmbSalesOfficeId.DisplayMember = "SoName";
                cmbSalesOfficeId.ValueMember = "SoID";
                cmbSalesOfficeId.DataSource = SalesOffice;
                cmbSalesOfficeId.SelectedIndexChanged += cmbSalesOfficeId_SelectedIndexChanged;
            }
            catch
            {
                throw;
            }
        }
        private void RefreshClientCombo()
        {
            try
            {
                int temp = 0;
                if (cmbClientId.SelectedIndex != -1)
                {
                    temp = int.Parse(cmbClientId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    Client.Insert(0, new M_ClientDsp { ClID = 0, ClName = "" });
                    Client.Add(new M_ClientDsp { ClID = -1, ClName = "削除済み" });
                }
                else
                {
                    Client.RemoveAll(x => x.ClID == 0 || x.ClID == -1);
                }
                cmbClientId.DataSource = null;
                cmbClientId.DisplayMember = "ClName";
                cmbClientId.ValueMember = "ClID";
                cmbClientId.DataSource = Client;
                cmbClientId.SelectedIndex = Client.FindIndex(x => x.ClID == temp);
            }
            catch
            {
                throw;
            }
        }
        private void RefreshEmployeeCombo()
        {
            try
            {
                int temp = 0;
                if (cmbEmployeeId.SelectedIndex != -1)
                {
                    temp = int.Parse(cmbEmployeeId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    Employee.Insert(0, new M_EmployeeDsp { EmID = 0, EmName = "" });
                    Employee.Add(new M_EmployeeDsp { EmID = -1, EmName = "削除済み" });
                }
                else
                {
                    Employee.RemoveAll(x => x.EmID == 0 || x.EmID == -1);
                }
                cmbEmployeeId.DataSource = null;
                cmbEmployeeId.DisplayMember = "EmName";
                cmbEmployeeId.ValueMember = "EmID";
                cmbEmployeeId.DataSource = Employee;
                cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == temp);
            }
            catch
            {
                throw;
            }
        }
        private void RefreshSmallClassCombo()
        {
            try
            {
                int temp = 0;
                if (cmbSmall.SelectedIndex != -1)
                {
                    temp = int.Parse(cmbSmall.SelectedValue.ToString());
                }
                smallClass.Insert(0, new M_SmallClassificationDsp { ScID = 0, ScName = "" });
                smallClass.Add(new M_SmallClassificationDsp { ScID = -1, ScName = "削除済み" });
                cmbSmall.DataSource = null;
                cmbSmall.DisplayMember = "ScName";
                cmbSmall.ValueMember = "ScID";
                cmbSmall.DataSource = smallClass;
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
                if (cmbProductId.SelectedIndex != -1)
                {
                    temp = int.Parse(cmbProductId.SelectedValue.ToString());
                }
                foreach (var item in Product)
                {
                    string ProdactColorName = item.PrName + "(" + item.PrColor + ")";

                    item.PrName = ProdactColorName;
                }
                Product.Insert(0, new M_ProductDsp { PrID = 0, PrName = "" });
                Product.Add(new M_ProductDsp { PrID = -1, PrName = "削除済み" });
                cmbProductId.DataSource = null;
                cmbProductId.DisplayMember = "PrName";
                cmbProductId.ValueMember = "PrID";
                cmbProductId.DataSource = Product;
                cmbProductId.SelectedIndex = Product.FindIndex(x => x.PrID == temp);
            }
            catch
            {
                throw;
            }
        }
        private void RefreshSaleCombo()
        {
            try
            {
                int temp = 0;
                if (cmbSaleId.SelectedValue != null)
                {
                    temp = int.Parse(cmbSaleId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    cmbSaleId.DataSource = subSale;
                    cmbSaleId.SelectedIndex = subSale.FindIndex(x => x.SaID == temp);
                }
                else
                {
                    cmbSaleId.DataSource = Sale;
                    cmbSaleId.SelectedIndex = Sale.FindIndex(x => x.SaID == temp);
                }
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
                cmbSaleId.SelectedIndex = -1;
                textBoxOrderId.Text = "";
                cmbSalesOfficeId.SelectedIndex = -1;
                cmbClientId.SelectedIndex = -1;
                cmbEmployeeId.SelectedIndex = -1;
                cmbProductId.SelectedIndex = -1;
                cmbMajor.SelectedIndex = -1;
                cmbSmall.SelectedIndex = -1;
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
        private void SetSelectDate()
        {
            try
            {
                dataGridViewDsp.DataSource = Sale;

                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Sale.Skip(pageSize * pageNo).Take(pageSize).ToList();

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Sale.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshSaleCombo();
            }
            catch
            {
                throw;
            }
        }
        private bool GetAllData()
        {
            try
            {
                //全件取得
                Sale = sale.GetSaleData();
                subSale = new List<T_SaleDsp>(Sale);
                subSale.Insert(0, new T_SaleDsp { SaID = null });
                if (Sale == null)
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
                dataGridViewDsp.DataSource = Sale;
                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Sale.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                ////ID
                dataGridViewDsp.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[0].Width = 95;
                ///受注ID
                dataGridViewDsp.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[1].Width = 95;
                ////顧客ID
                dataGridViewDsp.Columns[2].Visible = false;
                ////顧客名
                dataGridViewDsp.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[3].Width = 170;
                ////営業所ID
                dataGridViewDsp.Columns[4].Visible = false;
                ////営業所名
                dataGridViewDsp.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[5].Width = 190;
                ////社員ID
                dataGridViewDsp.Columns[6].Visible = false;
                ////社員名
                dataGridViewDsp.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[7].Width = 160;
                ////受注日
                dataGridViewDsp.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[8].Width = 180;
                ////論理削除
                dataGridViewDsp.Columns[9].Visible = false;
                ////論理削除フラグ
                dataGridViewDsp.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[10].Width = 90;
                ////非表示理由
                dataGridViewDsp.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[11].Width = 400;
                ////品数
                dataGridViewDsp.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[12].Width = 100;
                ////金額
                dataGridViewDsp.Columns[13].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[13].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewDsp.Columns[13].Width = 200;
                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Sale.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshSaleCombo();
            }
            catch
            {
                throw;
            }
        }

        private void radioBtnDetail_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnDetail.Checked == false)
                {
                    return;
                }
                RefreshSaleCombo();
                RefreshSalesOfficeCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = false;
                textBoxOrderId.Enabled = false; //受注ID

                label2.Enabled = false;
                cmbClientId.Enabled = false; //顧客ID

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                dateTimePickerS.Checked = true;
                label7.Visible = false;
                dateTimePickerE.Visible = false; //注文日

                label10.Enabled = false;
                label4.Enabled = false;
                cmbSalesOfficeId.Enabled = false;
                cmbEmployeeId.Enabled = false;

                label11.Visible = false;
                cmbProductId.Visible = false; //商品ID
                label3.Visible = false;
                cmbMajor.Visible = false; //大分類ID
                label9.Visible = false;
                cmbSmall.Visible = false; //小分類ID

                checkBoxFlag.Enabled = false; //非表示フラグ

                btnDetail.Enabled = true;
                btnSea.Enabled = false;
                btnHidden.Enabled = false; //ボタン有効化

                updFlg = true; //自動入力フラグ
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void radioBtnHidden_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnHidden.Checked == false)
                {
                    return;
                }
                RefreshSaleCombo();
                RefreshSalesOfficeCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = false;
                textBoxOrderId.Enabled = false; //受注ID

                label2.Enabled = false;
                cmbClientId.Enabled = false; //顧客ID

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                dateTimePickerS.Checked = true;
                label7.Visible = false;
                dateTimePickerE.Visible = false; //注文日

                label10.Enabled = false;
                label4.Enabled = false;
                cmbSalesOfficeId.Enabled = false;
                cmbEmployeeId.Enabled = false;

                label11.Visible = false;
                cmbProductId.Visible = false; //商品ID
                label3.Visible = false;
                cmbMajor.Visible = false; //大分類ID
                label9.Visible = false;
                cmbSmall.Visible = false; //小分類ID

                checkBoxFlag.Enabled = true; //非表示フラグ

                btnDetail.Enabled = false;
                btnSea.Enabled = false;
                btnHidden.Enabled = true; //ボタン有効化

                updFlg = true; //自動入力フラグ
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
                RefreshSaleCombo();
                RefreshSalesOfficeCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = true;
                textBoxOrderId.Enabled = true; //受注ID

                label2.Enabled = true;
                cmbClientId.Enabled = true; //顧客ID

                label5.Enabled = true;
                dateTimePickerS.Enabled = true;
                label7.Visible = true;
                label7.Enabled = false;
                dateTimePickerE.Visible = true; //注文日

                label10.Enabled = true;
                label4.Enabled = true;
                cmbSalesOfficeId.Enabled = true;
                cmbEmployeeId.Enabled = true;

                label11.Visible = true;
                cmbProductId.Visible = true; //商品ID
                label3.Visible = true;
                cmbMajor.Visible = true; //大分類ID
                label9.Visible = true;
                cmbSmall.Visible = true; //小分類ID

                checkBoxFlag.Enabled = true; //非表示フラグ

                btnDetail.Enabled = false;
                btnSea.Enabled = true;
                btnHidden.Enabled = false; //ボタン有効化

                updFlg = false; //自動入力フラグ
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
                if (checkBoxFlag.Checked && (radioBtnSea.Checked || radioBtnHidden.Checked))
                {
                    label6.Enabled = true;
                    textBoxHideRea.Enabled = true;
                }
                else
                {
                    label6.Enabled = false;
                    textBoxHideRea.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewDsp.DataSource = Sale.Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                textBoxPageNo.Text = "1";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            try
            {
                int pageNo = int.Parse(textBoxPageNo.Text) - 2;
                dataGridViewDsp.DataSource = Sale.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                if (pageNo + 1 > 1)
                    textBoxPageNo.Text = (pageNo + 1).ToString();
                else
                    textBoxPageNo.Text = "1";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            try
            {
                int pageNo = int.Parse(textBoxPageNo.Text);
                //最終ページの計算
                int lastNo = (int)Math.Ceiling(Sale.Count / (double)pageSize) - 1;
                //最終ページでなければ
                if (pageNo <= lastNo)
                    dataGridViewDsp.DataSource = Sale.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                int lastPage = (int)Math.Ceiling(Sale.Count / (double)pageSize);
                if (pageNo >= lastPage)
                    textBoxPageNo.Text = lastPage.ToString();
                else
                    textBoxPageNo.Text = (pageNo + 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            try
            {
                //最終ページの計算
                int pageNo = (int)Math.Ceiling(Sale.Count / (double)pageSize) - 1;
                dataGridViewDsp.DataSource = Sale.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                textBoxPageNo.Text = (pageNo + 1).ToString();
            }
            catch (Exception ex)
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
                    cmbSaleId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
                    textBoxOrderId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[1].Value.ToString();
                    cmbClientId.SelectedIndex = Client.FindIndex(x => x.ClID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[2].Value.ToString()));
                    cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[6].Value.ToString()));
                    dateTimePickerS.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[8].Value.ToString();
                    checkBoxFlag.Checked = (bool)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[10].Value;
                    //null処理を空欄処理
                    if (dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    else
                        textBoxHideRea.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbSmall_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (cmbSmall.SelectedIndex == -1 || cmbSmall.SelectedIndex == 0 || cmbSmall.SelectedIndex == smallClass.Count - 1)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbProductId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (cmbProductId.SelectedIndex == -1 || cmbProductId.SelectedIndex == 0 || cmbProductId.SelectedIndex == Product.Count - 1)
                {
                    return;
                }
                var selectedProduct = Product.Single(x => x.PrID == int.Parse(cmbProductId.SelectedValue.ToString()));
                int smallIndex = smallClass.FindIndex(x => x.ScID == selectedProduct.ScID);
                int majorIndex = majorClass.FindIndex(x => x.McID == selectedProduct.McID);
                if (smallIndex == -1)
                {
                    cmbSmall.SelectedIndex = smallClass.Count - 1;
                }
                else
                {
                    cmbSmall.SelectedIndex = smallIndex;
                }
                if (majorIndex == -1)
                {
                    cmbMajor.SelectedIndex = majorClass.Count - 1;
                }
                else
                {
                    cmbMajor.SelectedIndex = majorIndex;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbSmall_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int scID = 0;
                int mcID = 0;
                if (cmbSmall.SelectedIndex != -1)
                {
                    scID = int.Parse(cmbSmall.SelectedValue.ToString());
                }
                if (scID == 0 && cmbMajor.SelectedIndex != -1)
                {
                    mcID = int.Parse(cmbMajor.SelectedValue.ToString());
                }
                Product = product.GetProductData(new M_ProductDsp { PrID = 0, McID = mcID, ScID = scID, PrFlag = 0, PrName = "", PrModelNumber = "", PrColor = "" });
                RefreshProductCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbMajor_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int mcID = 0;
                if (cmbMajor.SelectedIndex != -1)
                {
                    mcID = int.Parse(cmbMajor.SelectedValue.ToString());
                }
                smallClass = access.GetSmallClassificationDspData(mcID);
                cmbSmall.SelectedIndexChanged -= cmbSmall_SelectedIndexChanged;
                RefreshSmallClassCombo();
                cmbSmall.SelectedIndexChanged += cmbSmall_SelectedIndexChanged;
                int scID = 0;
                if (cmbSmall.SelectedIndex != -1)
                {
                    scID = int.Parse(cmbSmall.SelectedValue.ToString());
                }
                Product = product.GetProductData(new M_ProductDsp { PrID = 0, McID = mcID, ScID = scID, PrFlag = 0, PrName = "", PrModelNumber = "", PrColor = "" });
                RefreshProductCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbSaleId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (updFlg == true)
                {
                    if (cmbSaleId.SelectedValue == null)
                    {
                        AllClear();
                    }
                    else
                    {
                        int index = Sale.FindIndex(x => x.SaID == int.Parse(cmbSaleId.SelectedValue.ToString()));
                        textBoxOrderId.Text = Sale[index].OrID.ToString();
                        cmbClientId.SelectedIndex = Client.FindIndex(x => x.ClID == Sale[index].ClID);
                        cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == Sale[index].EmID);
                        dateTimePickerS.Text = Sale[index].SaDate.ToString();
                        checkBoxFlag.Checked = Sale[index].SaBoolFlag;
                        if (Sale[index].SaHidden == null)
                        {
                            textBoxHideRea.Text = "";
                        }
                        else
                        {
                            textBoxHideRea.Text = Sale[index].SaHidden.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void dateTimePickerS_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnSea.Checked && (!dateTimePickerS.Checked && !dateTimePickerE.Checked))
                {
                    label7.Enabled = false;
                }
                else
                {
                    label7.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void dateTimePickerE_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnSea.Checked && (!dateTimePickerS.Checked && !dateTimePickerE.Checked))
                {
                    label7.Enabled = false;
                }
                else
                {
                    label7.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private void cmbEmployeeId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (cmbEmployeeId.SelectedIndex == -1 || cmbEmployeeId.SelectedIndex == 0 || cmbEmployeeId.SelectedIndex == Employee.Count - 1)
                {
                    return;
                }
                int temp = SalesOffice.FindIndex(x => x.SoID == Employee.Single(y => y.EmID == int.Parse(cmbEmployeeId.SelectedValue.ToString())).SoID);
                if (temp == -1)
                {
                    cmbSalesOfficeId.SelectedIndex = SalesOffice.Count - 1;
                }
                else
                {
                    cmbSalesOfficeId.SelectedIndex = temp;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbClientId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (cmbClientId.SelectedIndex == -1 || cmbClientId.SelectedIndex == 0 || cmbClientId.SelectedIndex == Client.Count - 1)
                {
                    return;
                }
                int temp = SalesOffice.FindIndex(x => x.SoID == Client.Single(y => y.ClID == int.Parse(cmbClientId.SelectedValue.ToString())).SoID);
                if (temp == -1)
                {
                    cmbSalesOfficeId.SelectedIndex = SalesOffice.Count - 1;
                }
                else
                {
                    cmbSalesOfficeId.SelectedIndex = temp;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
    }
}
