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
    class M_Maker
    {
        public M_Maker()
        {
            M_Product = new HashSet<M_Product>();
            T_Hattyu = new HashSet<T_Hattyu>();
        }

        [Key]
        public int MaID { get; set; }           //メーカID
        [MaxLength(50)]
        [Required]
        public string MaName { get; set; }      //メーカ名	 
        [MaxLength(50)]
        [Required]
        public string MaAddress { get; set; }   //住所
        [MaxLength(13)]
        [Required]
        public string MaPhone { get; set; }     //電話番号
        [MaxLength(7)]
        [Required]
        public string MaPostal { get; set; }    //郵便番号
        [MaxLength(13)]
        [Required]
        public string MaFAX { get; set; }       //FAX		
        public int MaFlag { get; set; }         //メーカ管理フラグ
        public string MaHidden { get; set; }	//非表示理由		

        public virtual ICollection<M_Product> M_Product { get; set; }
        public virtual ICollection<T_Hattyu> T_Hattyu { get; set; }
    }

    class M_MakerDsp
    {
        [DisplayName("メーカーID")]
        public int MaID { get; set; }

        [DisplayName("メーカー名")]
        public string MaName { get; set; }
    }
}
