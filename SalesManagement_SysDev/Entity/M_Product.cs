using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_SysDev
{
    class M_Product
    {
        public M_Product()
        {
            T_ArrivalDetail = new HashSet<T_ArrivalDetail>();
            T_ChumonDetail = new HashSet<T_ChumonDetail>();
            T_HattyuDetail = new HashSet<T_HattyuDetail>();
            T_OrderDetail = new HashSet<T_OrderDetail>();
            T_SaleDetail = new HashSet<T_SaleDetail>();
            T_ShipmentDetail = new HashSet<T_ShipmentDetail>();
            T_Stock = new HashSet<T_Stock>();
            T_SyukkoDetail = new HashSet<T_SyukkoDetail>();
            T_WarehousingDetail = new HashSet<T_WarehousingDetail>();
        }

        [Key]
        public int PrID { get; set; }               //商品ID		
        public int MaID { get; set; }               //メーカID	
        [MaxLength(50)]
        [Required]
        public string PrName { get; set; }          //商品名		
        public decimal Price { get; set; }          //価格	
        [MaxLength(13)]
        public string PrJCode { get; set; }         //JANコード		
        public int PrSafetyStock { get; set; }      //安全在庫数	
        public int ScID { get; set; }               //小分類ID	                                                
        [MaxLength(20)]
        [Required]
        public string PrModelNumber { get; set; }   //型番
        [MaxLength(20)]
        [Required]
        public string PrColor { get; set; }         //色		
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime PrReleaseDate { get; set; } //発売日		
        public int PrFlag { get; set; }             //商品管理フラグ
        public string PrHidden { get; set; }	    //非表示理由		
        public int PrOrderPoint { get; set; }       //発注点
        public int PrOrderQuantity { get; set; }    //発注量

        public virtual M_Maker M_Maker { get; set; }
        public virtual M_SmallClassification M_SmallClassification { get; set; }
        public virtual ICollection<T_ArrivalDetail> T_ArrivalDetail { get; set; }
        public virtual ICollection<T_ChumonDetail> T_ChumonDetail { get; set; }
        public virtual ICollection<T_HattyuDetail> T_HattyuDetail { get; set; }
        public virtual ICollection<T_OrderDetail> T_OrderDetail { get; set; }
        public virtual ICollection<T_SaleDetail> T_SaleDetail { get; set; }
        public virtual ICollection<T_ShipmentDetail> T_ShipmentDetail { get; set; }
        public virtual ICollection<T_Stock> T_Stock { get; set; }
        public virtual ICollection<T_SyukkoDetail> T_SyukkoDetail { get; set; }
        public virtual ICollection<T_WarehousingDetail> T_WarehousingDetail { get; set; }
    }

    public class M_ProductDsp
    {
        [DisplayName("商品ID")]
        public int? PrID { get; set; }               //商品ID  0
        [DisplayName("商品名")]
        public string PrName { get; set; }          //商品名		1
        public int MaID { get; set; }               //メーカID  2
        [DisplayName("メーカ")]
        public string MaName { get; set; }          //メーカ	名  3
        [DisplayName("価格")]
        public decimal Price { get; set; }          //価格	4
        [DisplayName("JANコード")]
        public string PrJCode { get; set; }         //JANコード 5
        public int McID { get; set; }               //大分類ID  6
        [DisplayName("大分類")]
        public string McName { get; set; }          //大分類ID  7
        public int ScID { get; set; }               //小分類ID  8
        [DisplayName("小分類")]
        public string ScName { get; set; }          //小分類	  9
        [DisplayName("型番")]
        public string PrModelNumber { get; set; }   //型番   10
        [DisplayName("色")]
        public string PrColor { get; set; }         //色   11
        [DisplayName("発売日")]
        public DateTime PrReleaseDate { get; set; } //発売日	  12	
        [DisplayName("安全在庫数")]
        public int PrSafetyStock { get; set; }      //安全在庫数  13
        [DisplayName("発注点")]
        public int PrOrderPoint { get; set; }       //発注点  14
        [DisplayName("在庫数")]
        public int StQuantity { get; set; }         //在庫数  15
        [DisplayName("入庫予定")]
        public int PrPreWarehousing { get; set; }   //入庫予定数  16
        [DisplayName("発注量")]
        public int PrOrderQuantity { get; set; }    //発注量  17
        public int PrFlag { get; set; }
        [DisplayName("非表示")]
        public bool PrFlagBool { get; set; }       //商品管理フラグ  18
        [DisplayName("非表示理由")]
        public string PrHidden { get; set; }        //非表示理由   19
    }
}
