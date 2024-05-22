using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
                                 McID = smallCategory.McID,
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

                             where (selectCondition.PrID == 0||product.PrID == selectCondition.PrID) &&
                             (selectCondition.MaID == 0 ||(selectCondition.MaID==-1&&maker.MaFlag!=0)|| product.MaID == selectCondition.MaID) &&
                             (selectCondition.ScID == 0 ||(selectCondition.ScID==-1&&smallCategory.ScFlag!=0)|| product.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 ||(selectCondition.McID==-1&&majorCategory.McFlag!=0)|| smallCategory.McID == selectCondition.McID) &&
                             (string.IsNullOrEmpty(selectCondition.PrHidden) || product.PrHidden.Contains(selectCondition.PrHidden)) &&
                             (string.IsNullOrEmpty(selectCondition.PrJCode) || product.PrJCode.Contains(selectCondition.PrJCode)) &&
                             product.PrName.Contains(selectCondition.PrName) &&
                             product.PrModelNumber.Contains(selectCondition.PrModelNumber) &&
                             product.PrColor.Contains(selectCondition.PrColor) &&
                             (selectCondition.PrFlag == -1 || product.PrFlag == selectCondition.PrFlag)

                             select new M_ProductDsp
                             {
                                 MaID = product.MaID,
                                 ScID = product.ScID,
                                 McID = smallCategory.McID,
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
                             (selectCondition.MaID == 0 || (selectCondition.MaID == -1 && maker.MaFlag != 0) || product.MaID == selectCondition.MaID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && smallCategory.ScFlag != 0) || product.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && majorCategory.McFlag != 0) || smallCategory.McID == selectCondition.McID) &&
                             (!string.IsNullOrEmpty(selectCondition.PrHidden) ? product.PrHidden.Contains(selectCondition.PrHidden) : true) &&
                             (!string.IsNullOrEmpty(selectCondition.PrJCode) ? product.PrJCode.Contains(selectCondition.PrJCode) : true) &&
                             product.PrName.Contains(selectCondition.PrName) &&
                             product.PrModelNumber.Contains(selectCondition.PrModelNumber) &&
                             product.PrColor.Contains(selectCondition.PrColor) &&
                             (selectCondition.PrFlag==-1||product.PrFlag == selectCondition.PrFlag) &&

                             (startDay == null ? product.PrReleaseDate <= endDay : endDay == null ? product.PrReleaseDate >= startDay : (product.PrReleaseDate >= startDay && product.PrReleaseDate <= endDay))

                             select new M_ProductDsp
                             {
                                 MaID = product.MaID,
                                 ScID = product.ScID,
                                 McID = smallCategory.McID,
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
        public int ProductSumPreWarehousing(int Prid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    int res = (from hattyu in context.T_Hattyus
                               join hadetails in context.T_HattyuDetails on hattyu.HaID equals hadetails.HaID
                               where hadetails.PrID == Prid &&
                               hattyu.WaWarehouseFlag == 1
                               select (int?)hadetails.HaQuantity).Sum() ?? 0;
                    return res;
                }
            }
            catch
            {
                throw;
            }
        }
        public int ProductSumPreChumon(int Prid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    int res = (from order in context.T_Orders
                               join ordetails in context.T_OrderDetails on order.OrID equals ordetails.OrID
                               where ordetails.PrID == Prid &&
                               order.OrStateFlag == 1
                               select (int?)ordetails.OrQuantity).Sum() ?? 0;
                    return res;
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
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('M_Product',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('M_Product')");
                            throw new Exception("ログイン認証IDが既定の数字を超えています。");
                            throw new Exception("商品IDが既定の数字を超えています。");
                        }

                        var regDataStock = new T_Stock { M_Product = regDataProduct };
                        context.T_Stocks.Add(regDataStock);
                        context.SaveChanges();
                        if (regDataStock.StID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Stock',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Stock')");
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
        public int GetStockID(int Prid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var stock = context.T_Stocks.Single(x => x.PrID == Prid);
                    return stock.StID;
                }
            }
            catch
            {
                throw;
            }
        }
        public int GetStockQuant(int Prid)
        {
            try
            {
                using(var context=new SalesManagement_DevContext())
                {
                    var stock = context.T_Stocks.Single(x => x.PrID == Prid);
                    return stock.StQuantity;
                }
            }
            catch
            {
                throw;
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
