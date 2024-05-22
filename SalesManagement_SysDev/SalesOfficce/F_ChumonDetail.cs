using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    public partial class F_ChumonDetail : Form
    {
        public int chID;
        public int orID;
        public int status;
        public int orFlag;
        public int logEmID;
        private string[] statusString = { "未確定", "確定済" };
        DataAccess access = new DataAccess();
        ProductDbConnect product = new ProductDbConnect();
        OrderDbConnection order = new OrderDbConnection();
        ChumonDbConnection chumon = new ChumonDbConnection();
        private List<T_ChumonDetailDsp> ChumonDetail;
        private List<T_ChumonDetailDsp> subChumonDetail;
        private List<M_ProductDsp> Product;
        private List<M_MajorClassificationDsp> majorClass;
        private List<M_SmallClassificationDsp> smallClass;
        private int pageSize = 8;
        public F_ChumonDetail()
        {
            InitializeComponent();
        }

        private void F_ChumonDetail_Load(object sender, EventArgs e)
        {
            textBoxChumonId.Text = chID.ToString();
            textBoxState.Text = statusString[status];
            textBoxOrderId.Text = orID.ToString();
            if (orFlag != 0)
            {
                radioBtnCon.Enabled = false;
                textBoxOrderId.BackColor = SystemColors.AppWorkspace;

            }
            else if (status != 0)
            {
                radioBtnCon.Enabled = false;
            }

            Product = product.GetProductData();
            majorClass = access.GetMajorClassificationDspData();
            GetComboBoxData();
            GetAllData();
            radioBtnSea.Checked = true;

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void GetComboBoxData()
        {
            try
            {
                cmbChumonDetailId.DisplayMember = "ChDetailID";
                cmbChumonDetailId.ValueMember = "ChDetailID";
                cmbChumonDetailId.DataSource = ChumonDetail;
                cmbChumonDetailId.SelectedIndex = -1;

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
        private void RefreshChumonDetailCombo()
        {
            try
            {
                int temp = 0;
                if (cmbChumonDetailId.SelectedValue != null)
                {
                    temp = int.Parse(cmbChumonDetailId.SelectedValue.ToString());
                }
                cmbChumonDetailId.DataSource = subChumonDetail;
                cmbChumonDetailId.SelectedIndex = subChumonDetail.FindIndex(x => x.ChDetailID == temp);
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
                int OrID = int.Parse(textBoxOrderId.Text);
                int ChID = int.Parse(textBoxChumonId.Text);
                var chumonData = chumon.GetChumonData(new T_ChumonRead { ChID = ChID, ChFlag = -1, ChStateFlag = -1 }).Single();
                T_ChumonDsp updatedChumonData;
                if (!GetValidDataAtConfirm(chumonData))
                {
                    updatedChumonData = chumon.GetChumonData(new T_ChumonRead { ChID = ChID, ChFlag = -1, ChStateFlag = -1 }).Single();
                    radioBtnCon.Enabled = false;
                    return;
                }
                var chumonDetailData = chumon.GetChumonDetailData(new T_ChumonDetailDsp { ChID = chID,ChDetailID=0 });
                if (!GetValidDetailDataAtConfirm(chumonDetailData))
                {
                    GetAllData();
                    return;
                }
                var syukkoData = GenerateDataAtConfirm(chumonData);
                var syukkoDetailData = GenerateDetailDataAtConfirm(chumonDetailData);
                DialogResult dialog = MessageBox.Show(string.Format("注文ID：{0}を確定しますか。", ChID), "確認", MessageBoxButtons.OKCancel);
                if (dialog != DialogResult.OK)
                {
                    return;
                }
                chumon.FinalizeChumonData(syukkoData, syukkoDetailData, logEmID);
                updatedChumonData = chumon.GetChumonData(new T_ChumonRead { ChID = ChID, ChFlag = -1, ChStateFlag = -1 }).Single();
                status = updatedChumonData.ChStateFlag;
                textBoxState.Text = statusString[status];
                if (status != 0)
                {
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
        private bool GetValidDataAtConfirm(T_ChumonDsp chumonData)
        {
            try
            {
                if (!chumon.CheckChumonIsInsertable((int)chumonData.ChID))
                {
                    MessageBox.Show("本注文IDは確定済です。", "入力エラー");
                    status = 1;
                    textBoxState.Text = statusString[status];
                    return false;
                }
                if (!order.CheckOrderIsActive(chumonData.OrID))
                {
                    MessageBox.Show("本注文に対応する受注が削除済です。");
                    orFlag = 2;
                    textBoxOrderId.BackColor = SystemColors.AppWorkspace;

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
                int chdetailID = 0;
                if (cmbChumonDetailId.SelectedIndex != -1)
                {
                    chdetailID = (int)cmbChumonDetailId.SelectedValue;
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
                T_ChumonDetailDsp selectCondition = new T_ChumonDetailDsp()
                {
                    ChID = chID,
                    ChDetailID = chdetailID,
                    PrID = prID,
                    McID = mcID,
                    ScID = scID,
                };
                // データの抽出
                ChumonDetail = chumon.GetChumonDetailData(selectCondition);
                subChumonDetail = new List<T_ChumonDetailDsp>(ChumonDetail);
                subChumonDetail.Insert(0, new T_ChumonDetailDsp { ChDetailID = null });
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
                dataGridViewDsp.DataSource = ChumonDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
                int dataCount=ChumonDetail.Count;
                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(dataCount / (double)pageSize)) + "ページ";
                dataGridViewDsp.Refresh();
                RefreshChumonDetailCombo();
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
                if (ChumonDetail == null)
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
                //textsize
                this.dataGridViewDsp.DefaultCellStyle.Font = new Font("ＭＳ Ｐゴシック", 18);
                this.dataGridViewDsp.ColumnHeadersDefaultCellStyle.Font = new Font("ＭＳ Ｐゴシック", 18);
                //ヘッダーの高さ
                dataGridViewDsp.ColumnHeadersHeight = 60;
                //ヘッダーの折り返し表示
                dataGridViewDsp.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dataGridViewDsp.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //行単位選択
                dataGridViewDsp.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //複数指定無効処理
                dataGridViewDsp.MultiSelect = false;

                //ヘッダー文字位置、セル文字位置、列幅の設定
                ////注文詳細ID
                dataGridViewDsp.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[0].Width = 105;
                ////注文ID
                dataGridViewDsp.Columns[1].Visible = false;
                ////商品ID
                dataGridViewDsp.Columns[2].Visible = false;
                ////商品名
                dataGridViewDsp.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[3].Width = 200;
                ////大分類ID
                dataGridViewDsp.Columns[4].Visible = false;
                ////大分類
                dataGridViewDsp.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[5].Width = 260;
                ////小分類ID
                dataGridViewDsp.Columns[6].Visible = false;
                ////小分類
                dataGridViewDsp.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[7].Width = 190;
                ////数量
                dataGridViewDsp.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[8].Width = 70;

                dataGridViewDsp.Refresh();
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
                if (!radioBtnCon.Checked)
                {
                    return;
                }
                AllClear();
                label1.Enabled = false;
                cmbChumonDetailId.Enabled = false; //注文詳細ID

                label11.Enabled = false;
                label3.Enabled = false;
                label9.Enabled = false;
                cmbProductId.Enabled = false;
                cmbSmall.Enabled = false;
                cmbMajor.Enabled = false;

                btnSea.Enabled = false;
                btnCon.Enabled = true;
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
                AllClear();
                label1.Enabled = true;
                cmbChumonDetailId.Enabled = true;  //受注詳細ID

                label11.Enabled = true;
                label3.Enabled = true;
                label9.Enabled = true;
                cmbProductId.Enabled = true;
                cmbSmall.Enabled = true;
                cmbMajor.Enabled = true;

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
        private void AllClear()
        {
            try
            {
                cmbChumonDetailId.SelectedIndex = -1;
                cmbMajor.SelectedIndex = -1;
                cmbSmall.SelectedIndex = -1;
                cmbProductId.SelectedIndex = -1;
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
                RefreshProductCombo();
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
                dataGridViewDsp.DataSource = ChumonDetail.Take(pageSize).ToList();
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
                dataGridViewDsp.DataSource = ChumonDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
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
                int lastNo = (int)Math.Ceiling(ChumonDetail.Count / (double)pageSize);
                if (pageNo < lastNo)
                {
                    dataGridViewDsp.DataSource = ChumonDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
                    dataGridViewDsp.Refresh();
                    textBoxPageNo.Text = (pageNo + 1).ToString();
                }
                else
                {
                    textBoxPageNo.Text = lastNo.ToString();
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
                int pageNo = (int)Math.Ceiling(ChumonDetail.Count / (double)pageSize) - 1;
                dataGridViewDsp.DataSource = ChumonDetail.Skip(pageSize * pageNo).Take(pageSize).ToList();
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                textBoxPageNo.Text = (pageNo + 1).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
    }
}
