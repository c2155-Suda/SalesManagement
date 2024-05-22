using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SalesManagement_SysDev
{
    class ShipmentDbConnection
    {
        public List<T_ShipmentDsp> GetShipmentData()
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Shipment in context.T_Shipments
                             join Order in context.T_Orders on Shipment.OrID equals Order.OrID
                             join ShipmentDet in context.T_ShipmentDetails on Shipment.ShID equals ShipmentDet.ShID
                             into gjShipmentDet
                             from subShipmentDet in gjShipmentDet.DefaultIfEmpty()
                             join product in context.M_Products on subShipmentDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Shipment.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()
                             join Client in context.M_Clients on Shipment.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Shipment.SoID equals SalesOfficce.SoID

                             where Shipment.ShFlag == 0

                             group new { Shipment, subEmployee, Client, SalesOfficce,Order } by new { Shipment.ShID } into g

                             select new T_ShipmentDsp
                             {
                                 ClID = g.FirstOrDefault().Shipment.ClID,
                                 EmID = g.FirstOrDefault().Shipment.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 ShFlag = g.FirstOrDefault().Shipment.ShFlag,
                                 ShStateFlag = g.FirstOrDefault().Shipment.ShStateFlag,

                                 ShID = g.FirstOrDefault().Shipment.ShID,
                                 OrID = g.FirstOrDefault().Shipment.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 ShDate = g.FirstOrDefault().Shipment.ShFinishDate,
                                 ShBoolFlag = g.FirstOrDefault().Shipment.ShFlag == 0 ? false : true,
                                 ShHidden = g.FirstOrDefault().Shipment.ShHidden,
                                 KindOfProducts = context.T_ShipmentDetails.Where(x => x.ShID == g.FirstOrDefault().Shipment.ShID).Count(),
                                 ShBoolStateFlag = g.FirstOrDefault().Shipment.ShStateFlag == 0 ? false : true,
                                 OrderFlag = g.FirstOrDefault().Order.OrFlag
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_ShipmentDsp> GetShipmentData(T_ShipmentRead selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Shipment in context.T_Shipments
                             join Order in context.T_Orders on Shipment.OrID equals Order.OrID
                             join ShipmentDet in context.T_ShipmentDetails on Shipment.ShID equals ShipmentDet.ShID
                             into gjShipmentDet
                             from subShipmentDet in gjShipmentDet.DefaultIfEmpty()
                             join product in context.M_Products on subShipmentDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Shipment.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()
                             join Client in context.M_Clients on Shipment.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Shipment.SoID equals SalesOfficce.SoID

                             where (selectCondition.ShFlag==-1||Shipment.ShFlag == selectCondition.ShFlag) &&
                             (selectCondition.ShID == 0 || Shipment.ShID == selectCondition.ShID) &&
                             (selectCondition.EmID == 0 ||(selectCondition.EmID==-1&&subEmployee.EmFlag!=0)|| selectCondition.EmID == Shipment.EmID) &&
                             (selectCondition.ClID == 0 ||(selectCondition.ClID==-1&&Client.ClFlag!=0)|| selectCondition.ClID == Shipment.ClID) &&
                             (selectCondition.SoID == 0 ||(selectCondition.SoID==-1&&SalesOfficce.SoFlag!=0)|| selectCondition.SoID == Shipment.SoID) &&
                             (selectCondition.OrID == 0 ||(selectCondition.OrID==-1&&Order.OrFlag!=0)|| selectCondition.OrID == Shipment.OrID) &&
                             (selectCondition.ShStateFlag == -1 || selectCondition.ShStateFlag == Shipment.ShStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.ShHidden) || Shipment.ShHidden.Contains(selectCondition.ShHidden)) &&
                             (selectCondition.PrID == 0 ||(selectCondition.PrID==-1&&subProduct.PrFlag!=0)|| selectCondition.PrID == subShipmentDet.PrID) &&
                             (selectCondition.ScID == 0 ||(selectCondition.ScID==-1&&subSc.ScFlag!=0)|| selectCondition.ScID == subProduct.ScID) &&
                             (selectCondition.McID == 0 ||(selectCondition.McID==-1&&subMc.McFlag!=0)|| selectCondition.McID == subSc.McID)

                             group new { Shipment, subEmployee, Client, SalesOfficce,Order } by new { Shipment.ShID } into g

                             select new T_ShipmentDsp
                             {
                                 ClID = g.FirstOrDefault().Shipment.ClID,
                                 EmID = g.FirstOrDefault().Shipment.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 ShFlag = g.FirstOrDefault().Shipment.ShFlag,
                                 ShStateFlag = g.FirstOrDefault().Shipment.ShStateFlag,

                                 ShID = g.FirstOrDefault().Shipment.ShID,
                                 OrID = g.FirstOrDefault().Shipment.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 ShDate = g.FirstOrDefault().Shipment.ShFinishDate,
                                 ShBoolFlag = g.FirstOrDefault().Shipment.ShFlag == 0 ? false : true,
                                 ShHidden = g.FirstOrDefault().Shipment.ShHidden,
                                 KindOfProducts = context.T_ShipmentDetails.Where(x => x.ShID == g.FirstOrDefault().Shipment.ShID).Count(),
                                 ShBoolStateFlag = g.FirstOrDefault().Shipment.ShStateFlag == 0 ? false : true,
                                 OrderFlag = g.FirstOrDefault().Order.OrFlag
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_ShipmentDsp> GetShipmentData(T_ShipmentRead selectCondition, DateTime? startDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Shipment in context.T_Shipments
                             join Order in context.T_Orders on Shipment.OrID equals Order.OrID
                             join ShipmentDet in context.T_ShipmentDetails on Shipment.ShID equals ShipmentDet.ShID
                             into gjShipmentDet
                             from subShipmentDet in gjShipmentDet.DefaultIfEmpty()
                             join product in context.M_Products on subShipmentDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Shipment.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()
                             join Client in context.M_Clients on Shipment.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Shipment.SoID equals SalesOfficce.SoID

                             where (selectCondition.ShFlag == -1 || Shipment.ShFlag == selectCondition.ShFlag) &&
                             (selectCondition.ShID == 0 || Shipment.ShID == selectCondition.ShID) &&
                             (selectCondition.EmID == 0 || (selectCondition.EmID == -1 && subEmployee.EmFlag != 0) || selectCondition.EmID == Shipment.EmID) &&
                             (selectCondition.ClID == 0 || (selectCondition.ClID == -1 && Client.ClFlag != 0) || selectCondition.ClID == Shipment.ClID) &&
                             (selectCondition.SoID == 0 || (selectCondition.SoID == -1 && SalesOfficce.SoFlag != 0) || selectCondition.SoID == Shipment.SoID) &&
                             (selectCondition.OrID == 0 || (selectCondition.OrID == -1 && Order.OrFlag != 0) || selectCondition.OrID == Shipment.OrID) &&
                             (selectCondition.ShStateFlag == -1 || selectCondition.ShStateFlag == Shipment.ShStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.ShHidden) || Shipment.ShHidden.Contains(selectCondition.ShHidden)) &&
                             (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && subProduct.PrFlag != 0) || selectCondition.PrID == subShipmentDet.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && subSc.ScFlag != 0) || selectCondition.ScID == subProduct.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && subMc.McFlag != 0) || selectCondition.McID == subSc.McID)&&

                             (startDay == null ? Shipment.ShFinishDate <= endDay : endDay == null ? Shipment.ShFinishDate >= startDay : (Shipment.ShFinishDate >= startDay && Shipment.ShFinishDate <= endDay))


                             group new { Shipment, subEmployee, Client, SalesOfficce,Order } by new { Shipment.ShID } into g

                             select new T_ShipmentDsp
                             {
                                 ClID = g.FirstOrDefault().Shipment.ClID,
                                 EmID = g.FirstOrDefault().Shipment.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 ShFlag = g.FirstOrDefault().Shipment.ShFlag,
                                 ShStateFlag = g.FirstOrDefault().Shipment.ShStateFlag,

                                 ShID = g.FirstOrDefault().Shipment.ShID,
                                 OrID = g.FirstOrDefault().Shipment.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 ShDate = g.FirstOrDefault().Shipment.ShFinishDate,
                                 ShBoolFlag = g.FirstOrDefault().Shipment.ShFlag == 0 ? false : true,
                                 ShHidden = g.FirstOrDefault().Shipment.ShHidden,
                                 KindOfProducts = context.T_ShipmentDetails.Where(x => x.ShID == g.FirstOrDefault().Shipment.ShID).Count(),
                                 ShBoolStateFlag = g.FirstOrDefault().Shipment.ShStateFlag == 0 ? false : true,
                                 OrderFlag = g.FirstOrDefault().Order.OrFlag
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_ShipmentDetailDsp> GetShipmentDetailData(T_ShipmentDetailDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from ShipmentDet in context.T_ShipmentDetails
                             join Product in context.M_Products on ShipmentDet.PrID equals Product.PrID
                             join Sc in context.M_SmallClassifications on Product.ScID equals Sc.ScID
                             join Mc in context.M_MajorCassifications on Sc.McID equals Mc.McID

                             where ShipmentDet.ShID == selectCondition.ShID &&
                             (selectCondition.ShDetailID == 0 || ShipmentDet.ShDetailID == selectCondition.ShDetailID) &&
                             (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && Product.PrFlag != 0) || ShipmentDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && Sc.ScFlag != 0) || Product.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && Mc.McFlag != 0) || Sc.McID == selectCondition.McID)

                             select new T_ShipmentDetailDsp
                             {
                                 ShID = ShipmentDet.ShID,
                                 PrID = ShipmentDet.PrID,
                                 ScID = Product.ScID,
                                 McID = Sc.McID,

                                 ShDetailID = ShipmentDet.ShDetailID,
                                 PrName = Product.PrName + "(" + Product.PrColor + ")",
                                 ScName = Sc.ScName,
                                 McName = Mc.McName,
                                 ShQuantity = ShipmentDet.ShQuantity
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public void UpdateShipmentHidden(int Shid, int ShFlag, string ShHidden)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Shipments.Single(x => x.ShID == Shid);
                    target.ShFlag = ShFlag;
                    target.ShHidden = ShHidden;
                    context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public void FinalizeShipmentData(T_Sale regData, List<T_SaleDetail> regDetailData)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.T_Sale.Add(regData);
                        context.SaveChanges();
                        if (regData.SaID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Sale',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Sale')");
                            throw new Exception("売上IDが既定の数字を超えています。");
                        }
                        foreach (var row in regDetailData)
                        {
                            row.SaID = regData.SaID;
                            context.T_SaleDetails.Add(row);
                            context.SaveChanges();
                            if (row.SaDetailID > NumericRange.ID)
                            {
                                transaction.Rollback();
                                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_SaleDetail',RESEED,0)");
                                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_SaleDetail')");
                                throw new Exception("売上詳細IDが既定の数字を超えています。");
                            }
                        }
                        var shipment = context.T_Shipments.Single(x => x.OrID == regData.OrID);
                        var order = context.T_Orders.Single(x => x.OrID == regData.OrID);
                        shipment.ShStateFlag = 1;
                        shipment.EmID = regData.EmID;
                        shipment.ShFinishDate = regData.SaDate;
                        order.OrStateFlag = 5;
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
        public bool CheckShipmentExistence(int Shid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    return context.T_Shipments.Any(x => x.ShID == Shid);
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckShipmentIsActive(int Shid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Shipments.Single(x => x.ShID == Shid);
                    if (target.ShFlag == 0)
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
        public bool CheckShipmentIsInsertable(int Shid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Shipments.Single(x => x.ShID == Shid);
                    if (target.ShStateFlag == 0)
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
