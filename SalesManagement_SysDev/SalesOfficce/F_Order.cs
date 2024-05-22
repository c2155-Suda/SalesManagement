using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    public partial class F_Order : Form
    {
        OrderDbConnection order = new OrderDbConnection();
        EmployeeDbConnection employee = new EmployeeDbConnection();
        ClientDbConnection client = new ClientDbConnection();
        ProductDbConnect product = new ProductDbConnect();
        F_OrderDetail detail;
        DataAccess access = new DataAccess();
        private List<M_EmployeeDsp> Employee;
        private List<T_OrderDsp> Order;
        private List<T_OrderDsp> subOrder;
        private List<M_ClientDsp> Client;
        private List<M_ProductDsp> Product;
        private List<M_SalesOfficeDsp> SalesOffice;
        private List<M_MajorClassificationDsp> majorClass;
        private List<M_SmallClassificationDsp> smallClass;
        private bool updFlg;
        private int pageSize = 12;
        public int LoginEmID;
        public int LoginSoID;

        public bool loginUserInfoCheck = true;
        private class StateItemSet
        {
            public string itemDisp { get; set; }
            public int itemValue { get; set; }
        }
        private List<StateItemSet> State=new List<StateItemSet>();

        public F_Order()
        {
            InitializeComponent();
        }

        private void F_Order_Load(object sender, EventArgs e)
        {
            Employee = employee.GetEmployeeData();
            SalesOffice = access.GetSalesOfficeDspData(); 
            
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
                    MessageBox.Show("ログイン中の社員IDは削除済みです","エラー");
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

        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            {
                if (!InputCheckAtConfirm())
                    return;
                int OrID = int.Parse(cmbOrderId.SelectedValue.ToString());
                var orderData = Order.Single(x => x.OrID == OrID);
                T_OrderDsp updatedOrderData;
                if (!GetValidDataAtConfirm(orderData))
                {
                    updatedOrderData = order.GetOrderData(new T_OrderRead { OrID = OrID, OrFlag = -1, ClCharge = "", OrStateFlag = -1 }).Single();
                    orderData.OrStateFlag = updatedOrderData.OrStateFlag;
                    orderData.OrStateString = updatedOrderData.OrStateString;
                    orderData.KindOfProducts = updatedOrderData.KindOfProducts;
                    orderData.OrFlag = updatedOrderData.OrFlag;
                    orderData.OrFlagBool = updatedOrderData.OrFlagBool;
                    orderData.OrHidden = updatedOrderData.OrHidden;
                    checkBoxFlag.Checked = updatedOrderData.OrFlagBool;
                    if (updatedOrderData.OrHidden == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    dataGridViewDsp.Refresh();
                    cmbState.SelectedIndex = State.FindIndex(x => x.itemValue == orderData.OrStateFlag);
                    return;
                }
                var orderDetailData_Agr = order.GetOrderDetailData_Agr_NonFin(new T_OrderDetailDsp { OrID = OrID });
                if (!GetValidDetailDataAtConfirm(orderDetailData_Agr))
                {
                    return;
                }
                //確定情報作成
                var chumonData = GenerateDataAtConfirm(orderData);
                //確定詳細情報作成
                var chumonDetailData = GenerateDetailDataAtConfirm(orderDetailData_Agr);
                DialogResult dialog = MessageBox.Show(string.Format("受注ID：{0}を確定しますか。", chumonData.OrID), "確認", MessageBoxButtons.OKCancel);
                if (dialog != DialogResult.OK)
                {
                    return;
                }
                //確定処理
                order.FinalizeOrderData(chumonData, chumonDetailData,orderDetailData_Agr);
                updatedOrderData = order.GetOrderData(new T_OrderRead { OrID = OrID, OrFlag = -1, ClCharge = "", OrStateFlag = -1 }).Single();
                orderData.OrStateFlag = updatedOrderData.OrStateFlag;
                orderData.OrStateString = updatedOrderData.OrStateString;
                orderData.KindOfProducts = updatedOrderData.KindOfProducts;
                orderData.OrFlag = updatedOrderData.OrFlag;
                orderData.OrFlagBool = updatedOrderData.OrFlagBool;
                orderData.OrHidden = updatedOrderData.OrHidden;
                dataGridViewDsp.Refresh();
                AllClear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private bool InputCheckAtConfirm()
        {
            try
            {
                if (cmbOrderId.SelectedIndex == -1)
                {
                    MessageBox.Show("受注IDを選択してください。", "選択エラー");
                    cmbOrderId.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private bool GetValidDataAtConfirm(T_OrderDsp orderData)
        {
            try
            {
                if (!order.CheckOrderIsInsertable((int)orderData.OrID))
                {
                    MessageBox.Show("選択した受注IDは確定済です。", "選択エラー");
                    cmbOrderId.Focus();
                    return false;
                }
                if (orderData.KindOfProducts <= 0)
                {
                    MessageBox.Show("選択した受注に商品が含まれていません。", "選択エラー");
                    cmbOrderId.Focus();
                    return false;
                }
                if (!client.CheckClientIsActive(orderData.ClID))
                {
                    MessageBox.Show("選択した受注に対応する顧客が削除済です。", "選択エラー");
                    cmbOrderId.Focus();
                    return false;
                }
                if (!employee.CheckEmployeeIsActive(orderData.EmID))
                {
                    MessageBox.Show("選択した受注を登録した社員が削除済です。", "選択エラー");
                    cmbOrderId.Focus();
                    return false;
                }
                if (!order.CheckOrderIsActive((int)orderData.OrID))
                {
                    DialogResult dialog = MessageBox.Show("受注確定時に対象データの削除は取り消されます。", "確認", MessageBoxButtons.OKCancel);
                    if (dialog != DialogResult.OK)
                    {
                        cmbOrderId.Focus();
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
        private bool GetValidDetailDataAtConfirm(List<T_OrderDetailDsp_Agr_NonFin> orderDetailData)
        {
            try
            {
                foreach (var x in orderDetailData)
                {
                    if (!product.CheckProductIsActive(x.PrID))
                    {
                        MessageBox.Show(string.Format("商品ID：{0}は削除済です。",x.PrID), "選択エラー");
                        cmbOrderId.Focus();
                        return false;
                    }
                    if (x.OrPriceSum > NumericRange.TotalPrice)
                    {
                        MessageBox.Show(string.Format("商品ID：{0}の金額が上限を超えます。",x.PrID), "選択エラー");
                        cmbOrderId.Focus();
                        return false;
                    }
                    var temp = order.GetOrderDetailData(new T_OrderDetailDsp { OrID = x.OrID, PrID = x.PrID,OrDetailID=0 }).ToList();
                    foreach (var row2 in temp)
                    {
                        if (x.OrPriceUnit * row2.OrQuantity > NumericRange.TotalPrice || x.OrPriceUnit * row2.OrQuantity < NumericRange.TotalPrice_minus)
                        {
                            MessageBox.Show(string.Format("受注詳細ID：{0}の金額が適正ではありません。商品の価格を変更するか新規受注として登録してください。", row2.OrDetailID));
                        }
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private T_Chumon GenerateDataAtConfirm(T_OrderDsp orderData)
        {
            try
            {
                return new T_Chumon
                {
                    OrID = (int)orderData.OrID,
                    SoID = orderData.SoID,
                    ClID = orderData.ClID,
                    ChDate = DateTime.Now,
                };
            }
            catch
            {
                throw;
            }
        }

        private List<T_ChumonDetail> GenerateDetailDataAtConfirm(List<T_OrderDetailDsp_Agr_NonFin> orderDetailData)
        {
            try
            {
                var chumonDetailData = new List<T_ChumonDetail>();
                foreach (var x in orderDetailData)
                {
                    chumonDetailData.Add(new T_ChumonDetail
                    {
                        PrID = x.PrID,
                        ChQuantity = x.OrQuantitySum
                    });
                }
                return chumonDetailData;
            }
            catch
            {
                throw;
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            try
            {
                //入力チェック
                if (!GetValidDataAtRegistration())
                    return;
                //登録情報作成
                var regOrder = GenerateDataAtRegistration();
                //登録処理
                order.RegistOrderData(regOrder);
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
                if (cmbClientId.SelectedIndex == -1)
                {
                    MessageBox.Show("顧客名が未選択です", "選択エラー");
                    cmbClientId.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(textBoxCharge.Text.Trim()))
                {
                    MessageBox.Show("顧客担当者名が未入力です", "入力エラー");
                    textBoxCharge.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private T_Order GenerateDataAtRegistration()
        {
            try
            {
                DateTime releaseDate = dateTimePickerS.Value;
                int hideFlg;
                if (checkBoxFlag.Checked == false)
                {
                    hideFlg = 0;
                }
                else
                    hideFlg = 2;

                //登録データセット
                return new T_Order
                {
                    ClID = (int)cmbClientId.SelectedValue,
                    ClCharge = textBoxCharge.Text.Trim(),
                    SoID = (int)cmbSalesOfficeId.SelectedValue,
                    EmID = (int)cmbEmployeeId.SelectedValue,
                    OrDate = releaseDate,
                    OrFlag = hideFlg,
                    OrHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim()
                };
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
                if (cmbOrderId.SelectedIndex == -1)
                {
                    MessageBox.Show("受注IDを選択してください。", "選択エラー");
                    cmbOrderId.Focus();
                    return;
                }
                detail = new F_OrderDetail();
                int orID = (int)cmbOrderId.SelectedValue;
                detail.orID = orID;
                detail.status = int.Parse(cmbState.SelectedValue.ToString());
                detail.ShowDialog();
                var orderData = Order.Single(x => x.OrID == orID);
                var updatedOrderData = order.GetOrderData(new T_OrderRead { OrID = orID, OrFlag = -1, ClCharge = "", OrStateFlag = -1 }).Single();
                orderData.OrStateFlag = updatedOrderData.OrStateFlag;
                orderData.OrStateString = updatedOrderData.OrStateString;
                orderData.KindOfProducts = updatedOrderData.KindOfProducts;
                orderData.OrFlag = updatedOrderData.OrFlag;
                orderData.OrFlagBool = updatedOrderData.OrFlagBool;
                orderData.OrHidden = updatedOrderData.OrHidden;
                checkBoxFlag.Checked = updatedOrderData.OrFlagBool;
                if (updatedOrderData.OrHidden == null)
                {
                    textBoxHideRea.Text = "";
                }

                dataGridViewDsp.Refresh();
                cmbState.SelectedIndex = State.FindIndex(x => x.itemValue == orderData.OrStateFlag);
                detail.Dispose();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
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
            catch (Exception ex)
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
                int orID = 0;
                if (cmbOrderId.Text != "")
                {
                    orID = int.Parse(cmbOrderId.Text);
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
                    T_OrderRead selectCondition = new T_OrderRead()
                    {
                        OrID = orID,
                        SoID = soID,
                        EmID = emID,
                        ClID = clID,
                        ClCharge = textBoxCharge.Text.Trim(),
                        OrStateFlag = status,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        OrFlag = hideFlg,
                        OrHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Order = order.GetOrderData(selectCondition);
                    subOrder = new List<T_OrderDsp>(Order);
                    subOrder.Insert(0, new T_OrderDsp { OrID = null });
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
                    T_OrderRead selectCondition = new T_OrderRead()
                    {
                        OrID = orID,
                        SoID = soID,
                        EmID = emID,
                        ClID = clID,
                        ClCharge = textBoxCharge.Text.Trim(),
                        OrStateFlag = status,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        OrFlag = hideFlg,
                        OrHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Order = order.GetOrderData(selectCondition, sdate, edate);
                    subOrder = new List<T_OrderDsp>(Order);
                    subOrder.Insert(0, new T_OrderDsp { OrID = null });
                }
            }
            catch
            {
                throw;
            }
        }

        private void btnHidden_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbOrderId.SelectedIndex == -1)
                {
                    MessageBox.Show("受注IDを選択してください。", "選択エラー");
                    cmbOrderId.Focus();
                    return;
                }
                int orID = (int)cmbOrderId.SelectedValue;
                int iFlg = 0;
                if (checkBoxFlag.Checked == false)
                {
                    textBoxHideRea.Text = "";
                }
                else
                {
                    iFlg = 2;
                }
                var orderData = Order.Single(x => x.OrID == orID);
                if (iFlg == 2 && orderData.OrFlag == 0 && orderData.OrStateFlag > 0 && orderData.OrStateFlag < 5)
                {
                    DialogResult dialog = MessageBox.Show(string.Format("受注ID：{0}は確定済ですが非表示化すると以降の処理は中断されます。", orID), "確認", MessageBoxButtons.OKCancel);
                    if (dialog != DialogResult.OK)
                    {
                        return;
                    }
                }
                string orhidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim();
                order.UpdateOrderHidden(orID, iFlg, orhidden);
                GetAllData();
                AllClear();
            }
            catch(Exception ex)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private void GetComboBoxData()
        {
            try
            {
                //受注IDのコンボボックスData読込
                cmbOrderId.DisplayMember = "OrID";
                cmbOrderId.ValueMember = "OrID";
                cmbOrderId.DataSource = Order;
                cmbOrderId.SelectedIndex = -1;

                //営業所
                cmbSalesOfficeId.DisplayMember = "SoName";
                cmbSalesOfficeId.ValueMember = "SoID";
                cmbSalesOfficeId.DataSource = SalesOffice;
                cmbSalesOfficeId.SelectedIndex = -1;

                //大分類
                majorClass = access.GetMajorClassificationDspData();
                majorClass.Insert(0, new M_MajorClassificationDsp { McID = 0, McName = "" });
                majorClass.Add(new M_MajorClassificationDsp { McID = -1, McName = "削除済み" });
                cmbMajor.DisplayMember = "McName";
                cmbMajor.ValueMember = "McID";
                cmbMajor.DataSource = majorClass;
                cmbMajor.SelectedIndex = -1;

                //ステータス
                State.Add(new StateItemSet { itemValue = -1, itemDisp = "" });
                State.Add(new StateItemSet { itemValue = 0, itemDisp = "(未確定)" });
                State.Add(new StateItemSet { itemValue = 1, itemDisp = "受注済" });
                State.Add(new StateItemSet { itemValue = 2, itemDisp = "注文済" });
                State.Add(new StateItemSet { itemValue = 3, itemDisp = "出庫済" });
                State.Add(new StateItemSet { itemValue = 4, itemDisp = "入荷済" });
                State.Add(new StateItemSet { itemValue = 5, itemDisp = "出荷済" });

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
        private void RefreshOrderCombo()
        {
            try
            {
                int temp = 0;
                if (cmbOrderId.SelectedValue != null)
                {
                    temp = int.Parse(cmbOrderId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    cmbOrderId.DataSource = subOrder;
                    cmbOrderId.SelectedIndex = subOrder.FindIndex(x => x.OrID == temp);
                }
                else
                {
                    cmbOrderId.DataSource = Order;
                    cmbOrderId.SelectedIndex = Order.FindIndex(x => x.OrID == temp);
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
                textBoxCharge.Text = "";
                cmbOrderId.SelectedIndex = -1;
                cmbClientId.SelectedIndex = -1;
                if (!radioBtnReg.Checked)
                {
                    cmbEmployeeId.SelectedIndex = -1;
                }
                cmbProductId.SelectedIndex = -1;
                cmbMajor.SelectedIndex = -1;
                cmbSmall.SelectedIndex = -1;
                cmbState.SelectedIndex = -1;
                dateTimePickerS.Value = DateTime.Now;
                dateTimePickerE.Value = DateTime.Now;
                if (!radioBtnReg.Checked)
                {
                    dateTimePickerS.Checked = false;
                }
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
                dataGridViewDsp.DataSource = Order;

                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Order.Skip(pageSize * pageNo).Take(pageSize).ToList();

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Order.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshOrderCombo();
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
                Order = order.GetOrderData(new T_OrderRead { SoID = (int)cmbSalesOfficeId.SelectedValue, ClCharge = "", OrStateFlag = -1 });
                subOrder = new List<T_OrderDsp>(Order);
                subOrder.Insert(0, new T_OrderDsp { OrID = null });
                if (Order == null)
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
                dataGridViewDsp.DataSource = Order;
                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Order.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                ////受注ID
                dataGridViewDsp.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[0].Width = 95;
                ////顧客ID
                dataGridViewDsp.Columns[1].Visible = false;
                ////顧客名
                dataGridViewDsp.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[2].Width = 170;
                ////顧客担当者名
                dataGridViewDsp.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[3].Width = 200;
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
                dataGridViewDsp.Columns[14].Width = 150;

                dataGridViewDsp.Columns[15].Visible = false;
                dataGridViewDsp.Columns[16].Visible = false;

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Order.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshOrderCombo();
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
                RefreshOrderCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = true;
                label13.Visible = false;
                cmbOrderId.Enabled = true; //受注ID

                label2.Enabled = false;
                cmbClientId.Enabled = false; //顧客ID

                label1.Enabled = false;
                textBoxCharge.Enabled = false; //顧客担当者名

                label4.Enabled = false;
                cmbEmployeeId.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbEmployeeId.Enabled = false; //社員ID

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                dateTimePickerS.Checked = true;
                label10.Visible = false;
                dateTimePickerE.Visible = false; //受注日

                label12.Visible = true;
                label12.Enabled = false;
                cmbState.Enabled = false;
                cmbState.Visible = true; //ステータス

                label11.Visible = false;
                cmbProductId.Visible = false; //商品ID
                label7.Visible = false;
                cmbMajor.Visible = false; //大分類ID
                label9.Visible = false;
                cmbSmall.Visible = false; //小分類ID

                checkBoxFlag.Enabled = false; //非表示フラグ

                btnCon.Enabled = true;
                btnReg.Enabled = false;
                btnDetail.Enabled = false;
                btnSea.Enabled = false;
                btnHidden.Enabled = false; //ボタン有効化

                updFlg = true; //自動入力フラグ
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
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
                RefreshOrderCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = false;
                label13.Visible = true;
                cmbOrderId.Enabled = false; //受注ID

                label2.Enabled = true;
                cmbClientId.Enabled = true; //顧客ID

                label1.Enabled = true;
                textBoxCharge.Enabled = true; //顧客担当者名

                label4.Enabled = false;
                cmbEmployeeId.DropDownStyle = ComboBoxStyle.Simple;
                cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == LoginEmID);
                cmbEmployeeId.Enabled = false; //社員ID

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                dateTimePickerS.Checked = true;
                label10.Visible = false;
                dateTimePickerE.Visible = false; //受注日

                label12.Visible = false;
                cmbState.Visible = false; //ステータス

                label11.Visible = false;
                cmbProductId.Visible = false; //商品ID
                label7.Visible = false;
                cmbMajor.Visible = false; //大分類ID
                label9.Visible = false;
                cmbSmall.Visible = false; //小分類ID

                checkBoxFlag.Enabled = true; //非表示フラグ

                btnCon.Enabled = false;
                btnReg.Enabled = true;
                btnDetail.Enabled = false;
                btnSea.Enabled = false;
                btnHidden.Enabled = false; //ボタン有効化

                updFlg = false; //自動入力フラグ
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
                RefreshOrderCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = true;
                label13.Visible = false;
                cmbOrderId.Enabled = true; //受注ID

                label2.Enabled = false;
                cmbClientId.Enabled = false; //顧客ID

                label1.Enabled = false;
                textBoxCharge.Enabled = false; //顧客担当者名

                label4.Enabled = false;
                cmbEmployeeId.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbEmployeeId.Enabled = false; //社員ID

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                dateTimePickerS.Checked = true;
                label10.Visible = false;
                dateTimePickerE.Visible = false; //受注日

                label12.Visible = true;
                label12.Enabled = false;
                cmbState.Enabled = false;
                cmbState.Visible = true; //ステータス

                label11.Visible = false;
                cmbProductId.Visible = false; //商品ID
                label7.Visible = false;
                cmbMajor.Visible = false; //大分類ID
                label9.Visible = false;
                cmbSmall.Visible = false; //小分類ID

                checkBoxFlag.Enabled = false; //非表示フラグ

                btnCon.Enabled = false;
                btnReg.Enabled = false;
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

        private void radioBtnSea_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnSea.Checked == false)
                {
                    return;
                }
                RefreshOrderCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = true;
                label13.Visible = false;
                cmbOrderId.Enabled = true; //受注ID

                label2.Enabled = true;
                cmbClientId.Enabled = true; //顧客ID

                label1.Enabled = true;
                textBoxCharge.Enabled = true; //顧客担当者名

                label4.Enabled = true;
                cmbEmployeeId.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbEmployeeId.Enabled = true; //社員ID

                label5.Enabled = true;
                dateTimePickerS.Enabled = true;
                label10.Visible = true;
                label10.Enabled = false;
                dateTimePickerE.Visible = true; //受注日

                label12.Visible = true;
                label12.Enabled = true;
                cmbState.Enabled = true; ;
                cmbState.Visible = true; //ステータス

                label11.Visible = true;
                cmbProductId.Visible = true; //商品ID
                label7.Visible = true;
                cmbMajor.Visible = true; //大分類ID
                label9.Visible = true;
                cmbSmall.Visible = true; //小分類ID

                checkBoxFlag.Enabled = true; //非表示フラグ

                btnCon.Enabled = false;
                btnReg.Enabled = false;
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

        private void radioBtnHidden_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnHidden.Checked == false)
                {
                    return;
                }
                RefreshOrderCombo();
                RefreshClientCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = true;
                label13.Visible = false;
                cmbOrderId.Enabled = true; //受注ID

                label2.Enabled = false;
                cmbClientId.Enabled = false; //顧客ID

                label1.Enabled = false;
                textBoxCharge.Enabled = false; //顧客担当者名

                label4.Enabled = false;
                cmbEmployeeId.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbEmployeeId.Enabled = false; //社員ID

                label5.Enabled = false;
                dateTimePickerS.Enabled = false;
                dateTimePickerS.Checked = true;
                label10.Visible = false;
                dateTimePickerE.Visible = false; //受注日

                label12.Visible = true;
                label12.Enabled = false;
                cmbState.Enabled = false;
                cmbState.Visible = true; //ステータス

                label11.Visible = false;
                cmbProductId.Visible = false; //商品ID
                label7.Visible = false;
                cmbMajor.Visible = false; //大分類ID
                label9.Visible = false;
                cmbSmall.Visible = false; //小分類ID

                checkBoxFlag.Enabled = true; //非表示フラグ

                btnCon.Enabled = false;
                btnReg.Enabled = false;
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
        private void checkBoxFlag_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxFlag.Checked && (radioBtnReg.Checked || radioBtnSea.Checked || radioBtnHidden.Checked))
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
                dataGridViewDsp.DataSource = Order.Take(pageSize).ToList();

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
                dataGridViewDsp.DataSource = Order.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                int lastNo = (int)Math.Ceiling(Order.Count / (double)pageSize) - 1;
                //最終ページでなければ
                if (pageNo <= lastNo)
                    dataGridViewDsp.DataSource = Order.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                int lastPage = (int)Math.Ceiling(Order.Count / (double)pageSize);
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
                int pageNo = (int)Math.Ceiling(Order.Count / (double)pageSize) - 1;
                dataGridViewDsp.DataSource = Order.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                    cmbOrderId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
                    cmbClientId.SelectedIndex = Client.FindIndex(x => x.ClID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[1].Value.ToString()));
                    textBoxCharge.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[3].Value.ToString();
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
                    cmbState.SelectedValue = int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[13].Value.ToString());
                    if (radioBtnCon.Checked)
                    {
                        if((int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[13].Value != 0)
                        {
                            btnCon.Enabled = false;
                        }
                        else
                        {
                            btnCon.Enabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
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
            catch(Exception ex)
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
            catch(Exception ex)
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
                cmbSmall.SelectedIndexChanged -=  cmbSmall_SelectedIndexChanged;
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbOrderId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (updFlg == true)
                {
                    if (cmbOrderId.SelectedValue == null)
                    {
                        AllClear();
                    }
                    else
                    {
                        int index = Order.FindIndex(x => x.OrID == int.Parse(cmbOrderId.SelectedValue.ToString()));
                        cmbClientId.SelectedIndex = Client.FindIndex(x => x.ClID == Order[index].ClID);
                        textBoxCharge.Text = Order[index].ClCharge.ToString();
                        cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == Order[index].EmID);
                        dateTimePickerS.Text = Order[index].OrDate.ToString();
                        checkBoxFlag.Checked = Order[index].OrFlagBool;
                        if (Order[index].OrHidden == null)
                        {
                            textBoxHideRea.Text = "";
                        }
                        else
                        {
                            textBoxHideRea.Text = Order[index].OrHidden.ToString();
                        }
                        cmbState.SelectedValue = Order[index].OrStateFlag;
                        if (radioBtnCon.Checked)
                        {
                            if (Order[index].OrStateFlag != 0)
                            {
                                btnCon.Enabled = false;
                            }
                            else
                            {
                                btnCon.Enabled = true;
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
                    label10.Enabled = false;
                }
                else
                {
                    label10.Enabled = true;
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
                    label10.Enabled = false;
                }
                else
                {
                    label10.Enabled = true;
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
                if (e.ColumnIndex == 2&& (int)dataGridview.Rows[e.RowIndex].Cells[15].Value != 0)
                {
                    dataGridViewDsp.Rows[e.RowIndex].Cells[2].Style.BackColor = SystemColors.AppWorkspace;
                }
                if (e.ColumnIndex == 7 && (int)dataGridview.Rows[e.RowIndex].Cells[16].Value != 0)
                {
                    dataGridViewDsp.Rows[e.RowIndex].Cells[7].Style.BackColor = SystemColors.AppWorkspace;
                }
                if (e.ColumnIndex == 12 && (int)dataGridview.Rows[e.RowIndex].Cells[12].Value == 0)
                {
                    dataGridViewDsp.Rows[e.RowIndex].Cells[12].Style.BackColor = SystemColors.AppWorkspace;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
    }    
}
