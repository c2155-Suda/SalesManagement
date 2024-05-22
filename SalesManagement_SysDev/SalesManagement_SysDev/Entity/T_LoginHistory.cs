using System;
using System.Collections.Generic;
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
        public int LoHistoryID { get; set; }        //ログイン認証ID
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
}
