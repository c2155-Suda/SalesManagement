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
    class M_Employee
    {
        public M_Employee()
        {
            T_Arrival = new HashSet<T_Arrival>();
            T_Chumon = new HashSet<T_Chumon>();
            T_Hattyu = new HashSet<T_Hattyu>();
            T_Order = new HashSet<T_Order>();
            T_Sale = new HashSet<T_Sale>();
            T_Shipment = new HashSet<T_Shipment>();
            T_Syukko = new HashSet<T_Syukko>();
            T_Warehousing = new HashSet<T_Warehousing>();
            T_LoginHistory = new HashSet<T_LoginHistory>();
            T_OperationHistory = new HashSet<T_OperationHistory>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EmID { get; set; }               //社員ID
        [MaxLength(50)]
        [Required]
        public string EmName { get; set; }          //社員名		
        public int SoID { get; set; }               //営業所ID		
        public int PoID { get; set; }               //役職ID
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EmHiredate { get; set; }    //入社年月日
        [MaxLength(10)]
        [Required]
        public string EmPassword { get; set; }      //パスワード
        [MaxLength(13)]
        [Required]
        public string EmPhone { get; set; }         //電話番号	
        //[MaxLength(13)]
        //[Required]
        // public String EmBarcode { get; set; }    //社員バーコード		
        public int EmFlag { get; set; }             //社員管理フラグ
        public string EmHidden { get; set; }	    //非表示理由		

        public virtual M_Position M_Position { get; set; }
        public virtual M_SalesOffice M_SalesOffice { get; set; }
        public virtual ICollection<T_Arrival> T_Arrival { get; set; }
        public virtual ICollection<T_Chumon> T_Chumon { get; set; }
        public virtual ICollection<T_Hattyu> T_Hattyu { get; set; }
        public virtual ICollection<T_Order> T_Order { get; set; }
        public virtual ICollection<T_Sale> T_Sale { get; set; }
        public virtual ICollection<T_Shipment> T_Shipment { get; set; }
        public virtual ICollection<T_Syukko> T_Syukko { get; set; }
        public virtual ICollection<T_Warehousing> T_Warehousing { get; set; }
        public virtual ICollection<T_LoginHistory> T_LoginHistory { get; set; }
        public virtual ICollection<T_OperationHistory> T_OperationHistory { get; set; }
    }

    class M_EmployeeDsp
    {
        [DisplayName("社員ID")]
        public int EmID { get; set; }

        [DisplayName("社員名")]
        public string EmName { get; set; }

        [DisplayName("営業所ID")]
        public int SoID { get; set; }

        [DisplayName("営業所名")]
        public string SoName { get; set; }

        [DisplayName("役職ID")]
        public int PoID { get; set; }

        [DisplayName("役職名")]
        public string PoName { get; set; }

        [DisplayName("入社年月日")]
        public DateTime EmHiredate { get; set; }

        [DisplayName("パスワード")]
        public string EmPassword { get; set; }

        [DisplayName("電話番号")]
        public string EmPhone { get; set; }

        
        public int EmFlag { get; set; }

        [DisplayName("非表示フラグ")]
        public bool EmBoolFlag { get; set; }


        [DisplayName("非表示理由")]
        public string EmHidden { get; set; }

    }
}
