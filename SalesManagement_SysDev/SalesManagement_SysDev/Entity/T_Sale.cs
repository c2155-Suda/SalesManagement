using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_SysDev
{
    class T_Sale
    {
        public T_Sale()
        {
            T_SaleDetail = new HashSet<T_SaleDetail>();
        }

        [Key]
        public int SaID { get; set; }           //売上ID	
        public int ClID { get; set; }           //顧客ID	
        public int SoID { get; set; }           //営業所ID	
        public int EmID { get; set; }           //社員ID	
        public int ChID { get; set; }           //受注ID
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime SaDate { get; set; }    //売上日時
        public string SaHidden { get; set; }    //非表示理由	
        public int SaFlag { get; set; }	        //売上管理フラグ	

        public virtual M_Client M_Client { get; set; }
        public virtual M_Employee M_Employee { get; set; }
        public virtual M_SalesOffice M_SalesOffice { get; set; }
        public virtual T_Chumon T_Chumon { get; set; }
        public virtual ICollection<T_SaleDetail> T_SaleDetail { get; set; }
    }
}
