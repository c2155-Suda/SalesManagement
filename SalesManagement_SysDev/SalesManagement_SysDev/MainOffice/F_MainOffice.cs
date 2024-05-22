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
    public partial class F_MainOffice : Form
    {

        public F_Home parentFormHome;
        private int main;
        private F_Employee formEmployee;
        private F_Client formClient;
        private F_Sale formSale;

        public F_MainOffice()
        {
            InitializeComponent();
        }

        private void F_MainOffice_Load(object sender, EventArgs e)
        {
            ActiveControl = btnEmployee;

            formEmployee = new F_Employee();
            formEmployee.TopLevel = false;
            formEmployee.Dock = DockStyle.Fill;
            panelMainOffice.Controls.Add(formEmployee);

            formClient = new F_Client();
            formClient.TopLevel = false;
            formClient.Dock = DockStyle.Fill;
            panelMainOffice.Controls.Add(formClient);

            formSale = new F_Sale();
            formSale.TopLevel = false;
            formSale.Dock = DockStyle.Fill;
            panelMainOffice.Controls.Add(formSale);
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            main = 0;
            parentFormHome.InternalSelect("社員管理");
            MainScreen();
        }

        private void btnClient_Click(object sender, EventArgs e)
        {
            main = 1;
            parentFormHome.InternalSelect("顧客管理");
            MainScreen();
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            main = 2;
            parentFormHome.InternalSelect("売上管理");
            MainScreen();
        }

        private void MainScreen()
        {
            if (main == 0)
            {                
                panelMainOffice.Dock = DockStyle.Fill;
                formEmployee.Show();
            }
            if (main == 1)
            {
                panelMainOffice.Dock = DockStyle.Fill;
                formClient.Show();
            }
            if(main == 2)
            {
                panelMainOffice.Dock = DockStyle.Fill;
                formSale.Show();
            }
        }
    }
}
