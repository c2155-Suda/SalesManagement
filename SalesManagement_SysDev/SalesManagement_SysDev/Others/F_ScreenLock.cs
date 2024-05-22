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
        public F_ScreenLock()
        {
            InitializeComponent();
        }
        private int errolog;

        private void btnUnlock_Click(object sender, EventArgs e)
        {
            errolog = 0;
            CheckText();
            if (errolog == 0)
            {
                MessageBox.Show("画面解除しました。", "確認");
                textboxPassword.Text = "";
                this.Close();
            }
        }

        private void CheckText()
        {
            if (textboxPassword.Text == "")
            {
                MessageBox.Show("Passwordを入力してください。", "空欄");
                errolog = 1;
                return;
            }
        }
    }
}
