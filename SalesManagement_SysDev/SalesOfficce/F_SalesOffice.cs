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
    public partial class F_SalesOffice : Form
    {

        public F_Home parentFormHome;
        private int sales;
        private F_Order formOrder;
        private F_Arrival formArrival;
        private F_Shipment formShipment;
        private F_Chumon formChumon;

        public F_SalesOffice()
        {
            InitializeComponent();
        }
        private void F_SalesOffice_Load(object sender, EventArgs e)
        {
            ActiveControl = btnOrder;

            formOrder = new F_Order();
            formOrder.LoginEmID = parentFormHome.logID;
            formOrder.LoginSoID = parentFormHome.logSoID;
            formOrder.TopLevel = false;
            formOrder.Dock = DockStyle.Fill;
            panelSalesOffice.Controls.Add(formOrder);

            formArrival = new F_Arrival();
            formArrival.LoginEmID = parentFormHome.logID;
            formArrival.LoginSoID = parentFormHome.logSoID;
            formArrival.TopLevel = false;
            formArrival.Dock = DockStyle.Fill;
            panelSalesOffice.Controls.Add(formArrival);

            formShipment = new F_Shipment();
            formShipment.LoginEmID = parentFormHome.logID;
            formShipment.LoginSoID = parentFormHome.logSoID;
            formShipment.TopLevel = false;
            formShipment.Dock = DockStyle.Fill;
            panelSalesOffice.Controls.Add(formShipment);

            formChumon = new F_Chumon();
            formChumon.LoginEmID = parentFormHome.logID;
            formChumon.LoginSoID = parentFormHome.logSoID;
            formChumon.TopLevel = false;
            formChumon.Dock = DockStyle.Fill;
            panelSalesOffice.Controls.Add(formChumon);
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            sales = 0;
            parentFormHome.InternalSelect("受注管理");
            SalesScreen();
        }

        private void btnArrival_Click(object sender, EventArgs e)
        {
            sales = 1;
            parentFormHome.InternalSelect("入荷管理");
            SalesScreen();
        }

        private void btnShipment_Click(object sender, EventArgs e)
        {
            sales = 2;
            parentFormHome.InternalSelect("出荷管理");
            SalesScreen();
        }
        private void SalesScreen()
        {
            if (sales == 0)
            {
                panelSalesOffice.Dock = DockStyle.Fill;
                formOrder.Show();
                if (!formOrder.loginUserInfoCheck)
                {
                    parentFormHome.btnSalesOffice_PerformClick();
                }
            }
            else if (sales == 1)
            {
                panelSalesOffice.Dock = DockStyle.Fill;
                formArrival.Show();
                if (!formArrival.loginUserInfoCheck)
                {
                    parentFormHome.btnSalesOffice_PerformClick();
                }
            }
            else if (sales == 2)
            {
                panelSalesOffice.Dock = DockStyle.Fill;
                formShipment.Show();
                if (!formShipment.loginUserInfoCheck)
                {
                    parentFormHome.btnSalesOffice_PerformClick();
                }
            }
            else if (sales == 3)
            {
                panelSalesOffice.Dock = DockStyle.Fill;
                formChumon.Show();
                if (!formChumon.loginUserInfoCheck)
                {
                    parentFormHome.btnSalesOffice_PerformClick();
                }
            }
        }

        private void btnChumon_Click(object sender, EventArgs e)
        {
            sales = 3;
            parentFormHome.InternalSelect("注文管理");
            SalesScreen();
        }
    }
}            
