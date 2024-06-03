using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SalesManagement_SysDev
{
    public partial class F_LoginHistory : Form
    {
        LogDbConnection lohis = new LogDbConnection();
        EmployeeDbConnection employee = new EmployeeDbConnection();
        DataAccess access = new DataAccess();
        private List<T_LoginHistoryDsp> LoHis;
        private List<T_LoginHistoryDsp> subLoHis;
        private List<M_EmployeeDsp> Employee;
        private List<M_SalesOfficeDsp> SalesOffice;
        private List<M_PositionDsp> Position;
        private bool updFlg;
        private int pageSize = 12;
        public int LoginEmID;
        public bool loginUserInfoCheck = true;
        public F_LoginHistory()
        {
            InitializeComponent();
        }

        private void F_Employee_Load(object sender, EventArgs e)
        {
            if (!LockLoginUserInfo())
            {
                loginUserInfoCheck = false;
                return;
            }
            GetComboBoxData();
            GetAllData();
        }

        private bool LockLoginUserInfo()
        {
            try
            {
                if (!employee.CheckEmployeeExsistence(LoginEmID))
                {
                    MessageBox.Show("ログイン中の社員IDは削除済みです", "エラー");
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        //検索処理
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
            catch(Exception ex)
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
                int loID = 0;
                if (cmbLogHisId.Text != "")
                {
                    loID = int.Parse(cmbLogHisId.Text);
                }
                int emID = 0;
                if (cmbEmployeeId.SelectedIndex != -1)
                {
                    emID = (int)cmbEmployeeId.SelectedValue;
                }
                int soID = 0;
                if (cmbSalesOfficeId.SelectedIndex != -1)
                {
                    soID = (int)cmbSalesOfficeId.SelectedValue;
                }
                int poID = 0;
                if (cmbPosition.SelectedIndex != -1)
                {
                    poID = (int)cmbPosition.SelectedValue;
                }

                if (selectFlg == false)
                {
                    // 検索条件のセット
                    T_LoginHistoryDsp selectCondition = new T_LoginHistoryDsp()
                    {
                        LoHistoryID = loID,
                        SoID = soID,
                        PoID = poID,
                        EmID=emID
                    };
                    // データの抽出
                    LoHis = lohis.GetLoginHistoryData(selectCondition);
                    subLoHis = new List<T_LoginHistoryDsp>(LoHis);
                    subLoHis.Insert(0, new T_LoginHistoryDsp { LoHistoryID = null });
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
                    T_LoginHistoryDsp selectCondition = new T_LoginHistoryDsp()
                    {
                        LoHistoryID = loID,
                        SoID = soID,
                        PoID = poID,
                        EmID = emID
                    };
                    // データの抽出
                    LoHis = lohis.GetLoginHistoryData(selectCondition,sdate,edate);
                    subLoHis = new List<T_LoginHistoryDsp>(LoHis);
                    subLoHis.Insert(0, new T_LoginHistoryDsp { LoHistoryID = null });
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
                dataGridViewDsp.DataSource = LoHis;

                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = LoHis.Skip(pageSize * pageNo).Take(pageSize).ToList();

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(LoHis.Count / (double)pageSize)) + "ページ";

                RefreshLoHisCombo();

                dataGridViewDsp.Refresh();
            }
            catch
            {
                throw;
            }
        }

            //一覧表示
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


        //入力クリア
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                AllClear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void AllClear()
        {
            try
            {
                cmbLogHisId.SelectedIndex = -1;
                cmbSalesOfficeId.SelectedIndex = -1;
                cmbPosition.SelectedIndex = -1;
                cmbEmployeeId.SelectedIndex = -1;
                dateTimePickerS.Value = DateTime.Now;
                dateTimePickerE.Value = DateTime.Now;
                dateTimePickerS.Checked = false;
                dateTimePickerE.Checked = false;
            }
            catch
            {
                throw;
            }
        }


        //コンボボックスデータ取得
        private void GetComboBoxData()
        {
            try
            {
                //LoIDのコンボボックスData読込
                cmbLogHisId.DataSource = LoHis;
                cmbLogHisId.DisplayMember = "LoHistoryID";
                cmbLogHisId.ValueMember = "LoHistoryID";
                cmbLogHisId.SelectedIndex = -1;
                //役割のコンボボックスData読込
                Position = access.GetPositionDspData();
                Position.Insert(0, new M_PositionDsp { PoID = 0, PoName = "" });
                Position.Add(new M_PositionDsp { PoID = -1, PoName = "（非表示済）" });
                cmbPosition.DisplayMember = "PoName";
                cmbPosition.ValueMember = "PoID";
                cmbPosition.DataSource = Position;
                cmbPosition.SelectedIndex = -1;

                //営業所のコンボボックスData読込
                SalesOffice = access.GetSalesOfficeDspData();
                SalesOffice.Insert(0, new M_SalesOfficeDsp { SoID = 0, SoName = "" });
                SalesOffice.Add(new M_SalesOfficeDsp { SoID = -1, SoName = "（非表示済）" });
                cmbSalesOfficeId.DisplayMember = "SoName";
                cmbSalesOfficeId.ValueMember = "SoID";
                cmbSalesOfficeId.DataSource = SalesOffice;
                cmbSalesOfficeId.SelectedIndex = -1;
            }
            catch
            {
                throw;
            }
        }
        private void RefreshLoHisCombo()
        {
            try
            {
                
                int temp = 0;
                if (cmbLogHisId.SelectedValue != null)
                {
                    temp = int.Parse(cmbLogHisId.SelectedValue.ToString());
                }
                cmbLogHisId.DataSource = subLoHis;
                cmbLogHisId.SelectedIndex = subLoHis.FindIndex(x => x.EmID == temp);
                cmbLogHisId.DisplayMember = "LoHistoryID";
                cmbLogHisId.ValueMember = "LoHistoryID";
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
                LoHis = lohis.GetLoginHistoryData();
                subLoHis = new List<T_LoginHistoryDsp>(LoHis);
                subLoHis.Insert(0, new T_LoginHistoryDsp { LoHistoryID = null });

                if (LoHis == null)
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
                dataGridViewDsp.DataSource = LoHis;
                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = LoHis.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                ////履歴ID
                dataGridViewDsp.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[0].Width = 150;
                ////社員ID
                dataGridViewDsp.Columns[1].Visible = false;
                ////社員名
                dataGridViewDsp.Columns[2].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[2].Width = 170;
                ////営業所ID
                dataGridViewDsp.Columns[3].Visible = false;
                ////営業所名
                dataGridViewDsp.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[4].Width = 170;
                ////役割ID
                dataGridViewDsp.Columns[5].Visible = false;
                ////役職名
                dataGridViewDsp.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[6].Width = 120;
                ////ログイン年月日
                dataGridViewDsp.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[7].Width = 185;
                ////ログアウト年月日
                dataGridViewDsp.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[8].Width = 185;

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(LoHis.Count / (double)pageSize)) + "ページ";

                RefreshLoHisCombo();

                dataGridViewDsp.Refresh();
            }
            catch
            {
                throw;
            }
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridViewDsp.DataSource = LoHis.Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                textBoxPageNo.Text = "1";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            try
            {
                int pageNo = int.Parse(textBoxPageNo.Text) - 2;
                dataGridViewDsp.DataSource = LoHis.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                int lastNo = (int)Math.Ceiling(LoHis.Count / (double)pageSize) - 1;
                //最終ページでなければ
                if (pageNo <= lastNo)
                    dataGridViewDsp.DataSource = LoHis.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                int lastPage = (int)Math.Ceiling(LoHis.Count / (double)pageSize);
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
                int pageNo = (int)Math.Ceiling(LoHis.Count / (double)pageSize) - 1;
                dataGridViewDsp.DataSource = LoHis.Skip(pageSize * pageNo).Take(pageSize).ToList();

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

        private void dateTimePickerS_ValueChanged(object sender, EventArgs e)
        {
            if (!dateTimePickerS.Checked && !dateTimePickerE.Checked)
            {
                label9.Enabled = false;
            }
            else
            {
                label9.Enabled = true;
            }
        }

        private void dateTimePickerE_ValueChanged(object sender, EventArgs e)
        {
            if(!dateTimePickerS.Checked && !dateTimePickerE.Checked)
            {
                label9.Enabled = false;
            }
            else
            {
                label9.Enabled = true;
            }
        }

        private void cmbSalesOfficeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int soID = 0;
                if (cmbSalesOfficeId.SelectedIndex != -1)
                {
                    soID = int.Parse(cmbSalesOfficeId.SelectedValue.ToString());
                }
                int poID = 0;
                if (cmbPosition.SelectedIndex != -1)
                {
                    poID = int.Parse(cmbPosition.SelectedValue.ToString());
                }
                Employee = employee.GetEmployeeData(new M_EmployeeDsp { EmID=0,EmPhone="",EmName="",EmFlag=0,SoID=soID,PoID=poID});
                RefreshEmployeeCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int soID = 0;
                if (cmbSalesOfficeId.SelectedIndex != -1)
                {
                    soID = int.Parse(cmbSalesOfficeId.SelectedValue.ToString());
                }
                int poID = 0;
                if (cmbPosition.SelectedIndex != -1)
                {
                    poID = int.Parse(cmbPosition.SelectedValue.ToString());
                }
                Employee = employee.GetEmployeeData(new M_EmployeeDsp { EmID = 0, EmPhone = "", EmName = "", EmFlag = 0 ,SoID=soID,PoID=poID});
                RefreshEmployeeCombo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
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
                Employee.Insert(0, new M_EmployeeDsp { EmID = 0, EmName = "" });
                Employee.Add(new M_EmployeeDsp { EmID = -1, EmName = "削除済み" });
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

        private void cmbEmployeeId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (cmbEmployeeId.SelectedIndex == -1 || cmbEmployeeId.SelectedIndex == 0 || cmbEmployeeId.SelectedIndex == Employee.Count - 1)
                {
                    return;
                }
                var selectedEmployee = Employee.Single(x => x.EmID == int.Parse(cmbEmployeeId.SelectedValue.ToString()));
                int officeIndex = SalesOffice.FindIndex(x => x.SoID == selectedEmployee.SoID);
                int positionIndex = Position.FindIndex(x => x.PoID == selectedEmployee.PoID);
                if (officeIndex == -1)
                {
                    cmbSalesOfficeId.SelectedIndex = SalesOffice.Count - 1;
                }
                else
                {
                    cmbSalesOfficeId.SelectedIndex = officeIndex;
                }
                if (positionIndex == -1)
                {
                    cmbPosition.SelectedIndex = Position.Count - 1;
                }
                else
                {
                    cmbPosition.SelectedIndex = positionIndex;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
