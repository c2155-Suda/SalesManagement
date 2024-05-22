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

        private void F_Home_Load(object sender, EventArgs e)
        {
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


        private void CloseForm()
        {
            formLogin.Close();
            formMainOffice.Close();
            formSalesOffice.Close();
            formLogistics.Close();
            formHistory.Close();
        }

        private void ResetBtn()
        {
            btnMainOffice.BackColor = Color.White;
            btnSalesOffice.BackColor = Color.White;
            btnLogistics.BackColor = Color.White;
            btnLogin.BackColor = Color.White;
            btnHistory.BackColor = Color.White;
            btnScreenLock.BackColor = Color.White;
        }

        private void ActivateBtn(Button btn)
        {
            // form表示するボタンのリストを回して、
            // 引数のボタンを一致してたらactiveColorにして、
            // 一致してなかったらdefaultColorにする
            foreach (var b in _formButtons)
            {
                b.BackColor = b == btn ? _activeColor : _defaultColor;
            }
        }

        private void btnMainOffice_Click(object sender, EventArgs e)
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

        private void btnSalesOffice_Click(object sender, EventArgs e)
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

        private void btnLogistics_Click(object sender, EventArgs e)
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

        private void btnLogin_Click(object sender, EventArgs e)
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
                panelTitle.BackColor = Color.FromArgb(224, 224, 224);
                lblTitle.Text = "ログイン画面";
            }
            else if (logcount == 1)
            {
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
                }
                else if (dr == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

            }
        }

        private void btnHistory_Click(object sender, EventArgs e)
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

        private void btnScreenLock_Click(object sender, EventArgs e)
        {
            formScreenLock.ShowDialog();
        }

        private void lblHome_Click(object sender, EventArgs e)
        {
            CloseForm();

            ResetBtn();
            panelTitle.BackColor = Color.FromArgb(255,255,192);
            lblTitle.Text = "Home";
        }

        public void Logbtn()
        {
            btnLogin.Text = "ログアウト";
            lblTitle.Text = "Home";
            panelTitle.BackColor = Color.FromArgb(255, 255, 192);
            ResetBtn();
            logcount = 1;
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
            //ログイン状態時に自動ログアウト(データ記録)
            //elif　ログイン状態　return (ログアウト状態のみtrue)

            DialogResult result = MessageBox.Show("Formを閉じますか？", "確認", MessageBoxButtons.OKCancel);
            if (result == DialogResult.OK)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                btnLogin.Focus();
            }
        }
    }
}
