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
    public partial class F_Employee : Form
    {
        EmployeeDbConnection employee = new EmployeeDbConnection();
        DataAccess access = new DataAccess();
        InputFormCheck input = new InputFormCheck();
        private List<M_EmployeeDsp> Employee;
        private List<M_EmployeeDsp> subEmployee;
        private List<M_SalesOfficeDsp> SalesOffice;
        private List<M_PositionDsp> Position;
        private bool updFlg;
        private int pageSize = 12;
        public int LoginEmID;
        public bool loginUserInfoCheck = true;
        public F_Employee()
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
            radioBtnSea.Checked = true;
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

        //登録処理
        private void btnReg_Click(object sender, EventArgs e)
        {
            try
            {
                //入力チェック
                if (!GetValidDataAtRegistration())
                    return;
                //登録情報作成
                var regEmployee = GenerateDataAtRegistration();
                //登録処理
                employee.RegistEmployeeData(regEmployee);
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
                if (String.IsNullOrEmpty(cmbEmployeeId.Text.Trim()))
                {
                    MessageBox.Show("社員IDが未入力です", "入力エラー");
                    cmbEmployeeId.Focus();
                    return false;
                }
                if (int.TryParse(cmbEmployeeId.Text.Trim(), out var regEmployeeId))
                {
                    if (!(input.CheckIdFormat(regEmployeeId)))
                    {
                        MessageBox.Show("社員IDは6桁以下です", "入力エラー");
                        cmbEmployeeId.Focus();
                        return false;
                    }
                    //同じID
                    try
                    {
                        if (employee.CheckEmployeeExsistence(regEmployeeId))
                        {
                            MessageBox.Show("入力された社員IDは既に存在します", "入力エラー");
                            cmbEmployeeId.Focus();
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "DBエラー");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("社員IDは数字のみです", "入力エラー");
                    cmbEmployeeId.Focus();
                    return false;
                }

                if (String.IsNullOrEmpty(textBoxEmployeeName.Text.Trim()))
                {
                    MessageBox.Show("社員名が未入力です", "入力エラー");
                    textBoxEmployeeName.Focus();
                    return false;
                }
                if (cmbPosition.SelectedIndex == -1)
                {
                    MessageBox.Show("役職名が未選択です", "選択エラー");
                    cmbPosition.Focus();
                    return false;
                }
                if (cmbSalesOfficeId.SelectedIndex == -1)
                {
                    MessageBox.Show("営業所名が未選択です", "選択エラー");
                    cmbSalesOfficeId.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(textBoxPassword.Text.Trim()))
                {
                    MessageBox.Show("パスワードが未入力です", "入力エラー");
                    textBoxPassword.Focus();
                    return false;
                }
                if (!(input.CheckPasswordFormat(textBoxPassword.Text.Trim())))
                {
                    MessageBox.Show("パスワードは10文字以下の半角英数字です", "入力エラー");
                    textBoxPassword.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(textBoxTelNumber.Text.Trim()))
                {
                    MessageBox.Show("電話番号が未入力です", "入力エラー");
                    textBoxTelNumber.Focus();
                    return false;
                }
                if (!(input.CheckTelFaxFormat(textBoxTelNumber.Text.Trim())))
                {
                    MessageBox.Show("電話番号は13文字以下の数字のみです", "入力エラー");
                    textBoxTelNumber.Focus();
                    return false;
                }
                if (dateTimePickerS.Checked == false)
                {
                    MessageBox.Show("入社年月日が未選択です", "選択エラー");
                    dateTimePickerS.Focus();
                    return false;
                }

                return true;
            }
            catch
            {
                throw;
            }
        }
        private M_Employee GenerateDataAtRegistration()
        {
            try
            {
                DateTime mHiredata = DateTime.Parse(dateTimePickerS.Text);
                int iFlg;
                if (checkBoxFlag.Checked == false)
                {
                    iFlg = 0;
                }
                else
                    iFlg = 2;

                //登録データセット
                return new M_Employee
                {
                    EmID = int.Parse(cmbEmployeeId.Text.Trim()),
                    EmName = textBoxEmployeeName.Text.Trim(),
                    SoID = (int)cmbSalesOfficeId.SelectedValue,
                    PoID = (int)cmbPosition.SelectedValue,
                    EmHiredate = mHiredata,
                    EmPassword = textBoxPassword.Text.Trim(),
                    EmPhone = textBoxTelNumber.Text.Trim(),
                    EmFlag = iFlg,
                    EmHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim()
                };
            }
            catch
            {
                throw;
            }
        }


        //更新処理
        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                //入力チェック
                if (!GetValidDataAtUpdate())
                    return;
                //更新情報作成
                var updEmployee = GenerateDataAtUpdate();
                //更新処理
                employee.UpdateEmployeeData(updEmployee);
                GetAllData();
                AllClear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private bool GetValidDataAtUpdate()
        {
            try
            {
                if (cmbEmployeeId.SelectedIndex == -1)
                {
                    MessageBox.Show("社員IDが未選択です", "選択エラー");
                    cmbEmployeeId.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(textBoxEmployeeName.Text.Trim()))
                {
                    MessageBox.Show("社員名が未入力です", "入力エラー");
                    textBoxEmployeeName.Focus();
                    return false;
                }
                if (cmbPosition.SelectedIndex == -1)
                {
                    MessageBox.Show("役職名が未選択です", "選択エラー");
                    cmbPosition.Focus();
                    return false;
                }
                if (cmbSalesOfficeId.SelectedIndex == -1)
                {
                    MessageBox.Show("営業所名が未選択です", "選択エラー");
                    cmbSalesOfficeId.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(textBoxPassword.Text.Trim()))
                {
                    MessageBox.Show("パスワードが未入力です", "入力エラー");
                    textBoxPassword.Focus();
                    return false;
                }
                if (!(input.CheckPasswordFormat(textBoxPassword.Text.Trim())))
                {
                    MessageBox.Show("パスワードは10文字以下の半角英数字です", "入力エラー");
                    textBoxPassword.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(textBoxTelNumber.Text.Trim()))
                {
                    MessageBox.Show("電話番号が未入力です", "入力エラー");
                    textBoxTelNumber.Focus();
                    return false;
                }
                if (!(input.CheckTelFaxFormat(textBoxTelNumber.Text.Trim())))
                {
                    MessageBox.Show("電話番号は13文字以下の数字のみです", "入力エラー");
                    textBoxTelNumber.Focus();
                    return false;
                }
                if (dateTimePickerS.Checked == false)
                {
                    MessageBox.Show("入社年月日が未選択です", "選択エラー");
                    dateTimePickerS.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        private M_Employee GenerateDataAtUpdate()
        {
            try
            {
                DateTime mHiredata = DateTime.Parse(dateTimePickerS.Text);

                int iFlg;
                if (checkBoxFlag.Checked == false)
                {
                    iFlg = 0;
                }
                else
                    iFlg = 2;


                // 更新データセット
                return new M_Employee
                {
                    EmID = int.Parse(cmbEmployeeId.Text.Trim()),
                    EmName = textBoxEmployeeName.Text.Trim(),
                    SoID = (int)cmbSalesOfficeId.SelectedValue,
                    PoID = (int)cmbPosition.SelectedValue,
                    EmHiredate = mHiredata,
                    EmPassword = textBoxPassword.Text.Trim(),
                    EmPhone = textBoxTelNumber.Text.Trim(),
                    EmFlag = iFlg,
                    EmHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim()

                };
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
                int emID = 0;
                if (cmbEmployeeId.Text != "")
                {
                    emID = int.Parse(cmbEmployeeId.Text);
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

                int iFlg;
                if (checkBoxFlag.Checked == false)
                {
                    iFlg = 0;
                }
                else
                    iFlg = 2;


                if (selectFlg == false)
                {
                    // 検索条件のセット
                    M_EmployeeDsp selectCondition = new M_EmployeeDsp()
                    {
                        EmID = emID,
                        EmName = textBoxEmployeeName.Text.Trim(),
                        SoID = soID,
                        PoID = poID,
                        EmPhone = textBoxTelNumber.Text.Trim(),
                        EmFlag = iFlg,
                        EmHidden = textBoxHideRea.Text.Trim()
                    };
                    // データの抽出
                    Employee = employee.GetEmployeeData(selectCondition);
                    subEmployee = new List<M_EmployeeDsp>(Employee);
                    subEmployee.Insert(0, new M_EmployeeDsp { EmID = null });
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
                    M_EmployeeDsp selectCondition = new M_EmployeeDsp()
                    {
                        EmID = emID,
                        EmName = textBoxEmployeeName.Text.Trim(),
                        SoID = soID,
                        PoID = poID,
                        EmPhone = textBoxTelNumber.Text.Trim(),
                        EmFlag = iFlg,
                        EmHidden = textBoxHideRea.Text.Trim()
                    };

                    // データの抽出
                    Employee = employee.GetEmployeeData(selectCondition, sdate, edate);
                    subEmployee = new List<M_EmployeeDsp>(Employee);
                    subEmployee.Insert(0, new M_EmployeeDsp { EmID = null });
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
                dataGridViewDsp.DataSource = Employee;

                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Employee.Skip(pageSize * pageNo).Take(pageSize).ToList();

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Employee.Count / (double)pageSize)) + "ページ";

                RefreshEmployeeCombo();

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
                cmbEmployeeId.SelectedIndex = -1;
                cmbEmployeeId.Text = "";
                textBoxEmployeeName.Text = "";
                cmbSalesOfficeId.SelectedIndex = -1;
                cmbPosition.SelectedIndex = -1;
                textBoxPassword.Text = "";
                textBoxTelNumber.Text = "";
                textBoxHideRea.Text = "";
                dateTimePickerS.Value = DateTime.Now;
                dateTimePickerE.Value = DateTime.Now;
                dateTimePickerS.Checked = false;
                dateTimePickerE.Checked = false;
                checkBoxFlag.Checked = false;
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
                //社員IDのコンボボックスData読込
                cmbEmployeeId.DataSource = Employee;
                cmbEmployeeId.DisplayMember = "EmID";
                cmbEmployeeId.ValueMember = "EmID";
                cmbEmployeeId.SelectedIndex = -1;
                //役割のコンボボックスData読込
                Position = access.GetPositionDspData();
                cmbPosition.DataSource = Position;
                cmbPosition.DisplayMember = "PoName";
                cmbPosition.ValueMember = "PoID";
                cmbPosition.SelectedIndex = -1;

                //営業所のコンボボックスData読込
                SalesOffice = access.GetSalesOfficeDspData();
                cmbSalesOfficeId.DataSource = SalesOffice;
                cmbSalesOfficeId.DisplayMember = "SoName";
                cmbSalesOfficeId.ValueMember = "SoID";
                cmbSalesOfficeId.SelectedIndex = -1;
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
                if (radioBtnReg.Checked)
                {
                    return;
                }
                if (cmbEmployeeId.SelectedValue != null)
                {
                    temp = int.Parse(cmbEmployeeId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    cmbEmployeeId.DataSource = subEmployee;
                    cmbEmployeeId.SelectedIndex = subEmployee.FindIndex(x => x.EmID == temp);
                }
                else
                {
                    cmbEmployeeId.DataSource = Employee;
                    cmbEmployeeId.SelectedIndex = Employee.FindIndex(x => x.EmID == temp);
                }
                cmbEmployeeId.DisplayMember = "EmID";
                cmbEmployeeId.ValueMember = "EmID";
            }
            catch
            {
                throw;
            }
        }
        private void RefreshSalesOfficeCombo()
        {
            try
            {
                if (radioBtnSea.Checked)
                {
                    SalesOffice.Insert(0, new M_SalesOfficeDsp { SoID = 0, SoName = "" });
                    SalesOffice.Add(new M_SalesOfficeDsp { SoID = -1, SoName = "（非表示済）" });
                    cmbSalesOfficeId.DataSource = null;
                    cmbSalesOfficeId.DataSource = SalesOffice;
                    cmbSalesOfficeId.DisplayMember = "SoName";
                    cmbSalesOfficeId.ValueMember = "SoID";
                }
                else
                {
                    SalesOffice.RemoveAll(x => x.SoID == -1 || x.SoID == 0);
                    cmbSalesOfficeId.DataSource = null;
                    cmbSalesOfficeId.DataSource = SalesOffice;
                    cmbSalesOfficeId.DisplayMember = "SoName";
                    cmbSalesOfficeId.ValueMember = "SoID";
                }
            }
            catch
            {
                throw;
            }
        }
        private void RefreshPositionCombo()
        {
            try
            {
                if (radioBtnSea.Checked)
                {
                    Position.Insert(0, new M_PositionDsp { PoID = 0, PoName = "" });
                    Position.Add(new M_PositionDsp { PoID = -1, PoName = "（非表示済）" });
                    cmbPosition.DataSource = null;
                    cmbPosition.DataSource = Position;
                    cmbPosition.DisplayMember = "PoName";
                    cmbPosition.ValueMember = "PoID";
                }
                else
                {
                    Position.RemoveAll(x => x.PoID == -1 || x.PoID == 0);
                    cmbPosition.DataSource = null;
                    cmbPosition.DataSource = Position;
                    cmbPosition.DisplayMember = "PoName";
                    cmbPosition.ValueMember = "PoID";
                }
            }
            catch
            {
                throw;
            }
        }


        //入力範囲指定
        private void textBoxTelNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b' && (ModifierKeys & Keys.Control) != Keys.Control)
                {
                    e.Handled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void textBoxPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar < '0' || '9' < e.KeyChar) && (e.KeyChar < 'a' || 'z' < e.KeyChar) && (e.KeyChar < 'A' || 'Z' < e.KeyChar) && e.KeyChar != '\b' && (ModifierKeys & Keys.Control) != Keys.Control)
                {
                    e.Handled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbEmployeeId_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b' && (ModifierKeys & Keys.Control) != Keys.Control)
                {
                    e.Handled = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }


        //データグリッドビュー表示用
        private bool GetAllData()
        {
            try
            {
                //全件取得
                Employee = employee.GetEmployeeData();
                subEmployee = new List<M_EmployeeDsp>(Employee);
                subEmployee.Insert(0, new M_EmployeeDsp { EmID = null });

                if (Employee == null)
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
                dataGridViewDsp.DataSource = Employee;
                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Employee.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                ////社員ID
                dataGridViewDsp.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[0].Width = 120;
                ////社員名
                dataGridViewDsp.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[1].Width = 170;
                ////営業所ID
                dataGridViewDsp.Columns[2].Width = 130;
                dataGridViewDsp.Columns[2].Visible = false;
                ////営業所名
                dataGridViewDsp.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[3].Width = 180;
                ////役割ID
                dataGridViewDsp.Columns[4].Width = 130;
                dataGridViewDsp.Columns[4].Visible = false;
                ////役職名
                dataGridViewDsp.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[5].Width = 120;
                ////入社年月日
                dataGridViewDsp.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[6].Width = 185;
                ////password
                dataGridViewDsp.Columns[7].Width = 130;
                dataGridViewDsp.Columns[7].Visible = false;
                ////電話番号
                dataGridViewDsp.Columns[8].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[8].Width = 200;
                ////boolフラグ
                dataGridViewDsp.Columns[9].Width = 130;
                dataGridViewDsp.Columns[9].Visible = false;
                ////論理削除フラグ
                dataGridViewDsp.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[10].Width = 90;
                ////非表示理由
                dataGridViewDsp.Columns[11].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[11].Width = 430;

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Employee.Count / (double)pageSize)) + "ページ";

                RefreshEmployeeCombo();

                dataGridViewDsp.Refresh();
            }
            catch
            {
                throw;
            }
        }


        //処理選択
        private void radioBtnReg_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnReg.Checked == false)
                {
                    return;
                }
                RefreshSalesOfficeCombo();
                RefreshPositionCombo();
                AllClear();
                cmbEmployeeId.DropDownStyle = ComboBoxStyle.Simple;
                btnReg.Enabled = true;
                btnUp.Enabled = false;
                btnSea.Enabled = false;
                label9.Visible = false;
                dateTimePickerE.Visible = false;
                label8.Visible = true;
                textBoxPassword.Visible = true;
                updFlg = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void radioBtnUpd_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnUpd.Checked == false)
                {
                    return;
                }
                RefreshEmployeeCombo();
                RefreshSalesOfficeCombo();
                RefreshPositionCombo();
                AllClear();
                cmbEmployeeId.DropDownStyle = ComboBoxStyle.DropDownList;
                btnReg.Enabled = false;
                btnUp.Enabled = true;
                btnSea.Enabled = false;
                label9.Visible = false;
                dateTimePickerE.Visible = false;
                label8.Visible = true;
                textBoxPassword.Visible = true;
                updFlg = true;
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
                RefreshEmployeeCombo();
                RefreshSalesOfficeCombo();
                RefreshPositionCombo();
                AllClear();
                cmbEmployeeId.DropDownStyle = ComboBoxStyle.DropDownList;
                btnReg.Enabled = false;
                btnUp.Enabled = false;
                btnSea.Enabled = true;
                label9.Visible = true;
                label9.Enabled = false;
                dateTimePickerE.Visible = true;
                label8.Visible = false;
                textBoxPassword.Visible = false;
                updFlg = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }


        //更新用挿入処理
        private void dataGridViewDsp_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (updFlg == true)
                {
                    //データグリッドビューからクリックされたデータを各入力エリアへ
                    cmbEmployeeId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
                    textBoxEmployeeName.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[1].Value.ToString();
                    cmbSalesOfficeId.SelectedIndex = SalesOffice.FindIndex(x => x.SoID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[2].Value.ToString()));
                    cmbPosition.SelectedIndex = Position.FindIndex(x => x.PoID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[4].Value.ToString()));
                    dateTimePickerS.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[6].Value.ToString();
                    textBoxPassword.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[7].Value.ToString();
                    textBoxTelNumber.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[8].Value.ToString();
                    checkBoxFlag.Checked = (bool)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[10].Value;

                    //null処理を空欄処理
                    if (dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    else
                        textBoxHideRea.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[11].Value.ToString();
                }
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
                dataGridViewDsp.DataSource = Employee.Take(pageSize).ToList();

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
                dataGridViewDsp.DataSource = Employee.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                int lastNo = (int)Math.Ceiling(Employee.Count / (double)pageSize) - 1;
                //最終ページでなければ
                if (pageNo <= lastNo)
                    dataGridViewDsp.DataSource = Employee.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                int lastPage = (int)Math.Ceiling(Employee.Count / (double)pageSize);
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
                int pageNo = (int)Math.Ceiling(Employee.Count / (double)pageSize) - 1;
                dataGridViewDsp.DataSource = Employee.Skip(pageSize * pageNo).Take(pageSize).ToList();

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

        private void checkBoxFlag_CheckedChanged(object sender, EventArgs e)
        {
            try
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void cmbEmployeeId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnUpd.Checked)
                {
                    if (cmbEmployeeId.SelectedValue == null)
                    {
                        AllClear();
                    }
                    else
                    {
                        int index = Employee.FindIndex(x => x.EmID == int.Parse(cmbEmployeeId.SelectedValue.ToString()));
                        cmbEmployeeId.Text = Employee[index].EmID.ToString();
                        textBoxEmployeeName.Text = Employee[index].EmName.ToString();
                        cmbSalesOfficeId.SelectedIndex = SalesOffice.FindIndex(x => x.SoID == Employee[index].SoID);
                        cmbPosition.SelectedIndex = Position.FindIndex(x => x.PoID == Employee[index].PoID);
                        dateTimePickerS.Text = Employee[index].EmHiredate.ToString();
                        textBoxPassword.Text = Employee[index].EmPassword.ToString();
                        textBoxTelNumber.Text = Employee[index].EmPhone.ToString();
                        checkBoxFlag.Checked = Employee[index].EmBoolFlag;
                        if (Employee[index].EmHidden == null)
                        {
                            textBoxHideRea.Text = "";
                        }
                        else
                        {
                            textBoxHideRea.Text = Employee[index].EmHidden.ToString();
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
            if (radioBtnSea.Checked && (!dateTimePickerS.Checked && !dateTimePickerE.Checked))
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
            if (radioBtnSea.Checked && (!dateTimePickerS.Checked && !dateTimePickerE.Checked))
            {
                label9.Enabled = false;
            }
            else
            {
                label9.Enabled = true;
            }
        }
    }
}
