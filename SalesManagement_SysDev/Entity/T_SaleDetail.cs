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
    class T_SaleDetail
    {
        [Key]
        public int SaDetailID { get; set; }         //売上明細ID
        public int SaID { get; set; }               //売上ID
        public int PrID { get; set; }               //商品ID
        public int SaQuantity { get; set; }         //個数
        public decimal SaTotalPrice { get; set; }   //合計金額

        public virtual M_Product M_Product { get; set; }
        public virtual T_Sale T_Sale { get; set; }
    }
    class T_SaleDetailDsp
    {
        [DisplayName("売上詳細ID")]
        public int? SaDetailID { get; set; }
        public int SaID { get; set; }
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
        public int SaQuantity { get; set; }
        [DisplayName("金額")]
        public decimal SaPrice { get; set; }
    }
}
