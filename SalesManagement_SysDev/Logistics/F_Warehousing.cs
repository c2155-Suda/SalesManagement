using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    public partial class F_Warehousing : Form
    {
        WarehousingDbConnection warehousing = new WarehousingDbConnection();
        F_WarehousingDetail f_Detail;
        HattyuDbConnection hattyu = new HattyuDbConnection();
        EmployeeDbConnection employee = new EmployeeDbConnection();
        ProductDbConnect product = new ProductDbConnect();
        DataAccess access = new DataAccess();
        InputFormCheck icheck = new InputFormCheck();
        private List<T_WarehousingDsp> Warehousing;
        private List<T_WarehousingDsp> subWarehousing;
        private List<M_MakerDsp> Maker;
        private List<M_EmployeeDsp> Employee;
        private List<M_ProductDsp> Product;
        private List<M_MajorClassificationDsp> majorClass;
        private List<M_SmallClassificationDsp> smallClass;
        private List<StateItemSet> State = new List<StateItemSet>();
        private int pageSize = 12;
        private bool updFlg;
        public int LoginEmID;
        public bool loginUserInfoCheck = true;
        private class StateItemSet
        {
            public string itemDisp { get; set; }
            public int itemValue { get; set; }
        }
        public F_Warehousing()
        {
            InitializeComponent();
        }
        private void F_Warehousing_Load(object sender, EventArgs e)
        {
            Employee = employee.GetEmployeeData();
            Maker = access.GetMakerDspData();
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
                //IDのコンボボックスData読込
                cmbWarehousingId.DisplayMember = "WaID";
                cmbWarehousingId.ValueMember = "WaID";
                cmbWarehousingId.DataSource = Warehousing;
                cmbWarehousingId.SelectedIndex = -1;

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
                int WaId = int.Parse(cmbWarehousingId.SelectedValue.ToString());
                var warehousingData = Warehousing.Single(x => x.WaID == WaId);
                T_WarehousingDsp updatedWarehousingData;
                if (!GetValidDataAtConfirm(warehousingData))
                {
                    updatedWarehousingData = warehousing.GetWarehousingData(new T_WarehousingRead { WaID = WaId, WaFlag = -1, WaShelfFlag = -1 }).Single();
                    warehousingData.WaShelfFlag = updatedWarehousingData.WaShelfFlag;
                    warehousingData.WaShelfFlagBool = updatedWarehousingData.WaShelfFlagBool;
                    warehousingData.EmID = updatedWarehousingData.EmID;
                    warehousingData.EmName = updatedWarehousingData.EmName;
                    warehousingData.WaDate = updatedWarehousingData.WaDate;
                    dataGridViewDsp.Refresh();
                    checkBoxFlag.Checked = updatedWarehousingData.WaShelfFlagBool;
                    if (updatedWarehousingData.WaHidden == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == updatedWarehousingData.EmID);
                    cmbState.SelectedIndex = State.FindIndex(x => x.itemValue == updatedWarehousingData.WaShelfFlag);
                    if (updatedWarehousingData.WaShelfFlag != 0)
                    {
                        dateTimePickerS.Format = DateTimePickerFormat.Long;
                        dateTimePickerS.Text = updatedWarehousingData.WaDate.ToString();
                        dateTimePickerS.Checked = true;
                    }
                    return;
                }
                var warehousingDetailData = warehousing.GetWarehousingDetailData(new T_WarehousingDetailDsp { WaID = WaId,WaDetailID=0 });
                if (!GetValidDetailDataAtConfirm(warehousingDetailData))
                {
                    return;
                }
                DialogResult dialog = MessageBox.Show(string.Format("入庫ID：{0}を確定しますか。", warehousingData.WaID), "確認", MessageBoxButtons.OKCancel);
                if (dialog != DialogResult.OK)
                {
                    return;
                }
                //確定処理
                warehousing.FinalizeWarehousingData(WaId, LoginEmID);
                updatedWarehousingData = warehousing.GetWarehousingData(new T_WarehousingRead { WaID = WaId, WaFlag = -1, WaShelfFlag = -1 }).Single();
                warehousingData.WaShelfFlag = updatedWarehousingData.WaShelfFlag;
                warehousingData.WaShelfFlagBool = updatedWarehousingData.WaShelfFlagBool;
                warehousingData.EmID = updatedWarehousingData.EmID;
                warehousingData.EmName = updatedWarehousingData.EmName;
                warehousingData.WaDate = updatedWarehousingData.WaDate;
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
                if (cmbWarehousingId.SelectedIndex == -1)
                {
                    MessageBox.Show("入庫IDを選択してください。", "選択エラー");
                    cmbWarehousingId.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private bool GetValidDataAtConfirm(T_WarehousingDsp warehousingData)
        {
            try
            {
                if (!warehousing.CheckWarehousingIsInsertable((int)warehousingData.WaID))
                {
                    MessageBox.Show("選択した入庫IDは確定済です。", "選択エラー");
                    cmbWarehousingId.Focus();
                    return false;
                }
                if (!hattyu.CheckHattyuIsActive(warehousingData.HaID))
                {
                    MessageBox.Show("選択した入庫に対応する発注は削除されています。", "選択エラー");
                    cmbWarehousingId.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private bool GetValidDetailDataAtConfirm(List<T_WarehousingDetailDsp> warehousingDetailData)
        {
            try
            {
                foreach (var x in warehousingDetailData)
                {
                    int stock = product.GetStockQuant(x.PrID);
                    if (stock + x.WaQuantity > NumericRange.Quant)
                    {
                        MessageBox.Show(string.Format("商品ID：{0}の在庫が超過します。", x.PrID), "選択エラー");
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

        private void btnDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbWarehousingId.SelectedIndex == -1)
                {
                    MessageBox.Show("入庫IDを選択してください。", "選択エラー");
                    cmbWarehousingId.Focus();
                    return;
                }
                f_Detail = new F_WarehousingDetail();
                int waID = (int)cmbWarehousingId.SelectedValue;
                f_Detail.waID = waID;
                f_Detail.status = int.Parse(cmbState.SelectedValue.ToString());
                f_Detail.haFlag = hattyu.CheckHattyuIsActive(int.Parse(textBoxHattyuId.Text)) ? 0 : 2;
                f_Detail.haID = int.Parse(textBoxHattyuId.Text);
                f_Detail.logEmID = LoginEmID;
                f_Detail.ShowDialog();
                var warehousingData = Warehousing.Single(x => x.WaID == waID);
                var updatedWarehousingData = warehousing.GetWarehousingData(new T_WarehousingRead { WaID = waID, WaFlag = -1, WaShelfFlag = -1 }).Single();
                warehousingData.WaShelfFlag = updatedWarehousingData.WaShelfFlag;
                warehousingData.WaShelfFlagBool = updatedWarehousingData.WaShelfFlagBool;
                warehousingData.EmID = updatedWarehousingData.EmID;
                warehousingData.EmName = updatedWarehousingData.EmName;
                warehousingData.WaDate = updatedWarehousingData.WaDate;
                dataGridViewDsp.Refresh();
                checkBoxFlag.Checked = updatedWarehousingData.WaShelfFlagBool;
                if (updatedWarehousingData.WaHidden == null)
                {
                    textBoxHideRea.Text = "";
                }
                cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == updatedWarehousingData.EmID);
                cmbState.SelectedIndex = State.FindIndex(x => x.itemValue == warehousingData.WaShelfFlag);
                if (updatedWarehousingData.WaShelfFlag != 0)
                {
                    dateTimePickerS.Format = DateTimePickerFormat.Long;
                    dateTimePickerS.Text = updatedWarehousingData.WaDate.ToString();
                    dateTimePickerS.Checked = true;
                }
                else
                {
                    dateTimePickerS.Format = DateTimePickerFormat.Custom;
                    dateTimePickerS.CustomFormat = " ";
                    dateTimePickerS.Checked = false;
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
                if (!string.IsNullOrEmpty(textBoxHattyuId.Text.Trim()))
                {
                    if(int.TryParse(textBoxHattyuId.Text.Trim(),out int haID) && haID > 0 && haID <= NumericRange.ID)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("発注IDは1~999999の整数のみです。", "入力エラー");
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
                int waID = 0;
                if (cmbWarehousingId.Text != "")
                {
                    waID = int.Parse(cmbWarehousingId.Text);
                }
                int haID = 0;
                if (!string.IsNullOrEmpty(textBoxHattyuId.Text.Trim()))
                {
                    haID = int.Parse(textBoxHattyuId.Text.Trim());
                }
                int maID = 0;
                if (cmbMakerId.Text != "")
                {
                    maID = (int)cmbMakerId.SelectedValue;
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
                    T_WarehousingRead selectCondition = new T_WarehousingRead()
                    {
                        WaID=waID,
                        HaID = haID,
                        EmID = emID,
                        MaID = maID,
                        WaShelfFlag = status,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        WaFlag = hideFlg,
                        WaHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Warehousing = warehousing.GetWarehousingData(selectCondition);
                    subWarehousing = new List<T_WarehousingDsp>(Warehousing);
                    subWarehousing.Insert(0, new T_WarehousingDsp { WaID = null });
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
                    T_WarehousingRead selectCondition = new T_WarehousingRead()
                    {
                        WaID=waID,
                        HaID = haID,
                        EmID = emID,
                        MaID = maID,
                        WaShelfFlag = status,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        WaFlag = hideFlg,
                        WaHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Warehousing = warehousing.GetWarehousingData(selectCondition, sdate, edate);
                    subWarehousing = new List<T_WarehousingDsp>(Warehousing);
                    subWarehousing.Insert(0, new T_WarehousingDsp { WaID = null });
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
                if (cmbWarehousingId.SelectedIndex == -1)
                {
                    MessageBox.Show("入庫IDを選択してください。", "選択エラー");
                    cmbWarehousingId.Focus();
                    return;
                }
                int waID = (int)cmbWarehousingId.SelectedValue;
                int iFlg = 0;
                if (checkBoxFlag.Checked == false)
                {
                    textBoxHideRea.Text = "";
                }
                else
                {
                    iFlg = 2;
                }
                var warehousingData = Warehousing.Single(x => x.WaID == waID);
                if (iFlg == 2 && warehousingData.WaFlag == 0 && hattyu.CheckHattyuIsActive(int.Parse(textBoxHattyuId.Text)))
                {
                    DialogResult dialog = MessageBox.Show(string.Format("入庫ID：{0}は確定されておらず、対応する発注ID：{1}は削除されていませんがよろしいですか。", warehousingData.WaID, warehousingData.HaID), "確認", MessageBoxButtons.OKCancel);
                    if (dialog != DialogResult.OK)
                    {
                        return;
                    }
                }
                string wahidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim();
                warehousing.UpdateWarehousingHidden(waID, iFlg, wahidden);
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
        private void RefreshMakerCombo()
        {
            try
            {
                int temp = 0;
                if (cmbMakerId.SelectedIndex != -1)
                {
                    temp = int.Parse(cmbMakerId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    Maker.Insert(0, new M_MakerDsp { MaID = 0, MaName = "" });
                    Maker.Add(new M_MakerDsp { MaID = -1, MaName = "削除済み" });
                }
                else
                {
                    Maker.RemoveAll(x => x.MaID == 0 || x.MaID == -1);
                }
                cmbMakerId.DataSource = null;
                cmbMakerId.DisplayMember = "MaName";
                cmbMakerId.ValueMember = "MaID";
                cmbMakerId.DataSource = Maker;
                cmbMakerId.SelectedIndex = Maker.FindIndex(x => x.MaID == temp);
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
        private void RefreshWarehousingCombo()
        {
            try
            {
                int temp = 0;
                if (cmbWarehousingId.SelectedValue != null)
                {
                    temp = int.Parse(cmbWarehousingId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    cmbWarehousingId.DataSource = subWarehousing;
                    cmbWarehousingId.SelectedIndex = subWarehousing.FindIndex(x => x.WaID == temp);
                }
                else
                {
                    cmbWarehousingId.DataSource = Warehousing;
                    cmbWarehousingId.SelectedIndex = Warehousing.FindIndex(x => x.WaID == temp);
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
                cmbWarehousingId.SelectedIndex = -1;
                textBoxHattyuId.Text = "";
                cmbMakerId.SelectedIndex = -1;
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
                dataGridViewDsp.DataSource = Warehousing;

                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Warehousing.Skip(pageSize * pageNo).Take(pageSize).ToList();

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Warehousing.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshWarehousingCombo();
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
                Warehousing = warehousing.GetWarehousingData();
                subWarehousing = new List<T_WarehousingDsp>(Warehousing);
                subWarehousing.Insert(0, new T_WarehousingDsp { WaID = null });
                if (Warehousing == null)
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
                dataGridViewDsp.DataSource = Warehousing;
                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Warehousing.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                ////入庫ID
                dataGridViewDsp.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[0].Width = 95;
                ///発注ID
                dataGridViewDsp.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[1].Width = 95;
                ////メーカーID
                dataGridViewDsp.Columns[2].Visible = false;
                ////メーカー名
                dataGridViewDsp.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[3].Width = 170;
                ////社員ID
                dataGridViewDsp.Columns[4].Visible = false;
                ////社員名
                dataGridViewDsp.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[5].Width = 160;
                ////受注日
                dataGridViewDsp.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[6].Width = 180;
                ////論理削除
                dataGridViewDsp.Columns[7].Visible = false;
                ////論理削除フラグ
                dataGridViewDsp.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[8].Width = 90;
                ////非表示理由
                dataGridViewDsp.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[9].Width = 400;
                ////品数
                dataGridViewDsp.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[10].Width = 100;
                ////
                dataGridViewDsp.Columns[11].Visible = false;
                ////ステータス
                dataGridViewDsp.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[12].Width = 100;

                dataGridViewDsp.Columns[13].Visible = false;

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Warehousing.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshWarehousingCombo();
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
                RefreshWarehousingCombo();
                RefreshMakerCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = false;
                textBoxHattyuId.Enabled = false; //受注ID

                labelMaker.Enabled = false;
                cmbMakerId.Enabled = false; //メーカーID

                label12.Enabled = false;
                cmbState.Enabled = false; //ステータス

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                label7.Visible = false;
                dateTimePickerE.Visible = false;

                dateTimePickerS.Format = DateTimePickerFormat.Custom;
                dateTimePickerS.CustomFormat = " "; //日

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
                RefreshWarehousingCombo();
                RefreshMakerCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = false;
                textBoxHattyuId.Enabled = false; //ID

                labelMaker.Enabled = false;
                cmbMakerId.Enabled = false; //メーカーID

                label12.Enabled = false;
                cmbState.Enabled = false; //ステータス

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                label7.Visible = false;
                dateTimePickerE.Visible = false;

                dateTimePickerS.Format = DateTimePickerFormat.Custom;
                dateTimePickerS.CustomFormat = " ";
                //日

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
                RefreshWarehousingCombo();
                RefreshMakerCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = false;
                textBoxHattyuId.Enabled = false; //ID

                labelMaker.Enabled = false;
                cmbMakerId.Enabled = false; //メーカーID

                label12.Enabled = false;
                cmbState.Enabled = false; //ステータス

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                label7.Visible = false;
                dateTimePickerE.Visible = false;

                dateTimePickerS.Format = DateTimePickerFormat.Custom;
                dateTimePickerS.CustomFormat = " ";
                //日

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
                RefreshWarehousingCombo();
                RefreshMakerCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = true;
                textBoxHattyuId.Enabled = true; //ID

                labelMaker.Enabled = true;
                cmbMakerId.Enabled = true; //メーカーID

                label12.Enabled = true;
                cmbState.Enabled = true; //ステータス

                label5.Enabled = true;
                dateTimePickerS.Enabled = true;
                dateTimePickerS.Format = DateTimePickerFormat.Long;
                label7.Visible = true;
                label7.Enabled = false;
                dateTimePickerE.Visible = true; //日

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

                        label5.Enabled = true;
                        dateTimePickerS.Enabled = true;
                        dateTimePickerE.Enabled = true;
                        return;
                    }
                    else
                    {
                        cmbEmployeeId.SelectedIndex = -1;
                        dateTimePickerS.Checked = false;
                        dateTimePickerE.Checked = false;
                    }
                }
                label4.Enabled = false;
                cmbEmployeeId.Enabled = false;

                dateTimePickerS.Enabled = false;
                dateTimePickerE.Enabled = false;
                label5.Enabled = false;
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
                dataGridViewDsp.DataSource = Warehousing.Take(pageSize).ToList();

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
                dataGridViewDsp.DataSource = Warehousing.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                int lastNo = (int)Math.Ceiling(Warehousing.Count / (double)pageSize) - 1;
                //最終ページでなければ
                if (pageNo <= lastNo)
                    dataGridViewDsp.DataSource = Warehousing.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                int lastPage = (int)Math.Ceiling(Warehousing.Count / (double)pageSize);
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
                int pageNo = (int)Math.Ceiling(Warehousing.Count / (double)pageSize) - 1;
                dataGridViewDsp.DataSource = Warehousing.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                    cmbWarehousingId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
                    textBoxHattyuId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[1].Value.ToString();
                    cmbMakerId.SelectedIndex = Maker.FindIndex(x => x.MaID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[2].Value.ToString()));
                    if ((int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value != 0)
                    {
                        cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[4].Value.ToString()));
                        //日付フォーマットを元に戻す
                        dateTimePickerS.Format = DateTimePickerFormat.Long;
                        dateTimePickerS.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[6].Value.ToString();
                        dateTimePickerS.Checked = true;
                    }
                    else
                    {
                        cmbEmployeeId.SelectedIndex = -1;
                        //日付null処理
                        dateTimePickerS.Format = DateTimePickerFormat.Custom;
                        dateTimePickerS.CustomFormat = " ";
                        dateTimePickerS.Checked = false;
                    }
                    checkBoxFlag.Checked = (bool)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[8].Value;
                    //null処理を空欄処理
                    if (dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[9].Value == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    else
                        textBoxHideRea.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[9].Value.ToString();
                    cmbState.SelectedValue = (int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value;
                    if (radioBtnCon.Checked)
                    {
                        if ((int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value == 0&& (int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[13].Value==0)
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
                int makerIndex = Maker.FindIndex(x => x.MaID == selectedProduct.MaID);
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
                if (makerIndex == -1)
                {
                    cmbMakerId.SelectedIndex = Maker.Count - 1;
                }
                else
                {
                    cmbMakerId.SelectedIndex = makerIndex;
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
                int maID = 0;
                if (cmbMakerId.SelectedIndex != -1)
                {
                    maID = int.Parse(cmbMakerId.SelectedValue.ToString());
                }
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
                Product = product.GetProductData(new M_ProductDsp { PrID = 0, McID = mcID, ScID = scID,MaID=maID, PrFlag = 0, PrName = "", PrModelNumber = "", PrColor = "" });
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
                int maID = 0;
                if (cmbMakerId.SelectedIndex != -1)
                {
                    maID = int.Parse(cmbMakerId.SelectedValue.ToString());
                }
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
                Product = product.GetProductData(new M_ProductDsp { PrID = 0, McID = mcID, ScID = scID,MaID=maID, PrFlag = 0, PrName = "", PrModelNumber = "", PrColor = "" });
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
                    if (cmbWarehousingId.SelectedValue == null)
                    {
                        AllClear();
                    }
                    else
                    {
                        int index = Warehousing.FindIndex(x => x.WaID == int.Parse(cmbWarehousingId.SelectedValue.ToString()));
                        textBoxHattyuId.Text = Warehousing[index].HaID.ToString();
                        cmbMakerId.SelectedIndex = Maker.FindIndex(x => x.MaID == Warehousing[index].MaID);
                        if (Warehousing[index].WaShelfFlag != 0)
                        {
                            cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == Warehousing[index].EmID);
                            dateTimePickerS.Format = DateTimePickerFormat.Long;
                            dateTimePickerS.Text = Warehousing[index].WaDate.ToString();
                            dateTimePickerS.Checked = true;
                        }
                        else
                        {
                            cmbEmployeeId.SelectedIndex = -1;
                            dateTimePickerS.Format = DateTimePickerFormat.Custom;
                            dateTimePickerS.CustomFormat = " ";
                            dateTimePickerS.Checked = false;
                        }
                        checkBoxFlag.Checked = Warehousing[index].WaFlagBool;
                        if (Warehousing[index].WaHidden == null)
                        {
                            textBoxHideRea.Text = "";
                        }
                        else
                        {
                            textBoxHideRea.Text = Warehousing[index].WaHidden.ToString();
                        }
                        cmbState.SelectedValue = Warehousing[index].WaShelfFlag;
                        if (radioBtnCon.Checked)
                        {
                            if (Warehousing[index].WaShelfFlag == 0 && Warehousing[index].HattyuFlag==0)
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
                if (e.ColumnIndex==1&&(int)dataGridview.Rows[e.RowIndex].Cells[13].Value != 0)
                {
                    dataGridview.Rows[e.RowIndex].Cells[1].Style.BackColor = SystemColors.AppWorkspace;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbMakerId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int maID = 0;
                if (cmbMakerId.SelectedIndex != -1)
                {
                    maID = int.Parse(cmbMakerId.SelectedValue.ToString());
                }
                int mcID = 0;
                if (cmbMajor.SelectedIndex != -1)
                {
                    mcID = int.Parse(cmbMajor.SelectedValue.ToString());
                }
                int scID = 0;
                if (cmbSmall.SelectedIndex != -1)
                {
                    scID = int.Parse(cmbSmall.SelectedValue.ToString());
                }
                Product = product.GetProductData(new M_ProductDsp { PrID = 0, McID = mcID, ScID = scID, MaID = maID, PrFlag = 0, PrName = "", PrModelNumber = "", PrColor = "" });
                RefreshProductCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
    }
}
