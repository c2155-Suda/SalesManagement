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
        public int OrID { get; set; }           //受注ID
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime SaDate { get; set; }    //売上日時
        public string SaHidden { get; set; }    //非表示理由	
        public int SaFlag { get; set; }	        //売上管理フラグ	

        public virtual M_Client M_Client { get; set; }
        public virtual M_Employee M_Employee { get; set; }
        public virtual M_SalesOffice M_SalesOffice { get; set; }
        public virtual T_Order T_Order { get; set; }
        public virtual ICollection<T_SaleDetail> T_SaleDetail { get; set; }
    }
    class T_SaleDsp
    {
        [DisplayName("売上ID")]
        public int? SaID { get; set; }
        [DisplayName("受注ID")]
        public int OrID { get; set; }
        public int ClID { get; set; }

        [DisplayName("顧客")]
        public string ClName { get; set; }

        public int SoID { get; set; }

        [DisplayName("営業所")]
        public string SoName { get; set; }

        public int EmID { get; set; }

        [DisplayName("社員")]
        public string EmName { get; set; }

        [DisplayName("売上日")]
        public DateTime SaDate { get; set; }
        public int SaFlag { get; set; }

        [DisplayName("非表示")]
        public bool SaBoolFlag { get; set; }

        [DisplayName("非表示理由")]
        public string SaHidden { get; set; }

        [DisplayName("品数")]
        public int KindOfProducts { get; set; }
        [DisplayName("金額")]
        public decimal SumPrice { get; set; }
    }
    class T_SaleRead
    {
        public int SaID { get; set; }
        public int EmID { get; set; }
        public int ClID { get; set; }
        public int SoID { get; set; }
        public int OrID { get; set; }
        public DateTime SaDate { set; get; }
        public int SaFlag { get; set; }
        public string SaHidden { get; set; }
        public int PrID { get; set; }
        public int ScID { get; set; }
        public int McID { get; set; }
    }
}
