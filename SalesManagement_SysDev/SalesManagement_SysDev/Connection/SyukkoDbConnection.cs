using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_SysDev
{
    class SyukkoDbConnection
    {

        public List<T_SyukkoDsp> GetSyukkoData()
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Syukko in context.T_Syukkos
                             join SyukkoDet in context.T_SyukkoDetails on Syukko.SyID equals SyukkoDet.SyID
                             into gjSyukkoDet
                             from subSyukkoDet in gjSyukkoDet.DefaultIfEmpty()
                             join product in context.M_Products on subSyukkoDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Syukko.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()
                             join Client in context.M_Clients on Syukko.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Syukko.SoID equals SalesOfficce.SoID

                             where Syukko.SyFlag == 0

                             group new { Syukko, subEmployee, Client, SalesOfficce } by new { Syukko.SyID } into g

                             select new T_SyukkoDsp
                             {
                                 ClID = g.FirstOrDefault().Syukko.ClID,
                                 EmID = g.FirstOrDefault().Syukko.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 SyFlag = g.FirstOrDefault().Syukko.SyFlag,
                                 SyStateFlag = g.FirstOrDefault().Syukko.SyStateFlag,

                                 SyID = g.FirstOrDefault().Syukko.SyID,
                                 OrID = g.FirstOrDefault().Syukko.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 SyDate = g.FirstOrDefault().Syukko.SyDate,
                                 SyBoolFlag = g.FirstOrDefault().Syukko.SyFlag == 0 ? false : true,
                                 SyHidden = g.FirstOrDefault().Syukko.SyHidden,
                                 KindOfProducts = context.T_SyukkoDetails.Where(x => x.SyID == g.FirstOrDefault().Syukko.SyID).Count(),
                                 SyBoolStateFlag = g.FirstOrDefault().Syukko.SyStateFlag == 0 ? false : true
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_SyukkoDsp> GetSyukkoData(T_SyukkoRead selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Syukko in context.T_Syukkos
                             join SyukkoDet in context.T_SyukkoDetails on Syukko.SyID equals SyukkoDet.SyID
                             into gjSyukkoDet
                             from subSyukkoDet in gjSyukkoDet.DefaultIfEmpty()
                             join product in context.M_Products on subSyukkoDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Syukko.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()
                             join Client in context.M_Clients on Syukko.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Syukko.SoID equals SalesOfficce.SoID

                             where Syukko.SyFlag == selectCondition.SyFlag &&
                             (selectCondition.SyID == 0 || Syukko.SyID == selectCondition.SyID) &&
                             (selectCondition.EmID == 0 || selectCondition.EmID == Syukko.EmID) &&
                             (selectCondition.ClID == 0 || selectCondition.ClID == Syukko.ClID) &&
                             (selectCondition.SoID == 0 || selectCondition.SoID == Syukko.SoID) &&
                             (selectCondition.OrID == 0 || selectCondition.OrID == Syukko.OrID) &&
                             (selectCondition.SyStateFlag == -1 || selectCondition.SyStateFlag == Syukko.SyStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.SyHidden) || Syukko.SyHidden.Contains(selectCondition.SyHidden)) &&
                             (selectCondition.PrID == 0 || selectCondition.PrID == subSyukkoDet.PrID) &&
                             (selectCondition.ScID == 0 || selectCondition.ScID == subProduct.ScID) &&
                             (selectCondition.McID == 0 || selectCondition.McID == subSc.McID)

                             group new { Syukko, subEmployee, Client, SalesOfficce } by new { Syukko.SyID } into g

                             select new T_SyukkoDsp
                             {
                                 ClID = g.FirstOrDefault().Syukko.ClID,
                                 EmID = g.FirstOrDefault().Syukko.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 SyFlag = g.FirstOrDefault().Syukko.SyFlag,
                                 SyStateFlag = g.FirstOrDefault().Syukko.SyStateFlag,

                                 SyID = g.FirstOrDefault().Syukko.SyID,
                                 OrID = g.FirstOrDefault().Syukko.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 SyDate = g.FirstOrDefault().Syukko.SyDate,
                                 SyBoolFlag = g.FirstOrDefault().Syukko.SyFlag == 0 ? false : true,
                                 SyHidden = g.FirstOrDefault().Syukko.SyHidden,
                                 KindOfProducts = context.T_SyukkoDetails.Where(x => x.SyID == g.FirstOrDefault().Syukko.SyID).Count(),
                                 SyBoolStateFlag = g.FirstOrDefault().Syukko.SyStateFlag == 0 ? false : true
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_SyukkoDsp> GetSyukkoData(T_SyukkoRead selectCondition, DateTime? startDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Syukko in context.T_Syukkos
                             join SyukkoDet in context.T_SyukkoDetails on Syukko.SyID equals SyukkoDet.SyID
                             into gjSyukkoDet
                             from subSyukkoDet in gjSyukkoDet.DefaultIfEmpty()
                             join product in context.M_Products on subSyukkoDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Syukko.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()
                             join Client in context.M_Clients on Syukko.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Syukko.SoID equals SalesOfficce.SoID

                             where Syukko.SyFlag == selectCondition.SyFlag &&
                             (selectCondition.SyID == 0 || Syukko.SyID == selectCondition.SyID) &&
                             (selectCondition.EmID == 0 || selectCondition.EmID == Syukko.EmID) &&
                             (selectCondition.ClID == 0 || selectCondition.ClID == Syukko.ClID) &&
                             (selectCondition.SoID == 0 || selectCondition.SoID == Syukko.SoID) &&
                             (selectCondition.OrID == 0 || selectCondition.OrID == Syukko.OrID) &&
                             (selectCondition.SyStateFlag == -1 || selectCondition.SyStateFlag == Syukko.SyStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.SyHidden) || Syukko.SyHidden.Contains(selectCondition.SyHidden)) &&
                             (selectCondition.PrID == 0 || selectCondition.PrID == subSyukkoDet.PrID) &&
                             (selectCondition.ScID == 0 || selectCondition.ScID == subProduct.ScID) &&
                             (selectCondition.McID == 0 || selectCondition.McID == subSc.McID) &&

                             (startDay == null ? Syukko.SyDate <= endDay : endDay == null ? Syukko.SyDate >= startDay : (Syukko.SyDate >= startDay && Syukko.SyDate <= endDay))


                             group new { Syukko, subEmployee, Client, SalesOfficce } by new { Syukko.SyID } into g

                             select new T_SyukkoDsp
                             {
                                 ClID = g.FirstOrDefault().Syukko.ClID,
                                 EmID = g.FirstOrDefault().Syukko.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 SyFlag = g.FirstOrDefault().Syukko.SyFlag,
                                 SyStateFlag = g.FirstOrDefault().Syukko.SyStateFlag,

                                 SyID = g.FirstOrDefault().Syukko.SyID,
                                 OrID = g.FirstOrDefault().Syukko.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 SyDate = g.FirstOrDefault().Syukko.SyDate,
                                 SyBoolFlag = g.FirstOrDefault().Syukko.SyFlag == 0 ? false : true,
                                 SyHidden = g.FirstOrDefault().Syukko.SyHidden,
                                 KindOfProducts = context.T_SyukkoDetails.Where(x => x.SyID == g.FirstOrDefault().Syukko.SyID).Count(),
                                 SyBoolStateFlag = g.FirstOrDefault().Syukko.SyStateFlag == 0 ? false : true
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_SyukkoDetailDsp> GetSyukkoDetailData(T_SyukkoDetailDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from SyukkoDet in context.T_SyukkoDetails
                             join Product in context.M_Products on SyukkoDet.PrID equals Product.PrID
                             join Sc in context.M_SmallClassifications on Product.ScID equals Sc.ScID
                             join Mc in context.M_MajorCassifications on Sc.McID equals Mc.McID

                             where SyukkoDet.SyID == selectCondition.SyID &&
                             (selectCondition.SyDetailID == 0 || SyukkoDet.SyDetailID == selectCondition.SyDetailID) &&
                             (selectCondition.PrID == 0 || SyukkoDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 || Product.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 || Sc.McID == selectCondition.McID)

                             select new T_SyukkoDetailDsp
                             {
                                 SyID = SyukkoDet.SyID,
                                 PrID = SyukkoDet.PrID,
                                 ScID = Product.ScID,
                                 McID = Sc.McID,

                                 SyDetailID = SyukkoDet.SyDetailID,
                                 PrName = Product.PrName,
                                 ScName = Sc.ScName,
                                 McName = Mc.McName,
                                 SyQuantity = SyukkoDet.SyQuantity
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public void UpdateSyukkoHidden(int Syid, int SyFlag, string SyHidden)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Syukkos.Single(x => x.SyID == Syid);
                    target.SyFlag = SyFlag;
                    target.SyHidden = SyHidden;
                    context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public void FinalizeSyukkoData(T_Arrival regData, List<T_ArrivalDetail> regDetailData)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.T_Arrivals.Add(regData);
                        context.SaveChanges();
                        if (regData.ArID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            throw new Exception("入荷IDが既定の数字を超えています。");
                        }
                        foreach (var row in regDetailData)
                        {
                            context.T_ArrivalDetails.Add(row);
                            context.SaveChanges();
                            if (row.ArDetailID > NumericRange.ID)
                            {
                                transaction.Rollback();
                                throw new Exception("入荷詳細IDが既定の数字を超えています。");
                            }
                        }
                        var syukko = context.T_Syukkos.Single(x => x.OrID == regData.OrID);
                        var order = context.T_Orders.Single(x => x.OrID == regData.OrID);
                        syukko.SyStateFlag = 1;
                        order.OrStateFlag = 3;
                        context.SaveChanges();
                        transaction.Commit();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckSyukkoExistence(int Syid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    return context.T_Syukkos.Any(x => x.SyID == Syid);
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckSyukkoIsActive(int Syid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Syukkos.Single(x => x.SyID == Syid);
                    if (target.SyFlag == 0)
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
