using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    public partial class F_Shipment : Form
    {
        public F_Shipment()
        {
            InitializeComponent();
        }
        ShipmentDbConnection shipment = new ShipmentDbConnection();
        F_ShipmentDetail f_Detail;
        OrderDbConnection order = new OrderDbConnection();
        EmployeeDbConnection employee = new EmployeeDbConnection();
        ClientDbConnection client = new ClientDbConnection();
        ProductDbConnect product = new ProductDbConnect();
        DataAccess access = new DataAccess();
        InputFormCheck icheck = new InputFormCheck();
        private List<T_ShipmentDsp> Shipment;
        private List<T_ShipmentDsp> subShipment;
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

        private void F_Shipment_Load(object sender, EventArgs e)
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
                cmbShipmentId.DisplayMember = "ShID";
                cmbShipmentId.ValueMember = "ShID";
                cmbShipmentId.DataSource = Shipment;
                cmbShipmentId.SelectedIndex = -1;

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
                int Shid = int.Parse(cmbShipmentId.SelectedValue.ToString());
                var shipmentData = Shipment.Single(x => x.ShID == Shid);
                T_ShipmentDsp updatedShipmentData;
                if (!GetValidDataAtConfirm(shipmentData))
                {
                    updatedShipmentData = shipment.GetShipmentData(new T_ShipmentRead { ShID = Shid, ShFlag = -1, ShStateFlag = -1 }).Single();
                    shipmentData.ShStateFlag = updatedShipmentData.ShStateFlag;
                    shipmentData.ShBoolStateFlag = updatedShipmentData.ShBoolStateFlag;
                    shipmentData.EmID = updatedShipmentData.EmID;
                    shipmentData.EmName = updatedShipmentData.EmName;
                    shipmentData.ShDate = updatedShipmentData.ShDate;
                    dataGridViewDsp.Refresh();
                    checkBoxFlag.Checked = updatedShipmentData.ShBoolFlag;
                    if (updatedShipmentData.ShHidden == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == updatedShipmentData.EmID);
                    cmbState.SelectedIndex = State.FindIndex(x => x.itemValue == updatedShipmentData.ShStateFlag);
                    if (updatedShipmentData.ShStateFlag != 0)
                    {
                        dateTimePickerS.Format = DateTimePickerFormat.Long;
                        dateTimePickerS.Text = updatedShipmentData.ShDate.ToString();
                        dateTimePickerS.Checked = true;
                    }
                    return;
                }
                var shipmentDetailData = shipment.GetShipmentDetailData(new T_ShipmentDetailDsp { ShID = Shid, ShDetailID = 0 });
                //確定情報作成
                var saleData = GenerateDataAtConfirm(shipmentData);
                //確定詳細情報作成
                var orderDetailData = order.GetOrderDetailData_Agr_Fin(new T_OrderDetailDsp { OrID = shipmentData.OrID, OrDetailID = 0 });
                var saleDetailData = GenerateDetailDataAtConfirm(shipmentDetailData,orderDetailData);
                DialogResult dialog = MessageBox.Show(string.Format("出荷ID：{0}を確定しますか。", shipmentData.ShID), "確認", MessageBoxButtons.OKCancel);
                if (dialog != DialogResult.OK)
                {
                    return;
                }
                //確定処理
                shipment.FinalizeShipmentData(saleData, saleDetailData);
                updatedShipmentData = shipment.GetShipmentData(new T_ShipmentRead { ShID = Shid, ShFlag = -1, ShStateFlag = -1 }).Single();
                shipmentData.ShStateFlag = updatedShipmentData.ShStateFlag;
                shipmentData.ShBoolStateFlag = updatedShipmentData.ShBoolStateFlag;
                shipmentData.EmID = updatedShipmentData.EmID;
                shipmentData.EmName = updatedShipmentData.EmName;
                shipmentData.ShDate = updatedShipmentData.ShDate;
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
                if (cmbShipmentId.SelectedIndex == -1)
                {
                    MessageBox.Show("出荷IDを選択してください。", "選択エラー");
                    cmbShipmentId.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private bool GetValidDataAtConfirm(T_ShipmentDsp shipmentData)
        {
            try
            {
                if (!shipment.CheckShipmentIsInsertable((int)shipmentData.ShID))
                {
                    MessageBox.Show("選択した出荷IDは確定済です。", "選択エラー");
                    cmbShipmentId.Focus();
                    return false;
                }
                if (!order.CheckOrderIsActive(shipmentData.OrID))
                {
                    MessageBox.Show("選択した出荷に対応する受注は削除されています。", "選択エラー");
                    cmbShipmentId.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private T_Sale GenerateDataAtConfirm(T_ShipmentDsp shipmentData)
        {
            try
            {
                return new T_Sale
                {
                    OrID = shipmentData.OrID,
                    SoID = shipmentData.SoID,
                    ClID = shipmentData.ClID,
                    SaDate=DateTime.Now,
                    EmID=LoginEmID
                };
            }
            catch
            {
                throw;
            }
        }
        private List<T_SaleDetail> GenerateDetailDataAtConfirm(List<T_ShipmentDetailDsp> shipmentDetailData,List<T_OrderDetailDsp_Agr_Fin> orderDetailData)
        {
            try
            {
                var saleDetailData = new List<T_SaleDetail>();
                foreach (var x in shipmentDetailData)
                {
                    saleDetailData.Add(new T_SaleDetail
                    {
                        PrID = x.PrID,
                        SaQuantity = x.ShQuantity,
                        SaTotalPrice = orderDetailData.Single(y => y.PrID == x.PrID).OrPriceSum
                    });
                }
                return saleDetailData;
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
                if (cmbShipmentId.SelectedIndex == -1)
                {
                    MessageBox.Show("出荷IDを選択してください。", "選択エラー");
                    cmbShipmentId.Focus();
                    return;
                }
                f_Detail = new F_ShipmentDetail();
                int shID = (int)cmbShipmentId.SelectedValue;
                f_Detail.shID = shID;
                f_Detail.status = int.Parse(cmbState.SelectedValue.ToString());
                f_Detail.orFlag = order.CheckOrderIsActive(int.Parse(textBoxOrderId.Text)) ? 0 : 2;
                f_Detail.orID = int.Parse(textBoxOrderId.Text);
                f_Detail.logEmID = LoginEmID;
                f_Detail.ShowDialog();
                var shipmentData = Shipment.Single(x => x.ShID == shID);
                var updatedShipmentData = shipment.GetShipmentData(new T_ShipmentRead { ShID = shID, ShFlag = -1, ShStateFlag = -1 }).Single();
                shipmentData.ShStateFlag = updatedShipmentData.ShStateFlag;
                shipmentData.ShBoolStateFlag = updatedShipmentData.ShBoolStateFlag;
                shipmentData.EmID = updatedShipmentData.EmID;
                shipmentData.EmName = updatedShipmentData.EmName;
                shipmentData.ShDate = updatedShipmentData.ShDate;
                dataGridViewDsp.Refresh();
                checkBoxFlag.Checked = updatedShipmentData.ShBoolFlag;
                if (updatedShipmentData.ShHidden == null)
                {
                    textBoxHideRea.Text = "";
                }
                cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == updatedShipmentData.EmID);
                cmbState.SelectedIndex = State.FindIndex(x => x.itemValue == shipmentData.ShStateFlag);
                if (updatedShipmentData.ShStateFlag != 0)
                {
                    dateTimePickerS.Format = DateTimePickerFormat.Long;
                    dateTimePickerS.Text = updatedShipmentData.ShDate.ToString();
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
                if (!string.IsNullOrEmpty(textBoxOrderId.Text.Trim()))
                {
                    if (int.TryParse(textBoxOrderId.Text.Trim(), out int orID) && orID > 0 && orID <= NumericRange.ID)
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
                int shID = 0;
                if (cmbShipmentId.Text != "")
                {
                    shID = int.Parse(cmbShipmentId.Text);
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
                    T_ShipmentRead selectCondition = new T_ShipmentRead()
                    {
                        ShID = shID,
                        OrID = orID,
                        SoID = soID,
                        EmID = emID,
                        ClID = clID,
                        ShStateFlag = status,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        ShFlag = hideFlg,
                        ShHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Shipment = shipment.GetShipmentData(selectCondition);
                    subShipment = new List<T_ShipmentDsp>(Shipment);
                    subShipment.Insert(0, new T_ShipmentDsp { ShID = null });
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
                    T_ShipmentRead selectCondition = new T_ShipmentRead()
                    {
                        ShID = shID,
                        OrID = orID,
                        SoID = soID,
                        EmID = emID,
                        ClID = clID,
                        ShStateFlag = status,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        ShFlag = hideFlg,
                        ShHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Shipment = shipment.GetShipmentData(selectCondition, sdate, edate);
                    subShipment = new List<T_ShipmentDsp>(Shipment);
                    subShipment.Insert(0, new T_ShipmentDsp { ShID = null });
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
                if (cmbShipmentId.SelectedIndex == -1)
                {
                    MessageBox.Show("出荷IDを選択してください。", "選択エラー");
                    cmbShipmentId.Focus();
                    return;
                }
                int shID = (int)cmbShipmentId.SelectedValue;
                int iFlg = 0;
                if (checkBoxFlag.Checked == false)
                {
                    textBoxHideRea.Text = "";
                }
                else
                {
                    iFlg = 2;
                }
                var shipmentData = Shipment.Single(x => x.ShID == shID);
                if (iFlg == 2 && shipmentData.ShFlag == 0 && order.CheckOrderIsActive(int.Parse(textBoxOrderId.Text)))
                {
                    DialogResult dialog = MessageBox.Show(string.Format("出荷ID：{0}は確定されておらず、対応する受注ID：{1}は削除されていませんがよろしいですか。", shipmentData.ShID, shipmentData.OrID), "確認", MessageBoxButtons.OKCancel);
                    if (dialog != DialogResult.OK)
                    {
                        return;
                    }
                }
                string shhidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim();
                shipment.UpdateShipmentHidden(shID, iFlg, shhidden);
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
        private void RefreshShipmentCombo()
        {
            try
            {
                int temp = 0;
                if (cmbShipmentId.SelectedValue != null)
                {
                    temp = int.Parse(cmbShipmentId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    cmbShipmentId.DataSource = subShipment;
                    cmbShipmentId.SelectedIndex = subShipment.FindIndex(x => x.ShID == temp);
                }
                else
                {
                    cmbShipmentId.DataSource = Shipment;
                    cmbShipmentId.SelectedIndex = Shipment.FindIndex(x => x.ShID == temp);
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
                cmbShipmentId.SelectedIndex = -1;
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
                dataGridViewDsp.DataSource = Shipment;

                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Shipment.Skip(pageSize * pageNo).Take(pageSize).ToList();

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Shipment.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshShipmentCombo();
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
                Shipment = shipment.GetShipmentData(new T_ShipmentRead { SoID = (int)cmbSalesOfficeId.SelectedValue, ShStateFlag = -1 });
                subShipment = new List<T_ShipmentDsp>(Shipment);
                subShipment.Insert(0, new T_ShipmentDsp { ShID = null });
                if (Shipment == null)
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
                dataGridViewDsp.DataSource = Shipment;
                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Shipment.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                ////出庫日
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
                lblPage.Text = "/" + ((int)Math.Ceiling(Shipment.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshShipmentCombo();
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
                RefreshShipmentCombo();
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
                RefreshShipmentCombo();
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
                RefreshShipmentCombo();
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
                RefreshShipmentCombo();
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
                dataGridViewDsp.DataSource = Shipment.Take(pageSize).ToList();

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
                dataGridViewDsp.DataSource = Shipment.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                int lastNo = (int)Math.Ceiling(Shipment.Count / (double)pageSize) - 1;
                //最終ページでなければ
                if (pageNo <= lastNo)
                    dataGridViewDsp.DataSource = Shipment.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                int lastPage = (int)Math.Ceiling(Shipment.Count / (double)pageSize);
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
                int pageNo = (int)Math.Ceiling(Shipment.Count / (double)pageSize) - 1;
                dataGridViewDsp.DataSource = Shipment.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                    cmbShipmentId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
                    textBoxOrderId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[1].Value.ToString();
                    cmbClientId.SelectedIndex = Client.FindIndex(x => x.ClID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[2].Value.ToString()));
                    if ((int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[13].Value != 0)
                    {
                        cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[6].Value.ToString()));

                        //日付フォーマットを元に戻す
                        dateTimePickerS.Format = DateTimePickerFormat.Long;
                        dateTimePickerS.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[8].Value.ToString();
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
                        if ((int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[13].Value == 0 && (int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[15].Value == 0)
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

        private void cmbShipmentId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (updFlg == true)
                {
                    if (cmbShipmentId.SelectedValue == null)
                    {
                        AllClear();
                    }
                    else
                    {
                        int index = Shipment.FindIndex(x => x.ShID == int.Parse(cmbShipmentId.SelectedValue.ToString()));
                        textBoxOrderId.Text = Shipment[index].OrID.ToString();
                        cmbClientId.SelectedIndex = Client.FindIndex(x => x.ClID == Shipment[index].ClID);
                        if (Shipment[index].ShStateFlag != 0)
                        {
                            cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == Shipment[index].EmID);
                            dateTimePickerS.Format = DateTimePickerFormat.Long;
                            dateTimePickerS.Text = Shipment[index].ShDate.ToString();
                            dateTimePickerS.Checked = true;
                        }
                        else
                        {
                            cmbEmployeeId.SelectedIndex = -1;
                            dateTimePickerS.Format = DateTimePickerFormat.Custom;
                            dateTimePickerS.CustomFormat = " ";
                            dateTimePickerS.Checked = false;
                        }
                        checkBoxFlag.Checked = Shipment[index].ShBoolFlag;
                        if (Shipment[index].ShHidden == null)
                        {
                            textBoxHideRea.Text = "";
                        }
                        else
                        {
                            textBoxHideRea.Text = Shipment[index].ShHidden.ToString();
                        }
                        cmbState.SelectedValue = Shipment[index].ShStateFlag;
                        if (radioBtnCon.Checked)
                        {
                            if (Shipment[index].ShStateFlag == 0 && Shipment[index].OrderFlag == 0)
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
                if (e.ColumnIndex == 1 && (int)dataGridview.Rows[e.RowIndex].Cells[15].Value != 0)
                {
                    dataGridview.Rows[e.RowIndex].Cells[1].Style.BackColor = SystemColors.AppWorkspace;
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
    }
}
