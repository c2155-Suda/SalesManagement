using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SalesManagement_SysDev
{
    public partial class F_HattyuDetail : Form
    {
        public int haID;
        public int maID;
        public string maName;
        public int status;
        private string[] statusString = { "未確定", "発注済", "入庫済" };
        DataAccess access = new DataAccess();
        ProductDbConnect product = new ProductDbConnect();
        HattyuDbConnection hattyu = new HattyuDbConnection();
        EmployeeDbConnection employee = new EmployeeDbConnection();
        private List<T_HattyuDetailDsp> HattyuDetail;
        private List<T_HattyuDetailDsp> subHattyuDetail;
        private List<T_HattyuDetailDsp_Agr> HattyuDetail_Agr;
        private List<M_ProductDsp> Product;
        private List<M_MajorClassificationDsp> majorClass;
        private List<M_SmallClassificationDsp> smallClass;
        private int pageSize = 8;
        private class QuantItemSet
        {
            public string itemDisp { get; set; }
            public int itemValue { get; set; }
        }
        private List<QuantItemSet> Quant = new List<QuantItemSet>();

        public F_HattyuDetail()
        {
            InitializeComponent();
        }

        private void F_HattyuDetail_Load(object sender, EventArgs e)
        {
            dataGridViewDsp_Agr.Visible = false;
            textBoxHattyuId.Text = haID.ToString();
            textBoxState.Text = statusString[status];
            textBoxMaker.Text = maName;
            if (status != 0)
            {
                radioBtnReg.Enabled = false;
                radioBtnCon.Enabled = false;
            }

            Product = product.GetProductData();
            majorClass = access.GetMajorClassificationDspData();
            GetComboBoxData();
            GetAllData();
            radioBtnSea.Checked = true;
        }
        private void GetComboBoxData()
        {
            try
            {
                cmbHattyuDetailId.DisplayMember = "HaDetailID";
                cmbHattyuDetailId.ValueMember = "HaDetailID";
                cmbHattyuDetailId.DataSource = HattyuDetail;
                cmbHattyuDetailId.SelectedIndex = -1;

                //商品IDのコンボボックスData読込
                cmbProductId.DisplayMember = "PrName";
                cmbProductId.ValueMember = "PrID";
                cmbProductId.DataSource = Product;
                cmbProductId.SelectedIndex = -1;

                //大分類のコンボボックスData読込
                majorClass.Insert(0, new M_MajorClassificationDsp { McID = 0, McName = "" });
                majorClass.Add(new M_MajorClassificationDsp { McID = -1, McName = "削除済み" });
                cmbMajor.DisplayMember = "McName";
                cmbMajor.ValueMember = "McID";
                cmbMajor.DataSource = majorClass;
                cmbMajor.SelectedIndex = -1;

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
        private void RefreshProductCombo(bool flagNameColor)
        {
            try
            {
                int temp = 0;
                if (cmbProductId.SelectedIndex != -1)
                {
                    temp = int.Parse(cmbProductId.SelectedValue.ToString());
                }
                if (flagNameColor)
                {
                    foreach (var item in Product)
                    {
                        string ProdactColorName = item.PrName + "(" + item.PrColor + ")";

                        item.PrName = ProdactColorName;
                    }
                }
                if (radioBtnSea.Checked)
                {
                    Product.Insert(0, new M_ProductDsp { PrID = 0, PrName = "" });
                    Product.Add(new M_ProductDsp { PrID = -1, PrName = "削除済み" });
                }
                else
                {
                    Product.RemoveAll(x => x.PrID == 0 || x.PrID == -1);
                }
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
        private void RefreshHattyuDetailCombo()
        {
            try
            {
                int temp = 0;
                if (cmbHattyuDetailId.SelectedValue != null)
                {
                    temp = int.Parse(cmbHattyuDetailId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    cmbHattyuDetailId.DataSource = subHattyuDetail;
                    cmbHattyuDetailId.SelectedIndex = subHattyuDetail.FindIndex(x => x.HaDetailID == temp);
                }
                else
                {
                    cmbHattyuDetailId.DataSource = HattyuDetail;
                    cmbHattyuDetailId.SelectedIndex = HattyuDetail.FindIndex(x => x.HaDetailID == temp);
                }
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
                if (!GetValidDataAtRegistration())
                    return;
                //登録情報作成
                var regHattyuDetail = GenerateDataAtRegistration();
                //登録処理
                hattyu.RegistHattyuDetailData(regHattyuDetail);
                AllClear();
                GetAllData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private bool GetValidDataAtRegistration()
        {
            try
            {
                if (!hattyu.CheckHattyuIsInsertable(haID))
                {
                    MessageBox.Show("発注が確定済です。", "入力エラー");
                    return false;
                }
                if (cmbProductId.SelectedIndex == -1)
                {
                    MessageBox.Show("商品名が未選択です。", "選択エラー");
                    cmbProductId.Focus();
                    return false;
                }
                if (cmbQuant.SelectedIndex == -1)
                {
                    MessageBox.Show("発注量が未選択です", "選択エラー");
                    cmbQuant.Focus();
                    return false;
                }
                if (cmbQuant.SelectedIndex == 0 && hattyu.ProductSumInCurrentHattyu((int)cmbProductId.SelectedValue, haID) != 0)
                {
                    MessageBox.Show("選択した商品は既に登録済です。", "選択エラー");
                    cmbProductId.Focus();
                    return false;
                }
                if(cmbQuant.SelectedIndex==1&&hattyu.ProductSumInCurrentHattyu((int)cmbProductId.SelectedValue, haID) == 0)
                {
                    MessageBox.Show("選択した商品の合計発注量は既に0です。", "選択エラー");
                    cmbProductId.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        private T_HattyuDetail GenerateDataAtRegistration()
        {
            try
            {
                int prID = (int)cmbProductId.SelectedValue;
                int preRegistedQuantity = hattyu.ProductSumInCurrentHattyu(prID, haID);
                int prQuantity = (int)cmbQuant.SelectedValue;
                if (prQuantity == -1)
                {
                    prQuantity = preRegistedQuantity * (-1);
                }
                //登録データセット
                return new T_HattyuDetail
                {
                    HaID = haID,
                    PrID = prID,
                    HaQuantity = prQuantity
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
                //抽出
                GenerateDataAtSelect();

                //結果表示
                SetSelectDate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void GenerateDataAtSelect()
        {
            try
            {
                int hadetailID = 0;
                if (cmbHattyuDetailId.SelectedIndex != -1)
                {
                    hadetailID = (int)cmbHattyuDetailId.SelectedValue;
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


                // 検索条件のセット
                T_HattyuDetailDsp selectCondition = new T_HattyuDetailDsp()
                {
                    HaID = haID,
                    HaDetailID = hadetailID,
                    PrID = prID,
                    McID = mcID,
                    ScID = scID,
                };
                // データの抽出
                HattyuDetail = hattyu.GetHattyuDetailData(selectCondition);
                subHattyuDetail = new List<T_HattyuDetailDsp>(HattyuDetail);
                subHattyuDetail.Insert(0, new T_HattyuDetailDsp { HaDetailID = null });
                HattyuDetail_Agr = hattyu.GetHattyuDetailData_Agr(selectCondition);
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
                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = 0;
                dataGridViewDsp.DataSource = HattyuDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
                dataGridViewDsp_Agr.DataSource = HattyuDetail_Agr.Skip(pageSize * pageNo).Take(pageSize).ToList();
                RefreshMaxPage();
                dataGridViewDsp.Refresh();
                dataGridViewDsp_Agr.Refresh();
                RefreshHattyuDetailCombo();
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
                GenerateDataAtSelect();
                if (HattyuDetail == null)
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
                //dataGridViewのページ番号指定
                SetSelectDate();

                //列幅自動設定解除
                dataGridViewDsp.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dataGridViewDsp_Agr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                //textsize
                this.dataGridViewDsp.DefaultCellStyle.Font = new Font("ＭＳ Ｐゴシック", 18);
                dataGridViewDsp_Agr.DefaultCellStyle.Font = new Font("ＭＳ Ｐゴシック", 18);
                this.dataGridViewDsp.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ Ｐゴシック", 18);
                dataGridViewDsp_Agr.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ Ｐゴシック", 18);
                //ヘッダーの高さ
                dataGridViewDsp.ColumnHeadersHeight = 60;
                dataGridViewDsp_Agr.ColumnHeadersHeight = 60;
                //ヘッダーの折り返し表示
                dataGridViewDsp.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridViewDsp_Agr.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridViewDsp.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dataGridViewDsp_Agr.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //行単位選択
                dataGridViewDsp.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewDsp_Agr.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //複数指定無効処理
                dataGridViewDsp.MultiSelect = false;
                dataGridViewDsp_Agr.MultiSelect = false;

                //ヘッダー文字位置、セル文字位置、列幅の設定
                ////発注詳細ID
                dataGridViewDsp.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[0].Width = 105;
                dataGridViewDsp_Agr.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp_Agr.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp_Agr.Columns[0].Width = 105;
                ////発注ID
                dataGridViewDsp.Columns[1].Visible = false;
                dataGridViewDsp_Agr.Columns[1].Visible = false;
                ////商品ID
                dataGridViewDsp.Columns[2].Visible = false;
                dataGridViewDsp_Agr.Columns[2].Visible = false;
                ////商品名
                dataGridViewDsp.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[3].Width = 200;
                dataGridViewDsp_Agr.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp_Agr.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp_Agr.Columns[3].Width = 200;
                ////大分類ID
                dataGridViewDsp.Columns[4].Visible = false;
                dataGridViewDsp_Agr.Columns[4].Visible = false;
                ////大分類
                dataGridViewDsp.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[5].Width = 260;
                dataGridViewDsp_Agr.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp_Agr.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp_Agr.Columns[5].Width = 260;
                ////小分類ID
                dataGridViewDsp.Columns[6].Visible = false;
                dataGridViewDsp_Agr.Columns[6].Visible = false;
                ////小分類
                dataGridViewDsp.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[7].Width = 190;
                dataGridViewDsp_Agr.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp_Agr.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp_Agr.Columns[7].Width = 190;
                ////数量
                dataGridViewDsp.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[8].Width = 70;
                dataGridViewDsp_Agr.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp_Agr.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp_Agr.Columns[8].Width = 70;
                /////ProductFlag//安全在庫数
                dataGridViewDsp.Columns[9].Visible = false;
                dataGridViewDsp_Agr.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp_Agr.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp_Agr.Columns[9].Width = 70;
                ////発注点
                dataGridViewDsp_Agr.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp_Agr.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp_Agr.Columns[10].Width = 70;
                ////在庫数
                dataGridViewDsp_Agr.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp_Agr.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp_Agr.Columns[11].Width = 70;
                ////入庫予定
                dataGridViewDsp_Agr.Columns[12].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp_Agr.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp_Agr.Columns[12].Width = 70;
                ////ProductFlag
                dataGridViewDsp_Agr.Columns[13].Visible = false;
                ////MaID
                dataGridViewDsp_Agr.Columns[14].Visible = false;
                dataGridViewDsp.Refresh();
                dataGridViewDsp_Agr.Refresh();
            }
            catch
            {
                throw;
            }
        }
        private void checkBox_Agr_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox_Agr.Checked)
                {
                    label1.Enabled = false;
                    cmbHattyuDetailId.Enabled = false;
                    dataGridViewDsp.Visible = false;
                    dataGridViewDsp_Agr.Visible = true;
                }
                else
                {
                    dataGridViewDsp_Agr.Visible = false;
                    dataGridViewDsp.Visible = true;
                    if (radioBtnSea.Checked)
                    {
                        label1.Enabled = true;
                        cmbHattyuDetailId.Enabled = true;
                    }
                }
                SetSelectDate();
                RefreshMaxPage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private void RefreshMaxPage()
        {
            try
            {
                int dataCount;
                if (checkBox_Agr.Checked)
                {
                    dataCount = HattyuDetail_Agr.Count;
                }
                else
                {
                    dataCount = HattyuDetail.Count;
                }
                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(dataCount / (double)pageSize)) + "ページ";
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
                if (!radioBtnReg.Checked)
                {
                    return;
                }
                RefreshHattyuDetailCombo();
                RefreshProductCombo(false);
                AllClear();
                label1.Enabled = false;
                cmbHattyuDetailId.Enabled = false;
                cmbHattyuDetailId.DropDownStyle = ComboBoxStyle.Simple;
                label13.Visible = true;  //発注詳細ID

                labelQuant.Enabled = true;
                cmbQuant.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbQuant.Enabled = true; //発注量

                label11.Enabled = true;
                label3.Enabled = true;
                label9.Enabled = true;
                cmbProductId.Enabled = true;
                cmbSmall.Enabled = true;
                cmbMajor.Enabled = true;

                btnReg.Enabled = true;
                btnSea.Enabled = false;
                btnCon.Enabled = false;
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
                if (!radioBtnSea.Checked)
                {
                    return;
                }
                RefreshHattyuDetailCombo();
                RefreshProductCombo(false);
                AllClear();
                label1.Enabled = true;
                cmbHattyuDetailId.Enabled = true;
                cmbHattyuDetailId.DropDownStyle = ComboBoxStyle.DropDownList;
                label13.Visible = false;  //発注詳細ID

                labelQuant.Enabled = false;
                cmbQuant.DropDownStyle = ComboBoxStyle.Simple;
                cmbQuant.Enabled = false; //発注量

                label11.Enabled = true;
                label3.Enabled = true;
                label9.Enabled = true;
                cmbProductId.Enabled = true;
                cmbSmall.Enabled = true;
                cmbMajor.Enabled = true;

                btnReg.Enabled = false;
                btnSea.Enabled = true;
                btnCon.Enabled = false;
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
        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void AllClear()
        {
            try
            {
                cmbHattyuDetailId.SelectedIndex = -1;
                cmbMajor.SelectedIndex = -1;
                cmbSmall.SelectedIndex = -1;
                cmbProductId.SelectedIndex = -1;            }
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
                Product = product.GetProductData(new M_ProductDsp { PrID = 0, McID = mcID, ScID = scID,MaID=maID, PrFlag = 0, PrName = "", PrModelNumber = "", PrColor = "" });
                RefreshProductCombo(true);
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
                Product = product.GetProductData(new M_ProductDsp { PrID = 0, McID = mcID, ScID = scID,MaID=maID, PrFlag = 0, PrName = "", PrModelNumber = "", PrColor = "" });
                RefreshProductCombo(true);
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
                if (cmbProductId.SelectedIndex == -1 || (radioBtnSea.Checked && cmbProductId.SelectedIndex == 0) || (radioBtnSea.Checked && cmbProductId.SelectedIndex == Product.Count - 1))
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
        private void cmbProductId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbProductId.SelectedIndex == -1 || (radioBtnSea.Checked && cmbProductId.SelectedIndex == 0) || (radioBtnSea.Checked && cmbProductId.SelectedIndex == Product.Count - 1))
                {
                    textBoxSafty.Text = "";
                    textBoxPoint.Text = "";
                    textBoxStock.Text = "";
                    textBoxPreWare.Text = "";
                    Quant.Clear();
                    cmbQuant.DataSource = null;
                    cmbQuant.DisplayMember = "itemDisp";
                    cmbQuant.ValueMember = "itemValue";
                    cmbQuant.DataSource = Quant;
                    cmbQuant.SelectedIndex = -1;
                    return;
                }
                var selectedProduct = Product.Single(x => x.PrID == int.Parse(cmbProductId.SelectedValue.ToString()));
                textBoxSafty.Text = selectedProduct.PrSafetyStock.ToString();
                textBoxPoint.Text = selectedProduct.PrOrderPoint.ToString();
                textBoxStock.Text = selectedProduct.StQuantity.ToString();
                textBoxPreWare.Text = selectedProduct.PrPreWarehousing.ToString();
                if (radioBtnSea.Checked)
                {
                    cmbQuant.Text = selectedProduct.PrOrderQuantity.ToString();
                }
                else
                {
                    Quant.Clear();
                    Quant.Add(new QuantItemSet { itemValue = selectedProduct.PrOrderQuantity, itemDisp = selectedProduct.PrOrderQuantity.ToString() });
                    Quant.Add(new QuantItemSet { itemValue = -1, itemDisp = "発注消込" });
                    cmbQuant.DataSource = null;
                    cmbQuant.DisplayMember = "itemDisp";
                    cmbQuant.ValueMember = "itemValue";
                    cmbQuant.DataSource = Quant;
                    cmbQuant.SelectedIndex = -1;
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
                if (!checkBox_Agr.Checked)
                {
                    dataGridViewDsp.DataSource = HattyuDetail.Take(pageSize).ToList();
                    dataGridViewDsp.Refresh();
                }
                else
                {
                    dataGridViewDsp_Agr.DataSource = HattyuDetail_Agr.Take(pageSize).ToList();
                    dataGridViewDsp_Agr.Refresh();
                }
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
                if (!checkBox_Agr.Checked)
                {
                    dataGridViewDsp.DataSource = HattyuDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
                    dataGridViewDsp.Refresh();
                }
                else
                {
                    dataGridViewDsp_Agr.DataSource = HattyuDetail_Agr.Skip(pageSize * pageNo).Take(pageSize).ToList();
                    dataGridViewDsp_Agr.Refresh();
                }
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
                int lastNo;
                if (!checkBox_Agr.Checked)
                {
                    lastNo = (int)Math.Ceiling(HattyuDetail.Count / (double)pageSize);
                    if (pageNo < lastNo)
                    {
                        dataGridViewDsp.DataSource = HattyuDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
                        dataGridViewDsp.Refresh();
                        textBoxPageNo.Text = (pageNo + 1).ToString();
                    }
                }
                else
                {
                    lastNo = (int)Math.Ceiling(HattyuDetail_Agr.Count / (double)pageSize);
                    if (pageNo < lastNo)
                    {
                        dataGridViewDsp_Agr.DataSource = HattyuDetail_Agr.Skip(pageSize * pageNo).Take(pageSize).ToList();
                        dataGridViewDsp_Agr.Refresh();
                        textBoxPageNo.Text = (pageNo + 1).ToString();
                    }
                    else
                    {
                        textBoxPageNo.Text = lastNo.ToString();
                    }
                }
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
                int pageNo;
                if (!checkBox_Agr.Checked)
                {
                    pageNo = (int)Math.Ceiling(HattyuDetail.Count / (double)pageSize) - 1;
                    dataGridViewDsp.DataSource = HattyuDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
                    dataGridViewDsp.Refresh();
                }
                else
                {
                    pageNo = (int)Math.Ceiling(HattyuDetail_Agr.Count / (double)pageSize) - 1;
                    dataGridViewDsp_Agr.DataSource = HattyuDetail_Agr.Skip(pageSize * pageNo).Take(pageSize).ToList();
                    dataGridViewDsp_Agr.Refresh();
                }
                //ページ番号の設定
                textBoxPageNo.Text = (pageNo + 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void radioBtnCon_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (!radioBtnCon.Checked)
                {
                    return;
                }
                RefreshHattyuDetailCombo();
                RefreshProductCombo(false);
                AllClear();
                label1.Enabled = false;
                cmbHattyuDetailId.Enabled = false;
                cmbHattyuDetailId.DropDownStyle = ComboBoxStyle.DropDownList;
                label13.Visible = false;  //発注詳細ID

                labelQuant.Enabled = false;
                cmbQuant.DropDownStyle = ComboBoxStyle.Simple;
                cmbQuant.Enabled = false; //発注量

                label11.Enabled = false;
                label3.Enabled = false;
                label9.Enabled = false;
                cmbProductId.Enabled = false;
                cmbSmall.Enabled = false;
                cmbMajor.Enabled = false;

                btnReg.Enabled = false;
                btnSea.Enabled = false;
                btnCon.Enabled = true;
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
                int HaID = int.Parse(textBoxHattyuId.Text);
                var hattyuData = hattyu.GetHattyuData(new T_HattyuRead { HaID = HaID, HaFlag = -1, WaWarehouseFlag = -1 }).Single();
                T_HattyuDsp updatedHattyuData;
                if (!GetValidDataAtConfirm(hattyuData))
                {
                    updatedHattyuData = hattyu.GetHattyuData(new T_HattyuRead { HaID = HaID, HaFlag = -1, WaWarehouseFlag = -1 }).Single();
                    status = updatedHattyuData.WaWarehousingFlag;
                    textBoxState.Text = statusString[status];
                    if (status != 0)
                    {
                        radioBtnReg.Enabled = false;
                        radioBtnCon.Enabled = false;
                    }
                    GetAllData();
                    return;
                }
                var hattyuDetailData = hattyu.GetHattyuDetailData_Agr(new T_HattyuDetailDsp { HaID = HaID });
                if (!GetValidDetailDataAtConfirm(hattyuDetailData))
                {
                    GetAllData();
                    return;
                }
                var warehousingData = GenerateDataAtConfirm(hattyuData);
                var warehousingDetailData = GenerateDetailDataAtConfirm(hattyuDetailData);
                DialogResult dialog = MessageBox.Show(string.Format("発注ID：{0}を確定しますか。", warehousingData.HaID), "確認", MessageBoxButtons.OKCancel);
                if (dialog != DialogResult.OK)
                {
                    return;
                }
                hattyu.FinalizeHattyuData(warehousingData, warehousingDetailData);
                updatedHattyuData = hattyu.GetHattyuData(new T_HattyuRead { HaID = HaID, HaFlag = -1, WaWarehouseFlag = -1 }).Single();
                status = updatedHattyuData.WaWarehousingFlag;
                textBoxState.Text = statusString[status];
                if (status != 0)
                {
                    radioBtnReg.Enabled = false;
                    radioBtnCon.Enabled = false;
                }
                GetAllData();
                radioBtnSea.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private bool GetValidDataAtConfirm(T_HattyuDsp hattyuData)
        {
            try
            {
                if (!hattyu.CheckHattyuIsInsertable((int)hattyuData.HaID))
                {
                    MessageBox.Show("本発注IDは確定済です。", "入力エラー");
                    return false;
                }
                if (hattyuData.KindOfProducts <= 0)
                {
                    MessageBox.Show("本発注に商品が含まれていません。", "入力エラー");
                    return false;
                }
                if (!access.CheckMakerIsActive(hattyuData.MaID))
                {
                    MessageBox.Show("本発注に対応するメーカーが削除済です。", "選択エラー");
                    return false;
                }
                if (!employee.CheckEmployeeIsActive(hattyuData.EmID))
                {
                    MessageBox.Show("本発注を登録した社員が削除済です。", "選択エラー");
                    return false;
                }
                if (!hattyu.CheckHattyuIsActive((int)hattyuData.HaID))
                {
                    DialogResult dialog = MessageBox.Show("発注確定時に対象データの削除は取り消されます。", "確認", MessageBoxButtons.OKCancel);
                    if (dialog != DialogResult.OK)
                    {
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
        private bool GetValidDetailDataAtConfirm(List<T_HattyuDetailDsp_Agr> hattyuDetailData)
        {
            try
            {
                foreach (var x in hattyuDetailData)
                {
                    if (!product.CheckProductIsActive(x.PrID))
                    {
                        MessageBox.Show(string.Format("商品ID：{0}は削除済です。", x.PrID), "選択エラー");
                        return false;
                    }
                    if (x.MaID != maID)
                    {
                        MessageBox.Show(string.Format("商品ID：{0}のメーカーが更新されました。\n新規の発注を作成してください。", x.PrID), "選択エラー");
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
                    HaID = (int)hattyuData.HaID
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

        private void dataGridViewDsp_Agr_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                DataGridView dataGridview = (DataGridView)sender;
                if (e.ColumnIndex == 3 && (int)dataGridview.Rows[e.RowIndex].Cells[13].Value != 0)
                {
                    dataGridview.Rows[e.RowIndex].Cells[3].Style.BackColor = SystemColors.AppWorkspace;
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
                if (e.ColumnIndex == 3 && (int)dataGridview.Rows[e.RowIndex].Cells[9].Value != 0)
                {
                    dataGridview.Rows[e.RowIndex].Cells[3].Style.BackColor = SystemColors.AppWorkspace;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
    }
}
