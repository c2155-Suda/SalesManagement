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
    public partial class F_Hattyu : Form
    {
        HattyuDbConnection hattyu = new HattyuDbConnection();
        EmployeeDbConnection employee = new EmployeeDbConnection();
        ProductDbConnect product = new ProductDbConnect();
        F_HattyuDetail detail;
        DataAccess access = new DataAccess();
        private List<M_EmployeeDsp> Employee;
        private List<M_MakerDsp> Maker;
        private List<T_HattyuDsp> Hattyu;
        private List<T_HattyuDsp> subHattyu;
        private List<M_ProductDsp> Product;
        private List<M_MajorClassificationDsp> majorClass;
        private List<M_SmallClassificationDsp> smallClass;
        private bool updFlg;
        private int pageSize = 12;
        public int LoginEmID;

        public bool loginUserInfoCheck = true;
        private class StateItemSet
        {
            public string itemDisp { get; set; }
            public int itemValue { get; set; }
        }
        private List<StateItemSet> State=new List<StateItemSet>();

        public F_Hattyu()
        {
            InitializeComponent();
        }

        private void F_Hattyu_Load(object sender, EventArgs e)
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
                    MessageBox.Show("ログイン中の社員IDは削除済みです","エラー");
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

        private void btnCon_Click(object sender, EventArgs e)
        {
            try
            {
                if (!InputCheckAtConfirm())
                    return;
                int HaID = int.Parse(cmbHattyuId.SelectedValue.ToString());
                var HattyuData = Hattyu.Single(x => x.HaID == HaID);
                T_HattyuDsp updatedHattyuData;
                if (!GetValidDataAtConfirm(HattyuData))
                {
                    updatedHattyuData = hattyu.GetHattyuData(new T_HattyuRead { HaID = HaID, HaFlag = -1, WaWarehouseFlag = -1 }).Single();
                    HattyuData.WaWarehousingFlag = updatedHattyuData.WaWarehousingFlag;
                    HattyuData.WaWarehousingFlagString = updatedHattyuData.WaWarehousingFlagString;
                    HattyuData.KindOfProducts = updatedHattyuData.KindOfProducts;
                    HattyuData.HaFlag = updatedHattyuData.HaFlag;
                    HattyuData.HaFlagBool = updatedHattyuData.HaFlagBool;
                    HattyuData.HaHidden = updatedHattyuData.HaHidden;
                    checkBoxFlag.Checked = updatedHattyuData.HaFlagBool;
                    if (updatedHattyuData.HaHidden == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    dataGridViewDsp.Refresh();
                    cmbState.SelectedIndex = State.FindIndex(x => x.itemValue == HattyuData.WaWarehousingFlag);
                    return;
                }
                var hattyuDetailData_Agr = hattyu.GetHattyuDetailData_Agr(new T_HattyuDetailDsp { HaID = HaID });
                if (!GetValidDetailDataAtConfirm(hattyuDetailData_Agr,HattyuData.MaID))
                {
                    return;
                }
                //確定情報作成
                var warehousingData = GenerateDataAtConfirm(HattyuData);
                //確定詳細情報作成
                var warehousingDetailData = GenerateDetailDataAtConfirm(hattyuDetailData_Agr);
                DialogResult dialog = MessageBox.Show(string.Format("発注ID：{0}を確定しますか。", warehousingData.HaID), "確認", MessageBoxButtons.OKCancel);
                if (dialog != DialogResult.OK)
                {
                    return;
                }
                //確定処理
                hattyu.FinalizeHattyuData(warehousingData, warehousingDetailData);
                updatedHattyuData = hattyu.GetHattyuData(new T_HattyuRead { HaID = HaID, HaFlag = -1,WaWarehouseFlag = -1 }).Single();
                HattyuData.WaWarehousingFlag = updatedHattyuData.WaWarehousingFlag;
                HattyuData.WaWarehousingFlagString = updatedHattyuData.WaWarehousingFlagString;
                HattyuData.KindOfProducts = updatedHattyuData.KindOfProducts;
                HattyuData.HaFlag = updatedHattyuData.HaFlag;
                HattyuData.HaFlagBool = updatedHattyuData.HaFlagBool;
                HattyuData.HaHidden = updatedHattyuData.HaHidden;
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
                if (cmbHattyuId.SelectedIndex == -1)
                {
                    MessageBox.Show("発注IDを選択してください。", "選択エラー");
                    cmbHattyuId.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private bool GetValidDataAtConfirm(T_HattyuDsp hattyuData)
        {
            try
            {
                if (!hattyu.CheckHattyuIsInsertable((int)hattyuData.HaID))
                {
                    MessageBox.Show("選択した発注IDは確定済です。", "選択エラー");
                    cmbHattyuId.Focus();
                    return false;
                }
                if (hattyuData.KindOfProducts <= 0)
                {
                    MessageBox.Show("選択した発注に商品が含まれていません。", "選択エラー");
                    cmbHattyuId.Focus();
                    return false;
                }
                if (!access.CheckMakerIsActive(hattyuData.MaID))
                {
                    MessageBox.Show("選択した発注に対応するメーカーが削除済です。", "選択エラー");
                    cmbHattyuId.Focus();
                    return false;
                }
                if (!employee.CheckEmployeeIsActive(hattyuData.EmID))
                {
                    MessageBox.Show("選択した発注を登録した社員が削除済です。", "選択エラー");
                    cmbHattyuId.Focus();
                    return false;
                }
                if (!hattyu.CheckHattyuIsActive((int)hattyuData.HaID))
                {
                    DialogResult dialog = MessageBox.Show("発注確定時に対象データの削除は取り消されます。", "確認", MessageBoxButtons.OKCancel);
                    if (dialog != DialogResult.OK)
                    {
                        cmbHattyuId.Focus();
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
        private bool GetValidDetailDataAtConfirm(List<T_HattyuDetailDsp_Agr> hattyuDetailData,int Maid)
        {
            try
            {
                foreach (var x in hattyuDetailData)
                {
                    if (!product.CheckProductIsActive(x.PrID))
                    {
                        MessageBox.Show(string.Format("商品ID：{0}は削除済です。",x.PrID), "選択エラー");
                        cmbHattyuId.Focus();
                        return false;
                    }
                    if (x.MaID != Maid)
                    {
                        MessageBox.Show(string.Format("商品ID：{0}のメーカーが更新されました。\n新規の発注を作成してください。", x.PrID), "選択エラー");
                        cmbHattyuId.Focus();
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
        private T_Warehousing GenerateDataAtConfirm(T_HattyuDsp hattyuData)
        {
            try
            {
                return new T_Warehousing
                {
                    HaID = (int)hattyuData.HaID,
                };
            }
            catch
            {
                throw;
            }
        }

        private List<T_WarehousingDetail> GenerateDetailDataAtConfirm(List<T_HattyuDetailDsp_Agr> hattyuDetailData)
        {
            try
            {
                var warehousingDetailData = new List<T_WarehousingDetail>();
                foreach (var x in hattyuDetailData)
                {
                    warehousingDetailData.Add(new T_WarehousingDetail
                    {
                        PrID = x.PrID,
                        WaQuantity = x.HaQuantitySum
                    });
                }
                return warehousingDetailData;
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
                var regHattyu = GenerateDataAtRegistration();
                //登録処理
                hattyu.RegistHattyuData(regHattyu);
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
                if (cmbMakerId.SelectedIndex == -1)
                {
                    MessageBox.Show("メーカーが未選択です", "選択エラー");
                    cmbMakerId.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private T_Hattyu GenerateDataAtRegistration()
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
                return new T_Hattyu
                {
                    MaID = (int)cmbMakerId.SelectedValue,
                    EmID = (int)cmbEmployeeId.SelectedValue,
                    HaDate = releaseDate,
                    HaFlag = hideFlg,
                    HaHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim()
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
                if (cmbHattyuId.SelectedIndex == -1)
                {
                    MessageBox.Show("発注IDを選択してください。", "選択エラー");
                    cmbHattyuId.Focus();
                    return;
                }
                detail = new F_HattyuDetail();
                int haID = (int)cmbHattyuId.SelectedValue;
                detail.haID = haID;
                var hattyuData = Hattyu.Single(x => x.HaID == haID);
                detail.maID = hattyuData.MaID;
                detail.maName = hattyuData.MaName;
                detail.status = int.Parse(cmbState.SelectedValue.ToString());
                detail.ShowDialog();
                var updatedHattyuData = hattyu.GetHattyuData(new T_HattyuRead { HaID = haID, HaFlag = -1, WaWarehouseFlag = -1 }).Single();
                hattyuData.WaWarehousingFlag = updatedHattyuData.WaWarehousingFlag;
                hattyuData.WaWarehousingFlagString = updatedHattyuData.WaWarehousingFlagString;
                hattyuData.KindOfProducts = updatedHattyuData.KindOfProducts;
                hattyuData.HaFlag = updatedHattyuData.HaFlag;
                hattyuData.HaFlagBool = updatedHattyuData.HaFlagBool;
                hattyuData.HaHidden = updatedHattyuData.HaHidden;
                checkBoxFlag.Checked = updatedHattyuData.HaFlagBool;
                if (updatedHattyuData.HaHidden == null)
                {
                    textBoxHideRea.Text = "";
                }

                dataGridViewDsp.Refresh();
                cmbState.SelectedIndex = State.FindIndex(x => x.itemValue == hattyuData.WaWarehousingFlag);
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
                int haID = 0;
                if (cmbHattyuId.Text != "")
                {
                    haID = int.Parse(cmbHattyuId.Text);
                }
                int emID = 0;
                if (cmbEmployeeId.Text != "")
                {
                    emID = (int)cmbEmployeeId.SelectedValue;
                }
                int maID = 0;
                if (cmbMakerId.Text != "")
                {
                    maID = (int)cmbMakerId.SelectedValue;
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
                    T_HattyuRead selectCondition = new T_HattyuRead()
                    {
                        HaID = haID,
                        EmID = emID,
                        MaID = maID,
                        WaWarehouseFlag = status,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        HaFlag = hideFlg,
                        HaHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Hattyu = hattyu.GetHattyuData(selectCondition);
                    subHattyu = new List<T_HattyuDsp>(Hattyu);
                    subHattyu.Insert(0, new T_HattyuDsp { HaID = null });
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
                    T_HattyuRead selectCondition = new T_HattyuRead()
                    {
                        HaID = haID,
                        EmID = emID,
                        MaID = maID,
                        WaWarehouseFlag = status,
                        PrID = prID,
                        McID = mcID,
                        ScID = scID,
                        HaFlag = hideFlg,
                        HaHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Hattyu = hattyu.GetHattyuData(selectCondition, sdate, edate);
                    subHattyu = new List<T_HattyuDsp>(Hattyu);
                    subHattyu.Insert(0, new T_HattyuDsp { HaID = null });
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
                if (cmbHattyuId.SelectedIndex == -1)
                {
                    MessageBox.Show("発注IDを選択してください。", "選択エラー");
                    cmbHattyuId.Focus();
                    return;
                }
                int haID = (int)cmbHattyuId.SelectedValue;
                int iFlg = 0;
                if (checkBoxFlag.Checked == false)
                {
                    textBoxHideRea.Text = "";
                }
                else
                {
                    iFlg = 2;
                }
                var hattyuData = Hattyu.Single(x => x.HaID == haID);
                if (iFlg == 2 && hattyuData.HaFlag == 0 && hattyuData.WaWarehousingFlag==1 )
                {
                    DialogResult dialog = MessageBox.Show(string.Format("発注ID：{0}は確定済ですが非表示化すると以降の処理は中断されます。", haID), "確認", MessageBoxButtons.OKCancel);
                    if (dialog != DialogResult.OK)
                    {
                        return;
                    }
                }
                string hahidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim();
                hattyu.UpdateHattyuHidden(haID, iFlg, hahidden);
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
                //発注IDのコンボボックスData読込
                cmbHattyuId.DisplayMember = "HaID";
                cmbHattyuId.ValueMember = "HaID";
                cmbHattyuId.DataSource = Hattyu;
                cmbHattyuId.SelectedIndex = -1;



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
                State.Add(new StateItemSet { itemValue = 1, itemDisp = "発注済" });
                State.Add(new StateItemSet { itemValue = 2, itemDisp = "入庫済" });

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
        private void RefreshHattyuCombo()
        {
            try
            {
                int temp = 0;
                if (cmbHattyuId.SelectedValue != null)
                {
                    temp = int.Parse(cmbHattyuId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    cmbHattyuId.DataSource = subHattyu;
                    cmbHattyuId.SelectedIndex = subHattyu.FindIndex(x => x.HaID == temp);
                }
                else
                {
                    cmbHattyuId.DataSource = Hattyu;
                    cmbHattyuId.SelectedIndex = Hattyu.FindIndex(x => x.HaID == temp);
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
                cmbHattyuId.SelectedIndex = -1;
                cmbMakerId.SelectedIndex = -1;
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
                dataGridViewDsp.DataSource = Hattyu;

                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Hattyu.Skip(pageSize * pageNo).Take(pageSize).ToList();

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Hattyu.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshHattyuCombo();
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
                Hattyu = hattyu.GetHattyuData();
                subHattyu = new List<T_HattyuDsp>(Hattyu);
                subHattyu.Insert(0, new T_HattyuDsp { HaID = null });
                if (Hattyu == null)
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
                dataGridViewDsp.DataSource = Hattyu;
                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Hattyu.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                ////発注ID
                dataGridViewDsp.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[0].Width = 95;
                ////メーカーID
                dataGridViewDsp.Columns[1].Visible = false;
                ////メーカー
                dataGridViewDsp.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[2].Width = 170;
                ////社員ID
                dataGridViewDsp.Columns[3].Visible = false;
                ////社員名
                dataGridViewDsp.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[4].Width = 160;
                ////受注日
                dataGridViewDsp.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[5].Width = 180;
                ////論理削除
                dataGridViewDsp.Columns[6].Visible = false;
                ////論理削除フラグ
                dataGridViewDsp.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[7].Width = 90;
                ////非表示理由
                dataGridViewDsp.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[8].Width = 400;
                ////品数
                dataGridViewDsp.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[9].Width = 100;
                ////
                dataGridViewDsp.Columns[10].Visible = false;
                ////ステータス
                dataGridViewDsp.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[11].Width = 150;

                dataGridViewDsp.Columns[12].Visible = false;
                dataGridViewDsp.Columns[13].Visible = false;

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Hattyu.Count / (double)pageSize)) + "ページ";

                dataGridViewDsp.Refresh();
                RefreshHattyuCombo();
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
                RefreshHattyuCombo();
                RefreshMakerCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = true;
                label13.Visible = false;
                cmbHattyuId.Enabled = true; //発注ID

                label1.Enabled = false;
                cmbMakerId.Enabled = false; //メーカー

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
                RefreshHattyuCombo();
                RefreshMakerCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = false;
                label13.Visible = true;
                cmbHattyuId.Enabled = false; //発注ID

                label1.Enabled = true;
                cmbMakerId.Enabled = true; //メーカー

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
                RefreshHattyuCombo();
                RefreshMakerCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = true;
                label13.Visible = false;
                cmbHattyuId.Enabled = true; //発注ID

                label1.Enabled = false;
                cmbMakerId.Enabled = false; //メーカー

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
                RefreshHattyuCombo();
                RefreshMakerCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = true;
                label13.Visible = false;
                cmbHattyuId.Enabled = true; //発注ID

                label1.Enabled = true;
                cmbMakerId.Enabled = true; //メーカー

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
                RefreshHattyuCombo();
                RefreshMakerCombo();
                RefreshEmployeeCombo();
                AllClear();

                label8.Enabled = true;
                label13.Visible = false;
                cmbHattyuId.Enabled = true; //発注ID

                label1.Enabled = false;
                cmbMakerId.Enabled = false; //メーカー

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
                dataGridViewDsp.DataSource = Hattyu.Take(pageSize).ToList();

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
                dataGridViewDsp.DataSource = Hattyu.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                int lastNo = (int)Math.Ceiling(Hattyu.Count / (double)pageSize) - 1;
                //最終ページでなければ
                if (pageNo <= lastNo)
                    dataGridViewDsp.DataSource = Hattyu.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                int lastPage = (int)Math.Ceiling(Hattyu.Count / (double)pageSize);
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
                int pageNo = (int)Math.Ceiling(Hattyu.Count / (double)pageSize) - 1;
                dataGridViewDsp.DataSource = Hattyu.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                    cmbHattyuId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
                    cmbMakerId.SelectedIndex = Maker.FindIndex(x => x.MaID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[1].Value.ToString()));
                    cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[3].Value.ToString()));
                    dateTimePickerS.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[5].Value.ToString();
                    checkBoxFlag.Checked = (bool)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[7].Value;
                    //null処理を空欄処理
                    if (dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[8].Value == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    else
                        textBoxHideRea.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[8].Value.ToString();
                    cmbState.SelectedValue = int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[10].Value.ToString());
                    if (radioBtnCon.Checked)
                    {
                        if((int)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[10].Value != 0)
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
            catch(Exception ex)
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
            catch(Exception ex)
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
                cmbSmall.SelectedIndexChanged -=  cmbSmall_SelectedIndexChanged;
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbHattyuId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (updFlg == true)
                {
                    if (cmbHattyuId.SelectedValue == null)
                    {
                        AllClear();
                    }
                    else
                    {
                        int index = Hattyu.FindIndex(x => x.HaID == int.Parse(cmbHattyuId.SelectedValue.ToString()));
                        cmbMakerId.SelectedIndex = Maker.FindIndex(x => x.MaID == Hattyu[index].MaID);
                        cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == Hattyu[index].EmID);
                        dateTimePickerS.Text = Hattyu[index].HaDate.ToString();
                        checkBoxFlag.Checked = Hattyu[index].HaFlagBool;
                        if (Hattyu[index].HaHidden == null)
                        {
                            textBoxHideRea.Text = "";
                        }
                        else
                        {
                            textBoxHideRea.Text = Hattyu[index].HaHidden.ToString();
                        }
                        cmbState.SelectedValue = Hattyu[index].WaWarehousingFlag;
                        if (radioBtnCon.Checked)
                        {
                            if (Hattyu[index].WaWarehousingFlag != 0)
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
                if (e.ColumnIndex == 2&& (int)dataGridview.Rows[e.RowIndex].Cells[12].Value != 0)
                {
                    dataGridViewDsp.Rows[e.RowIndex].Cells[2].Style.BackColor = SystemColors.AppWorkspace;
                }
                if (e.ColumnIndex == 4 && (int)dataGridview.Rows[e.RowIndex].Cells[13].Value != 0)
                {
                    dataGridViewDsp.Rows[e.RowIndex].Cells[4].Style.BackColor = SystemColors.AppWorkspace;
                }
                if (e.ColumnIndex == 9 && (int)dataGridview.Rows[e.RowIndex].Cells[9].Value == 0)
                {
                    dataGridViewDsp.Rows[e.RowIndex].Cells[9].Style.BackColor = SystemColors.AppWorkspace;
                }
            }
            catch (Exception ex)
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
                Product = product.GetProductData(new M_ProductDsp { PrID = 0, McID = mcID, ScID = scID,MaID=maID, PrFlag = 0, PrName = "", PrModelNumber = "", PrColor = "" });
                RefreshProductCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
    }    
}
