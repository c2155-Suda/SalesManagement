using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SalesManagement_SysDev
{
    class HattyuDbConnection
    {
        static string[] StateFlagStrings = { string.Empty, "発注済", "入庫済" };
        public List<T_HattyuDsp> GetHattyuData()
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from hattyu in context.T_Hattyus
                             join hattyuDet in context.T_HattyuDetails on hattyu.HaID equals hattyuDet.HaID
                             into gjHaDet
                             from subHaDet in gjHaDet.DefaultIfEmpty()
                             join product in context.M_Products on subHaDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join sc in context.M_SmallClassifications on subProduct.ScID equals sc.ScID
                             into gjSmallClass
                             from subSmallClass in gjSmallClass.DefaultIfEmpty()
                             join mc in context.M_MajorCassifications on subSmallClass.McID equals mc.McID
                             into gjMajorClass
                             from subMajorClass in gjMajorClass.DefaultIfEmpty()
                             join maker in context.M_Makers on hattyu.MaID equals maker.MaID
                             join employee in context.M_Employees on hattyu.EmID equals employee.EmID

                             where hattyu.HaFlag == 0

                             group new { hattyu, maker, employee } by new { hattyu.HaID } into g

                             select new T_HattyuDsp
                             {
                                 MaID = g.FirstOrDefault().hattyu.MaID,
                                 EmID = g.FirstOrDefault().employee.EmID,
                                 HaFlag = g.FirstOrDefault().hattyu.HaFlag,
                                 WaWarehousingFlag = g.FirstOrDefault().hattyu.WaWarehouseFlag,

                                 HaID = g.FirstOrDefault().hattyu.HaID,
                                 MaName = g.FirstOrDefault().maker.MaName,
                                 EmName = g.FirstOrDefault().employee.EmName,
                                 HaDate = g.FirstOrDefault().hattyu.HaDate,
                                 HaFlagBool = g.FirstOrDefault().hattyu.HaFlag == 0 ? false : true,
                                 HaHidden = g.FirstOrDefault().hattyu.HaHidden,
                                 KindOfProducts = context.T_HattyuDetails.Where(x => x.HaID == g.FirstOrDefault().hattyu.HaID).GroupBy(x => x.PrID).Where(x => x.Sum(y => y.HaQuantity) > 0).Count(),
                                 MakerFlag=g.FirstOrDefault().maker.MaFlag,
                                 EmployeeFlag=g.FirstOrDefault().employee.EmFlag
                             };
                    var res = new List<T_HattyuDsp>();
                    foreach (var row in tb)
                    {
                        row.WaWarehousingFlagString = StateFlagStrings[row.WaWarehousingFlag];
                        res.Add(row);
                    }
                    return res;
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_HattyuDsp> GetHattyuData(T_HattyuRead selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from hattyu in context.T_Hattyus
                             join hattyuDet in context.T_HattyuDetails on hattyu.HaID equals hattyuDet.HaID
                             into gjHaDet
                             from subHaDet in gjHaDet.DefaultIfEmpty()
                             join product in context.M_Products on subHaDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join sc in context.M_SmallClassifications on subProduct.ScID equals sc.ScID
                             into gjSmallClass
                             from subSmallClass in gjSmallClass.DefaultIfEmpty()
                             join mc in context.M_MajorCassifications on subSmallClass.McID equals mc.McID
                             into gjMajorClass
                             from subMajorClass in gjMajorClass.DefaultIfEmpty()
                             join maker in context.M_Makers on hattyu.MaID equals maker.MaID
                             join employee in context.M_Employees on hattyu.EmID equals employee.EmID

                             where (selectCondition.HaID==0||selectCondition.HaID==hattyu.HaID) &&
                                   (selectCondition.MaID==0||(selectCondition.MaID==-1&&maker.MaFlag!=0)||selectCondition.MaID==hattyu.MaID) &&
                                   (selectCondition.EmID==0||(selectCondition.EmID==-1&&employee.EmFlag!=0)||selectCondition.EmID==hattyu.EmID) &&
                                   (selectCondition.ScID==0||(selectCondition.ScID==-1&&subSmallClass.ScFlag!=0)||selectCondition.ScID==subProduct.ScID) &&
                                   (selectCondition.McID==0||(selectCondition.McID==-1&&subMajorClass.McFlag!=0)||selectCondition.McID==subSmallClass.McID) &&
                                   (selectCondition.PrID==0||(selectCondition.PrID==-1&&subProduct.PrFlag!=0)||selectCondition.PrID==subHaDet.PrID) &&
                                   (!string.IsNullOrEmpty(selectCondition.HaHidden) ? hattyu.HaHidden.Contains(selectCondition.HaHidden) : true) &&
                                   (selectCondition.HaFlag==-1||hattyu.HaFlag == selectCondition.HaFlag) &&
                                   (selectCondition.WaWarehouseFlag != -1 ? hattyu.WaWarehouseFlag == selectCondition.WaWarehouseFlag : true)

                             group new { hattyu, maker, employee } by new { hattyu.HaID } into g

                             select new T_HattyuDsp
                             {
                                 MaID = g.FirstOrDefault().hattyu.MaID,
                                 EmID = g.FirstOrDefault().employee.EmID,
                                 HaFlag = g.FirstOrDefault().hattyu.HaFlag,
                                 WaWarehousingFlag = g.FirstOrDefault().hattyu.WaWarehouseFlag,

                                 HaID = g.FirstOrDefault().hattyu.HaID,
                                 MaName = g.FirstOrDefault().maker.MaName,
                                 EmName = g.FirstOrDefault().employee.EmName,
                                 HaDate = g.FirstOrDefault().hattyu.HaDate,
                                 HaFlagBool = g.FirstOrDefault().hattyu.HaFlag == 0 ? false : true,
                                 HaHidden = g.FirstOrDefault().hattyu.HaHidden,
                                 KindOfProducts = context.T_HattyuDetails.Where(x => x.HaID == g.FirstOrDefault().hattyu.HaID).GroupBy(x => x.PrID).Where(x => x.Sum(y => y.HaQuantity) > 0).Count(),
                                 MakerFlag = g.FirstOrDefault().maker.MaFlag,
                                 EmployeeFlag = g.FirstOrDefault().employee.EmFlag
                             };
                    var res = new List<T_HattyuDsp>();
                    foreach (var row in tb)
                    {
                        row.WaWarehousingFlagString = StateFlagStrings[row.WaWarehousingFlag];
                        res.Add(row);
                    }
                    return res;
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_HattyuDsp> GetHattyuData(T_HattyuRead selectCondition, DateTime? startDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from hattyu in context.T_Hattyus
                             join hattyuDet in context.T_HattyuDetails on hattyu.HaID equals hattyuDet.HaID
                             into gjHaDet
                             from subHaDet in gjHaDet.DefaultIfEmpty()
                             join product in context.M_Products on subHaDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join sc in context.M_SmallClassifications on subProduct.ScID equals sc.ScID
                             into gjSmallClass
                             from subSmallClass in gjSmallClass.DefaultIfEmpty()
                             join mc in context.M_MajorCassifications on subSmallClass.McID equals mc.McID
                             into gjMajorClass
                             from subMajorClass in gjMajorClass.DefaultIfEmpty()
                             join maker in context.M_Makers on hattyu.MaID equals maker.MaID
                             join employee in context.M_Employees on hattyu.EmID equals employee.EmID

                             where (selectCondition.HaID == 0 || selectCondition.HaID == hattyu.HaID) &&
                                   (selectCondition.MaID == 0 || (selectCondition.MaID == -1 && maker.MaFlag != 0) || selectCondition.MaID == hattyu.MaID) &&
                                   (selectCondition.EmID == 0 || (selectCondition.EmID == -1 && employee.EmFlag != 0) || selectCondition.EmID == hattyu.EmID) &&
                                   (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && subSmallClass.ScFlag != 0) || selectCondition.ScID == subProduct.ScID) &&
                                   (selectCondition.McID == 0 || (selectCondition.McID == -1 && subMajorClass.McFlag != 0) || selectCondition.McID == subSmallClass.McID) &&
                                   (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && subProduct.PrFlag != 0) || selectCondition.PrID == subHaDet.PrID) &&
                                   (!string.IsNullOrEmpty(selectCondition.HaHidden) ? hattyu.HaHidden.Contains(selectCondition.HaHidden) : true) &&
                                   (selectCondition.HaFlag == -1 || hattyu.HaFlag == selectCondition.HaFlag) &&
                                   (selectCondition.WaWarehouseFlag != -1 ? hattyu.WaWarehouseFlag == selectCondition.WaWarehouseFlag : true) &&

                                   (startDay == null ? hattyu.HaDate <= endDay : endDay == null ? hattyu.HaDate >= startDay : (hattyu.HaDate >= startDay && hattyu.HaDate <= endDay))

                             group new { hattyu, maker, employee } by new { hattyu.HaID } into g

                             select new T_HattyuDsp
                             {
                                 MaID = g.FirstOrDefault().hattyu.MaID,
                                 EmID = g.FirstOrDefault().employee.EmID,
                                 HaFlag = g.FirstOrDefault().hattyu.HaFlag,
                                 WaWarehousingFlag = g.FirstOrDefault().hattyu.WaWarehouseFlag,

                                 HaID = g.FirstOrDefault().hattyu.HaID,
                                 MaName = g.FirstOrDefault().maker.MaName,
                                 EmName = g.FirstOrDefault().employee.EmName,
                                 HaDate = g.FirstOrDefault().hattyu.HaDate,
                                 HaFlagBool = g.FirstOrDefault().hattyu.HaFlag == 0 ? false : true,
                                 HaHidden = g.FirstOrDefault().hattyu.HaHidden,
                                 KindOfProducts = context.T_HattyuDetails.Where(x => x.HaID == g.FirstOrDefault().hattyu.HaID).GroupBy(x => x.PrID).Where(x => x.Sum(y => y.HaQuantity) > 0).Count(),
                                 MakerFlag = g.FirstOrDefault().maker.MaFlag,
                                 EmployeeFlag = g.FirstOrDefault().employee.EmFlag
                             };
                    var res = new List<T_HattyuDsp>();
                    foreach (var row in tb)
                    {
                        row.WaWarehousingFlagString = StateFlagStrings[row.WaWarehousingFlag];
                        res.Add(row);
                    }
                    return res;
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_HattyuDetailDsp> GetHattyuDetailData(T_HattyuDetailDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from hattyuDet in context.T_HattyuDetails
                             join product in context.M_Products on hattyuDet.PrID equals product.PrID
                             join sc in context.M_SmallClassifications on product.ScID equals sc.ScID
                             join mc in context.M_MajorCassifications on sc.McID equals mc.McID

                             where hattyuDet.HaID == selectCondition.HaID &&
                                   (selectCondition.HaDetailID != 0 ? hattyuDet.HaDetailID == selectCondition.HaDetailID : true) &&
                                   (selectCondition.ScID==0||(selectCondition.ScID==-1&&sc.ScFlag!=0)||selectCondition.ScID==product.ScID) &&
                                   (selectCondition.McID==0||(selectCondition.McID==-1&&mc.McFlag!=0)||selectCondition.McID==sc.McID) &&
                                   (selectCondition.PrID==0||(selectCondition.PrID==-1&&product.PrFlag!=0)||selectCondition.PrID==hattyuDet.PrID)

                             select new T_HattyuDetailDsp
                             {
                                 HaID = hattyuDet.HaID,
                                 PrID = product.PrID,
                                 ScID = product.ScID,
                                 McID = sc.McID,

                                 HaDetailID = hattyuDet.HaDetailID,
                                 PrName = product.PrName + "(" + product.PrColor + ")",
                                 ScName = sc.ScName,
                                 McName = mc.McName,
                                 HaQuantity = hattyuDet.HaQuantity,
                                 ProduntFlag=product.PrFlag
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_HattyuDetailDsp_Agr> GetHattyuDetailData_Agr(T_HattyuDetailDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from hattyuDet in context.T_HattyuDetails
                             join product in context.M_Products on hattyuDet.PrID equals product.PrID
                             join sc in context.M_SmallClassifications on product.ScID equals sc.ScID
                             join mc in context.M_MajorCassifications on sc.McID equals mc.McID
                             join st in context.T_Stocks on product.PrID equals st.PrID

                             where (hattyuDet.HaID == selectCondition.HaID) &&
                                   (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && sc.ScFlag != 0) || selectCondition.ScID == product.ScID) &&
                                   (selectCondition.McID == 0 || (selectCondition.McID == -1 && mc.McFlag != 0) || selectCondition.McID == sc.McID) &&
                                   (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && product.PrFlag != 0) || selectCondition.PrID == hattyuDet.PrID)

                             group new { product, sc, mc, st, hattyuDet } by new { product.PrID } into g

                             where g.Sum(x => x.hattyuDet.HaQuantity) > 0

                             select new T_HattyuDetailDsp_Agr
                             {
                                 HaID = g.FirstOrDefault().hattyuDet.HaID,
                                 PrID = g.FirstOrDefault().product.PrID,
                                 ScID = g.FirstOrDefault().sc.ScID,
                                 McID = g.FirstOrDefault().mc.McID,

                                 PrName = g.FirstOrDefault().product.PrName + "(" + g.FirstOrDefault().product.PrColor + ")",
                                 ScName = g.FirstOrDefault().sc.ScName,
                                 McName = g.FirstOrDefault().mc.McName,
                                 PrSafetyStock=g.FirstOrDefault().product.PrSafetyStock,
                                 PrOrderPoint = g.FirstOrDefault().product.PrOrderPoint,
                                 StQuantity = g.FirstOrDefault().st.StQuantity,
                                 PrPreWarehousing = (from hattyu in context.T_Hattyus
                                                     join hadetails in context.T_HattyuDetails on hattyu.HaID equals hadetails.HaID
                                                     where hadetails.PrID == g.FirstOrDefault().product.PrID && hattyu.WaWarehouseFlag == 1
                                                     select (int?)hadetails.HaQuantity).Sum() ?? 0,
                                 HaQuantitySum = g.Sum(x => x.hattyuDet.HaQuantity),
                                 ProductFalg=g.FirstOrDefault().product.PrFlag,
                                 MaID=g.FirstOrDefault().product.MaID
                             };
                    var res = new List<T_HattyuDetailDsp_Agr>();
                    int agrId = 0;
                    foreach (var row in tb)
                    {
                        agrId++;
                        row.AgrID = agrId;
                        res.Add(row);
                    }
                    return res;
                }
            }
            catch
            {
                throw;
            }
        }
        public int ProductSumInCurrentHattyu(int Prid, int Haid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    int res = (from hadetails in context.T_HattyuDetails
                               where hadetails.PrID == Prid &&
                              hadetails.HaID == Haid
                               select (int?)hadetails.HaQuantity).Sum() ?? 0;
                    return res;
                }
            }
            catch
            {
                throw;
            }
        }
        public void RegistHattyuData(T_Hattyu regData)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.T_Hattyus.Add(regData);
                        context.SaveChanges();
                        if (regData.HaID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Hattyu',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Hattyu')");
                            throw new Exception("発注IDが既定の数字を超えています。");
                        }
                        transaction.Commit();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public void RegistHattyuDetailData(T_HattyuDetail regDetailData)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.T_HattyuDetails.Add(regDetailData);
                        context.SaveChanges();
                        if (regDetailData.HaID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_HattyuDetail',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_HattyuDetail')");
                            throw new Exception("発詳細注IDが既定の数字を超えています。");

                        }
                        transaction.Commit();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public void UpdateHattyuHidden(int Haid, int Haflag, string Hahidden)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Hattyus.Single(x => x.HaID == Haid);
                    target.HaFlag = Haflag;
                    target.HaHidden = Hahidden;
                    context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public void FinalizeHattyuData(T_Warehousing regData, List<T_WarehousingDetail> regDetailData)
        {
            using (var context = new SalesManagement_DevContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.T_Warehousings.Add(regData);
                        context.SaveChanges();
                        if (regData.WaID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Warehousing',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Warehousing')");
                            throw new Exception("入庫IDが既定の数字を超えています。");
                        }
                        foreach (var row in regDetailData)
                        {
                            row.WaID = regData.WaID;
                            context.T_WarehousingDetails.Add(row);
                            var hattyuDetail = context.T_HattyuDetails.Where(x => x.HaID == regData.HaID && x.PrID == row.PrID).ToList();
                            foreach (var row2 in hattyuDetail)
                            {
                                row2.QuantFlag = 1;
                            }
                            context.SaveChanges();
                            if (row.WaDetailID > NumericRange.ID)
                            {
                                transaction.Rollback();
                                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_WarehousingDetail',RESEED,0)");
                                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_WarehousingDetail')");
                                throw new Exception("入庫詳細IDが既定の数字を超えています。");
                            }
                        }

                        var hattyu = context.T_Hattyus.Single(x => x.HaID == regData.HaID);
                        if (hattyu.HaFlag != 0)
                        {
                            hattyu.HaFlag = 0;
                            hattyu.HaHidden = null;
                        }
                        hattyu.WaWarehouseFlag = 1;
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
        public List<T_HattyuDetail> TrimHattyuDetailData(int Haid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var res = context.T_HattyuDetails.Where(x => x.HaID == Haid).GroupBy(x => x.PrID).Where(x => x.Sum(y => y.HaQuantity) > 0).Select(x => new { HaID = Haid, PrID = x.Key, HaQuantity = x.Sum(y => y.HaQuantity) })
                              .AsEnumerable().Select(x => new T_HattyuDetail { HaID = x.HaID, PrID = x.PrID, HaQuantity = x.HaQuantity });
                    return res.ToList();
                }
            }
            catch
            {
                throw;
            }

        }
        public bool CheckHattyuIsInsertable(int Haid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Hattyus.Single(x => x.HaID == Haid);
                    if (target.WaWarehouseFlag > 0)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckMakerConsistency(int Haid, List<T_HattyuDetail> Details)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    int Maid = context.T_Hattyus.Single(x => x.HaID == Haid).MaID;
                    foreach (var row in Details)
                    {
                        var Product = context.M_Products.Single(x => x.PrID == row.PrID);
                        if (Product.MaID != Maid)
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckHattyuExistence(int Haid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    return context.T_Hattyus.Any(x => x.HaID == Haid);
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckHattyuIsActive(int Haid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Hattyus.Single(x => x.HaID == Haid);
                    if (target.HaFlag == 0)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
