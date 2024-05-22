using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SalesManagement_SysDev
{
    class T_Warehousing
    {
        public T_Warehousing()
        {
            T_WarehousingDetail = new HashSet<T_WarehousingDetail>();
        }

        [Key]
        public int WaID { get; set; }               //入庫ID	
        public int HaID { get; set; }               //発注ID	
        public int? EmID { get; set; }               //社員ID
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? WaDate { get; set; }       //入庫年月日	
        public int WaShelfFlag { get; set; }        //入庫済フラグ(棚）
        public string WaHidden { get; set; }	    //非表示理由
        public int WaFlag { get; set; }             //入庫管理フラグ

        public virtual M_Employee M_Employee { get; set; }
        public virtual T_Hattyu T_Hattyu { get; set; }
        public virtual ICollection<T_WarehousingDetail> T_WarehousingDetail { get; set; }
    }
}
