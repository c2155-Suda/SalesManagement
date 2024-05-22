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
    class T_ShipmentDsp
    {
        [DisplayName("出荷ID")]
        public int? ShID { get; set; }

        [DisplayName("受注ID")]
        public int OrID { get; set; }

        public int ClID { get; set; }

        [DisplayName("顧客")]
        public string ClName { get; set; }

        public int SoID { get; set; }

        [DisplayName("営業所")]
        public string SoName { get; set; }

        public int? EmID { get; set; }

        [DisplayName("社員")]
        public string EmName { get; set; }

        [DisplayName("出荷日")]
        public DateTime? ShDate { get; set; }
        public int ShFlag { get; set; }

        [DisplayName("非表示")]
        public bool ShBoolFlag { get; set; }

        [DisplayName("非表示理由")]
        public string ShHidden { get; set; }

        [DisplayName("品数")]
        public int KindOfProducts { get; set; }
        public int ShStateFlag { get; set; }

        [DisplayName("確定")]
        public bool ShBoolStateFlag { get; set; }
        public int OrderFlag { get; set; }
    }
    class T_ShipmentRead
    {
        public int ShID { get; set; }
        public int EmID { get; set; }
        public int ClID { get; set; }
        public int SoID { get; set; }
        public int OrID { get; set; }
        public DateTime ShDate { set; get; }
        public int ShStateFlag { get; set; }
        public int ShFlag { get; set; }
        public string ShHidden { get; set; }
        public int PrID { get; set; }
        public int ScID { get; set; }
        public int McID { get; set; }
    }

}
