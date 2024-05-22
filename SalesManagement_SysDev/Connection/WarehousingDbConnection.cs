using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SalesManagement_SysDev
{
    class WarehousingDbConnection
    {
        public List<T_WarehousingDsp> GetWarehousingData()
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Warehousing in context.T_Warehousings
                             join Hattyu in context.T_Hattyus on Warehousing.HaID equals Hattyu.HaID
                             join Maker in context.M_Makers on Hattyu.MaID equals Maker.MaID
                             join WareDet in context.T_WarehousingDetails on Warehousing.WaID equals WareDet.WaID
                             into gjWareDet
                             from subWareDet in gjWareDet.DefaultIfEmpty()
                             join product in context.M_Products on subWareDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Warehousing.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()

                             where Warehousing.WaFlag == 0

                             group new { Warehousing, subEmployee,Hattyu,Maker } by new { Warehousing.WaID } into g

                             select new T_WarehousingDsp
                             {
                                 EmID = g.FirstOrDefault().Warehousing.EmID,
                                 WaFlag = g.FirstOrDefault().Warehousing.WaFlag,
                                 WaShelfFlag = g.FirstOrDefault().Warehousing.WaShelfFlag,

                                 WaID = g.FirstOrDefault().Warehousing.WaID,
                                 HaID = g.FirstOrDefault().Warehousing.HaID,
                                 MaID=g.FirstOrDefault().Hattyu.MaID,
                                 MaName=g.FirstOrDefault().Maker.MaName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 WaDate = g.FirstOrDefault().Warehousing.WaDate,
                                 WaFlagBool = g.FirstOrDefault().Warehousing.WaFlag == 0 ? false : true,
                                 WaHidden = g.FirstOrDefault().Warehousing.WaHidden,
                                 KindOfProducts = context.T_WarehousingDetails.Where(x => x.WaID == g.FirstOrDefault().Warehousing.WaID).Count(),
                                 WaShelfFlagBool = g.FirstOrDefault().Warehousing.WaShelfFlag == 0 ? false : true,
                                 HattyuFlag=g.FirstOrDefault().Hattyu.HaFlag
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_WarehousingDsp> GetWarehousingData(T_WarehousingRead selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Warehousing in context.T_Warehousings
                             join Hattyu in context.T_Hattyus on Warehousing.HaID equals Hattyu.HaID
                             join Maker in context.M_Makers on Hattyu.MaID equals Maker.MaID
                             join WareDet in context.T_WarehousingDetails on Warehousing.WaID equals WareDet.WaID
                             into gjWareDet
                             from subWareDet in gjWareDet.DefaultIfEmpty()
                             join product in context.M_Products on subWareDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Warehousing.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()

                             where (selectCondition.WaFlag==-1||Warehousing.WaFlag == selectCondition.WaFlag) &&
                             (selectCondition.WaID == 0 || Warehousing.WaID == selectCondition.WaID) &&
                             (selectCondition.EmID == 0 ||(selectCondition.EmID==-1&&subEmployee.EmFlag!=0)|| selectCondition.EmID == Warehousing.EmID) &&
                             (selectCondition.HaID == 0 ||(selectCondition.HaID==-1&&Hattyu.HaFlag!=0)|| selectCondition.HaID == Warehousing.HaID) &&
                             (selectCondition.MaID == 0 ||(selectCondition.MaID==-1&&Maker.MaFlag!=0)||selectCondition.MaID==Hattyu.MaID) &&
                             (selectCondition.WaShelfFlag == -1 || selectCondition.WaShelfFlag == Warehousing.WaShelfFlag) &&
                             (string.IsNullOrEmpty(selectCondition.WaHidden) || Warehousing.WaHidden.Contains(selectCondition.WaHidden)) &&
                             (selectCondition.PrID == 0 ||(selectCondition.PrID==-1&&subProduct.PrFlag!=0)|| selectCondition.PrID == subWareDet.PrID) &&
                             (selectCondition.ScID == 0 ||(selectCondition.ScID==-1&&subSc.ScFlag!=0)|| selectCondition.ScID == subProduct.ScID) &&
                             (selectCondition.McID == 0 ||(selectCondition.McID==-1&&subMc.McFlag!=0)|| selectCondition.McID == subSc.McID)

                             group new { Warehousing, subEmployee,Hattyu,Maker } by new { Warehousing.WaID } into g

                             select new T_WarehousingDsp
                             {
                                 EmID = g.FirstOrDefault().Warehousing.EmID,
                                 WaFlag = g.FirstOrDefault().Warehousing.WaFlag,
                                 WaShelfFlag = g.FirstOrDefault().Warehousing.WaShelfFlag,

                                 WaID = g.FirstOrDefault().Warehousing.WaID,
                                 HaID = g.FirstOrDefault().Warehousing.HaID,
                                 MaID=g.FirstOrDefault().Hattyu.MaID,
                                 MaName=g.FirstOrDefault().Maker.MaName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 WaDate = g.FirstOrDefault().Warehousing.WaDate,
                                 WaFlagBool = g.FirstOrDefault().Warehousing.WaFlag == 0 ? false : true,
                                 WaHidden = g.FirstOrDefault().Warehousing.WaHidden,
                                 KindOfProducts = context.T_WarehousingDetails.Where(x => x.WaID == g.FirstOrDefault().Warehousing.WaID).Count(),
                                 WaShelfFlagBool = g.FirstOrDefault().Warehousing.WaShelfFlag == 0 ? false : true,
                                 HattyuFlag=g.FirstOrDefault().Hattyu.HaFlag
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_WarehousingDsp> GetWarehousingData(T_WarehousingRead selectCondition, DateTime? startDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Warehousing in context.T_Warehousings
                             join Hattyu in context.T_Hattyus on Warehousing.HaID equals Hattyu.HaID
                             join Maker in context.M_Makers on Hattyu.MaID equals Maker.MaID
                             join WareDet in context.T_WarehousingDetails on Warehousing.WaID equals WareDet.WaID
                             into gjWareDet
                             from subWareDet in gjWareDet.DefaultIfEmpty()
                             join product in context.M_Products on subWareDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Warehousing.EmID equals Employee.EmID
                             into gjEmployee
                             from subEmployee in gjEmployee.DefaultIfEmpty()

                             where (selectCondition.WaFlag == -1 || Warehousing.WaFlag == selectCondition.WaFlag) &&
                             (selectCondition.WaID == 0 || Warehousing.WaID == selectCondition.WaID) &&
                             (selectCondition.EmID == 0 || (selectCondition.EmID == -1 && subEmployee.EmFlag != 0) || selectCondition.EmID == Warehousing.EmID) &&
                             (selectCondition.HaID == 0 || (selectCondition.HaID == -1 && Hattyu.HaFlag != 0) || selectCondition.HaID == Warehousing.HaID) &&
                             (selectCondition.MaID == 0 || (selectCondition.MaID == -1 && Maker.MaFlag != 0) || selectCondition.MaID == Hattyu.MaID) &&
                             (selectCondition.WaShelfFlag == -1 || selectCondition.WaShelfFlag == Warehousing.WaShelfFlag) &&
                             (string.IsNullOrEmpty(selectCondition.WaHidden) || Warehousing.WaHidden.Contains(selectCondition.WaHidden)) &&
                             (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && subProduct.PrFlag != 0) || selectCondition.PrID == subWareDet.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && subSc.ScFlag != 0) || selectCondition.ScID == subProduct.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && subMc.McFlag != 0) || selectCondition.McID == subSc.McID) &&

                             (startDay == null ? Warehousing.WaDate <= endDay : endDay == null ? Warehousing.WaDate >= startDay : (Warehousing.WaDate >= startDay && Warehousing.WaDate <= endDay))


                             group new { Warehousing, subEmployee ,Hattyu,Maker} by new { Warehousing.WaID } into g

                             select new T_WarehousingDsp
                             {
                                 EmID = g.FirstOrDefault().Warehousing.EmID,
                                 WaFlag = g.FirstOrDefault().Warehousing.WaFlag,
                                 WaShelfFlag = g.FirstOrDefault().Warehousing.WaShelfFlag,

                                 WaID = g.FirstOrDefault().Warehousing.WaID,
                                 HaID = g.FirstOrDefault().Warehousing.HaID,
                                 MaID=g.FirstOrDefault().Hattyu.MaID,
                                 MaName=g.FirstOrDefault().Maker.MaName,
                                 EmName = g.FirstOrDefault().subEmployee.EmName,
                                 WaDate = g.FirstOrDefault().Warehousing.WaDate,
                                 WaFlagBool = g.FirstOrDefault().Warehousing.WaFlag == 0 ? false : true,
                                 WaHidden = g.FirstOrDefault().Warehousing.WaHidden,
                                 KindOfProducts = context.T_WarehousingDetails.Where(x => x.WaID == g.FirstOrDefault().Warehousing.WaID).Count(),
                                 WaShelfFlagBool = g.FirstOrDefault().Warehousing.WaShelfFlag == 0 ? false : true,
                                 HattyuFlag=g.FirstOrDefault().Hattyu.HaFlag
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_WarehousingDetailDsp> GetWarehousingDetailData(T_WarehousingDetailDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from WareDet in context.T_WarehousingDetails
                             join Product in context.M_Products on WareDet.PrID equals Product.PrID
                             join Sc in context.M_SmallClassifications on Product.ScID equals Sc.ScID
                             join Mc in context.M_MajorCassifications on Sc.McID equals Mc.McID

                             where (WareDet.WaID == selectCondition.WaID) &&
                             (selectCondition.WaDetailID == 0 || WareDet.WaDetailID == selectCondition.WaDetailID) &&
                             (selectCondition.PrID == 0 ||(selectCondition.PrID==-1&&Product.PrFlag!=0)|| WareDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 ||(selectCondition.ScID==-1&&Sc.ScFlag!=0)|| Product.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 ||(selectCondition.McID==-1&&Mc.McFlag!=0)|| Sc.McID == selectCondition.McID)

                             select new T_WarehousingDetailDsp
                             {
                                 WaID = WareDet.WaID,
                                 PrID = WareDet.PrID,
                                 ScID = Product.ScID,
                                 McID = Sc.McID,

                                 WaDetailID = WareDet.WaDetailID,
                                 PrName = Product.PrName + "(" + Product.PrColor + ")",
                                 ScName = Sc.ScName,
                                 McName = Mc.McName,
                                 WaQuantity = WareDet.WaQuantity
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public void UpdateWarehousingHidden(int Waid, int WaFlag, string WaHidden)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Warehousings.Single(x => x.WaID == Waid);
                    target.WaFlag = WaFlag;
                    target.WaHidden = WaHidden;
                    context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public void FinalizeWarehousingData(int WaID, int EmID)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        var finData = context.T_Warehousings.Single(x => x.WaID == WaID);
                        var waDet = context.T_WarehousingDetails.Where(x => x.WaID == finData.WaID);
                        foreach (var row in waDet)
                        {
                            var stock = context.T_Stocks.Single(x => x.PrID == row.PrID);
                            stock.StQuantity += row.WaQuantity;
                            context.SaveChanges();
                        }
                        var hattyu = context.T_Hattyus.Single(x => x.HaID == finData.HaID);
                        var warehousing = context.T_Warehousings.Single(x => x.WaID == finData.WaID);
                        hattyu.WaWarehouseFlag = 2;
                        warehousing.WaShelfFlag = 1;
                        warehousing.EmID = EmID;
                        warehousing.WaDate = DateTime.Now;
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
        public bool CheckWarehousingExistence(int Waid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    return context.T_Warehousings.Any(x => x.WaID == Waid);
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckWarehousingIsActive(int Waid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Warehousings.Single(x => x.WaID == Waid);
                    if (target.WaFlag == 0)
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
        public bool CheckWarehousingIsInsertable(int Waid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Warehousings.Single(x => x.WaID == Waid);
                    if (target.WaShelfFlag == 0)
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
