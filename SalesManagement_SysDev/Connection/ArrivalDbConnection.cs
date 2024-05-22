using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace SalesManagement_SysDev
{
    class ArrivalDbConnection
    {
        public List<T_ArrivalDsp> GetArrivalData()
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Arrival in context.T_Arrivals
                             join Order in context.T_Orders on Arrival.OrID equals Order.OrID
                             join ArrivalDet in context.T_ArrivalDetails on Arrival.ArID equals ArrivalDet.ArID
                             into gjArrivalDet
                             from subArrivalDet in gjArrivalDet.DefaultIfEmpty()
                             join product in context.M_Products on subArrivalDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Arrival.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()
                             join Client in context.M_Clients on Arrival.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Arrival.SoID equals SalesOfficce.SoID

                             where Arrival.ArFlag == 0

                             group new { Arrival, subEmployee, Client, SalesOfficce ,Order} by new { Arrival.ArID } into g

                             select new T_ArrivalDsp
                             {
                                 ClID = g.FirstOrDefault().Arrival.ClID,
                                 EmID = g.FirstOrDefault().Arrival.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 ArFlag = g.FirstOrDefault().Arrival.ArFlag,
                                 ArStateFlag = g.FirstOrDefault().Arrival.ArStateFlag,

                                 ArID = g.FirstOrDefault().Arrival.ArID,
                                 OrID = g.FirstOrDefault().Arrival.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 ArDate = g.FirstOrDefault().Arrival.ArDate,
                                 ArBoolFlag = g.FirstOrDefault().Arrival.ArFlag == 0 ? false : true,
                                 ArHidden = g.FirstOrDefault().Arrival.ArHidden,
                                 KindOfProducts = context.T_ArrivalDetails.Where(x => x.ArID == g.FirstOrDefault().Arrival.ArID).Count(),
                                 ArBoolStateFlag = g.FirstOrDefault().Arrival.ArStateFlag == 0 ? false : true,
                                 OrderFlag=g.FirstOrDefault().Order.OrFlag
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_ArrivalDsp> GetArrivalData(T_ArrivalRead selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Arrival in context.T_Arrivals
                             join Order in context.T_Orders on Arrival.OrID equals Order.OrID
                             join ArrivalDet in context.T_ArrivalDetails on Arrival.ArID equals ArrivalDet.ArID
                             into gjArrivalDet
                             from subArrivalDet in gjArrivalDet.DefaultIfEmpty()
                             join product in context.M_Products on subArrivalDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Arrival.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()
                             join Client in context.M_Clients on Arrival.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Arrival.SoID equals SalesOfficce.SoID

                             where (selectCondition.ArFlag==-1||Arrival.ArFlag == selectCondition.ArFlag) &&
                             (selectCondition.ArID == 0 || Arrival.ArID == selectCondition.ArID) &&
                             (selectCondition.EmID == 0 ||(selectCondition.EmID==-1&&subEmployee.EmFlag!=0)|| selectCondition.EmID == Arrival.EmID) &&
                             (selectCondition.ClID == 0 ||(selectCondition.ClID==-1&&Client.ClFlag!=0)|| selectCondition.ClID == Arrival.ClID) &&
                             (selectCondition.SoID == 0 ||(selectCondition.SoID==-1&&SalesOfficce.SoFlag!=0)|| selectCondition.SoID == Arrival.SoID) &&
                             (selectCondition.OrID == 0 ||(selectCondition.OrID==-1&&Order.OrFlag!=0)|| selectCondition.OrID == Arrival.OrID) &&
                             (selectCondition.ArStateFlag == -1 || selectCondition.ArStateFlag == Arrival.ArStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.ArHidden) || Arrival.ArHidden.Contains(selectCondition.ArHidden)) &&
                             (selectCondition.PrID == 0 ||(selectCondition.PrID==-1&&subProduct.PrFlag!=0)|| selectCondition.PrID == subArrivalDet.PrID) &&
                             (selectCondition.ScID == 0 ||(selectCondition.ScID==-1&&subSc.ScFlag!=0)|| selectCondition.ScID == subProduct.ScID) &&
                             (selectCondition.McID == 0 ||(selectCondition.McID==-1&&subMc.McFlag!=0)|| selectCondition.McID == subSc.McID)

                             group new { Arrival, subEmployee, Client, SalesOfficce,Order } by new { Arrival.ArID } into g

                             select new T_ArrivalDsp
                             {
                                 ClID = g.FirstOrDefault().Arrival.ClID,
                                 EmID = g.FirstOrDefault().Arrival.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 ArFlag = g.FirstOrDefault().Arrival.ArFlag,
                                 ArStateFlag = g.FirstOrDefault().Arrival.ArStateFlag,

                                 ArID = g.FirstOrDefault().Arrival.ArID,
                                 OrID = g.FirstOrDefault().Arrival.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 ArDate = g.FirstOrDefault().Arrival.ArDate,
                                 ArBoolFlag = g.FirstOrDefault().Arrival.ArFlag == 0 ? false : true,
                                 ArHidden = g.FirstOrDefault().Arrival.ArHidden,
                                 KindOfProducts = context.T_ArrivalDetails.Where(x => x.ArID == g.FirstOrDefault().Arrival.ArID).Count(),
                                 ArBoolStateFlag = g.FirstOrDefault().Arrival.ArStateFlag == 0 ? false : true,
                                 OrderFlag=g.FirstOrDefault().Order.OrFlag
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_ArrivalDsp> GetArrivalData(T_ArrivalRead selectCondition, DateTime? startDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Arrival in context.T_Arrivals
                             join Order in context.T_Orders on Arrival.OrID equals Order.OrID
                             join ArrivalDet in context.T_ArrivalDetails on Arrival.ArID equals ArrivalDet.ArID
                             into gjArrivalDet
                             from subArrivalDet in gjArrivalDet.DefaultIfEmpty()
                             join product in context.M_Products on subArrivalDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Arrival.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()
                             join Client in context.M_Clients on Arrival.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Arrival.SoID equals SalesOfficce.SoID

                             where (selectCondition.ArFlag==-1&&Arrival.ArFlag == selectCondition.ArFlag) &&
                             (selectCondition.ArID == 0 || Arrival.ArID == selectCondition.ArID) &&
                             (selectCondition.EmID == 0 ||(selectCondition.EmID==-1&&subEmployee.EmFlag!=0)|| selectCondition.EmID == Arrival.EmID) &&
                             (selectCondition.ClID == 0 ||(selectCondition.ClID==-1&&Client.ClFlag!=0)|| selectCondition.ClID == Arrival.ClID) &&
                             (selectCondition.SoID == 0 ||(selectCondition.SoID==-1&&SalesOfficce.SoFlag!=0)|| selectCondition.SoID == Arrival.SoID) &&
                             (selectCondition.OrID == 0 ||(selectCondition.OrID==-1&&Order.OrFlag!=0)|| selectCondition.OrID == Arrival.OrID) &&
                             (selectCondition.ArStateFlag == -1 || selectCondition.ArStateFlag == Arrival.ArStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.ArHidden) || Arrival.ArHidden.Contains(selectCondition.ArHidden)) &&
                             (selectCondition.PrID == 0 ||(selectCondition.PrID==-1&&subProduct.PrFlag!=0)|| selectCondition.PrID == subArrivalDet.PrID) &&
                             (selectCondition.ScID == 0 ||(selectCondition.ScID==-1&&subSc.ScFlag!=0)|| selectCondition.ScID == subProduct.ScID) &&
                             (selectCondition.McID == 0 ||(selectCondition.McID==-1&&subMc.McFlag!=0)||selectCondition.McID == subSc.McID) &&

                             (startDay == null ? Arrival.ArDate <= endDay : endDay == null ? Arrival.ArDate >= startDay : (Arrival.ArDate >= startDay && Arrival.ArDate <= endDay))


                             group new { Arrival, subEmployee, Client, SalesOfficce,Order } by new { Arrival.ArID } into g

                             select new T_ArrivalDsp
                             {
                                 ClID = g.FirstOrDefault().Arrival.ClID,
                                 EmID = g.FirstOrDefault().Arrival.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 ArFlag = g.FirstOrDefault().Arrival.ArFlag,
                                 ArStateFlag = g.FirstOrDefault().Arrival.ArStateFlag,

                                 ArID = g.FirstOrDefault().Arrival.ArID,
                                 OrID = g.FirstOrDefault().Arrival.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 ArDate = g.FirstOrDefault().Arrival.ArDate,
                                 ArBoolFlag = g.FirstOrDefault().Arrival.ArFlag == 0 ? false : true,
                                 ArHidden = g.FirstOrDefault().Arrival.ArHidden,
                                 KindOfProducts = context.T_ArrivalDetails.Where(x => x.ArID == g.FirstOrDefault().Arrival.ArID).Count(),
                                 ArBoolStateFlag = g.FirstOrDefault().Arrival.ArStateFlag == 0 ? false : true,
                                 OrderFlag=g.FirstOrDefault().Order.OrFlag
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_ArrivalDetailDsp> GetArrivalDetailData(T_ArrivalDetailDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from ArrivalDet in context.T_ArrivalDetails
                             join Product in context.M_Products on ArrivalDet.PrID equals Product.PrID
                             join Sc in context.M_SmallClassifications on Product.ScID equals Sc.ScID
                             join Mc in context.M_MajorCassifications on Sc.McID equals Mc.McID

                             where ArrivalDet.ArID == selectCondition.ArID &&
                             (selectCondition.ArDetailID == 0 || ArrivalDet.ArDetailID == selectCondition.ArDetailID) &&
                             (selectCondition.PrID == 0||(selectCondition.PrID==-1&&Product.PrFlag!=0)|| ArrivalDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 ||(selectCondition.ScID==-1&&Sc.ScFlag!=0)|| Product.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 ||(selectCondition.McID==-1&&Mc.McFlag!=0)|| Sc.McID == selectCondition.McID)

                             select new T_ArrivalDetailDsp
                             {
                                 ArID = ArrivalDet.ArID,
                                 PrID = ArrivalDet.PrID,
                                 ScID = Product.ScID,
                                 McID = Sc.McID,

                                 ArDetailID = ArrivalDet.ArDetailID,
                                 PrName = Product.PrName + "(" + Product.PrColor + ")",
                                 ScName = Sc.ScName,
                                 McName = Mc.McName,
                                 ArQuantity = ArrivalDet.ArQuantity
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public void UpdateArrivalHidden(int Arid, int ArFlag, string ArHidden)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Arrivals.Single(x => x.ArID == Arid);
                    target.ArFlag = ArFlag;
                    target.ArHidden = ArHidden;
                    context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public void FinalizeArrivalData(T_Shipment regData, List<T_ShipmentDetail> regDetailData, int EmID)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.T_Shipments.Add(regData);
                        context.SaveChanges();
                        if (regData.ShID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Shipment',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Shipment')");
                            throw new Exception("出荷IDが既定の数字を超えています。");
                        }
                        foreach (var row in regDetailData)
                        {
                            row.ShID = regData.ShID;
                            context.T_ShipmentDetails.Add(row);
                            context.SaveChanges();
                            if (row.ShDetailID > NumericRange.ID)
                            {
                                transaction.Rollback();
                                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_ShipmentDetail',RESEED,0)");
                                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_ShipmentDetail')");
                                throw new Exception("出荷詳細IDが既定の数字を超えています。");
                            }
                        }
                        var arrival = context.T_Arrivals.Single(x => x.OrID == regData.OrID);
                        var order = context.T_Orders.Single(x => x.OrID == regData.OrID);
                        arrival.ArStateFlag = 1;
                        arrival.EmID = EmID;
                        arrival.ArDate = DateTime.Now;
                        order.OrStateFlag = 4;
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
        public bool CheckArrivalExistence(int Arid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    return context.T_Arrivals.Any(x => x.ArID == Arid);
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckArrivalIsActive(int Arid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Arrivals.Single(x => x.ArID == Arid);
                    if (target.ArFlag == 0)
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
        public bool CheckArrivalIsInsertable(int Arid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Arrivals.Single(x => x.ArID == Arid);
                    if (target.ArStateFlag == 0)
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
