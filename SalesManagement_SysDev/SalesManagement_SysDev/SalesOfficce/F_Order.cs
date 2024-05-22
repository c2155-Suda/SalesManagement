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
    public partial class F_Order : Form
    {
        OrderDbConnection order = new OrderDbConnection();
        EmployeeDbConnection employee = new EmployeeDbConnection();
        ClientDbConnection client = new ClientDbConnection();
        ProductDbConnect product = new ProductDbConnect();
        F_OrderDetail detail = new F_OrderDetail();
        DataAccess access = new DataAccess();
        private List<T_OrderDsp> Order;
        private List<M_EmployeeDsp> Employee;
        private List<M_ClientDsp> Client;
        private List<M_ProductDsp> Product;
        private bool updFlg;
        private int pageSize = 12;
        private bool trimview;
        
        public F_Order()
        {
            InitializeComponent();
        }

        private void F_Order_Load(object sender, EventArgs e)
        {
            Order = order.GetOrderData();
            Employee = employee.GetEmployeeData();
            Client = client.GetClientData();
            Product = product.GetProductData();
            GetAllData();
            radioBtnSea.Checked = true;
            trimview = false;
        }

        private void btnCon_Click(object sender, EventArgs e)
        {

        }

        private void btnReg_Click(object sender, EventArgs e)
        {

        }

        private void btnDetail_Click(object sender, EventArgs e)
        {

            detail.ShowDialog();
        }

        private void btnSea_Click(object sender, EventArgs e)
        {

        }

        private void btnHidden_Click(object sender, EventArgs e)
        {

        }
        private void btnView_Click(object sender, EventArgs e)
        {
            AllClear();
            GetAllData();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            AllClear();
            GetAllData();
        }

        private void btnTrim_Click(object sender, EventArgs e)
        {
            if(trimview == false)
            {
                dataGridViewDsp.Visible = false;
                dataGridViewTrimDsp.Visible = true;
                trimview = true;
            }
            else if(trimview == true)
            {
                dataGridViewDsp.Visible = true;
                dataGridViewTrimDsp.Visible = false;
                trimview = false;
            }
        }

        //コンボボックスデータ取得
        private void GetComboBoxData()
        {
            //商品IDのコンボボックスData読込
            var listOrder = Order;
            cmbOrderId.DataSource = listOrder;
            cmbOrderId.DisplayMember = "OrID";
            cmbOrderId.ValueMember = "OrID";
            cmbOrderId.SelectedIndex = -1;

            //営業所のコンボボックスData読込
            var listSalesOffice = access.GetSalesOfficeDspData();
            cmbSalesOfficeId.DataSource = listSalesOffice;
            cmbSalesOfficeId.DisplayMember = "SoName";
            cmbSalesOfficeId.ValueMember = "SoID";
            cmbSalesOfficeId.SelectedIndex = -1;

            //社員IDのコンボボックスData読込
            var listEmployee = Employee;
            cmbEmployeeId.DataSource = listEmployee;
            cmbEmployeeId.DisplayMember = "EmName";
            cmbEmployeeId.ValueMember = "EmID";
            cmbEmployeeId.SelectedIndex = -1;

            //顧客IDのコンボボックスData読込
            var listClient = Client;
            cmbClientId.DataSource = listClient;
            cmbClientId.DisplayMember = "ClName";
            cmbClientId.ValueMember = "ClID";
            cmbClientId.SelectedIndex = -1;

            //商品IDのコンボボックスData読込
            var listProduct = Product;
            cmbProductId.DataSource = listProduct;
            cmbProductId.DisplayMember = "PrName";
            cmbProductId.ValueMember = "PrID";
            cmbProductId.SelectedIndex = -1;

            //大分類のコンボボックスData読込
            var listMajor = access.GetMajorClassificationDspData();
            cmbMajor.DataSource = listMajor;
            cmbMajor.DisplayMember = "McName";
            cmbMajor.ValueMember = "McID";
            cmbMajor.SelectedIndex = -1;

            //ステータスのコンボボックスData読込
            cmbState.Items.AddRange(new object[] { "すべて", "(未確定)", "受注済", "注文済", "出庫済", "入荷済", "出荷済" });
            cmbState.SelectedIndex = -1;

        }

        private void AllClear()
        {
            textBoxCharge.Text = "";
            cmbOrderId.SelectedIndex = -1;
            cmbClientId.SelectedIndex = -1;
            cmbEmployeeId.SelectedIndex = -1;
            cmbSalesOfficeId.SelectedIndex = -1;
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

        //データグリッドビュー表示用
        private bool GetAllData()
        {
            //全件取得
            Order = order.GetOrderData();
            if (Order == null)
                return false;
            //データグリッドビューへの設定
            SetDataGridView();
            return true;
        }

        private void SetDataGridView()
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
            dataGridViewDsp.Columns[1].Width = 130;
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
            dataGridViewDsp.Columns[4].Width = 130;
            dataGridViewDsp.Columns[4].Visible = false;
            ////営業所名
            dataGridViewDsp.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewDsp.Columns[5].Width = 190;
            ////社員ID
            dataGridViewDsp.Columns[6].Width = 130;
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
            dataGridViewDsp.Columns[9].Width = 130;
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
            dataGridViewDsp.Columns[13].Width = 130;
            dataGridViewDsp.Columns[13].Visible = false;
            ////ステータス
            dataGridViewDsp.Columns[14].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[14].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewDsp.Columns[14].Width = 200;
            
            //dataGridViewの総ページ数
            lblPage.Text = "/" + ((int)Math.Ceiling(Order.Count / (double)pageSize)) + "ページ";

            GetComboBoxData();

            dataGridViewDsp.Refresh();
        }

        private void radioBtnCon_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnCon.Checked == false)
            {
                return;
            }
            dateTimePickerS.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = false;
            label4.Enabled = false;
            label5.Enabled = false;
            label6.Enabled = false;
            label12.Enabled = false;
            textBoxCharge.Enabled = false;
            checkBoxFlag.Enabled = false;
            cmbClientId.Enabled = false;
            cmbEmployeeId.Enabled = false;
            cmbSalesOfficeId.Enabled = false; 
            cmbState.Enabled = false;
            btnCon.Enabled = true;
            btnReg.Enabled = false;
            btnDetail.Enabled = false;
            btnSea.Enabled = false;
            btnHidden.Enabled = false;

            dateTimePickerE.Visible = false;
            label7.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;            
            label13.Visible = false;            
            cmbProductId.Visible = false;
            cmbMajor.Visible = false;
            cmbSmall.Visible = false;

            updFlg = true;
        }

        private void radioBtnReg_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnReg.Checked == false)
            {
                return;
            }
            dateTimePickerS.Enabled = true;
            label1.Enabled = true;
            label2.Enabled = true;
            label3.Enabled = true;
            label4.Enabled = true;
            label5.Enabled = true;
            label6.Enabled = true;
            label12.Enabled = false;

            dateTimePickerE.Visible = false;
            label7.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label13.Visible = true;

            textBoxCharge.Enabled = true;
            checkBoxFlag.Enabled = true;
            cmbClientId.Enabled = true;
            cmbEmployeeId.Enabled = true;
            cmbSalesOfficeId.Enabled = true;
            cmbState.Enabled = false;

            cmbProductId.Visible = false;
            cmbMajor.Visible = false;
            cmbSmall.Visible = false;

            btnCon.Enabled = false;
            btnReg.Enabled = true;
            btnDetail.Enabled = false;
            btnSea.Enabled = false;
            btnHidden.Enabled = false;

            updFlg = false;
        }

        private void radioBtnDetail_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnDetail.Checked == false)
            {
                return;
            }
            dateTimePickerS.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = false;
            label4.Enabled = false;
            label5.Enabled = false;
            label6.Enabled = false;
            label12.Enabled = false;
            textBoxCharge.Enabled = false;
            checkBoxFlag.Enabled = false;
            cmbClientId.Enabled = false;
            cmbEmployeeId.Enabled = false;
            cmbSalesOfficeId.Enabled = false;
            cmbState.Enabled = false;
            btnCon.Enabled = false;
            btnReg.Enabled = false;
            btnDetail.Enabled = true;
            btnSea.Enabled = false;
            btnHidden.Enabled = false;

            dateTimePickerE.Visible = false;
            label7.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label13.Visible = false;
            cmbProductId.Visible = false;
            cmbMajor.Visible = false;
            cmbSmall.Visible = false;

            updFlg = true;
        }

        private void radioBtnSea_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnSea.Checked == false)
            {
                return;
            }
            dateTimePickerS.Enabled = true;
            label1.Enabled = true;
            label2.Enabled = true;
            label3.Enabled = true;
            label4.Enabled = true;
            label5.Enabled = true;
            label6.Enabled = true;
            label12.Enabled = true;
            textBoxCharge.Enabled = true;
            checkBoxFlag.Enabled = true;
            cmbClientId.Enabled = true;
            cmbEmployeeId.Enabled = true;
            cmbSalesOfficeId.Enabled = true;
            cmbState.Enabled = true;
            btnCon.Enabled = false;
            btnReg.Enabled = false;
            btnDetail.Enabled = false;
            btnSea.Enabled = true;
            btnHidden.Enabled = false;

            dateTimePickerE.Visible = true;
            label7.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label11.Visible = true;
            label13.Visible = false;
            cmbProductId.Visible = true;
            cmbMajor.Visible = true;
            cmbSmall.Visible = true;

            updFlg = false;
        }

        private void radioBtnHidden_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnHidden.Checked == false)
            {
                return;
            }
            dateTimePickerS.Enabled = false;
            label1.Enabled = false;
            label2.Enabled = false;
            label3.Enabled = false;
            label4.Enabled = false;
            label5.Enabled = false;
            label6.Enabled = true;
            label12.Enabled = false;
            textBoxCharge.Enabled = false;
            checkBoxFlag.Enabled = true;
            cmbClientId.Enabled = false;
            cmbEmployeeId.Enabled = false;
            cmbSalesOfficeId.Enabled = false;
            cmbState.Enabled = false;
            btnCon.Enabled = false;
            btnReg.Enabled = false;
            btnDetail.Enabled = false;
            btnSea.Enabled = false;
            btnHidden.Enabled = true;

            dateTimePickerE.Visible = false;
            label7.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label13.Visible = false;
            cmbProductId.Visible = false;
            cmbMajor.Visible = false;
            cmbSmall.Visible = false;

            updFlg = true;
        }


        private void cmbMajor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var listSmall = access.GetSmallClassificationDspData();

            if (cmbMajor.SelectedIndex == -1)
            {
                label9.Enabled = false;
                cmbSmall.Enabled = false;
            }
            else if (cmbMajor.SelectedIndex != -1)
            {
                if (int.TryParse(cmbMajor.SelectedValue.ToString(), out int mcID))
                {
                    listSmall = access.GetSmallClassificationDspData(mcID);
                }
                label9.Enabled = true;
                cmbSmall.Enabled = true;
                //小分類のコンボボックスData読込
                cmbSmall.DataSource = listSmall;
                cmbSmall.DisplayMember = "ScName";
                cmbSmall.ValueMember = "ScID";
                cmbSmall.SelectedIndex = -1;
            }
        }

        private void checkBoxFlag_CheckedChanged(object sender, EventArgs e)
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

        private void dataGridViewDsp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (updFlg == true)
            {
                //データグリッドビューからクリックされたデータを各入力エリアへ
                cmbOrderId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
                cmbClientId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[2].Value.ToString();
                textBoxCharge.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[3].Value.ToString();
                cmbSalesOfficeId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[5].Value.ToString();
                cmbEmployeeId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[7].Value.ToString();
                dateTimePickerS.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[8].Value.ToString();
                checkBoxFlag.Checked = (bool)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[10].Value;
                //null処理を空欄処理
                if(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value == null)
                {
                    textBoxHideRea.Text = "";
                }
                else
                    textBoxHideRea.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value.ToString();

                if(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[14].Value.ToString() == string.Empty)
                {
                    cmbState.Text = null;
                }
                else
                    cmbState.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[14].Value.ToString();
            }
        }

        private void dataGridViewTrimDsp_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }    
}
