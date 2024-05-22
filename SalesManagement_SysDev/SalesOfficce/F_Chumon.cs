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
    public partial class F_Chumon : Form
    {
        ChumonDbConnection chumon = new ChumonDbConnection();
        F_ChumonDetail f_Detail;
        OrderDbConnection order = new OrderDbConnection();
        EmployeeDbConnection employee = new EmployeeDbConnection();
        ClientDbConnection client = new ClientDbConnection();
        ProductDbConnect product = new ProductDbConnect();
        DataAccess access = new DataAccess();
        InputFormCheck icheck = new InputFormCheck();
        private List<T_ChumonDsp> Chumon;
        private List<T_ChumonDsp> subChumon;
        private List<M_SalesOfficeDsp> SalesOffice;
        private List<M_ClientDsp> Client;
        private List<M_EmployeeDsp> Employee;
        private List<M_ProductDsp> Product;
        private List<M_MajorClassificationDsp> majorClass;
        private List<M_SmallClassificationDsp> smallClass;
        private List<StateItemSet> State = new List<StateItemSet>();
        private int pageSize = 12;
        private bool updFlg;
        public int LoginEmID;
        public int LoginSoID;
        public bool loginUserInfoCheck = true;
        private class StateItemSet
        {
            public string itemDisp { get; set; }
            public int itemValue { get; set; }
        }
        public F_Chumon()
        {
            InitializeComponent();
        }
        private void F_Chumon_Load(object sender, EventArgs e)
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
                if (!Employee.Any(x => x.EmID == LoginEmID))
                {
                    MessageBox.Show("ログイン中の社員IDは削除済みです", "エラー");
                    return false;
                }
                if (!SalesOffice.Any(x => x.SoID == LoginSoID))
                {
                    MessageBox.Show("ログイン中の営業所IDは削除済みです", "エラー");
                    return false;
                }
                cmbSalesOfficeId.SelectedIndexChanged -= cmbSalesOfficeId_SelectedIndexChanged;
                GetComboBoxData();
                cmbSalesOfficeId.SelectedIndexChanged += cmbSalesOfficeId_SelectedIndexChanged;
                cmbSalesOfficeId.SelectedIndex = SalesOffice.FindIndex(x => x.SoID == LoginSoID);
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
                cmbChumonId.DisplayMember = "ChID";
                cmbChumonId.ValueMember = "ChID";
                cmbChumonId.DataSource = Chumon;
                cmbChumonId.SelectedIndex = -1;

                //営業所IDのコンボボックスData読込
                cmbSalesOfficeId.DisplayMember = "SoName";
                cmbSalesOfficeId.ValueMember = "SoID";
                cmbSalesOfficeId.DataSource = SalesOffice;
                cmbSalesOfficeId.SelectedIndex = -1;

                //大分類のコンボボックスData読込
                majorClass = access.GetMajorClassificationDspData();
                majorClass.Insert(0, new M_MajorClassificationDsp { McID = 0, McName = "" });
                majorClass.Add(new M_MajorClassificationDsp { McID = -1, McName = "削除済み" });
                cmbMajor.DisplayMember = "McName";
                cmbMajor.ValueMember = "McID";
                cmbMajor.DataSource = majorClass;
                cmbMajor.SelectedIndex = -1;

                //ステータスのコンボボックスData読込
                State.Add(new StateItemSet { itemValue = -1, itemDisp = "" });
                State.Add(new StateItemSet { itemValue = 0, itemDisp = "未確定" });
                State.Add(new StateItemSet { itemValue = 1, itemDisp = "確定済" });

                cmbState.DisplayMember = "itemDisp";
                cmbState.ValueMember = "itemValue";
                cmbState.DataSource = State;
                cmbState.SelectedIndex = -1;
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

        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            {
                if (!InputCheckAtConfirm())
                    return;
                int ChId = int.Parse(cmbChumonId.SelectedValue.ToString());
                var chumonData = Chumon.Single(x => x.ChID == ChId);
                T_ChumonDsp updatedChumonData;
                if (!GetValidDataAtConfirm(chumonData))
                {
                    updatedChumonData = chumon.GetChumonData(new T_ChumonRead { ChID = ChId, ChFlag = -1, ChStateFlag = -1 }).Single();
                    chumonData.ChStateFlag = updatedChumonData.ChStateFlag;
                    chumonData.ChBoolStateFlag = updatedChumonData.ChBoolStateFlag;
                    chumonData.EmID = updatedChumonData.EmID;
                    chumonData.EmName = updatedChumonData.EmName;
                    dataGridViewDsp.Refresh();
                    checkBoxFlag.Checked = updatedChumonData.ChBoolFlag;
                    if (updatedChumonData.ChHidden == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == updatedChumonData.EmID);
                    cmbState.SelectedIndex = State.FindIndex(x => x.itemValue == updatedChumonData.ChStateFlag);
                    return;
                }
                var chumonDetailData = chumon.GetChumonDetailData(new T_ChumonDetailDsp { ChID = ChId,ChDetailID=0 });
                if (!GetValidDetailDataAtConfirm(chumonDetailData))
                {
                    return;
                }
                //確定情報作成
                var syukkoData = GenerateDataAtConfirm(chumonData);
                //確定詳細情報作成
                var syukkoDetailData = GenerateDetailDataAtConfirm(chumonDetailData);
                DialogResult dialog = MessageBox.Show(string.Format("注文ID：{0}を確定しますか。", chumonData.ChID), "確認", MessageBoxButtons.OKCancel);
                if (dialog != DialogResult.OK)
                {
                    return;
                }
                //確定処理
                chumon.FinalizeChumonData(syukkoData, syukkoDetailData, LoginEmID);
                updatedChumonData = chumon.GetChumonData(new T_ChumonRead { ChID = ChId, ChFlag = -1, ChStateFlag = -1 }).Single();
                chumonData.ChStateFlag = updatedChumonData.ChStateFlag;
                chumonData.ChBoolStateFlag = updatedChumonData.ChBoolStateFlag;
                chumonData.EmID = updatedChumonData.EmID;
                chumonData.EmName = updatedChumonData.EmName;
                dataGridViewDsp.Refresh();
                AllClear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private bool InputCheckAtConfirm()
        {
            try
            {
                if (cmbChumonId.SelectedIndex == -1)
                {
                    MessageBox.Show("注文IDを選択してください。", "選択エラー");
                    cmbChumonId.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private bool GetValidDataAtConfirm(T_ChumonDsp chumonData)
        {
            try
            {
                if (!chumon.CheckChumonIsInsertable((int)chumonData.ChID))
                {
                    MessageBox.Show("選択した注文IDは確定済です。", "選択エラー");
                    cmbChumonId.Focus();
                    return false;
                }
                if (!order.CheckOrderIsActive(chumonData.OrID))
                {
                    MessageBox.Show("選択した注文に対応する受注は削除されています。", "選択エラー");
                    cmbChumonId.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private bool GetValidDetailDataAtConfirm(List<T_ChumonDetailDsp> chumonDetailData)
        {
            try
            {
                foreach (var x in chumonDetailData)
                {
                    int stock = product.GetStockQuant(x.PrID);
                    if (stock - x.ChQuantity < 0)
                    {
                        MessageBox.Show(string.Format("商品ID：{0}の在庫が不足しています。", x.PrID), "選択エラー");
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
        private T_Syukko GenerateDataAtConfirm(T_ChumonDsp chumonData)
        {
            try
            {
                return new T_Syukko
                {
                    OrID = chumonData.OrID,
                    SoID = chumonData.SoID,
                    ClID = chumonData.ClID,
                };
            }
            catch
            {
                throw;
            }
        }
        private List<T_SyukkoDetail> GenerateDetailDataAtConfirm(List<T_ChumonDetailDsp> chumonDetailData)
        {
            try
            {
                var syukkoDetailData = new List<T_SyukkoDetail>();
                foreach (var x in chumonDetailData)
                {
                    syukkoDetailData.Add(new T_SyukkoDetail
                    {
                        PrID = x.PrID,
                        SyQuantity = x.ChQuantity
                    });
                }
                return syukkoDetailData;
            }
            catch
            {
                throw;
            }
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbChumonId.SelectedIndex == -1)
                {
                    MessageBox.Show("注文IDを選択してください。", "選択エラー");
                    cmbChumonId.Focus();
                    return;
                }
                f_Detail = new F_ChumonDetail();
                int chID = (int)cmbChumonId.SelectedValue;
                f_Detail.chID = chID;
                f_Detail.status = int.Parse(cmbState.SelectedValue.ToString());
                f_Detail.orFlag = order.CheckOrderIsActive(int.Parse(textBoxOrderId.Text)) ? 0 : 2;
                f_Detail.orID = int.Parse(textBoxOrderId.Text);
                f_Detail.logEmID = LoginEmID;
                f_Detail.ShowDialog();
                var chumonData = Chumon.Single(x => x.ChID == chID);
                var updatedChumonData = chumon.GetChumonData(new T_ChumonRead { ChID = chID, ChFlag = -1, ChStateFlag = -1 }).Single();
                chumonData.ChStateFlag = updatedChumonData.ChStateFlag;
                chumonData.ChBoolStateFlag = updatedChumonData.ChBoolStateFlag;
                chumonData.EmID = updatedChumonData.EmID;
                chumonData.EmName = updatedChumonData.EmName;
                dataGridViewDsp.Refresh();
                checkBoxFlag.Checked = updatedChumonData.ChBoolFlag;
                if (updatedChumonData.ChHidden == null)
                {
                    textBoxHideRea.Text = "";
                }
                cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == updatedChumonData.EmID);
                cmbState.SelectedIndex = State.FindIndex(x => x.itemValue == chumonData.ChStateFlag);
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
                int chID = 0;
                if (cmbChumonId.Text != "")
                {
                    chID = int.Parse(cmbChumonId.Text);
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

                int status = -1;
                if (cmbState.SelectedIndex != -1)
                {
                    status = (int)cmbState.SelectedValue;
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
                    T_ChumonRead selectCondition = new T_ChumonRead()
                    {
                        ChID=chID,
                        OrID = orID,
                        SoID = soID,
                        EmID = emID,
                        ClID = clID,
                        ChStateFlag = status,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        ChFlag = hideFlg,
                        ChHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Chumon = chumon.GetChumonData(selectCondition);
                    subChumon = new List<T_ChumonDsp>(Chumon);
                    subChumon.Insert(0, new T_ChumonDsp { ChID = null });
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
                    T_ChumonRead selectCondition = new T_ChumonRead()
                    {
                        ChID=chID,
                        OrID = orID,
                        SoID = soID,
                        EmID = emID,
                        ClID = clID,
                        ChStateFlag = status,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        ChFlag = hideFlg,
                        ChHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Chumon = chumon.GetChumonData(selectCondition, sdate, edate);
                    subChumon = new List<T_ChumonDsp>(Chumon);
                    subChumon.Insert(0, new T_ChumonDsp { ChID = null });
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
                if (cmbChumonId.SelectedIndex == -1)
                {
                    MessageBox.Show("注文IDを選択してください。", "選択エラー");
                    cmbChumonId.Focus();
                    return;
                }
                int chID = (int)cmbChumonId.SelectedValue;
                int iFlg = 0;
                if (checkBoxFlag.Checked == false)
                {
                    textBoxHideRea.Text = "";
                }
                else
                {
                    iFlg = 2;
                }
                var chumonData = Chumon.Single(x => x.ChID == chID);
                if (iFlg == 2 && chumonData.ChFlag == 0 && order.CheckOrderIsActive(int.Parse(textBoxOrderId.Text)))
                {
                    DialogResult dialog = MessageBox.Show(string.Format("注文ID：{0}は確定されておらず、対応する受注ID：{1}は削除されていませんがよろしいですか。", chumonData.ChID, chumonData.OrID), "確認", MessageBoxButtons.OKCancel);
                    if (dialog != DialogResult.OK)
                    {
                        return;
                    }
                }
                string orhidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim();
                chumon.UpdateChumonHidden(chID, iFlg, orhidden);
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
        private void RefreshChumonCombo()
        {
            try
            {
                int temp = 0;
                if (cmbChumonId.SelectedValue != null)
                {
                    temp = int.Parse(cmbChumonId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    cmbChumonId.DataSource = subChumon;
                    cmbChumonId.SelectedIndex = subChumon.FindIndex(x => x.ChID == temp);
                }
                else
                {
                    cmbChumonId.DataSource = Chumon;
                    cmbChumonId.SelectedIndex = Chumon.FindIndex(x => x.ChID == temp);
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
                cmbChumonId.SelectedIndex = -1;
                textBoxOrderId.Text = "";
                cmbClientId.SelectedIndex = -1;
                cmbEmployeeId.SelectedIndex = -1;
                cmbProductId.SelectedIndex = -1;
                cmbMajor.SelectedIndex = -1;
                cmbSmall.SelectedIndex = -1;
                cmbState.SelectedIndex = -1;
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
                dataGridViewDsp.DataSource = Chumon;

                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Chumon.Skip(pageSize * pageNo).Take(pageSize).ToList();

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Chumon.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshChumonCombo();
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
                Chumon = chumon.GetChumonData(new T_ChumonRead { SoID = (int)cmbSalesOfficeId.SelectedValue, ChStateFlag = -1 });
                subChumon = new List<T_ChumonDsp>(Chumon);
                subChumon.Insert(0, new T_ChumonDsp { ChID = null });
                if (Chumon == null)
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
                dataGridViewDsp.DataSource = Chumon;
                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Chumon.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                ////注文ID
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
                ////
                dataGridViewDsp.Columns[13].Visible = false;
                ////ステータス
                dataGridViewDsp.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[14].Width = 100;

                dataGridViewDsp.Columns[15].Visible = false;

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Chumon.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshChumonCombo();
            }
            catch
            {
                throw;
            }
        }

        private void radioBtnCon_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnCon.Checked == false)
                {
                    return;
                }
                RefreshChumonCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = false;
                textBoxOrderId.Enabled = false; //受注ID

                label2.Enabled = false;
                cmbClientId.Enabled = false; //顧客ID

                label12.Enabled = false;
                cmbState.Enabled = false; //ステータス

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                dateTimePickerS.Checked = true;
                label7.Visible = false;
                dateTimePickerE.Visible = false; //注文日

                label11.Visible = false;
                cmbProductId.Visible = false; //商品ID
                label3.Visible = false;
                cmbMajor.Visible = false; //大分類ID
                label9.Visible = false;
                cmbSmall.Visible = false; //小分類ID

                checkBoxFlag.Enabled = false; //非表示フラグ

                btnCon.Enabled = true;
                btnDetail.Enabled = false;
                btnSea.Enabled = false;
                btnHidden.Enabled = false; //ボタン有効化

                updFlg = true; //自動入力フラグ
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
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
                RefreshChumonCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = false;
                textBoxOrderId.Enabled = false; //受注ID

                label2.Enabled = false;
                cmbClientId.Enabled = false; //顧客ID

                label12.Enabled = false;
                cmbState.Enabled = false; //ステータス

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                dateTimePickerS.Checked = true;
                label7.Visible = false;
                dateTimePickerE.Visible = false; //注文日

                label11.Visible = false;
                cmbProductId.Visible = false; //商品ID
                label3.Visible = false;
                cmbMajor.Visible = false; //大分類ID
                label9.Visible = false;
                cmbSmall.Visible = false; //小分類ID

                checkBoxFlag.Enabled = false; //非表示フラグ

                btnCon.Enabled = false;
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
                RefreshChumonCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = false;
                textBoxOrderId.Enabled = false; //受注ID

                label2.Enabled = false;
                cmbClientId.Enabled = false; //顧客ID

                label12.Enabled = false;
                cmbState.Enabled = false; //ステータス

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                dateTimePickerS.Checked = true;
                label7.Visible = false;
                dateTimePickerE.Visible = false; //注文日

                label11.Visible = false;
                cmbProductId.Visible = false; //商品ID
                label3.Visible = false;
                cmbMajor.Visible = false; //大分類ID
                label9.Visible = false;
                cmbSmall.Visible = false; //小分類ID

                checkBoxFlag.Enabled = true; //非表示フラグ

                btnCon.Enabled = false;
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
                RefreshChumonCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = true;
                textBoxOrderId.Enabled = true; //受注ID

                label2.Enabled = true;
                cmbClientId.Enabled = true; //顧客ID

                label12.Enabled = true;
                cmbState.Enabled = true; //ステータス

                label5.Enabled = true;
                dateTimePickerS.Enabled = true;
                label7.Visible = true;
                label7.Enabled = false;
                dateTimePickerE.Visible = true; //注文日

                label11.Visible = true;
                cmbProductId.Visible = true; //商品ID
                label3.Visible = true;
                cmbMajor.Visible = true; //大分類ID
                label9.Visible = true;
                cmbSmall.Visible = true; //小分類ID

                checkBoxFlag.Enabled = true; //非表示フラグ

                btnCon.Enabled = false;
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

        private void cmbState_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnSea.Checked)
                {
                    if (cmbState.SelectedIndex == 2)
                    {
                        label4.Enabled = true;
                        cmbEmployeeId.Enabled = true;
                        return;
                    }
                    else
                    {
                        cmbEmployeeId.SelectedIndex = -1;
                    }
                }
                label4.Enabled = false;
                cmbEmployeeId.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewDsp.DataSource = Chumon.Take(pageSize).ToList();

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
                dataGridViewDsp.DataSource = Chumon.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                int lastNo = (int)Math.Ceiling(Chumon.Count / (double)pageSize) - 1;
                //最終ページでなければ
                if (pageNo <= lastNo)
                    dataGridViewDsp.DataSource = Chumon.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                int lastPage = (int)Math.Ceiling(Chumon.Count / (double)pageSize);
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
                int pageNo = (int)Math.Ceiling(Chumon.Count / (double)pageSize) - 1;
                dataGridViewDsp.DataSource = Chumon.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                    cmbChumonId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
                    textBoxOrderId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[1].Value.ToString();
                    cmbClientId.SelectedIndex = Client.FindIndex(x => x.ClID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[2].Value.ToString()));
                    if ((int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[13].Value != 0)
                    {
                        cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[6].Value.ToString()));
                    }
                    else
                    {
                        cmbEmployeeId.SelectedIndex = -1;
                    }
                    dateTimePickerS.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[8].Value.ToString();
                    checkBoxFlag.Checked = (bool)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[10].Value;
                    //null処理を空欄処理
                    if (dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    else
                        textBoxHideRea.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value.ToString();
                    cmbState.SelectedValue = (int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[13].Value;
                    if (radioBtnCon.Checked)
                    {
                        if ((int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[13].Value == 0&& (int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[15].Value==0)
                        {
                            btnCon.Enabled = true;
                        }
                        else
                        {
                            btnCon.Enabled = false;
                        }
                    }
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

        private void cmbChumonId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (updFlg == true)
                {
                    if (cmbChumonId.SelectedValue == null)
                    {
                        AllClear();
                    }
                    else
                    {
                        int index = Chumon.FindIndex(x => x.ChID == int.Parse(cmbChumonId.SelectedValue.ToString()));
                        textBoxOrderId.Text = Chumon[index].OrID.ToString();
                        cmbClientId.SelectedIndex = Client.FindIndex(x => x.ClID == Chumon[index].ClID);
                        if (Chumon[index].ChStateFlag != 0)
                        {
                            cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == Chumon[index].EmID);
                        }
                        else
                        {
                            cmbEmployeeId.SelectedIndex = -1;
                        }
                        dateTimePickerS.Text = Chumon[index].ChDate.ToString();
                        checkBoxFlag.Checked = Chumon[index].ChBoolFlag;
                        if (Chumon[index].ChHidden == null)
                        {
                            textBoxHideRea.Text = "";
                        }
                        else
                        {
                            textBoxHideRea.Text = Chumon[index].ChHidden.ToString();
                        }
                        cmbState.SelectedValue = Chumon[index].ChStateFlag;
                        if (radioBtnCon.Checked)
                        {
                            if (Chumon[index].ChStateFlag == 0 && Chumon[index].OrderFlag==0)
                            {
                                btnCon.Enabled = true;
                            }
                            else
                            {
                                btnCon.Enabled = false;
                            }
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

        private void dataGridViewDsp_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                DataGridView dataGridview = (DataGridView)sender;
                if (e.ColumnIndex==1&&(int)dataGridview.Rows[e.RowIndex].Cells[15].Value != 0)
                {
                    dataGridview.Rows[e.RowIndex].Cells[1].Style.BackColor = SystemColors.AppWorkspace;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
    }
}
