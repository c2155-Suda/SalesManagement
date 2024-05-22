using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_SysDev
{
    class ChumonDbConnection
    {
        public List<T_ChumonDsp> GetChumonData()
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from chumon in context.T_Chumons
                             join order in context.T_Orders on chumon.OrID equals order.OrID
                             join chumondet in context.T_ChumonDetails on chumon.ChID equals chumondet.ChID
                             into gjchumondet
                             from subchumondet in gjchumondet.DefaultIfEmpty()
                             join product in context.M_Products on subchumondet.PrID equals product.PrID
                             into gjproduct
                             from subproduct in gjproduct.DefaultIfEmpty()
                             join sc in context.M_SmallClassifications on subproduct.ScID equals sc.ScID
                             into gjsc
                             from subsc in gjsc.DefaultIfEmpty()
                             join mc in context.M_MajorCassifications on subsc.McID equals mc.McID
                             into gjmc
                             from submc in gjmc.DefaultIfEmpty()
                             join em in context.M_Employees on chumon.EmID equals em.EmID
                             into gjem
                             from subem in gjem.DefaultIfEmpty()
                             join cl in context.M_Clients on chumon.ClID equals cl.ClID
                             join so in context.M_SalesOffices on chumon.SoID equals so.SoID

                             where chumon.ChFlag == 0

                             group new { chumon, subem, cl, so,order } by new { chumon.ChID } into g


                             select new T_ChumonDsp
                             {
                                 ClID = g.FirstOrDefault().cl.ClID,
                                 SoID = g.FirstOrDefault().so.SoID,
                                 EmID = g.FirstOrDefault().subem.EmID,
                                 ChFlag = g.FirstOrDefault().cl.ClFlag,
                                 ChStateFlag = g.FirstOrDefault().chumon.ChStateFlag,

                                 ChID = g.FirstOrDefault().chumon.ChID,
                                 OrID = g.FirstOrDefault().chumon.OrID,
                                 ClName = g.FirstOrDefault().cl.ClName,
                                 EmName = g.FirstOrDefault().subem.EmName,
                                 SoName = g.FirstOrDefault().so.SoName,
                                 ChDate = g.FirstOrDefault().chumon.ChDate,
                                 ChBoolFlag = g.FirstOrDefault().chumon.ChFlag == 0 ? false : true,
                                 ChHidden = g.FirstOrDefault().chumon.ChHidden,
                                 KindOfProducts = context.T_ChumonDetails.Where(x => x.ChID == g.FirstOrDefault().chumon.ChID).Count(),
                                 ChBoolStateFlag = g.FirstOrDefault().chumon.ChStateFlag == 0 ? false : true,
                                 OrderFlag=g.FirstOrDefault().order.OrFlag

                             };

                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_ChumonDsp> GetChumonData(T_ChumonRead selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from chumon in context.T_Chumons
                             join order in context.T_Orders on chumon.OrID equals order.OrID
                             join chumondet in context.T_ChumonDetails on chumon.ChID equals chumondet.ChID
                             into gjchumondet
                             from subchumondet in gjchumondet.DefaultIfEmpty()
                             join product in context.M_Products on subchumondet.PrID equals product.PrID
                             into gjproduct
                             from subproduct in gjproduct.DefaultIfEmpty()
                             join sc in context.M_SmallClassifications on subproduct.ScID equals sc.ScID
                             into gjsc
                             from subsc in gjsc.DefaultIfEmpty()
                             join mc in context.M_MajorCassifications on subsc.McID equals mc.McID
                             into gjmc
                             from submc in gjmc.DefaultIfEmpty()
                             join em in context.M_Employees on chumon.EmID equals em.EmID
                             into gjem
                             from subem in gjem.DefaultIfEmpty()
                             join cl in context.M_Clients on chumon.ClID equals cl.ClID
                             join so in context.M_SalesOffices on chumon.SoID equals so.SoID

                             where (chumon.ChFlag == selectCondition.ChFlag||selectCondition.ChFlag==-1) &&
                             (selectCondition.ChID == 0 || chumon.ChID == selectCondition.ChID) &&
                             (selectCondition.EmID == 0 ||(selectCondition.EmID==-1&&subem.EmFlag!=0)|| selectCondition.EmID == chumon.EmID) &&
                             (selectCondition.ClID == 0 ||(selectCondition.ClID==-1&&cl.ClFlag!=0)||selectCondition.ClID == chumon.ClID) &&
                             (selectCondition.SoID == 0 ||(selectCondition.SoID==-1&&so.SoFlag!=0)|| selectCondition.SoID == chumon.SoID) &&
                             (selectCondition.OrID == 0 ||(selectCondition.OrID==-1&&order.OrFlag!=0)|| selectCondition.OrID == chumon.OrID) &&
                             (selectCondition.ChStateFlag == -1 || selectCondition.ChStateFlag == chumon.ChStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.ChHidden) || chumon.ChHidden.Contains(selectCondition.ChHidden)) &&
                             (selectCondition.PrID == 0 ||(selectCondition.PrID==-1&&subproduct.PrFlag!=0)|| selectCondition.PrID == subchumondet.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && subsc.ScFlag!=0)||selectCondition.ScID == subproduct.ScID) &&
                             (selectCondition.McID == 0 ||(selectCondition.McID==-1&&submc.McFlag!=0)|| selectCondition.McID == subsc.McID)

                             group new { chumon, subem, cl, so,order } by new { chumon.ChID } into g


                             select new T_ChumonDsp
                             {
                                 ClID = g.FirstOrDefault().cl.ClID,
                                 SoID = g.FirstOrDefault().so.SoID,
                                 EmID = g.FirstOrDefault().subem.EmID,
                                 ChFlag = g.FirstOrDefault().cl.ClFlag,
                                 ChStateFlag = g.FirstOrDefault().chumon.ChStateFlag,

                                 ChID = g.FirstOrDefault().chumon.ChID,
                                 OrID = g.FirstOrDefault().chumon.OrID,
                                 ClName = g.FirstOrDefault().cl.ClName,
                                 EmName = g.FirstOrDefault().subem.EmName,
                                 SoName = g.FirstOrDefault().so.SoName,
                                 ChDate = g.FirstOrDefault().chumon.ChDate,
                                 ChBoolFlag = g.FirstOrDefault().chumon.ChFlag == 0 ? false : true,
                                 ChHidden = g.FirstOrDefault().chumon.ChHidden,
                                 KindOfProducts = context.T_ChumonDetails.Where(x => x.ChID == g.FirstOrDefault().chumon.ChID).Count(),
                                 ChBoolStateFlag = g.FirstOrDefault().chumon.ChStateFlag == 0 ? false : true,
                                 OrderFlag=g.FirstOrDefault().order.OrFlag

                             };

                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_ChumonDsp> GetChumonData(T_ChumonRead selectCondition, DateTime? startDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from chumon in context.T_Chumons
                             join order in context.T_Orders on chumon.OrID equals order.OrID
                             join chumondet in context.T_ChumonDetails on chumon.ChID equals chumondet.ChID
                             into gjchumondet
                             from subchumondet in gjchumondet.DefaultIfEmpty()
                             join product in context.M_Products on subchumondet.PrID equals product.PrID
                             into gjproduct
                             from subproduct in gjproduct.DefaultIfEmpty()
                             join sc in context.M_SmallClassifications on subproduct.ScID equals sc.ScID
                             into gjsc
                             from subsc in gjsc.DefaultIfEmpty()
                             join mc in context.M_MajorCassifications on subsc.McID equals mc.McID
                             into gjmc
                             from submc in gjmc.DefaultIfEmpty()
                             join em in context.M_Employees on chumon.EmID equals em.EmID
                             into gjem
                             from subem in gjem.DefaultIfEmpty()
                             join cl in context.M_Clients on chumon.ClID equals cl.ClID
                             join so in context.M_SalesOffices on chumon.SoID equals so.SoID

                             where (chumon.ChFlag == selectCondition.ChFlag || selectCondition.ChFlag == -1) &&
                             (selectCondition.ChID == 0 || chumon.ChID == selectCondition.ChID) &&
                             (selectCondition.EmID == 0 || (selectCondition.EmID == -1 && subem.EmFlag != 0) || selectCondition.EmID == chumon.EmID) &&
                             (selectCondition.ClID == 0 || (selectCondition.ClID == -1 && cl.ClFlag != 0) || selectCondition.ClID == chumon.ClID) &&
                             (selectCondition.SoID == 0 || (selectCondition.SoID == -1 && so.SoFlag != 0) || selectCondition.SoID == chumon.SoID) &&
                             (selectCondition.OrID == 0 || (selectCondition.OrID == -1 && order.OrFlag != 0) || selectCondition.OrID == chumon.OrID) &&
                             (selectCondition.ChStateFlag == -1 || selectCondition.ChStateFlag == chumon.ChStateFlag) &&
                             (string.IsNullOrEmpty(selectCondition.ChHidden) || chumon.ChHidden.Contains(selectCondition.ChHidden)) &&
                             (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && subproduct.PrFlag != 0) || selectCondition.PrID == subchumondet.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && subsc.ScFlag != 0) || selectCondition.ScID == subproduct.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && submc.McFlag != 0) || selectCondition.McID == subsc.McID) &&

                             (startDay == null ? chumon.ChDate <= endDay : endDay == null ? chumon.ChDate >= startDay : (chumon.ChDate >= startDay && chumon.ChDate <= endDay))


                             group new { chumon, subem, cl, so,order } by new { chumon.ChID } into g


                             select new T_ChumonDsp
                             {
                                 ClID = g.FirstOrDefault().cl.ClID,
                                 SoID = g.FirstOrDefault().so.SoID,
                                 EmID = g.FirstOrDefault().subem.EmID,
                                 ChFlag = g.FirstOrDefault().cl.ClFlag,
                                 ChStateFlag = g.FirstOrDefault().chumon.ChStateFlag,

                                 ChID = g.FirstOrDefault().chumon.ChID,
                                 OrID = g.FirstOrDefault().chumon.OrID,
                                 ClName = g.FirstOrDefault().cl.ClName,
                                 EmName = g.FirstOrDefault().subem.EmName,
                                 SoName = g.FirstOrDefault().so.SoName,
                                 ChDate = g.FirstOrDefault().chumon.ChDate,
                                 ChBoolFlag = g.FirstOrDefault().chumon.ChFlag == 0 ? false : true,
                                 ChHidden = g.FirstOrDefault().chumon.ChHidden,
                                 KindOfProducts = context.T_ChumonDetails.Where(x => x.ChID == g.FirstOrDefault().chumon.ChID).Count(),
                                 ChBoolStateFlag = g.FirstOrDefault().chumon.ChStateFlag == 0 ? false : true,
                                 OrderFlag=g.FirstOrDefault().order.OrFlag

                             };

                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_ChumonDetailDsp> GetChumonDetailData(T_ChumonDetailDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from ChumonDet in context.T_ChumonDetails
                             join Product in context.M_Products on ChumonDet.PrID equals Product.PrID
                             join Sc in context.M_SmallClassifications on Product.ScID equals Sc.ScID
                             join Mc in context.M_MajorCassifications on Sc.McID equals Mc.McID

                             where ChumonDet.ChID == selectCondition.ChID &&
                             (selectCondition.ChDetailID == 0 || ChumonDet.ChDetailID == selectCondition.ChDetailID) &&
                             (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && Product.PrFlag != 0) || ChumonDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && Sc.ScFlag != 0) || Product.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && Mc.McFlag != 0) || Sc.McID == selectCondition.McID)

                             select new T_ChumonDetailDsp
                             {
                                 ChID = ChumonDet.ChID,
                                 PrID = ChumonDet.PrID,
                                 ScID = Product.ScID,
                                 McID = Sc.McID,

                                 ChDetailID = ChumonDet.ChDetailID,
                                 PrName = Product.PrName + "(" + Product.PrColor + ")",
                                 ScName = Sc.ScName,
                                 McName = Mc.McName,
                                 ChQuantity = ChumonDet.ChQuantity
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public void UpdateChumonHidden(int Chid, int ChFlag, string ChHidden)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Chumons.Single(x => x.ChID == Chid);
                    target.ChFlag = ChFlag;
                    target.ChHidden = ChHidden;
                    context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public void FinalizeChumonData(T_Syukko regData, List<T_SyukkoDetail> regDetailData, int EmID)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.T_Syukkos.Add(regData);
                        context.SaveChanges();
                        if (regData.SyID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Syukko',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_Syukko')");
                            throw new Exception("出庫IDが既定の数字を超えています。");
                        }
                        foreach (var row in regDetailData)
                        {
                            row.SyID = regData.SyID;
                            context.T_SyukkoDetails.Add(row);
                            context.SaveChanges();
                            if (row.SyDetailID > NumericRange.ID)
                            {
                                transaction.Rollback();
                                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_SyukkoDetail',RESEED,0)");
                                context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_SyukkoDetail')");
                                throw new Exception("出庫詳細IDが既定の数字を超えています。");
                            }
                            var stock = context.T_Stocks.Single(x => x.PrID == row.PrID);
                            stock.StQuantity -= row.SyQuantity;
                            context.SaveChanges();
                        }
                        var chumon = context.T_Chumons.Single(x => x.OrID == regData.OrID);
                        var order = context.T_Orders.Single(x => x.OrID == regData.OrID);
                        chumon.ChStateFlag = 1;
                        chumon.EmID = EmID;
                        order.OrStateFlag = 2;
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
        public bool CheckChumonExistence(int Chid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    return context.T_Chumons.Any(x => x.ChID == Chid);
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckChumonIsActive(int Chid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Chumons.Single(x => x.ChID == Chid);
                    if (target.ChFlag == 0)
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
        public bool CheckChumonIsInsertable(int Chid)
        {
            try
            {
                using(var context=new SalesManagement_DevContext())
                {
                    var target = context.T_Chumons.Single(x => x.ChID == Chid);
                    if (target.ChStateFlag == 0)
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
