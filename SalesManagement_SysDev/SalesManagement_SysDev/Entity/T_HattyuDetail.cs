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
    class T_HattyuDetail
    {
        [Key]
        public int HaDetailID { get; set; } //発注詳細ID
        public int HaID { get; set; }       //発注ID
        public int PrID { get; set; }       //商品ID
        public int HaQuantity { get; set; } //数量

        public virtual M_Product M_Product { get; set; }
        public virtual T_Hattyu T_Hattyu { get; set; }
    }

    public class T_HattyuDetailDsp
    {
        [DisplayName("発注詳細ID")]
        public int HaDetailID { get; set; }
        public int HaID { get; set; }
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
        public int HaQuantity { get; set; }
    }
    public class T_HattyuDetailDsp_Agr
    {
        [DisplayName("項番")]
        public int AgrID { get; set; }
        public int HaID { get; set; }
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
        public int HaQuantitySum { get; set; }
        [DisplayName("発注点")]
        public int PrOrderPoint { get; set; }
        [DisplayName("在庫数")]
        public int StQuantity { get; set; }
        [DisplayName("入庫予定")]
        public int PrPreWarehousing { get; set; }
    }
}
