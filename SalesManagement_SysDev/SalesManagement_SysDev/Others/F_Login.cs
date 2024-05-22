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
    public partial class F_Login : Form
    {

        private int errolog;
        public string shainname;
        public F_Home parentFormHome;
        public F_Login()
        {
            InitializeComponent();
        }

        private void F_Login_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            errolog = 0;
            CheckText();
            if (errolog == 0)
            {
                MessageBox.Show("ログインしました。", "確認");
                parentFormHome.Logbtn();
                this.Close();
            }
        }

        private void CheckText()
        {
            if (textBoxEmployeeID.Text == "")
            {
                MessageBox.Show("社員IDを入力してください。", "空欄");
                errolog = 1;
                return;
            }
            if (textBoxPassword.Text == "")
            {
                MessageBox.Show("Passwordを入力してください。", "空欄");
                errolog = 1;
                return;
            }
            //入力内容(社員ID・Password)をDBで一致・不一致
            //社員IDから社員名を変数へ(shainname)
        }
    }
}
