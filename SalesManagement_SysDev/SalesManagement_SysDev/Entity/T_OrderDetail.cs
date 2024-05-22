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
    class T_OrderDetail
    {
        [Key]
        public int OrDetailID { get; set; }         //受注詳細ID
        public int OrID { get; set; }               //受注ID
        public int PrID { get; set; }               //商品ID
        public int OrQuantity { get; set; }	        //数量
        public decimal OrTotalPrice { get; set; }   //合計金額

        public virtual M_Product M_Product { get; set; }
        public virtual T_Order T_Order { get; set; }
    }
    public class T_OrderDetailDsp
    {
        [DisplayName("受注詳細ID")]
        public int OrDetailID { get; set; }
        public int OrID { get; set; }
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
        public int OrQuantity { get; set; }
        public decimal OrPrice { get; set; }
    }
    public class T_OrderDetailDsp_Agr_NonFin
    {
        [DisplayName("項番")]
        public int AgrID { get; set; }
        public int OrID { get; set; }
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
        public int OrQuantitySum { get; set; }
        [DisplayName("金額(暫定)")]
        public decimal OrPriceSum { get; set; }
    }
    public class T_OrderDetailDsp_Agr_Fin
    {
        [DisplayName("項番")]
        public int AgrID { get; set; }
        public int OrID { get; set; }
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
        public int OrQuantitySum { get; set; }
        [DisplayName("金額")]
        public decimal OrPriceSum { get; set; }
    }
}
