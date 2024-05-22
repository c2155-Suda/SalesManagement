using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_SysDev
{
    class T_Shipment
    {
        public T_Shipment()
        {
            T_ShipmentDetail = new HashSet<T_ShipmentDetail>();
        }

        [Key]
        public int ShID { get; set; }               //出荷ID		
        public int ClID { get; set; }               //顧客ID		
        public int? EmID { get; set; }               //社員ID		
        public int SoID { get; set; }               //営業所ID		
        public int OrID { get; set; }               //受注ID		
        public int ShStateFlag { get; set; }	    //出荷状態フラグ
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? ShFinishDate { get; set; } //出荷完了年月日
        public int ShFlag { get; set; }	            //出荷管理フラグ
        public string ShHidden { get; set; }	    //非表示理由		

        public virtual M_Client M_Client { get; set; }
        public virtual M_Employee M_Employee { get; set; }
        public virtual M_SalesOffice M_SalesOffice { get; set; }
        public virtual T_Order T_Order { get; set; }
        public virtual ICollection<T_ShipmentDetail> T_ShipmentDetail { get; set; }
    }
}
