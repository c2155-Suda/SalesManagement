using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SalesManagement_SysDev
{
    class T_LoginHistory
    {
        [Key]
        public int LoHistoryID { get; set; }        //ログイン履歴ID
        public int EmID { get; set; }               //社員ID
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime LoginDate { get; set; }     //ログイン年月日
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? LogoutDate { get; set; }	//ログアウト年月日

        public virtual M_Employee M_Employee { get; set; }
    }
    class T_LoginHistoryDsp
    {
        [DisplayName("ログイン履歴ID")]
        public int? LoHistoryID { get; set; }
        public int EmID { get; set; }
        [DisplayName("社員名")]
        public string EmName { get; set; }

        public int SoID { get; set; }

        [DisplayName("営業所")]
        public string SoName { get; set; }

        public int PoID { get; set; }

        [DisplayName("役職")]
        public string PoName { get; set; }

        [DisplayName("ログイン日")]
        public DateTime LoginDate { get; set; }
        [DisplayName("ログアウト日")]
        public DateTime? LogoutDate { get; set; }

    }
}
