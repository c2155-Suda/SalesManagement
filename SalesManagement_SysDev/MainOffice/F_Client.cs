using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    public partial class F_Client : Form
    {
        ClientDbConnection client = new ClientDbConnection();
        DataAccess access = new DataAccess();
        InputFormCheck input = new InputFormCheck();
        private List<M_ClientDsp> Client;
        private List<M_ClientDsp> subClient;
        private List<M_SalesOfficeDsp>SalesOffice;
        private bool updFlg;
        private int pageSize = 12;
        public int LoginEmID;
        public bool loginUserInfoCheck = true;
        public F_Client()
        {
            InitializeComponent();
        }

        private void F_Client_Load(object sender, EventArgs e)
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
                EmployeeDbConnection empDb = new EmployeeDbConnection();
                if (!empDb.CheckEmployeeExsistence(LoginEmID))
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

        private void btnReg_Click(object sender, EventArgs e)
        {
            try
            {
                //入力チェック
                if (!GetValidDataAtRegistration())
                    return;
                //登録情報作成
                var regClient = GenerateDataAtRegistration();
                //登録処理
                client.RegistClientData(regClient);
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
                if (String.IsNullOrEmpty(textBoxClientName.Text.Trim()))
                {
                    MessageBox.Show("顧客名が未入力です", "入力エラー");
                    textBoxClientName.Focus();
                    return false;
                }
                if (cmbSalesOfficeId.SelectedIndex == -1)
                {
                    MessageBox.Show("営業所名が未選択です", "選択エラー");
                    cmbSalesOfficeId.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(textBoxAddress.Text.Trim()))
                {
                    MessageBox.Show("住所が未入力です", "入力エラー");
                    textBoxAddress.Focus();
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
                if (String.IsNullOrEmpty(textBoxPostNumber.Text.Trim()))
                {
                    MessageBox.Show("郵便番号が未入力です", "入力エラー");
                    textBoxPostNumber.Focus();
                    return false;
                }
                if (!(input.CheckPostCodeFormat(textBoxPostNumber.Text.Trim())))
                {
                    MessageBox.Show("郵便番号は7文字の半角数字のみです", "入力エラー");
                    textBoxPostNumber.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(textBoxFAX.Text.Trim()))
                {
                    MessageBox.Show("FAXが未入力です", "入力エラー");
                    textBoxFAX.Focus();
                    return false;
                }
                if (!(input.CheckTelFaxFormat(textBoxFAX.Text.Trim())))
                {
                    MessageBox.Show("FAXは13文字以下の数字のみです", "入力エラー");
                    textBoxFAX.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private M_Client GenerateDataAtRegistration()
        {
            try
            {
                int iFlg;
                if (checkBoxFlag.Checked == false)
                {
                    iFlg = 0;
                }
                else
                    iFlg = 2;

                //登録データセット
                return new M_Client
                {
                    ClName = textBoxClientName.Text.Trim(),
                    SoID = (int)cmbSalesOfficeId.SelectedValue,
                    ClAddress = textBoxAddress.Text.Trim(),
                    ClPhone = textBoxTelNumber.Text.Trim(),
                    ClPostal = textBoxPostNumber.Text.Trim(),
                    ClFAX = textBoxFAX.Text.Trim(),
                    ClFlag = iFlg,
                    ClHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim()
                };
            }
            catch
            {
                throw;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            try
            {
                //入力チェック
                if (!GetValidDataAtUpdate())
                    return;
                //更新情報作成
                var updClient = GenerateDataAtUpdate();
                //更新処理
                client.UpdateClientData(updClient);
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
                if (cmbClientId.SelectedIndex == -1)
                {
                    MessageBox.Show("顧客IDが未選択です", "選択エラー");
                    cmbClientId.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(textBoxClientName.Text.Trim()))
                {
                    MessageBox.Show("顧客名が未入力です", "入力エラー");
                    textBoxClientName.Focus();
                    return false;
                }
                if (cmbSalesOfficeId.SelectedIndex == -1)
                {
                    MessageBox.Show("営業所名が未選択です", "選択エラー");
                    cmbSalesOfficeId.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(textBoxAddress.Text.Trim()))
                {
                    MessageBox.Show("住所が未入力です", "入力エラー");
                    textBoxAddress.Focus();
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
                if (String.IsNullOrEmpty(textBoxPostNumber.Text.Trim()))
                {
                    MessageBox.Show("郵便番号が未入力です", "入力エラー");
                    textBoxPostNumber.Focus();
                    return false;
                }
                if (!(input.CheckPostCodeFormat(textBoxPostNumber.Text.Trim())))
                {
                    MessageBox.Show("郵便番号は7文字の半角数字のみです", "入力エラー");
                    textBoxPostNumber.Focus();
                    return false;
                }
                if (String.IsNullOrEmpty(textBoxFAX.Text.Trim()))
                {
                    MessageBox.Show("FAXが未入力です", "入力エラー");
                    textBoxFAX.Focus();
                    return false;
                }
                if (!(input.CheckTelFaxFormat(textBoxFAX.Text.Trim())))
                {
                    MessageBox.Show("FAXは13文字以下の数字のみです", "入力エラー");
                    textBoxFAX.Focus();
                    return false;
                }
                return true;
            }
            catch
            {
                throw;
            }
        }
        private M_Client GenerateDataAtUpdate()
        {
            try
            {
                int iFlg;
                if (checkBoxFlag.Checked == false)
                {
                    iFlg = 0;
                }
                else
                    iFlg = 2;


                // 更新データセット
                return new M_Client
                {
                    ClID = int.Parse(cmbClientId.Text.Trim()),
                    ClName = textBoxClientName.Text.Trim(),
                    SoID = (int)cmbSalesOfficeId.SelectedValue,
                    ClAddress = textBoxAddress.Text.Trim(),
                    ClPhone = textBoxTelNumber.Text.Trim(),
                    ClPostal = textBoxPostNumber.Text.Trim(),
                    ClFAX = textBoxFAX.Text.Trim(),
                    ClFlag = iFlg,
                    ClHidden = textBoxHideRea.Text.Trim() == string.Empty ? null : textBoxHideRea.Text.Trim()
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        private void GenerateDataAtSelect()
        {
            try
            {
                int clID = 0;
                if (cmbClientId.Text != "")
                {
                    clID = int.Parse(cmbClientId.Text);
                }

                int soID = 0;
                if (cmbSalesOfficeId.SelectedIndex != -1)
                {
                    soID = (int)cmbSalesOfficeId.SelectedValue;
                }

                int iFlg;
                if (checkBoxFlag.Checked == false)
                {
                    iFlg = 0;
                }
                else
                    iFlg = 2;

                // 検索条件のセット
                M_ClientDsp selectCondition = new M_ClientDsp()
                {
                    ClID = clID,
                    ClName = textBoxClientName.Text.Trim(),
                    SoID = soID,
                    ClAddress = textBoxAddress.Text.Trim(),
                    ClPhone = textBoxTelNumber.Text.Trim(),
                    ClPostal = textBoxPostNumber.Text.Trim(),
                    ClFAX = textBoxFAX.Text.Trim(),
                    ClFlag = iFlg,
                    ClHidden = textBoxHideRea.Text.Trim()
                };
                // データの抽出
                Client = client.GetClientData(selectCondition);
                subClient = new List<M_ClientDsp>(Client);
                subClient.Insert(0, new M_ClientDsp { ClID = null });
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
                dataGridViewDsp.DataSource = Client;

                //dataGridViewのページ番号指定
                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Client.Skip(pageSize * pageNo).Take(pageSize).ToList();

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Client.Count / (double)pageSize)) + "ページ";

                RefreshClientCombo();

                dataGridViewDsp.Refresh();
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void GetComboBoxData()
        {
            try
            {
                //顧客IDのコンボボックスData読込
                cmbClientId.DataSource = Client;
                cmbClientId.DisplayMember = "ClID";
                cmbClientId.ValueMember = "ClID";
                cmbClientId.SelectedIndex = -1;

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
        private void RefreshClientCombo()
        {
            try
            {
                int temp = 0;
                if (radioBtnReg.Checked)
                {
                    return;
                }
                if (cmbClientId.SelectedValue != null)
                {
                    temp = int.Parse(cmbClientId.SelectedValue.ToString());
                }
                if (radioBtnSea.Checked)
                {
                    cmbClientId.DataSource = subClient;
                    cmbClientId.SelectedIndex = subClient.FindIndex(x => x.ClID == temp);
                }
                else
                {
                    cmbClientId.DataSource = Client;
                    cmbClientId.SelectedIndex = Client.FindIndex(x => x.ClID == temp);
                }
                cmbClientId.DisplayMember = "ClID";
                cmbClientId.ValueMember = "ClID";
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

        private void textBoxPostNumber_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBoxFAX_KeyPress(object sender, KeyPressEventArgs e)
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
                Client = client.GetClientData();
                subClient = new List<M_ClientDsp>(Client);
                subClient.Insert(0, new M_ClientDsp { ClID = null });
                if (Client == null)
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
                dataGridViewDsp.DataSource = Client;

                textBoxPageNo.Text = "1";
                int pageNo = int.Parse(textBoxPageNo.Text) - 1;
                dataGridViewDsp.DataSource = Client.Skip(pageSize * pageNo).Take(pageSize).ToList();

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
                ////顧客ID
                dataGridViewDsp.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[0].Width = 70;
                ////顧客名
                dataGridViewDsp.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[1].Width = 160;
                ////営業所ID
                dataGridViewDsp.Columns[2].Width = 110;
                dataGridViewDsp.Columns[2].Visible = false;
                ////営業所名
                dataGridViewDsp.Columns[3].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[3].Width = 180;
                ////住所
                dataGridViewDsp.Columns[4].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[4].Width = 470;
                ////電話番号
                dataGridViewDsp.Columns[5].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[5].Width = 200;
                ////郵便番号
                dataGridViewDsp.Columns[6].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[6].Width = 130;
                ////FAX
                dataGridViewDsp.Columns[7].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[7].Width = 200;
                ////boolフラグ
                dataGridViewDsp.Columns[8].Width = 130;
                dataGridViewDsp.Columns[8].Visible = false;
                ////論理削除フラグ
                dataGridViewDsp.Columns[9].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[9].Width = 90;
                ////非表示理由
                dataGridViewDsp.Columns[10].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dataGridViewDsp.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                dataGridViewDsp.Columns[10].Width = 400;

                //dataGridViewの総ページ数
                lblPage.Text = "/" + ((int)Math.Ceiling(Client.Count / (double)pageSize)) + "ページ";

                RefreshClientCombo();

                dataGridViewDsp.Refresh();
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
                cmbClientId.SelectedIndex = -1;
                textBoxClientName.Text = "";
                cmbSalesOfficeId.SelectedIndex = -1;
                textBoxAddress.Text = "";
                textBoxTelNumber.Text = "";
                textBoxPostNumber.Text = "";
                textBoxFAX.Text = "";
                checkBoxFlag.Checked = false;
                textBoxHideRea.Text = "";
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
                if (radioBtnReg.Checked == false)
                {
                    return;
                }
                RefreshSalesOfficeCombo();
                AllClear();
                btnReg.Enabled = true;
                btnUp.Enabled = false;
                btnSea.Enabled = false;
                label1.Enabled = false;
                label9.Visible = true;
                cmbClientId.Enabled = false;
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
                RefreshClientCombo();
                RefreshSalesOfficeCombo();
                AllClear();
                btnReg.Enabled = false;
                btnUp.Enabled = true;
                btnSea.Enabled = false;
                label1.Enabled = true;
                label9.Visible = false;
                cmbClientId.Enabled = true;
                updFlg = true;
            }
            catch(Exception ex)
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
                RefreshClientCombo();
                RefreshSalesOfficeCombo();
                AllClear();
                btnReg.Enabled = false;
                btnUp.Enabled = false;
                btnSea.Enabled = true;
                label1.Enabled = true;
                label9.Visible = false;
                cmbClientId.Enabled = true;
                updFlg = false;
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
                dataGridViewDsp.DataSource = Client.Take(pageSize).ToList();

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
                dataGridViewDsp.DataSource = Client.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                if (pageNo + 1 > 1)
                    textBoxPageNo.Text = (pageNo + 1).ToString();
                else
                    textBoxPageNo.Text = "1";
            }
            catch(Exception ex)
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
                int lastNo = (int)Math.Ceiling(Client.Count / (double)pageSize) - 1;
                //最終ページでなければ
                if (pageNo <= lastNo)
                    dataGridViewDsp.DataSource = Client.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                int lastPage = (int)Math.Ceiling(Client.Count / (double)pageSize);
                if (pageNo >= lastPage)
                    textBoxPageNo.Text = lastPage.ToString();
                else
                    textBoxPageNo.Text = (pageNo + 1).ToString();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            try
            {
                //最終ページの計算
                int pageNo = (int)Math.Ceiling(Client.Count / (double)pageSize) - 1;
                dataGridViewDsp.DataSource = Client.Skip(pageSize * pageNo).Take(pageSize).ToList();

                // DataGridViewを更新
                dataGridViewDsp.Refresh();
                //ページ番号の設定
                textBoxPageNo.Text = (pageNo + 1).ToString();
            }
            catch(Exception ex)
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
                    cmbClientId.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[0].Value.ToString();
                    textBoxClientName.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[1].Value.ToString();
                    cmbSalesOfficeId.SelectedIndex = SalesOffice.FindIndex(x => x.SoID == int.Parse(dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[2].Value.ToString()));
                    textBoxAddress.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[4].Value.ToString();
                    textBoxTelNumber.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[5].Value.ToString();
                    textBoxPostNumber.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[6].Value.ToString();
                    textBoxFAX.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[7].Value.ToString();

                    checkBoxFlag.Checked = (bool)dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[9].Value;

                    //null処理を空欄処理
                    if (dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[10].Value == null)
                    {
                        textBoxHideRea.Text = "";
                    }
                    else
                        textBoxHideRea.Text = dataGridViewDsp.Rows[dataGridViewDsp.CurrentRow.Index].Cells[10].Value.ToString();
                }
            }
            catch(Exception ex)
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

        private void cmbClientId_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioBtnUpd.Checked)
                {
                    if (cmbClientId.SelectedValue == null)
                    {
                        AllClear();
                    }
                    else
                    {
                        int index = Client.FindIndex(x => x.ClID == int.Parse(cmbClientId.SelectedValue.ToString()));
                        cmbClientId.Text = Client[index].ClID.ToString();
                        textBoxClientName.Text = Client[index].ClName.ToString();
                        cmbSalesOfficeId.SelectedIndex = SalesOffice.FindIndex(x => x.SoID == Client[index].SoID);
                        textBoxAddress.Text = Client[index].ClAddress.ToString();
                        textBoxTelNumber.Text = Client[index].ClPhone.ToString();
                        textBoxPostNumber.Text = Client[index].ClPostal.ToString();
                        textBoxFAX.Text = Client[index].ClFAX.ToString();

                        checkBoxFlag.Checked = Client[index].ClBoolFlag;

                        if (Client[index].ClHidden == null)
                        {
                            textBoxHideRea.Text = "";
                        }
                        else
                            textBoxHideRea.Text = Client[index].ClHidden.ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
    }
}

