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
    class T_WarehousingDetail
    {
        [Key]
        public int WaDetailID { get; set; }     //入庫詳細ID
        public int WaID { get; set; }           //入庫ID
        public int PrID { get; set; }           //商品ID
        public int WaQuantity { get; set; }     //数量

        public virtual M_Product M_Product { get; set; }
        public virtual T_Warehousing T_Warehousing { get; set; }
    }
    class T_WarehousingDetailDsp
    {
        [DisplayName("入庫詳細ID")]
        public int? WaDetailID { get; set; }
        public int WaID { get; set; }
        public int PrID { get; set; }
        [DisplayName("商品")]
        public string PrName { get; set; }
        public int McID { get; set; }
        [DisplayName("大分類")]
        public string McName { get; set; }
        public int ScID { get; set; }
        [DisplayName("小分類")]
        public string ScName { get; set; }
        [DisplayName("数量")]
        public int WaQuantity { get; set; }
    }
}
