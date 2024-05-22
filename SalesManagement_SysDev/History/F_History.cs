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
    public partial class F_History : Form
    {
        public F_Home parentFormHome;
        private int his;
        private F_LoginHistory formLoginHis;
        private F_OperationHistory formOpeHis;

        public F_History()
        {
            InitializeComponent();
        }


        private void F_History_Load(object sender, EventArgs e)
        {
            formLoginHis = new F_LoginHistory();
            formLoginHis.TopLevel = false;
            formLoginHis.Dock = DockStyle.Fill;
            panelHisSelect.Controls.Add(formLoginHis);

            formOpeHis = new F_OperationHistory();
            formOpeHis.TopLevel = false;
            formOpeHis.Dock = DockStyle.Fill;
            panelHisSelect.Controls.Add(formOpeHis);
        }

        private void btnLoginHis_Click(object sender, EventArgs e)
        {
            his = 0;
            parentFormHome.InternalSelect("ログイン履歴");
            HisSelScreen();
        }

        private void btnOpeHis_Click(object sender, EventArgs e)
        {
            his = 1;
            parentFormHome.InternalSelect("操作履歴");
            HisSelScreen();
        }

        private void HisSelScreen()
        {
            if (his == 0)
            {
                panelHisSelect.Dock = DockStyle.Fill;
                formLoginHis.Show();
            }
            if (his == 1)
            {
                panelHisSelect.Dock = DockStyle.Fill;
                formOpeHis.Show();
            }
        }        
    }
}
