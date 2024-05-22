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
    public partial class F_Chumon : Form
    {
        F_ChumonDetail f_Detail = new F_ChumonDetail();
        public F_Chumon()
        {
            InitializeComponent();
        }

        private void btnDetail_Click(object sender, EventArgs e)
        {
            f_Detail.ShowDialog();
        }

        private void F_Chumon_Load(object sender, EventArgs e)
        {

        }
    }
}
