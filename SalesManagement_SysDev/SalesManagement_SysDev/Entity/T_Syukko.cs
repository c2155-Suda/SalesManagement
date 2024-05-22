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
    class T_Syukko
    {
        public T_Syukko()
        {
            T_SyukkoDetail = new HashSet<T_SyukkoDetail>();
        }

        [Key]
        public int SyID { get; set; }               //出庫ID	
        public int? EmID { get; set; }              //社員ID	
        public int ClID { get; set; }               //顧客ID	
        public int SoID { get; set; }               //営業所ID	
        public int OrID { get; set; }               //受注ID
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? SyDate { get; set; }       //出庫年月日	
        public int SyStateFlag { get; set; }        //出庫状態フラグ
        public int SyFlag { get; set; }	            //出庫管理フラグ
        public string SyHidden { get; set; }	    //非表示理由	

        public virtual M_Client M_Client { get; set; }
        public virtual M_Employee M_Employee { get; set; }
        public virtual M_SalesOffice M_SalesOffice { get; set; }
        public virtual T_Order T_Order { get; set; }
        public virtual ICollection<T_SyukkoDetail> T_SyukkoDetail { get; set; }
    }
    class T_SyukkoDsp
    {
        [DisplayName("出庫ID")]
        public int SyID { get; set; }

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

        [DisplayName("出庫日")]
        public DateTime? SyDate { get; set; }
        public int SyFlag { get; set; }

        [DisplayName("非表示")]
        public bool SyBoolFlag { get; set; }

        [DisplayName("非表示理由")]
        public string SyHidden { get; set; }

        [DisplayName("品数")]
        public int KindOfProducts { get; set; }
        public int SyStateFlag { get; set; }

        [DisplayName("確定")]
        public bool SyBoolStateFlag { get; set; }

    }
    class T_SyukkoRead
    {
        public int SyID { get; set; }
        public int EmID { get; set; }
        public int ClID { get; set; }
        public int SoID { get; set; }
        public int OrID { get; set; }
        public DateTime SyDate { set; get; }
        public int SyStateFlag { get; set; }
        public int SyFlag { get; set; }
        public string SyHidden { get; set; }
        public int PrID { get; set; }
        public int ScID { get; set; }
        public int McID { get; set; }

    }
}
