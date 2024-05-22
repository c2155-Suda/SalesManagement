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
    class M_Client
    {
        public M_Client()
        {
            T_Arrival = new HashSet<T_Arrival>();
            T_Chumon = new HashSet<T_Chumon>();
            T_Order = new HashSet<T_Order>();
            T_Sale = new HashSet<T_Sale>();
            T_Shipment = new HashSet<T_Shipment>();
            T_Syukko = new HashSet<T_Syukko>();
        }

        [Key]
        public int ClID { get; set; }           //顧客ID		
        public int SoID { get; set; }           //営業所ID
        [MaxLength(50)]
        [Required]
        public string ClName { get; set; }      //顧客名
        [MaxLength(50)]
        [Required]
        public string ClAddress { get; set; }   //住所	 
        [MaxLength(13)]
        [Required]
        public string ClPhone { get; set; }     //電話番号	
        [MaxLength(7)]
        [Required]
        public string ClPostal { get; set; }    //郵便番号
        [MaxLength(13)]
        [Required]
        public string ClFAX { get; set; }       //FAX		
        public int ClFlag { get; set; }         //顧客管理フラグ	
        public string ClHidden { get; set; }	//非表示理由		

        public virtual M_SalesOffice M_SalesOffice { get; set; }
        public virtual ICollection<T_Arrival> T_Arrival { get; set; }
        public virtual ICollection<T_Chumon> T_Chumon { get; set; }
        public virtual ICollection<T_Order> T_Order { get; set; }
        public virtual ICollection<T_Sale> T_Sale { get; set; }
        public virtual ICollection<T_Shipment> T_Shipment { get; set; }
        public virtual ICollection<T_Syukko> T_Syukko { get; set; }
    }
    class M_ClientDsp
    {
        [DisplayName("顧客ID")]
        public int ClID { get; set; }

        [DisplayName("顧客名")]
        public string ClName { get; set; }

        [DisplayName("営業所ID")]
        public int SoID { get; set; }

        [DisplayName("営業所名")]
        public string SoName { get; set; }

        [DisplayName("住所")]
        public string ClAddress {  get; set; }

        [DisplayName("電話番号")]
        public string ClPhone { get; set; }

        [DisplayName("郵便番号")]
        public string ClPostal { get; set; }

        [DisplayName("FAX")]
        public string ClFAX { get; set; }

        public int ClFlag { get; set; }

        [DisplayName("非表示フラグ")]
        public bool ClBoolFlag { get; set; }

        [DisplayName("非表示理由")]
        public string ClHidden { get; set; }
    }
}
