using System;
using System.Collections.Generic;
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
}
