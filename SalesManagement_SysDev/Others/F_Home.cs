using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    public partial class F_Home : Form
    {
        public F_Home()
        {
            InitializeComponent();
        }

        private F_MainOffice formMainOffice;
        private F_SalesOffice formSalesOffice;
        private F_Logistics formLogistics;
        private F_Login formLogin;
        private F_History formHistory;
        F_ScreenLock formScreenLock = new F_ScreenLock();

        //ボタンカラー
        private Color _activeColor = Color.Aqua;
        private Color _defaultColor = Color.Gray;
        //一致、不一致確認
        private IEnumerable<Button> _formButtons;
        //ログイン・ログアウト
        private int logcount = 0;

        //ログイン状態・履歴変数
        public int logHID;
        public int logID;
        public int logSoID;
        public int logPoID;
        public string logPassword;
        public DateTime logDataS;
        public DateTime logDataE;
        private string logEmName;
        private string logSoName;

        LogDbConnection logDb = new LogDbConnection();

        private void F_Home_Load(object sender, EventArgs e)
        {
            try
            {
                BtnReset();
                ActiveControl = btnLogin;

                formMainOffice = new F_MainOffice();
                formMainOffice.TopLevel = false;
                formMainOffice.Dock = DockStyle.Fill;
                panelScreen.Controls.Add(formMainOffice);

                formSalesOffice = new F_SalesOffice();
                formSalesOffice.TopLevel = false;
                formSalesOffice.Dock = DockStyle.Fill;
                panelScreen.Controls.Add(formSalesOffice);

                formLogistics = new F_Logistics();
                formLogistics.TopLevel = false;
                formLogistics.Dock = DockStyle.Fill;
                panelScreen.Controls.Add(formLogistics);

                formLogin = new F_Login();
                formLogin.TopLevel = false;
                formLogin.Dock = DockStyle.Fill;
                panelScreen.Controls.Add(formLogin);

                formHistory = new F_History();
                formHistory.TopLevel = false;
                formHistory.Dock = DockStyle.Fill;
                panelScreen.Controls.Add(formHistory);


                _formButtons = new List<Button>()
            {
                btnMainOffice,btnSalesOffice,btnLogistics,btnLogin,btnHistory
            };
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
            
        }


        private void CloseForm()
        {
            try
            {
                formLogin.Close();
                formMainOffice.Close();
                formSalesOffice.Close();
                formLogistics.Close();
                formHistory.Close();
            }
            catch
            {
                throw;
            }
        }
                
        private void ActivateBtn(Button btn)
        {
            try
            {
                // form表示するボタンのリストを回して、
                // 引数のボタンを一致してたらactiveColorにして、
                // 一致してなかったらdefaultColorにする
                foreach (var b in _formButtons)
                {
                    b.BackColor = b == btn ? _activeColor : _defaultColor;
                }
                if (logPoID == 2)
                {
                    btnMainOffice.BackColor = Color.Black;
                    btnLogistics.BackColor = Color.Black;
                    btnHistory.BackColor = Color.Black;
                }
                else if (logPoID == 3)
                {
                    btnMainOffice.BackColor = Color.Black;
                    btnSalesOffice.BackColor = Color.Black;
                    btnHistory.BackColor = Color.Black;
                }
            }
            catch
            {
                throw;
            }
        }

        private void btnMainOffice_Click(object sender, EventArgs e)
        {
            try
            {
                CloseForm();

                formMainOffice = new F_MainOffice();
                formMainOffice.TopLevel = false;
                formMainOffice.Dock = DockStyle.Fill;

                formMainOffice.parentFormHome = this;
                panelScreen.Controls.Add(formMainOffice);

                formMainOffice.Show();
                ActivateBtn((Button)sender);
                panelTitle.BackColor = Color.LightGreen;
                lblTitle.Text = "本社管理";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        public void btnMainOffice_PerformClick()
        {
            btnMainOffice.PerformClick();
        }
        private void btnSalesOffice_Click(object sender, EventArgs e)
        {
            try
            {
                CloseForm();

                formSalesOffice = new F_SalesOffice();
                formSalesOffice.TopLevel = false;
                formSalesOffice.Dock = DockStyle.Fill;

                formSalesOffice.parentFormHome = this;
                panelScreen.Controls.Add(formSalesOffice);

                formSalesOffice.Show();
                ActivateBtn((Button)sender);
                panelTitle.BackColor = Color.LightBlue;
                lblTitle.Text = "営業所管理";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        public void btnSalesOffice_PerformClick()
        {
            btnSalesOffice.PerformClick();
        }

        private void btnLogistics_Click(object sender, EventArgs e)
        {
            try
            {
                CloseForm();

                formLogistics = new F_Logistics();
                formLogistics.TopLevel = false;
                formLogistics.Dock = DockStyle.Fill;

                formLogistics.parentFormHome = this;
                panelScreen.Controls.Add(formLogistics);

                formLogistics.Show();
                ActivateBtn((Button)sender);
                panelTitle.BackColor = Color.LightSalmon;
                lblTitle.Text = "物流管理";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }
        public void btnLogistics_PerformClick()
        {
            btnLogistics.PerformClick();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                CloseForm();

                formLogin = new F_Login();
                formLogin.TopLevel = false;
                formLogin.Dock = DockStyle.Fill;

                formLogin.parentFormHome = this;
                panelScreen.Controls.Add(formLogin);

                if (logcount == 0)
                {
                    formLogin.Show();
                    ActivateBtn((Button)sender);
                    BtnControl();
                    panelTitle.BackColor = Color.FromArgb(224, 224, 224);
                    lblTitle.Text = "ログイン画面";
                }
                else if (logcount == 1)
                {
                    panelTitle.BackColor = Color.FromArgb(224, 224, 224);
                    lblTitle.Text = "ログアウト画面";
                    DialogResult dr = MessageBox.Show("ログアウトしますか？", "確認", MessageBoxButtons.OKCancel);
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        //ログアウトする
                        formLogin.Show();
                        ActivateBtn((Button)sender);
                        panelTitle.BackColor = Color.FromArgb(224, 224, 224);
                        lblTitle.Text = "ログイン画面";
                        btnLogin.Text = "ログイン";
                        logcount = 0;
                        lblLogHis.Text = "ログイン履歴ID：";
                        lblEmName.Text = "ログイン社員：";
                        lblSoName.Text = "営業所：";
                        lblDateTime.Text = "ログイン日：";
                        logDataE = DateTime.Now;
                        logDb.UpdateLogHistoryData(logHID, logDataE);
                        BtnControl();
                    }
                    else if (dr == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
            
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            try
            {
                CloseForm();

                formHistory = new F_History();
                formHistory.TopLevel = false;
                formHistory.Dock = DockStyle.Fill;

                formHistory.parentFormHome = this;
                panelScreen.Controls.Add(formHistory);

                formHistory.Show();
                ActivateBtn((Button)sender);
                panelTitle.BackColor = Color.FromArgb(224, 224, 224);
                lblTitle.Text = "履歴";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void btnScreenLock_Click(object sender, EventArgs e)
        {
            try
            {
                formScreenLock.parentFormHome = this;
                formScreenLock.ShowDialog();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void lblHome_Click(object sender, EventArgs e)
        {
            try
            {
                CloseForm();
                BtnReset();
                panelTitle.BackColor = Color.FromArgb(255, 255, 192);
                lblTitle.Text = "Home";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        public void Logbtn()
        {
            try
            {
                btnLogin.Text = "ログアウト";
                lblTitle.Text = "Home";
                panelTitle.BackColor = Color.FromArgb(255, 255, 192);
                logcount = 1;
                BtnControl();

                // 検索条件のセット
                M_EmployeeDsp selectCondition = new M_EmployeeDsp()
                {
                    EmID = logID,
                    SoID = logSoID,
                };
                // データの抽出
                using (var context = new SalesManagement_DevContext())
                {
                    var targetEm = context.M_Employees.Single(x => x.EmID == logID);
                    logEmName = targetEm.EmName;

                    var targetSo = context.M_SalesOffices.Single(x => x.SoID == logSoID);
                    logSoName = targetSo.SoName;
                }

                lblLogHis.Text = lblLogHis.Text + logHID.ToString();
                lblEmName.Text = lblEmName.Text + logEmName;
                lblSoName.Text = lblSoName.Text + logSoName;
                lblDateTime.Text = lblDateTime.Text + logDataS.ToShortDateString();
            }
            catch
            {
                throw;
            }
        }

        public void InternalSelect(string dspstring)
        {
            lblTitle.Text = dspstring;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void F_Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (logcount == 1)
                {
                    logDataE = DateTime.Now;
                    logDb.UpdateLogHistoryData(logHID, logDataE);
                    //ログイン状態時に自動ログアウト(データ記録)
                    //elif　ログイン状態　return (ログアウト状態のみtrue)
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void BtnControl()
        {
            try
            {
                if (logcount == 0)
                {
                    btnMainOffice.BackColor = Color.Black;
                    btnSalesOffice.BackColor = Color.Black;
                    btnLogistics.BackColor = Color.Black;
                    btnHistory.BackColor = Color.Black;
                    btnScreenLock.BackColor = Color.Black;
                    btnMainOffice.Enabled = false;
                    btnSalesOffice.Enabled = false;
                    btnLogistics.Enabled = false;
                    btnHistory.Enabled = false;
                    btnScreenLock.Enabled = false;
                }
                else
                {
                    if (logPoID == 1)
                    {
                        btnMainOffice.BackColor = Color.Gray;
                        btnSalesOffice.BackColor = Color.Gray;
                        btnLogistics.BackColor = Color.Gray;
                        btnHistory.BackColor = Color.Gray;
                        btnScreenLock.BackColor = Color.Gray;
                        btnMainOffice.Enabled = true;
                        btnSalesOffice.Enabled = true;
                        btnLogistics.Enabled = true;
                        btnHistory.Enabled = true;
                        btnScreenLock.Enabled = true;
                        btnLogin.Focus();
                    }
                    else if (logPoID == 2)
                    {
                        btnSalesOffice.BackColor = Color.Gray;
                        btnScreenLock.BackColor = Color.Gray;
                        btnSalesOffice.Enabled = true;
                        btnScreenLock.Enabled = true;
                        btnLogin.Focus();
                    }
                    else if (logPoID == 3)
                    {
                        btnLogistics.BackColor = Color.Gray;
                        btnScreenLock.BackColor = Color.Gray;
                        btnLogistics.Enabled = true;
                        btnScreenLock.Enabled = true;
                        btnLogin.Focus();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        private void BtnReset()
        {
            try
            {
                if (logcount == 0)
                {
                    btnMainOffice.BackColor = Color.Black;
                    btnSalesOffice.BackColor = Color.Black;
                    btnLogistics.BackColor = Color.Black;
                    btnHistory.BackColor = Color.Black;
                    btnScreenLock.BackColor = Color.Black;
                    btnMainOffice.Enabled = false;
                    btnSalesOffice.Enabled = false;
                    btnLogistics.Enabled = false;
                    btnHistory.Enabled = false;
                    btnScreenLock.Enabled = false;
                }
                else
                {
                    if (logPoID == 1)
                    {
                        btnMainOffice.BackColor = Color.White;
                        btnSalesOffice.BackColor = Color.White;
                        btnLogistics.BackColor = Color.White;
                        btnHistory.BackColor = Color.White;
                        btnScreenLock.BackColor = Color.White;
                        btnMainOffice.Enabled = true;
                        btnSalesOffice.Enabled = true;
                        btnLogistics.Enabled = true;
                        btnHistory.Enabled = true;
                        btnScreenLock.Enabled = true;
                    }
                    else if (logPoID == 2)
                    {
                        btnSalesOffice.BackColor = Color.White;
                        btnScreenLock.BackColor = Color.White;
                        btnSalesOffice.Enabled = true;
                        btnScreenLock.Enabled = true;
                    }
                    else if (logPoID == 3)
                    {
                        btnLogistics.BackColor = Color.White;
                        btnScreenLock.BackColor = Color.White;
                        btnLogistics.Enabled = true;
                        btnScreenLock.Enabled = true;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

