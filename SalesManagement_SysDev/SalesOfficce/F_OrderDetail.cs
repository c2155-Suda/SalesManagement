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

namespace SalesManagement_SysDev
{
    public partial class F_OrderDetail : Form
    {
        public int orID;
        public int status;
        private string[] statusString = { "未確定", "受注済", "注文済", "出庫済", "入荷済", "出荷済" };
        DataAccess access = new DataAccess();
        ProductDbConnect product = new ProductDbConnect();
        OrderDbConnection order = new OrderDbConnection();
        ClientDbConnection client = new ClientDbConnection();
        EmployeeDbConnection employee = new EmployeeDbConnection();
        private List<T_OrderDetailDsp> OrderDetail;
        private List<T_OrderDetailDsp> subOrderDetail;
        private List<T_OrderDetailDsp_Agr_Fin> OrderDetail_Fin;
        private List<T_OrderDetailDsp_Agr_NonFin> OrderDetail_NonFin;
        private List<M_ProductDsp> Product;
        private List<M_MajorClassificationDsp> majorClass;
        private List<M_SmallClassificationDsp> smallClass;
        private int pageSize = 8;

        public F_OrderDetail()
        {
            InitializeComponent();
        }

        private void F_OrderDetail_Load(object sender, EventArgs e)
        {
            dataGridViewDsp_Agr.Visible = false;
            textBoxOrderId.Text = orID.ToString();
            textBoxState.Text = statusString[status];
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
                cmbOrderDetailId.DisplayMember = "OrDetailID";
                cmbOrderDetailId.ValueMember = "OrDetailID";
                cmbOrderDetailId.DataSource = OrderDetail;
                cmbOrderDetailId.SelectedIndex = -1;

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
        private void RefreshOrderDetailCombo()
        {
            try
            {
                int temp = 0;
                if (cmbOrderDetailId.SelectedValue != null)
                {
                    temp = int.Parse(cmbOrderDetailId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    cmbOrderDetailId.DataSource = subOrderDetail;
                    cmbOrderDetailId.SelectedIndex = subOrderDetail.FindIndex(x => x.OrDetailID == temp);
                }
                else
                {
                    cmbOrderDetailId.DataSource = OrderDetail;
                    cmbOrderDetailId.SelectedIndex = OrderDetail.FindIndex(x => x.OrDetailID == temp);
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
                var regOrderDetail = GenerateDataAtRegistration();
                //登録処理
                order.RegistOrderDetailData(regOrderDetail);
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
                if (!order.CheckOrderIsInsertable(orID))
                {
                    MessageBox.Show("受注が確定済です。", "入力エラー");
                    return false;
                }
                if (cmbProductId.SelectedIndex == -1)
                {
                    MessageBox.Show("商品名が未選択です。", "選択エラー");
                    cmbProductId.Focus();
                    return false;
                }
                if (numericUpDown.Value == 0)
                {
                    MessageBox.Show("発注量0で登録できません。", "入力エラー");
                    numericUpDown.Focus();
                    return false;
                }
                if (numericUpDown.Value + order.ProductQuantSumInCurrentOrder((int)cmbProductId.SelectedValue, orID) > NumericRange.Quant)
                {
                    MessageBox.Show("商品の合計発注量が9999を超過します。", "入力エラー");
                    numericUpDown.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        private T_OrderDetail GenerateDataAtRegistration()
        {
            try
            {
                int prID = (int)cmbProductId.SelectedValue;
                int preRegistedQuantity = order.ProductQuantSumInCurrentOrder(prID, orID);
                int prQuantity = (int)numericUpDown.Value;
                if (prQuantity + preRegistedQuantity < 0)
                {
                    prQuantity = preRegistedQuantity * (-1);
                }

                //登録データセット
                return new T_OrderDetail
                {
                    OrID = orID,
                    PrID = prID,
                    OrQuantity = prQuantity,
                    OrTotalPrice = 0
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
                int ordetailID = 0;
                if (cmbOrderDetailId.SelectedIndex != -1)
                {
                    ordetailID = (int)cmbOrderDetailId.SelectedValue;
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
                T_OrderDetailDsp selectCondition = new T_OrderDetailDsp()
                {
                    OrID = orID,
                    OrDetailID = ordetailID,
                    PrID = prID,
                    McID = mcID,
                    ScID = scID,
                };
                // データの抽出
                OrderDetail = order.GetOrderDetailData(selectCondition);
                subOrderDetail = new List<T_OrderDetailDsp>(OrderDetail);
                subOrderDetail.Insert(0, new T_OrderDetailDsp { OrDetailID = null });
                if (status == 0)
                {
                    OrderDetail_NonFin = order.GetOrderDetailData_Agr_NonFin(selectCondition);
                }
                else
                {
                    OrderDetail_Fin = order.GetOrderDetailData_Agr_Fin(selectCondition);
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
                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = 0;
                dataGridViewDsp.DataSource = OrderDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
                if (status == 0)
                {
                    dataGridViewDsp_Agr.DataSource = OrderDetail_NonFin.Skip(pageSize * pageNo).Take(pageSize).ToList();
                }
                else
                {
                    dataGridViewDsp_Agr.DataSource = OrderDetail_Fin.Skip(pageSize * pageNo).Take(pageSize).ToList();
                }

                RefreshMaxPage();
                dataGridViewDsp.Refresh();
                dataGridViewDsp_Agr.Refresh();
                RefreshOrderDetailCombo();
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
                if (OrderDetail == null)
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
                ////受注詳細ID
                dataGridViewDsp.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[0].Width = 105;
                dataGridViewDsp_Agr.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp_Agr.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp_Agr.Columns[0].Width = 105;
                ////受注ID
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
                ////価格
                dataGridViewDsp.Columns[9].Visible = false;
                dataGridViewDsp_Agr.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp_Agr.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridViewDsp_Agr.Columns[9].Width = 200;

                dataGridViewDsp.Columns[10].Visible = false;
                dataGridViewDsp_Agr.Columns[10].Visible = false;

                dataGridViewDsp_Agr.Columns[11].Visible = false;
                if (status == 0)
                {
                    dataGridViewDsp_Agr.Columns[10].Visible = false;
                }

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
                    cmbOrderDetailId.Enabled = false;
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
                        cmbOrderDetailId.Enabled = true;
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
                    if (status == 0)
                    {
                        dataCount = OrderDetail_NonFin.Count;
                    }
                    else
                    {
                        dataCount = OrderDetail_Fin.Count;
                    }
                }
                else
                {
                    dataCount = OrderDetail.Count;
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
                RefreshOrderDetailCombo();
                RefreshProductCombo(false);
                AllClear();
                label1.Enabled = false;
                cmbOrderDetailId.Enabled = false;
                cmbOrderDetailId.DropDownStyle = ComboBoxStyle.Simple;
                label13.Visible = true;  //受注詳細ID

                label8.Visible = true;
                numericUpDown.Visible = true; //数量

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
                RefreshOrderDetailCombo();
                RefreshProductCombo(false);
                AllClear();
                label1.Enabled = true;
                cmbOrderDetailId.Enabled = true;
                cmbOrderDetailId.DropDownStyle = ComboBoxStyle.DropDownList;
                label13.Visible = false;  //受注詳細ID

                label8.Visible = false;
                numericUpDown.Visible = false; //数量

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
                cmbOrderDetailId.SelectedIndex = -1;
                cmbMajor.SelectedIndex = -1;
                cmbSmall.SelectedIndex = -1;
                cmbProductId.SelectedIndex = -1;
                numericUpDown.Value = 0;
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
                Product = product.GetProductData(new M_ProductDsp { PrID = 0, McID = mcID, ScID = scID, PrFlag = 0, PrName = "", PrModelNumber = "", PrColor = "" });
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

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkBox_Agr.Checked)
                {
                    dataGridViewDsp.DataSource = OrderDetail.Take(pageSize).ToList();
                    dataGridViewDsp.Refresh();
                }
                else
                {
                    if (status == 0)
                    {
                        dataGridViewDsp_Agr.DataSource = OrderDetail_NonFin.Take(pageSize).ToList();
                    }
                    else
                    {
                        dataGridViewDsp_Agr.DataSource = OrderDetail_Fin.Take(pageSize).ToList();
                    }
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
                    dataGridViewDsp.DataSource = OrderDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
                    dataGridViewDsp.Refresh();
                }
                else
                {
                    if (status == 0)
                    {
                        dataGridViewDsp_Agr.DataSource = OrderDetail_NonFin.Skip(pageSize * pageNo).Take(pageSize).ToList();
                    }
                    else
                    {
                        dataGridViewDsp_Agr.DataSource = OrderDetail_Fin.Skip(pageSize * pageNo).Take(pageSize).ToList();
                    }
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
                    lastNo = (int)Math.Ceiling(OrderDetail.Count / (double)pageSize);
                    if (pageNo < lastNo)
                    {
                        dataGridViewDsp.DataSource = OrderDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
                        dataGridViewDsp.Refresh();
                        textBoxPageNo.Text = (pageNo + 1).ToString();
                    }
                }
                else
                {
                    if (status == 0)
                    {
                        lastNo = (int)Math.Ceiling(OrderDetail_NonFin.Count / (double)pageSize);
                        if (pageNo < lastNo)
                        {
                            dataGridViewDsp_Agr.DataSource = OrderDetail_NonFin.Skip(pageSize * pageNo).Take(pageSize).ToList();
                            dataGridViewDsp_Agr.Refresh();
                            textBoxPageNo.Text = (pageNo + 1).ToString();
                        }
                        else
                        {
                            textBoxPageNo.Text = lastNo.ToString();
                        }
                    }
                    else
                    {
                        lastNo = (int)Math.Ceiling(OrderDetail_Fin.Count / (double)pageSize);
                        if (pageNo < lastNo)
                        {
                            dataGridViewDsp_Agr.DataSource = OrderDetail_Fin.Skip(pageSize * pageNo).Take(pageSize).ToList();
                            dataGridViewDsp_Agr.Refresh();
                            textBoxPageNo.Text = (pageNo + 1).ToString();
                        }
                        else
                        {
                            textBoxPageNo.Text = lastNo.ToString();
                        }
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
                    pageNo = (int)Math.Ceiling(OrderDetail.Count / (double)pageSize) - 1;
                    dataGridViewDsp.DataSource = OrderDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
                    dataGridViewDsp.Refresh();
                }
                else
                {
                    if (status == 0)
                    {
                        pageNo = (int)Math.Ceiling(OrderDetail_NonFin.Count / (double)pageSize) - 1;
                        dataGridViewDsp_Agr.DataSource = OrderDetail_NonFin.Skip(pageSize * pageNo).Take(pageSize).ToList();
                        dataGridViewDsp_Agr.Refresh();
                    }
                    else
                    {
                        pageNo = (int)Math.Ceiling(OrderDetail_Fin.Count / (double)pageSize) - 1;
                        dataGridViewDsp_Agr.DataSource = OrderDetail_Fin.Skip(pageSize * pageNo).Take(pageSize).ToList();
                        dataGridViewDsp_Agr.Refresh();
                    }
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
                RefreshOrderDetailCombo();
                RefreshProductCombo(false);
                AllClear();
                label1.Enabled = false;
                cmbOrderDetailId.Enabled = false;
                cmbOrderDetailId.DropDownStyle = ComboBoxStyle.DropDownList;
                label13.Visible = false;  //受注詳細ID

                label8.Visible = false;
                numericUpDown.Visible = false; //数量

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
                int OrID = int.Parse(textBoxOrderId.Text);
                var orderData = order.GetOrderData(new T_OrderRead { OrID = OrID, OrFlag = -1, ClCharge = "", OrStateFlag = -1 }).Single();
                T_OrderDsp updatedOrderData;
                if (!GetValidDataAtConfirm(orderData))
                {
                    updatedOrderData = order.GetOrderData(new T_OrderRead { OrID = OrID, OrFlag = -1, ClCharge = "", OrStateFlag = -1 }).Single();
                    status = updatedOrderData.OrStateFlag;
                    textBoxState.Text = statusString[status];
                    if (status != 0)
                    {
                        radioBtnReg.Enabled = false;
                        radioBtnCon.Enabled = false;
                    }
                    GetAllData();
                    return;
                }
                var orderDetailData = order.GetOrderDetailData_Agr_NonFin(new T_OrderDetailDsp { OrID = OrID });
                if (!GetValidDetailDataAtConfirm(orderDetailData))
                {
                    GetAllData();
                    return;
                }
                var chumonData = GenerateDataAtConfirm(orderData);
                var chumonDetailData = GenerateDetailDataAtConfirm(orderDetailData);
                DialogResult dialog = MessageBox.Show(string.Format("受注ID：{0}を確定しますか。", chumonData.OrID), "確認", MessageBoxButtons.OKCancel);
                if (dialog != DialogResult.OK)
                {
                    return;
                }
                order.FinalizeOrderData(chumonData, chumonDetailData, orderDetailData);
                updatedOrderData = order.GetOrderData(new T_OrderRead { OrID = OrID, OrFlag = -1, ClCharge = "", OrStateFlag = -1 }).Single();
                status = updatedOrderData.OrStateFlag;
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
        private bool GetValidDataAtConfirm(T_OrderDsp orderData)
        {
            try
            {
                if (!order.CheckOrderIsInsertable((int)orderData.OrID))
                {
                    MessageBox.Show("本受注IDは確定済です。", "入力エラー");
                    return false;
                }
                if (orderData.KindOfProducts <= 0)
                {
                    MessageBox.Show("本受注に商品が含まれていません。", "入力エラー");
                    return false;
                }
                if (!client.CheckClientIsActive(orderData.ClID))
                {
                    MessageBox.Show("本受注に対応する顧客が削除済です。", "入力エラー");
                    return false;
                }
                if (!employee.CheckEmployeeIsActive(orderData.EmID))
                {
                    MessageBox.Show("本受注を登録した社員が削除済です。", "選択エラー");
                    return false;
                }
                if (!order.CheckOrderIsActive((int)orderData.OrID))
                {
                    DialogResult dialog = MessageBox.Show("受注確定時に本受注の削除は取り消されます。", "確認", MessageBoxButtons.OKCancel);
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
        private bool GetValidDetailDataAtConfirm(List<T_OrderDetailDsp_Agr_NonFin> orderDetailData)
        {
            try
            {
                foreach (var x in orderDetailData)
                {
                    if (!product.CheckProductIsActive(x.PrID))
                    {
                        MessageBox.Show(string.Format("商品ID：{0}は削除済です。", x.PrID), "選択エラー");
                        return false;
                    }
                    if (x.OrPriceSum > NumericRange.TotalPrice)
                    {
                        MessageBox.Show(string.Format("商品ID：{0}の金額が上限を超えます。", x.PrID), "選択エラー");
                        return false;
                    }
                    var temp = order.GetOrderDetailData(new T_OrderDetailDsp { OrID = x.OrID, PrID = x.PrID,OrDetailID=0 }).ToList();
                    foreach(var row2 in temp)
                    {
                        if(x.OrPriceUnit*row2.OrQuantity>NumericRange.TotalPrice|| x.OrPriceUnit * row2.OrQuantity < NumericRange.TotalPrice_minus)
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

        private void dataGridViewDsp_Agr_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                DataGridView dataGridview = (DataGridView)sender;
                if (e.ColumnIndex == 3 && (int)dataGridview.Rows[e.RowIndex].Cells[11].Value != 0)
                {
                    dataGridview.Rows[e.RowIndex].Cells[3].Style.BackColor = SystemColors.AppWorkspace;
                }
                if (status == 0 && e.ColumnIndex == 9 && decimal.Parse(dataGridview.Rows[e.RowIndex].Cells[9].Value.ToString()) > NumericRange.TotalPrice)
                {
                    dataGridview.Rows[e.RowIndex].Cells[9].Style.BackColor = SystemColors.AppWorkspace;
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
                if (e.ColumnIndex == 3 && (int)dataGridview.Rows[e.RowIndex].Cells[10].Value != 0)
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
