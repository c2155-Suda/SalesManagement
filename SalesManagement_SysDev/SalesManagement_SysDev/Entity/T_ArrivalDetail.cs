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
    class T_ArrivalDetail
    {
        [Key]
        public int ArDetailID { get; set; }     //入荷詳細ID
        public int ArID { get; set; }           //入荷ID
        public int PrID { get; set; }           //商品ID
        public int ArQuantity { get; set; }	    //数量

        public virtual M_Product M_Product { get; set; }
        public virtual T_Arrival T_Arrival { get; set; }
    }
    class T_ArrivalDetailDsp
    {
        [DisplayName("入荷詳細ID")]
        public int ArDetailID { get; set; }
        public int ArID { get; set; }
        public int PrID { get; set; }
        [DisplayName("商品")]
        public string PrName { get; set; }
        public int ScID { get; set; }
        [DisplayName("小分類")]
        public string ScName { get; set; }
        public int McID { get; set; }
        [DisplayName("大分類")]
        public string McName { get; set; }
        [DisplayName("数量")]
        public int ArQuantity { get; set; }
    }
}
