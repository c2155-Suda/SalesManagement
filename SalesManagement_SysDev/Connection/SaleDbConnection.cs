using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_SysDev
{
    class SaleDbConnection
    {
        public List<T_SaleDsp> GetSaleData()
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Sale in context.T_Sale
                             join SaleDet in context.T_SaleDetails on Sale.SaID equals SaleDet.SaID
                             into gjSaleDet
                             from subSaleDet in gjSaleDet.DefaultIfEmpty()
                             join product in context.M_Products on subSaleDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Sale.EmID equals Employee.EmID
                             join Client in context.M_Clients on Sale.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Sale.SoID equals SalesOfficce.SoID

                             where Sale.SaFlag == 0

                             group new { Sale, Employee, Client, SalesOfficce } by new { Sale.SaID } into g

                             select new T_SaleDsp
                             {
                                 ClID = g.FirstOrDefault().Sale.ClID,
                                 EmID = g.FirstOrDefault().Sale.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 SaFlag = g.FirstOrDefault().Sale.SaFlag,

                                 SaID = g.FirstOrDefault().Sale.SaID,
                                 OrID = g.FirstOrDefault().Sale.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().Employee.EmName,
                                 SaDate = g.FirstOrDefault().Sale.SaDate,
                                 SaBoolFlag = g.FirstOrDefault().Sale.SaFlag == 0 ? false : true,
                                 SaHidden = g.FirstOrDefault().Sale.SaHidden,
                                 KindOfProducts = context.T_SaleDetails.Where(x => x.SaID == g.FirstOrDefault().Sale.SaID).Count(),
                                 SumPrice = (long)context.T_SaleDetails.Where(x => x.SaID == g.FirstOrDefault().Sale.SaID).Sum(x => x.SaTotalPrice)
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_SaleDsp> GetSaleData(T_SaleRead selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Sale in context.T_Sale
                             join SaleDet in context.T_SaleDetails on Sale.SaID equals SaleDet.SaID
                             into gjSaleDet
                             from subSaleDet in gjSaleDet.DefaultIfEmpty()
                             join product in context.M_Products on subSaleDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Sale.EmID equals Employee.EmID
                             join Client in context.M_Clients on Sale.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Sale.SoID equals SalesOfficce.SoID

                             where (selectCondition.SaFlag==-1||Sale.SaFlag == selectCondition.SaFlag) &&
                             (selectCondition.SaID == 0 || Sale.SaID == selectCondition.SaID) &&
                             (selectCondition.EmID == 0 ||(selectCondition.EmID==-1&&Employee.EmFlag!=0)|| selectCondition.EmID == Sale.EmID) &&
                             (selectCondition.ClID == 0 ||(selectCondition.ClID==-1&&Client.ClFlag!=0)|| selectCondition.ClID == Sale.ClID) &&
                             (selectCondition.SoID == 0 ||(selectCondition.SoID==-1&&SalesOfficce.SoFlag!=0)|| selectCondition.SoID == Sale.SoID) &&
                             (selectCondition.OrID == 0 || selectCondition.OrID == Sale.OrID) &&
                             (string.IsNullOrEmpty(selectCondition.SaHidden) || Sale.SaHidden.Contains(selectCondition.SaHidden)) &&
                             (selectCondition.PrID == 0 ||(selectCondition.PrID==-1&&subProduct.PrFlag!=0)|| selectCondition.PrID == subSaleDet.PrID) &&
                             (selectCondition.ScID == 0 ||(selectCondition.ScID==-1&&subSc.ScFlag!=0)|| selectCondition.ScID == subProduct.ScID) &&
                             (selectCondition.McID == 0 ||(selectCondition.McID==-1&&subMc.McFlag!=0)|| selectCondition.McID == subSc.McID)

                             group new { Sale, Employee, Client, SalesOfficce } by new { Sale.SaID } into g

                             select new T_SaleDsp
                             {
                                 ClID = g.FirstOrDefault().Sale.ClID,
                                 EmID = g.FirstOrDefault().Sale.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 SaFlag = g.FirstOrDefault().Sale.SaFlag,

                                 SaID = g.FirstOrDefault().Sale.SaID,
                                 OrID = g.FirstOrDefault().Sale.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().Employee.EmName,
                                 SaDate = g.FirstOrDefault().Sale.SaDate,
                                 SaBoolFlag = g.FirstOrDefault().Sale.SaFlag == 0 ? false : true,
                                 SaHidden = g.FirstOrDefault().Sale.SaHidden,
                                 KindOfProducts = context.T_SaleDetails.Where(x => x.SaID == g.FirstOrDefault().Sale.SaID).Count(),
                                 SumPrice = (long)context.T_SaleDetails.Where(x => x.SaID == g.FirstOrDefault().Sale.SaID).Sum(x => x.SaTotalPrice)
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_SaleDsp> GetSaleData(T_SaleRead selectCondition, DateTime? startDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from Sale in context.T_Sale
                             join SaleDet in context.T_SaleDetails on Sale.SaID equals SaleDet.SaID
                             into gjSaleDet
                             from subSaleDet in gjSaleDet.DefaultIfEmpty()
                             join product in context.M_Products on subSaleDet.PrID equals product.PrID
                             into gjProduct
                             from subProduct in gjProduct.DefaultIfEmpty()
                             join Sc in context.M_SmallClassifications on subProduct.ScID equals Sc.ScID
                             into gjSc
                             from subSc in gjSc.DefaultIfEmpty()
                             join Mc in context.M_MajorCassifications on subSc.McID equals Mc.McID
                             into gjMc
                             from subMc in gjMc.DefaultIfEmpty()
                             join Employee in context.M_Employees on Sale.EmID equals Employee.EmID
                             join Client in context.M_Clients on Sale.ClID equals Client.ClID
                             join SalesOfficce in context.M_SalesOffices on Sale.SoID equals SalesOfficce.SoID

                             where (selectCondition.SaFlag == -1 || Sale.SaFlag == selectCondition.SaFlag) &&
                             (selectCondition.SaID == 0 || Sale.SaID == selectCondition.SaID) &&
                             (selectCondition.EmID == 0 || (selectCondition.EmID == -1 && Employee.EmFlag != 0) || selectCondition.EmID == Sale.EmID) &&
                             (selectCondition.ClID == 0 || (selectCondition.ClID == -1 && Client.ClFlag != 0) || selectCondition.ClID == Sale.ClID) &&
                             (selectCondition.SoID == 0 || (selectCondition.SoID == -1 && SalesOfficce.SoFlag != 0) || selectCondition.SoID == Sale.SoID) &&
                             (selectCondition.OrID == 0 || selectCondition.OrID == Sale.OrID) &&
                             (string.IsNullOrEmpty(selectCondition.SaHidden) || Sale.SaHidden.Contains(selectCondition.SaHidden)) &&
                             (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && subProduct.PrFlag != 0) || selectCondition.PrID == subSaleDet.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && subSc.ScFlag != 0) || selectCondition.ScID == subProduct.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && subMc.McFlag != 0) || selectCondition.McID == subSc.McID) &&

                             (startDay == null ? Sale.SaDate <= endDay : endDay == null ? Sale.SaDate >= startDay : (Sale.SaDate >= startDay && Sale.SaDate <= endDay))


                             group new { Sale, Employee, Client, SalesOfficce } by new { Sale.SaID } into g

                             select new T_SaleDsp
                             {
                                 ClID = g.FirstOrDefault().Sale.ClID,
                                 EmID = g.FirstOrDefault().Sale.EmID,
                                 SoID = g.FirstOrDefault().SalesOfficce.SoID,
                                 SaFlag = g.FirstOrDefault().Sale.SaFlag,

                                 SaID = g.FirstOrDefault().Sale.SaID,
                                 OrID = g.FirstOrDefault().Sale.OrID,
                                 ClName = g.FirstOrDefault().Client.ClName,
                                 SoName = g.FirstOrDefault().SalesOfficce.SoName,
                                 EmName = g.FirstOrDefault().Employee.EmName,
                                 SaDate = g.FirstOrDefault().Sale.SaDate,
                                 SaBoolFlag = g.FirstOrDefault().Sale.SaFlag == 0 ? false : true,
                                 SaHidden = g.FirstOrDefault().Sale.SaHidden,
                                 KindOfProducts = context.T_SaleDetails.Where(x => x.SaID == g.FirstOrDefault().Sale.SaID).Count(),
                                 SumPrice = (long)context.T_SaleDetails.Where(x => x.SaID == g.FirstOrDefault().Sale.SaID).Sum(x => x.SaTotalPrice)
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_SaleDetailDsp> GetSaleDetailData(T_SaleDetailDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from SaleDet in context.T_SaleDetails
                             join Product in context.M_Products on SaleDet.PrID equals Product.PrID
                             join Sc in context.M_SmallClassifications on Product.ScID equals Sc.ScID
                             join Mc in context.M_MajorCassifications on Sc.McID equals Mc.McID

                             where SaleDet.SaID == selectCondition.SaID &&
                             (selectCondition.SaDetailID == 0 || SaleDet.SaDetailID == selectCondition.SaDetailID) &&
                             (selectCondition.PrID == 0 || (selectCondition.PrID == -1 && Product.PrFlag != 0) || SaleDet.PrID == selectCondition.PrID) &&
                             (selectCondition.ScID == 0 || (selectCondition.ScID == -1 && Sc.ScFlag != 0) || Product.ScID == selectCondition.ScID) &&
                             (selectCondition.McID == 0 || (selectCondition.McID == -1 && Mc.McFlag != 0) || Sc.McID == selectCondition.McID)

                             select new T_SaleDetailDsp
                             {
                                 SaID = SaleDet.SaID,
                                 PrID = SaleDet.PrID,
                                 ScID = Product.ScID,
                                 McID = Sc.McID,

                                 SaDetailID = SaleDet.SaDetailID,
                                 PrName = Product.PrName + "(" + Product.PrColor + ")",
                                 ScName = Sc.ScName,
                                 McName = Mc.McName,
                                 SaQuantity = SaleDet.SaQuantity,
                                 SaPrice = (long)SaleDet.SaTotalPrice
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public void UpdateSaleHidden(int Said, int SaFlag, string SaHidden)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Sale.Single(x => x.SaID == Said);
                    target.SaFlag = SaFlag;
                    target.SaHidden = SaHidden;
                    context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckSaleExistence(int Said)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    return context.T_Sale.Any(x => x.SaID == Said);
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckSaleIsActive(int Said)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_Sale.Single(x => x.SaID == Said);
                    if (target.SaFlag == 0)
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

