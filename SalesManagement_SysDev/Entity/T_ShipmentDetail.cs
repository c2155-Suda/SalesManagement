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
    class T_ShipmentDetail
    {
        [Key]
        public int ShDetailID { get; set; }     //出荷詳細ID
        public int ShID { get; set; }           //出荷ID
        public int PrID { get; set; }           //商品ID
        public int ShQuantity { get; set; } //数量

        public virtual M_Product M_Product { get; set; }
        public virtual T_Shipment T_Shipment { get; set; }
    }
    class T_ShipmentDetailDsp
    {
        [DisplayName("出荷詳細ID")]
        public int? ShDetailID { get; set; }
        public int ShID { get; set; }
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
        public int ShQuantity { get; set; }
    }
}
