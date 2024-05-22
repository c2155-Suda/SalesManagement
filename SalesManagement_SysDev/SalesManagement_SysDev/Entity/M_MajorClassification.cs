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
    class M_MajorClassification
    {
        public M_MajorClassification()
        {
            M_SmallClassification = new HashSet<M_SmallClassification>();
        }

        [Key]
        public int McID { get; set; }           //大分類ID
        [MaxLength(50)]
        [Required]
        public string McName { get; set; }      //大分類名		
        public int McFlag { get; set; }         //大分類管理フラグ
        public string McHidden { get; set; }	//非表示理由		

        public virtual ICollection<M_SmallClassification> M_SmallClassification { get; set; }
    }

    class M_MajorClassificationDsp
    {
        [DisplayName("大分類ID")]
        public int McID { get; set; }

        [DisplayName("大分類")]
        public string McName { get; set; }
    }
}
