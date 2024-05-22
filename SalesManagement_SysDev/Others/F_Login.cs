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

        private bool Flg = false;
        public string shainname;
        public F_Home parentFormHome;
        public int logHisId;
        private int loginId;
        private string logPass;
        private DateTime logDate;
        public int logSoId;
        public int logPoId;
        LogDbConnection log = new LogDbConnection(); 
        public F_Login()
        {
            InitializeComponent();
        }

        private void F_Login_Load(object sender, EventArgs e)
        {
            ActiveControl = textBoxEmployeeID;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                CheckText();
                if (Flg == true)
                {
                    MessageBox.Show("ログインしました。", "確認");
                    parentFormHome.logID = loginId;
                    parentFormHome.logSoID = logSoId;
                    parentFormHome.logPoID = logPoId;
                    parentFormHome.logPassword = logPass;
                    logDate = DateTime.Now;
                    parentFormHome.logDataS = logDate;
                    var regLogHis = GenerateDataAtRegistration();
                    log.RegistLogHistoryData(regLogHis);
                    parentFormHome.logHID = logHisId;
                    parentFormHome.Logbtn();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("ログインできませんでした。", "エラー");
                    textBoxEmployeeID.Focus();
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー");
            }
        }

        private void CheckText()
        {
            try
            {
                if (textBoxEmployeeID.Text.Trim() == "")
                {

                    MessageBox.Show("社員IDを入力してください。", "空欄");
                    return;
                }
                else
                    loginId = int.Parse(textBoxEmployeeID.Text.Trim());
                if (textBoxPassword.Text == "")
                {
                    MessageBox.Show("Passwordを入力してください。", "空欄");
                    return;
                }
                else
                    logPass = textBoxPassword.Text.Trim();

                log.parentlog = this;
                Flg = log.CheckLoginIDPass(loginId, logPass);
                //入力内容(社員ID・Password)をDBで一致・不一致
                //社員IDから社員名を変数へ(shainname)
            }
            catch
            {
                throw;
            }
        }

        private T_LoginHistory GenerateDataAtRegistration()
        {
            try
            {
                //登録データセット
                return new T_LoginHistory
                {
                    EmID = loginId,
                    LoginDate = logDate
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
