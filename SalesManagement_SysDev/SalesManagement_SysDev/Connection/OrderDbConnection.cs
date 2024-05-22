using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    class OrderDbConnection
    {
        static string[] StateFlagStrings = { string.Empty, "受注済", "注文済", "出庫済", "入荷済", "出荷済" };

        //データ読込
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
                                 KindOfProducts = context.T_OrderDetails.Where(x => x.OrID == g.FirstOrDefault().order.OrID).GroupBy(x => x.PrID).Where(x => x.Sum(y => y.OrQuantity) > 0).Count()

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
        //検索用
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

                             where order.OrFlag == selectCondition.OrFlag &&
                             (selectCondition.SoID == 0 || order.OrFlag == selectCondition.OrFlag) &&
                             (selectCondition.EmID == 0 || order.EmID == selectCondition.EmID) &&
                             (selectCondition.ClID == 0 || order.ClID == selectCondition.ClID) &&
                             order.ClCharge.Contains(selectCondition.ClCharge) &&
                             (selectCondition.OrStateFlag == -1 || order.OrStateFlag == selectCondition.OrStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.OrHidden) || order.OrHidden.Contains(selectCondition.OrHidden)) &&
                             (selectCondition.PrID == 0 || subOrderDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 || subProduct.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 || subSc.McID == selectCondition.McID)


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
                                 KindOfProducts = context.T_OrderDetails.Where(x => x.OrID == g.FirstOrDefault().order.OrID).GroupBy(x => x.PrID).Where(x => x.Sum(y => y.OrQuantity) > 0).Count()

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
        //time検索用
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

                             where order.OrFlag == selectCondition.OrFlag &&
                             (selectCondition.SoID == 0 || order.OrFlag == selectCondition.OrFlag) &&
                             (selectCondition.EmID == 0 || order.EmID == selectCondition.EmID) &&
                             (selectCondition.ClID == 0 || order.ClID == selectCondition.ClID) &&
                             order.ClCharge.Contains(selectCondition.ClCharge) &&
                             (selectCondition.OrStateFlag == -1 || order.OrStateFlag == selectCondition.OrStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.OrHidden) || order.OrHidden.Contains(selectCondition.OrHidden)) &&
                             (selectCondition.PrID == 0 || subOrderDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 || subProduct.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 || subSc.McID == selectCondition.McID) &&

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
                                 KindOfProducts = context.T_OrderDetails.Where(x => x.OrID == g.FirstOrDefault().order.OrID).GroupBy(x => x.PrID).Where(x => x.Sum(y => y.OrQuantity) > 0).Count()

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
        //詳細検索
        public List<T_OrderDetailDsp> GetOrderDetailData(T_OrderDetailDsp selectConditon)
        {

            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from orderDet in context.T_OrderDetails
                             join product in context.M_Products on orderDet.PrID equals product.PrID
                             join sc in context.M_SmallClassifications on product.ScID equals sc.ScID
                             join mc in context.M_MajorCassifications on sc.McID equals mc.McID

                             where orderDet.OrID == selectConditon.OrID &&
                             (selectConditon.OrDetailID == 0 || orderDet.OrDetailID == selectConditon.OrDetailID) &&
                             (selectConditon.PrID == 0 || orderDet.PrID == selectConditon.PrID) &&
                             (selectConditon.ScID == 0 || product.ScID == selectConditon.ScID) &&
                             (selectConditon.McID == 0 || sc.McID == selectConditon.McID)

                             select new T_OrderDetailDsp
                             {
                                 OrID = orderDet.OrID,
                                 PrID = orderDet.PrID,
                                 ScID = product.ScID,
                                 McID = sc.McID,

                                 PrName = product.PrName,
                                 ScName = sc.ScName,
                                 McName = mc.McName,
                                 OrQuantity = orderDet.OrQuantity,
                                 OrPrice = (long)(orderDet.OrTotalPrice != 0 ? orderDet.OrTotalPrice : product.Price * orderDet.OrQuantity)
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

                             group new { orderDet, product, sc, mc } by new { product.PrID } into g

                             where g.Sum(x => x.orderDet.OrQuantity) > 0

                             select new T_OrderDetailDsp_Agr_NonFin
                             {
                                 OrID = g.FirstOrDefault().orderDet.OrID,
                                 PrID = g.FirstOrDefault().product.PrID,
                                 ScID = g.FirstOrDefault().sc.ScID,
                                 McID = g.FirstOrDefault().mc.McID,

                                 PrName = g.FirstOrDefault().product.PrName,
                                 ScName = g.FirstOrDefault().sc.ScName,
                                 McName = g.FirstOrDefault().mc.McName,
                                 OrQuantitySum = g.Sum(x => x.orderDet.OrQuantity),
                                 OrPriceSum = (long)(g.Sum(x => x.orderDet.OrQuantity) * g.FirstOrDefault().product.Price)
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

                             group new { orderDet, product, sc, mc } by new { product.PrID } into g

                             where g.Sum(x => x.orderDet.OrQuantity) > 0

                             select new T_OrderDetailDsp_Agr_Fin
                             {
                                 OrID = g.FirstOrDefault().orderDet.OrID,
                                 PrID = g.FirstOrDefault().product.PrID,
                                 ScID = g.FirstOrDefault().sc.ScID,
                                 McID = g.FirstOrDefault().mc.McID,

                                 PrName = g.FirstOrDefault().product.PrName,
                                 ScName = g.FirstOrDefault().sc.ScName,
                                 McName = g.FirstOrDefault().mc.McName,
                                 OrQuantitySum = g.Sum(x => x.orderDet.OrQuantity),
                                 OrPriceSum = (long)(g.Sum(x => x.orderDet.OrTotalPrice))
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

        //合計
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
        public decimal ProductPriceSumInCurrentOrder(int Prid, int Orid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    decimal res = (from ordetail in context.T_OrderDetails
                                   where ordetail.PrID == Prid &&
                                   ordetail.OrID == Orid
                                   select (decimal?)ordetail.OrQuantity).Sum() ?? 0;
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
        public void FinalizeOrderData(T_Chumon regData, List<T_ChumonDetail> regDetailData)
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
                            throw new Exception("注文IDが既定の数字を超えています。");
                        }
                        foreach (var row in regDetailData)
                        {
                            context.T_ChumonDetails.Add(row);
                            context.SaveChanges();
                            if (row.ChDetailID > NumericRange.ID)
                            {
                                transaction.Rollback();
                                throw new Exception("注文詳細IDが既定の数字を超えています。");
                            }
                        }
                        var order = context.T_Orders.Single(x => x.OrID == regData.OrID);
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
        public List<T_OrderDetail> TrimOrderDetailData(int Orid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var res = context.T_OrderDetails.Where(x => x.OrID == Orid).GroupBy(x => x.PrID).Where(x => x.Sum(y => y.OrQuantity) > 0).Select(x => new { OrID = Orid, PrID = x.Key, OrQuantity = x.Sum(y => y.OrQuantity) })
                              .AsEnumerable().Select(x => new T_OrderDetail { OrID = x.OrID, PrID = x.PrID, OrQuantity = x.OrQuantity });
                    return res.ToList();
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
