using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    class ProductDbConnect
    {
        public List<M_ProductDsp> GetProductData()
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from product in context.M_Products
                             join maker in context.M_Makers on product.MaID equals maker.MaID
                             join smallCategory in context.M_SmallClassifications on product.ScID equals smallCategory.ScID
                             join majorCategory in context.M_MajorCassifications on smallCategory.McID equals majorCategory.McID
                             join stock in context.T_Stocks on product.PrID equals stock.PrID

                             where product.PrFlag == 0

                             select new M_ProductDsp
                             {
                                 MaID = product.MaID,
                                 ScID = product.ScID,
                                 McID = smallCategory.ScID,
                                 PrFlag = product.PrFlag,

                                 PrID = product.PrID,
                                 MaName = maker.MaName,
                                 PrName = product.PrName,
                                 Price = (long)product.Price,
                                 PrJCode = product.PrJCode,
                                 ScName = smallCategory.ScName,
                                 McName = majorCategory.McName,
                                 PrModelNumber = product.PrModelNumber,
                                 PrColor = product.PrColor,
                                 PrReleaseDate = product.PrReleaseDate,
                                 PrSafetyStock = product.PrSafetyStock,
                                 PrOrderPoint = product.PrOrderPoint,
                                 PrOrderQuantity = product.PrOrderQuantity,
                                 StQuantity = stock.StQuantity,
                                 PrPreWarehousing = (from hattyu in context.T_Hattyus
                                                     join hadetails in context.T_HattyuDetails on hattyu.HaID equals hadetails.HaID
                                                     where hadetails.PrID == product.PrID && hattyu.WaWarehouseFlag == 1
                                                     select (int?)hadetails.HaQuantity).Sum() ?? 0,
                                 PrFlagBool = product.PrFlag == 0 ? false : true,
                                 PrHidden = product.PrHidden

                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<M_ProductDsp> GetProductData(M_ProductDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {

                    var tb = from product in context.M_Products
                             join maker in context.M_Makers on product.MaID equals maker.MaID
                             join smallCategory in context.M_SmallClassifications on product.ScID equals smallCategory.ScID
                             join majorCategory in context.M_MajorCassifications on smallCategory.McID equals majorCategory.McID
                             join stock in context.T_Stocks on product.PrID equals stock.PrID

                             where (selectCondition.PrID != 0 ? product.PrID == selectCondition.PrID : true) &&
                             (selectCondition.MaID != 0 ? product.MaID == selectCondition.MaID : true) &&
                             (selectCondition.ScID != 0 ? product.ScID == selectCondition.ScID : true) &&
                             (selectCondition.McID != 0 ? smallCategory.McID == selectCondition.McID : true) &&
                             (!string.IsNullOrEmpty(selectCondition.PrHidden) ? product.PrHidden.Contains(selectCondition.PrHidden) : true) &&
                             (!string.IsNullOrEmpty(selectCondition.PrJCode) ? product.PrJCode.Contains(selectCondition.PrJCode) : true) &&
                             product.PrName.Contains(selectCondition.PrName) &&
                             product.PrModelNumber.Contains(selectCondition.PrModelNumber) &&
                             product.PrColor.Contains(selectCondition.PrColor) &&
                             product.PrFlag == selectCondition.PrFlag

                             select new M_ProductDsp
                             {
                                 MaID = product.MaID,
                                 ScID = product.ScID,
                                 McID = smallCategory.ScID,
                                 PrFlag = product.PrFlag,

                                 PrID = product.PrID,
                                 MaName = maker.MaName,
                                 PrName = product.PrName,
                                 Price = (long)product.Price,
                                 PrJCode = product.PrJCode,
                                 ScName = smallCategory.ScName,
                                 McName = majorCategory.McName,
                                 PrModelNumber = product.PrModelNumber,
                                 PrColor = product.PrColor,
                                 PrReleaseDate = product.PrReleaseDate,
                                 PrSafetyStock = product.PrSafetyStock,
                                 PrOrderPoint = product.PrOrderPoint,
                                 PrOrderQuantity = product.PrOrderQuantity,
                                 StQuantity = stock.StQuantity,
                                 PrPreWarehousing = (from hattyu in context.T_Hattyus
                                                     join hadetails in context.T_HattyuDetails on hattyu.HaID equals hadetails.HaID
                                                     where hadetails.PrID == product.PrID && hattyu.WaWarehouseFlag == 1
                                                     select (int?)hadetails.HaQuantity).Sum() ?? 0,
                                 PrFlagBool = product.PrFlag == 0 ? false : true,
                                 PrHidden = product.PrHidden

                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<M_ProductDsp> GetProductData(M_ProductDsp selectCondition, DateTime? startDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from product in context.M_Products
                             join maker in context.M_Makers on product.MaID equals maker.MaID
                             join smallCategory in context.M_SmallClassifications on product.ScID equals smallCategory.ScID
                             join majorCategory in context.M_MajorCassifications on smallCategory.McID equals majorCategory.McID
                             join stock in context.T_Stocks on product.PrID equals stock.PrID

                             where (selectCondition.PrID != 0 ? product.PrID == selectCondition.PrID : true) &&
                             (selectCondition.MaID != 0 ? product.MaID == selectCondition.MaID : true) &&
                             (selectCondition.ScID != 0 ? product.ScID == selectCondition.ScID : true) &&
                             (selectCondition.McID != 0 ? smallCategory.McID == selectCondition.McID : true) &&
                             (!string.IsNullOrEmpty(selectCondition.PrHidden) ? product.PrHidden.Contains(selectCondition.PrHidden) : true) &&
                             (!string.IsNullOrEmpty(selectCondition.PrJCode) ? product.PrJCode.Contains(selectCondition.PrJCode) : true) &&
                             product.PrName.Contains(selectCondition.PrName) &&
                             product.PrModelNumber.Contains(selectCondition.PrModelNumber) &&
                             product.PrColor.Contains(selectCondition.PrColor) &&
                             product.PrFlag == selectCondition.PrFlag &&

                             (startDay == null ? product.PrReleaseDate <= endDay : endDay == null ? product.PrReleaseDate >= startDay : (product.PrReleaseDate >= startDay && product.PrReleaseDate <= endDay))

                             select new M_ProductDsp
                             {
                                 MaID = product.MaID,
                                 ScID = product.ScID,
                                 McID = smallCategory.ScID,
                                 PrFlag = product.PrFlag,

                                 PrID = product.PrID,
                                 MaName = maker.MaName,
                                 PrName = product.PrName,
                                 Price = (long)product.Price,
                                 PrJCode = product.PrJCode,
                                 ScName = smallCategory.ScName,
                                 McName = majorCategory.McName,
                                 PrModelNumber = product.PrModelNumber,
                                 PrColor = product.PrColor,
                                 PrReleaseDate = product.PrReleaseDate,
                                 PrSafetyStock = product.PrSafetyStock,
                                 PrOrderPoint = product.PrOrderPoint,
                                 PrOrderQuantity = product.PrOrderQuantity,
                                 StQuantity = stock.StQuantity,
                                 PrPreWarehousing = (from hattyu in context.T_Hattyus
                                                     join hadetails in context.T_HattyuDetails on hattyu.HaID equals hadetails.HaID
                                                     where hadetails.PrID == product.PrID && hattyu.WaWarehouseFlag == 1
                                                     select (int?)hadetails.HaQuantity).Sum() ?? 0,
                                 PrFlagBool = product.PrFlag == 0 ? false : true,
                                 PrHidden = product.PrHidden

                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public void RegistProductData(M_Product regDataProduct)
        {
            using (var context = new SalesManagement_DevContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.M_Products.Add(regDataProduct);
                        context.SaveChanges();
                        if (regDataProduct.PrID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            throw new Exception("商品IDが既定の数字を超えています。");
                        }

                        var regDataStock = new T_Stock { M_Product = regDataProduct };
                        context.T_Stocks.Add(regDataStock);
                        context.SaveChanges();
                        if (regDataStock.StID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            throw new Exception("在庫IDが既定の数字を超えています。");
                        }
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;

                    }
                }
            }
        }
        public void UpdateProductData(M_Product updDataProduct, T_Stock updDataStock)
        {
            using (var context = new SalesManagement_DevContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var productTarget = context.M_Products.Single(x => x.PrID == updDataProduct.PrID);
                        var stockTarget = context.T_Stocks.Single(x => x.PrID == updDataProduct.PrID);

                        productTarget.PrName = updDataProduct.PrName;
                        productTarget.MaID = updDataProduct.MaID;
                        productTarget.Price = updDataProduct.Price;
                        productTarget.PrJCode = updDataProduct.PrJCode;
                        productTarget.PrSafetyStock = updDataProduct.PrSafetyStock;
                        productTarget.ScID = updDataProduct.ScID;
                        productTarget.PrModelNumber = updDataProduct.PrModelNumber;
                        productTarget.PrColor = updDataProduct.PrColor;
                        productTarget.PrFlag = updDataProduct.PrFlag;
                        productTarget.PrHidden = updDataProduct.PrHidden;
                        productTarget.PrOrderPoint = updDataProduct.PrOrderPoint;
                        productTarget.PrOrderQuantity = updDataProduct.PrOrderQuantity;
                        productTarget.PrReleaseDate = updDataProduct.PrReleaseDate;

                        context.SaveChanges();

                        stockTarget.StQuantity = updDataStock.StQuantity;
                        context.SaveChanges();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public bool CheckProductExistence(int Prid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    return context.M_Products.Any(x => x.PrID == Prid);
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckProductIsActive(int Prid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.M_Products.Single(x => x.PrID == Prid);
                    if (target.PrFlag == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        
    }
}
