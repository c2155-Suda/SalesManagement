using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_SysDev
{
    class OrderDbConnection
    {
        static string[] StateFlagStrings = { string.Empty, "受注済", "注文済", "出庫済", "入荷済", "出荷済" };

        public List<T_OrderDsp> GetOrderData()
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from order in context.T_Orders
                             join orderDet in context.T_OrderDetails on order.OrID equals orderDet.OrID
                             into gjOrderDet
                             from subOrderDet in gjOrderDet.DefaultIfEmpty()
                             join product in context.M_Products on subOrderDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join sc in context.M_SmallClassifications on subProduct.ScID equals sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join mc in context.M_MajorCassifications on subSc.McID equals mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join so in context.M_SalesOffices on order.SoID equals so.SoID
                             join em in context.M_Employees on order.EmID equals em.EmID
                             join cl in context.M_Clients on order.ClID equals cl.ClID

                             where order.OrFlag == 0

                             group new { order, so, em, cl } by new { order.OrID } into g

                             select new T_OrderDsp
                             {
                                 ClID = g.FirstOrDefault().cl.ClID,
                                 SoID = g.FirstOrDefault().so.SoID,
                                 EmID = g.FirstOrDefault().em.EmID,
                                 OrFlag = g.FirstOrDefault().order.OrFlag,
                                 OrStateFlag = g.FirstOrDefault().order.OrStateFlag,

                                 OrID = g.FirstOrDefault().order.OrID,
                                 ClName = g.FirstOrDefault().cl.ClName,
                                 ClCharge = g.FirstOrDefault().order.ClCharge,
                                 SoName = g.FirstOrDefault().so.SoName,
                                 EmName = g.FirstOrDefault().em.EmName,
                                 OrDate = g.FirstOrDefault().order.OrDate,
                                 OrFlagBool = g.FirstOrDefault().order.OrFlag == 0 ? false : true,
                                 OrHidden = g.FirstOrDefault().order.OrHidden,
                                 KindOfProducts = context.T_OrderDetails.Where(x => x.OrID == g.FirstOrDefault().order.OrID).GroupBy(x => x.PrID).Where(x => x.Sum(y => y.OrQuantity) > 0).Count(),
                                 ClientFlag = g.FirstOrDefault().cl.ClFlag,
                                 EmployeeFlag=g.FirstOrDefault().em.EmFlag
                             };
                    var res = new List<T_OrderDsp>();
                    foreach (var row in tb)
                    {
                        row.OrStateString = StateFlagStrings[row.OrStateFlag];
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
        public List<T_OrderDsp> GetOrderData(T_OrderRead selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from order in context.T_Orders
                             join orderDet in context.T_OrderDetails on order.OrID equals orderDet.OrID
                             into gjOrderDet
                             from subOrderDet in gjOrderDet.DefaultIfEmpty()
                             join product in context.M_Products on subOrderDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join sc in context.M_SmallClassifications on subProduct.ScID equals sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join mc in context.M_MajorCassifications on subSc.McID equals mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join so in context.M_SalesOffices on order.SoID equals so.SoID
                             join em in context.M_Employees on order.EmID equals em.EmID
                             join cl in context.M_Clients on order.ClID equals cl.ClID

                             where (order.OrFlag == selectCondition.OrFlag||selectCondition.OrFlag==-1) &&
                             (selectCondition.OrID==0||order.OrID==selectCondition.OrID) &&
                             (selectCondition.SoID == 0 ||(selectCondition.SoID==-1&&so.SoFlag!=0)|| order.SoID == selectCondition.SoID) &&
                             (selectCondition.EmID == 0 ||(selectCondition.EmID==-1&&em.EmFlag!=0)||order.EmID == selectCondition.EmID) &&
                             (selectCondition.ClID == 0 ||(selectCondition.ClID==-1&&cl.ClFlag!=0)|| order.ClID == selectCondition.ClID) &&
                             order.ClCharge.Contains(selectCondition.ClCharge) &&
                             (selectCondition.OrStateFlag == -1 || order.OrStateFlag == selectCondition.OrStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.OrHidden) || order.OrHidden.Contains(selectCondition.OrHidden)) &&
                             (selectCondition.PrID == 0 ||(selectCondition.PrID==-1&&subProduct.PrFlag!=0)|| subOrderDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 ||(selectCondition.ScID==-1&&subSc.ScFlag!=0)|| subProduct.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 ||(selectCondition.McID==-1&&subMc.McFlag!=0)|| subSc.McID == selectCondition.McID)


                             group new { order, so, em, cl } by new { order.OrID } into g

                             select new T_OrderDsp
                             {
                                 ClID = g.FirstOrDefault().cl.ClID,
                                 SoID = g.FirstOrDefault().so.SoID,
                                 EmID = g.FirstOrDefault().em.EmID,
                                 OrFlag = g.FirstOrDefault().order.OrFlag,
                                 OrStateFlag = g.FirstOrDefault().order.OrStateFlag,

                                 OrID = g.FirstOrDefault().order.OrID,
                                 ClName = g.FirstOrDefault().cl.ClName,
                                 ClCharge = g.FirstOrDefault().order.ClCharge,
                                 SoName = g.FirstOrDefault().so.SoName,
                                 EmName = g.FirstOrDefault().em.EmName,
                                 OrDate = g.FirstOrDefault().order.OrDate,
                                 OrFlagBool = g.FirstOrDefault().order.OrFlag == 0 ? false : true,
                                 OrHidden = g.FirstOrDefault().order.OrHidden,
                                 KindOfProducts = context.T_OrderDetails.Where(x => x.OrID == g.FirstOrDefault().order.OrID).GroupBy(x => x.PrID).Where(x => x.Sum(y => y.OrQuantity) > 0).Count(),
                                 ClientFlag = g.FirstOrDefault().cl.ClFlag,
                                 EmployeeFlag = g.FirstOrDefault().em.EmFlag
                             };
                    var res = new List<T_OrderDsp>();
                    foreach (var row in tb)
                    {
                        row.OrStateString = StateFlagStrings[row.OrStateFlag];
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
        public List<T_OrderDsp> GetOrderData(T_OrderRead selectCondition, DateTime? startDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from order in context.T_Orders
                             join orderDet in context.T_OrderDetails on order.OrID equals orderDet.OrID
                             into gjOrderDet
                             from subOrderDet in gjOrderDet.DefaultIfEmpty()
                             join product in context.M_Products on subOrderDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join sc in context.M_SmallClassifications on subProduct.ScID equals sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join mc in context.M_MajorCassifications on subSc.McID equals mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join so in context.M_SalesOffices on order.SoID equals so.SoID
                             join em in context.M_Employees on order.EmID equals em.EmID
                             join cl in context.M_Clients on order.ClID equals cl.ClID

                             where (order.OrFlag == selectCondition.OrFlag || selectCondition.OrFlag == -1) &&
                             (selectCondition.OrID == 0 || order.OrID == selectCondition.OrID) &&
                             (selectCondition.SoID == 0 || (selectCondition.SoID == -1 && so.SoFlag != 0) || order.SoID == selectCondition.SoID) &&
                             (selectCondition.EmID == 0 || (selectCondition.EmID == -1 && em.EmFlag != 0) || order.EmID == selectCondition.EmID) &&
                             (selectCondition.ClID == 0 || (selectCondition.ClID == -1 && cl.ClFlag != 0) || order.ClID == selectCondition.ClID) &&
                             order.ClCharge.Contains(selectCondition.ClCharge) &&
                             (selectCondition.OrStateFlag == -1 || order.OrStateFlag == selectCondition.OrStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.OrHidden) || order.OrHidden.Contains(selectCondition.OrHidden)) &&
                             (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && subProduct.PrFlag != 0) || subOrderDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && subSc.ScFlag != 0) || subProduct.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && subMc.McFlag != 0) || subSc.McID == selectCondition.McID) &&

                             (startDay == null ? order.OrDate <= endDay : endDay == null ? order.OrDate >= startDay : (order.OrDate >= startDay && order.OrDate <= endDay))



                             group new { order, so, em, cl } by new { order.OrID } into g

                             select new T_OrderDsp
                             {
                                 ClID = g.FirstOrDefault().cl.ClID,
                                 SoID = g.FirstOrDefault().so.SoID,
                                 EmID = g.FirstOrDefault().em.EmID,
                                 OrFlag = g.FirstOrDefault().order.OrFlag,
                                 OrStateFlag = g.FirstOrDefault().order.OrStateFlag,

                                 OrID = g.FirstOrDefault().order.OrID,
                                 ClName = g.FirstOrDefault().cl.ClName,
                                 ClCharge = g.FirstOrDefault().order.ClCharge,
                                 SoName = g.FirstOrDefault().so.SoName,
                                 EmName = g.FirstOrDefault().em.EmName,
                                 OrDate = g.FirstOrDefault().order.OrDate,
                                 OrFlagBool = g.FirstOrDefault().order.OrFlag == 0 ? false : true,
                                 OrHidden = g.FirstOrDefault().order.OrHidden,
                                 KindOfProducts = context.T_OrderDetails.Where(x => x.OrID == g.FirstOrDefault().order.OrID).GroupBy(x => x.PrID).Where(x => x.Sum(y => y.OrQuantity) > 0).Count(),
                                 ClientFlag = g.FirstOrDefault().cl.ClFlag,
                                 EmployeeFlag = g.FirstOrDefault().em.EmFlag
                             };
                    var res = new List<T_OrderDsp>();
                    foreach (var row in tb)
                    {
                        row.OrStateString = StateFlagStrings[row.OrStateFlag];
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
        public List<T_OrderDetailDsp> GetOrderDetailData(T_OrderDetailDsp selectCondition)
        {

            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from orderDet in context.T_OrderDetails
                             join product in context.M_Products on orderDet.PrID equals product.PrID
                             join sc in context.M_SmallClassifications on product.ScID equals sc.ScID
                             join mc in context.M_MajorCassifications on sc.McID equals mc.McID

                             where orderDet.OrID == selectCondition.OrID &&
                             (selectCondition.OrDetailID == 0 || orderDet.OrDetailID == selectCondition.OrDetailID) &&
                             (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && product.PrFlag != 0) || orderDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && sc.ScFlag != 0) || product.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && mc.McFlag != 0) || sc.McID == selectCondition.McID)

                             select new T_OrderDetailDsp
                             {
                                 OrDetailID = orderDet.OrDetailID,
                                 OrID = orderDet.OrID,
                                 PrID = orderDet.PrID,
                                 ScID = product.ScID,
                                 McID = sc.McID,

                                 PrName = product.PrName + "(" + product.PrColor + ")",
                                 ScName = sc.ScName,
                                 McName = mc.McName,
                                 OrQuantity = orderDet.OrQuantity,
                                 OrPrice = 0,
                                 ProductFlag=product.PrFlag
                             };
                    return tb.ToList();

                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_OrderDetailDsp_Agr_NonFin> GetOrderDetailData_Agr_NonFin(T_OrderDetailDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from orderDet in context.T_OrderDetails
                             join product in context.M_Products on orderDet.PrID equals product.PrID
                             join sc in context.M_SmallClassifications on product.ScID equals sc.ScID
                             join mc in context.M_MajorCassifications on sc.McID equals mc.McID

                             where orderDet.OrID == selectCondition.OrID &&
                             (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && product.PrFlag != 0) || orderDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && sc.ScFlag != 0) || product.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && mc.McFlag != 0) || sc.McID == selectCondition.McID)

                             group new { orderDet, product, sc, mc } by new { product.PrID } into g

                             where g.Sum(x => x.orderDet.OrQuantity) > 0

                             select new T_OrderDetailDsp_Agr_NonFin
                             {                                 
                                 OrID = g.FirstOrDefault().orderDet.OrID,
                                 PrID = g.FirstOrDefault().product.PrID,
                                 ScID = g.FirstOrDefault().sc.ScID,
                                 McID = g.FirstOrDefault().mc.McID,

                                 PrName = g.FirstOrDefault().product.PrName+"("+g.FirstOrDefault().product.PrColor+")",
                                 ScName = g.FirstOrDefault().sc.ScName,
                                 McName = g.FirstOrDefault().mc.McName,
                                 OrQuantitySum = g.Sum(x => x.orderDet.OrQuantity),
                                 OrPriceUnit=g.FirstOrDefault().product.Price,
                                 OrPriceSum = (long)(g.Sum(x => x.orderDet.OrQuantity) * g.FirstOrDefault().product.Price),
                                 ProductFlag=g.FirstOrDefault().product.PrFlag
                             };
                    var res = new List<T_OrderDetailDsp_Agr_NonFin>();
                    int agrID = 0;
                    foreach (var row in tb)
                    {
                        agrID++;
                        row.AgrID = agrID;
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
        public List<T_OrderDetailDsp_Agr_Fin> GetOrderDetailData_Agr_Fin(T_OrderDetailDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from orderDet in context.T_OrderDetails
                             join product in context.M_Products on orderDet.PrID equals product.PrID
                             join sc in context.M_SmallClassifications on product.ScID equals sc.ScID
                             join mc in context.M_MajorCassifications on sc.McID equals mc.McID

                             where orderDet.OrID == selectCondition.OrID &&
                             (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && product.PrFlag != 0) || orderDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && sc.ScFlag != 0) || product.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && mc.McFlag != 0) || sc.McID == selectCondition.McID)

                             group new { orderDet, product, sc, mc } by new { product.PrID } into g

                             where g.Sum(x => x.orderDet.OrQuantity) > 0

                             select new T_OrderDetailDsp_Agr_Fin
                             {
                                 OrID = g.FirstOrDefault().orderDet.OrID,
                                 PrID = g.FirstOrDefault().product.PrID,
                                 ScID = g.FirstOrDefault().sc.ScID,
                                 McID = g.FirstOrDefault().mc.McID,

                                 PrName = g.FirstOrDefault().product.PrName + "(" + g.FirstOrDefault().product.PrColor + ")",
                                 ScName = g.FirstOrDefault().sc.ScName,
                                 McName = g.FirstOrDefault().mc.McName,
                                 OrQuantitySum = g.Sum(x => x.orderDet.OrQuantity),
                                 OrPriceSum = (long)(g.Sum(x => x.orderDet.OrTotalPrice)),
                                 OrPriceUnit=0,
                                 ProductFlag = g.FirstOrDefault().product.PrFlag
                             };
                    var res = new List<T_OrderDetailDsp_Agr_Fin>();
                    int agrID = 0;
                    foreach (var row in tb)
                    {
                        agrID++;
                        row.AgrID = agrID;
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
        public int ProductQuantSumInCurrentOrder(int Prid, int Orid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    int res = (from ordetails in context.T_OrderDetails
                               where ordetails.PrID == Prid &&
                               ordetails.OrID == Orid
                               select (int?)ordetails.OrQuantity).Sum() ?? 0;
                    return res;
                }
            }
            catch
            {
                throw;
            }
        }
        public void RegistOrderData(T_Order regData)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.T_Orders.Add(regData);
                        context.SaveChanges();
                        if (regData.OrID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Order',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Order')");
                            throw new Exception("受注IDが既定の数字を超えています。");
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
        public void RegistOrderDetailData(T_OrderDetail regDetailData)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.T_OrderDetails.Add(regDetailData);
                        context.SaveChanges();
                        if (regDetailData.OrDetailID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_OrderDetail',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_OrderDetail')");
                            throw new Exception("受注詳細IDが既定の数字を超えています。");
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
        public void UpdateOrderHidden(int Orid, int OrFlag, string OrHidden)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Orders.Single(x => x.OrID == Orid);
                    target.OrFlag = OrFlag;
                    target.OrHidden = OrHidden;
                    context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public void FinalizeOrderData(T_Chumon regData, List<T_ChumonDetail> regDetailData,List<T_OrderDetailDsp_Agr_NonFin>OrderDetailData)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.T_Chumons.Add(regData);
                        context.SaveChanges();
                        if (regData.ChID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Chumon',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Chumon')");
                            throw new Exception("注文IDが既定の数字を超えています。");
                        }
                        foreach (var row in regDetailData)
                        {
                            row.ChID = regData.ChID;
                            context.T_ChumonDetails.Add(row);
                            context.SaveChanges();
                            if (row.ChDetailID > NumericRange.ID)
                            {
                                transaction.Rollback();
                                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_ChumonDetail',RESEED,0)");
                                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_ChumonDetail')");
                                throw new Exception("注文詳細IDが既定の数字を超えています。");
                            }
                        }
                        foreach(var row in OrderDetailData)
                        {
                            var orderDetail = context.T_OrderDetails.Where(x =>x.OrID==regData.OrID&& x.PrID == row.PrID).ToList();
                            foreach(var row2 in orderDetail)
                            {
                                row2.OrTotalPrice = row.OrPriceUnit * row2.OrQuantity;
                                row2.QuantFlag = 1;
                            }
                            context.SaveChanges();
                        }
                        var order = context.T_Orders.Single(x => x.OrID == regData.OrID);
                        if (order.OrFlag != 0)
                        {
                            order.OrFlag = 0;
                            order.OrHidden = null;
                        }
                        order.OrStateFlag = 1;
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
        public bool CheckOrderIsInsertable(int Orid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Orders.Single(x => x.OrID == Orid);
                    if (target.OrStateFlag > 0)
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
        public bool CheckOrderExistence(int Orid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    return context.T_Orders.Any(x => x.OrID == Orid);
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckOrderIsActive(int Orid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Orders.Single(x => x.OrID == Orid);
                    if (target.OrFlag == 0)
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
