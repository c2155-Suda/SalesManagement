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
    public partial class F_Logistics : Form
    {
        public F_Home parentFormHome;
        private int logistics;
        private F_Product formProduct;
        private F_Chumon formChumon;
        private F_Hattyu formHattyu;
        private F_Warehousing formWarehousing;
        private F_Syukko formSyukko;
        public F_Logistics()
        {
            InitializeComponent();
        }

        private void F_Logistics_Load(object sender, EventArgs e)
        {
            ActiveControl = btnProduct;

            formProduct = new F_Product();
            formProduct.TopLevel = false;
            formProduct.Dock = DockStyle.Fill;
            panelLogistics.Controls.Add(formProduct);

            formChumon = new F_Chumon();
            formChumon.TopLevel = false;
            formChumon.Dock = DockStyle.Fill;
            panelLogistics.Controls.Add(formChumon);

            formHattyu = new F_Hattyu();
            formHattyu.TopLevel = false;
            formHattyu.Dock = DockStyle.Fill;
            panelLogistics.Controls.Add(formHattyu);

            formWarehousing = new F_Warehousing();
            formWarehousing.TopLevel = false;
            formWarehousing.Dock = DockStyle.Fill;
            panelLogistics.Controls.Add(formWarehousing);

            formSyukko = new F_Syukko();
            formSyukko.TopLevel = false;
            formSyukko.Dock = DockStyle.Fill;
            panelLogistics.Controls.Add(formSyukko);
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            logistics = 0;
            parentFormHome.InternalSelect("商品管理");
            LogisticsScreen();
        }

        private void btnChumon_Click(object sender, EventArgs e)
        {
            logistics = 1;
            parentFormHome.InternalSelect("注文管理");
            LogisticsScreen();
        }

        private void btnHattyu_Click(object sender, EventArgs e)
        {
            logistics = 2;
            parentFormHome.InternalSelect("発注管理");
            LogisticsScreen();
        }

        private void btnWarehousing_Click(object sender, EventArgs e)
        {
            logistics = 3;
            parentFormHome.InternalSelect("入庫管理");
            LogisticsScreen();
        }

        private void btnSyukko_Click(object sender, EventArgs e)
        {
            logistics = 4;
            parentFormHome.InternalSelect("出庫管理");
            LogisticsScreen();
        }

        private void LogisticsScreen()
        {
            if(logistics == 0)
            {
                panelLogistics.Dock = DockStyle.Fill;
                formProduct.Show();
            }
            if(logistics == 1)
            {
                panelLogistics.Dock = DockStyle.Fill;
                formChumon.Show();
            }
            if(logistics == 2)
            {
                panelLogistics.Dock = DockStyle.Fill;
                formHattyu.Show();
            }
            if(logistics == 3)
            {
                panelLogistics.Dock = DockStyle.Fill;
                formWarehousing.Show();
            }
            if(logistics == 4)
            {
                panelLogistics.Dock = DockStyle.Fill;
                formSyukko.Show();
            }
        }
    }
}
