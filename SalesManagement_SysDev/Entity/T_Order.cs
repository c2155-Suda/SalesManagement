using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;


namespace SalesManagement_SysDev
{
    class T_Order
    {
        public T_Order()
        {
            T_Arrival = new HashSet<T_Arrival>();
            T_Chumon = new HashSet<T_Chumon>();
            T_OrderDetail = new HashSet<T_OrderDetail>();
            T_Shipment = new HashSet<T_Shipment>();
            T_Syukko = new HashSet<T_Syukko>();
            T_Sale = new HashSet<T_Sale>();
        }

        [Key]
        public int OrID { get; set; }           //受注ID		
        public int SoID { get; set; }           //営業所ID		
        public int EmID { get; set; }           //社員ID		
        public int ClID { get; set; }           //顧客ID
        [MaxLength(50)]
        [Required]
        public string ClCharge { get; set; }    //顧客担当者名
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime OrDate { get; set; }    //受注年月日
        public int OrStateFlag { get; set; }    //受注状態フラグ
        public int OrFlag { get; set; }         //受注管理フラグ
        public string OrHidden { get; set; }    //非表示理由

        public virtual M_Client M_Client { get; set; }
        public virtual M_Employee M_Employee { get; set; }
        public virtual M_SalesOffice M_SalesOffice { get; set; }
        public virtual ICollection<T_Arrival> T_Arrival { get; set; }
        public virtual ICollection<T_Chumon> T_Chumon { get; set; }
        public virtual ICollection<T_OrderDetail> T_OrderDetail { get; set; }
        public virtual ICollection<T_Shipment> T_Shipment { get; set; }
        public virtual ICollection<T_Syukko> T_Syukko { get; set; }
        public virtual ICollection<T_Sale> T_Sale { get; set; }
    }
    class T_OrderRead
    {
        public int OrID { get; set; }
        public int SoID { get; set; }
        public int EmID { get; set; }
        public int ClID { get; set; }
        public string ClCharge { get; set; }
        public DateTime OrDate { get; set; }
        public int OrStateFlag { get; set; }
        public int OrFlag { get; set; }
        public string OrHidden { get; set; }
        public int PrID { get; set; }
        public int ScID { get; set; }
        public int McID { get; set; }
    }
    class T_OrderDsp
    {
        [DisplayName("受注ID")]
        public int? OrID { get; set; }
        public int ClID { get; set; }
        [DisplayName("顧客")]
        public string ClName { get; set; }
        [DisplayName("顧客担当者名")]
        public string ClCharge { get; set; }
        public int SoID { get; set; }
        [DisplayName("営業所")]
        public string SoName { get; set; }
        public int EmID { get; set; }
        [DisplayName("社員")]
        public string EmName { get; set; }
        [DisplayName("受注日")]
        public DateTime OrDate { get; set; }
        public int OrFlag { get; set; }
        [DisplayName("非表示")]
        public bool OrFlagBool { get; set; }
        [DisplayName("非表示理由")]
        public string OrHidden { get; set; }
        [DisplayName("品数")]
        public int KindOfProducts { get; set; }
        public int OrStateFlag { get; set; }
        [DisplayName("ステータス")]
        public string OrStateString { get; set; }
        public int ClientFlag { get; set; }
        public int EmployeeFlag { get; set; }
    }

}
