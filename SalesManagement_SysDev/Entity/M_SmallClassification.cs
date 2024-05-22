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
    class M_SmallClassification
    {
        public M_SmallClassification()
        {
            M_Product = new HashSet<M_Product>();
        }

        [Key]
        public int ScID { get; set; }           //小分類ID		
        public int McID { get; set; }           //大分類ID
        [MaxLength(50)]
        [Required]
        public string ScName { get; set; }      //小分類名		
        public int ScFlag { get; set; }         //小分類管理フラグ
        public string ScHidden { get; set; }	//非表示理由		

        public virtual M_MajorClassification M_MajorClassification { get; set; }
        public virtual ICollection<M_Product> M_Product { get; set; }
    }

    class M_SmallClassificationDsp
    {
        [DisplayName("小分類ID")]
        public int ScID { get; set; }

        [DisplayName("大分類ID")]
        public int McID { get; set; }

        [DisplayName("小分類")]
        public string ScName { get; set; }
    }
}
