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
    public partial class F_ScreenLock : Form
    {
        public F_Home parentFormHome;
        public F_ScreenLock()
        {
            InitializeComponent();
        }

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            bool errorPass = true;
            errorPass = CheckPassword(errorPass);
            if (errorPass == true)
            {
                MessageBox.Show("画面解除しました。", "確認");
                textboxPassword.Text = "";
                this.Close();
            }
        }

        private bool CheckPassword(bool errorPass)
        {
            if (textboxPassword.Text.Trim() == "")
            {
                MessageBox.Show("Passwordを入力してください。", "空欄");
                textboxPassword.Text = "";
                textboxPassword.Focus();
                return false;
            }            
            if(textboxPassword.Text.Trim() != parentFormHome.logPassword)
            {
                MessageBox.Show("Passwordが違います。", "エラー");
                textboxPassword.Text = "";
                textboxPassword.Focus();
                return false;
            }
            return true;
        }

        private void F_ScreenLock_Load(object sender, EventArgs e)
        {
            textboxPassword.Focus();
        }
    }
}
