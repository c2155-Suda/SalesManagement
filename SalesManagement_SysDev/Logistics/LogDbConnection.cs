using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement_SysDev
{
    class LogDbConnection
    {   
        public F_Login parentlog;        
        public bool CheckLoginIDPass(int logID,string logPass)
        {
            //EmIDの存在を判定する
            bool flg = false;
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    //社員IDで一致するデータが存在するか
                    var target = context.M_Employees.Single(x => x.EmID == logID);
                    if (target.EmPassword != logPass)
                    {
                        flg = false;
                    }
                    else
                    {
                        parentlog.logSoId = target.SoID;
                        parentlog.logPoId = target.PoID;
                        flg = true;
                    }
                }
            }
            catch
            {
                throw;
            }
            return flg;
        }
        public void RegistLogHistoryData(T_LoginHistory regData)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        context.T_LoginHistorys.Add(regData);
                        context.SaveChanges();
                        if (regData.LoHistoryID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_LoginHistory',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('T_LoginHistory')");
                            throw new Exception("ログイン履歴IDが既定の数字を超えています。");
                        }
                        parentlog.logHisId = regData.LoHistoryID;
                        transaction.Commit();
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        public void UpdateLogHistoryData(int logHisId, DateTime logOutData)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.T_LoginHistorys.Single(x => x.LoHistoryID == logHisId);
                    target.LogoutDate = logOutData;
                    context.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_LoginHistoryDsp> GetLoginHistoryData()
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from loghistory in context.T_LoginHistorys
                             join employee in context.M_Employees on loghistory.EmID equals employee.EmID
                             join salesoffice in context.M_SalesOffices on employee.SoID equals salesoffice.SoID
                             join position in context.M_Positions on employee.PoID equals position.PoID

                             select new T_LoginHistoryDsp
                             {
                                 LoHistoryID=loghistory.LoHistoryID,
                                 EmID = employee.EmID,
                                 EmName = employee.EmName,
                                 SoID = employee.SoID,
                                 SoName = salesoffice.SoName,
                                 PoID = employee.PoID,
                                 PoName = position.PoName,
                                 LoginDate=loghistory.LoginDate,
                                 LogoutDate=loghistory.LogoutDate
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_LoginHistoryDsp> GetLoginHistoryData(T_LoginHistoryDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from loghistory in context.T_LoginHistorys
                             join employee in context.M_Employees on loghistory.EmID equals employee.EmID
                             join salesoffice in context.M_SalesOffices on employee.SoID equals salesoffice.SoID
                             join position in context.M_Positions on employee.PoID equals position.PoID

                             where (selectCondition.LoHistoryID==0||selectCondition.LoHistoryID==loghistory.LoHistoryID)&&
                             (selectCondition.EmID==0||(selectCondition.EmID==-1&&employee.EmFlag!=0||selectCondition.EmID==employee.EmID))&&
                             (selectCondition.SoID==0||(selectCondition.SoID==-1&&salesoffice.SoFlag!=0)||selectCondition.SoID==salesoffice.SoID)&&
                             (selectCondition.PoID==0||(selectCondition.PoID==-1&&position.PoFlag!=0)||selectCondition.PoID==position.PoID)

                             select new T_LoginHistoryDsp
                             {
                                 LoHistoryID = loghistory.LoHistoryID,
                                 EmID = employee.EmID,
                                 EmName = employee.EmName,
                                 SoID = employee.SoID,
                                 SoName = salesoffice.SoName,
                                 PoID = employee.PoID,
                                 PoName = position.PoName,
                                 LoginDate = loghistory.LoginDate,
                                 LogoutDate = loghistory.LogoutDate
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<T_LoginHistoryDsp> GetLoginHistoryData(T_LoginHistoryDsp selectCondition, DateTime? starDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from loghistory in context.T_LoginHistorys
                             join employee in context.M_Employees on loghistory.EmID equals employee.EmID
                             join salesoffice in context.M_SalesOffices on employee.SoID equals salesoffice.SoID
                             join position in context.M_Positions on employee.PoID equals position.PoID

                             where (selectCondition.LoHistoryID == 0 || selectCondition.LoHistoryID == loghistory.LoHistoryID) &&
                             (selectCondition.EmID == 0 || (selectCondition.EmID == -1 && employee.EmFlag != 0 || selectCondition.EmID == employee.EmID)) &&
                             (selectCondition.SoID == 0 || (selectCondition.SoID == -1 && salesoffice.SoFlag != 0) || selectCondition.SoID == salesoffice.SoID) &&
                             (selectCondition.PoID == 0 || (selectCondition.PoID == -1 && position.PoFlag != 0) || selectCondition.PoID == position.PoID) &&
                             (starDay == null && employee.EmHiredate <= endDay ||
                             endDay == null && employee.EmHiredate >= starDay ||
                             (starDay <= employee.EmHiredate && employee.EmHiredate <= endDay))

                             select new T_LoginHistoryDsp
                             {
                                 LoHistoryID = loghistory.LoHistoryID,
                                 EmID = employee.EmID,
                                 EmName = employee.EmName,
                                 SoID = employee.SoID,
                                 SoName = salesoffice.SoName,
                                 PoID = employee.PoID,
                                 PoName = position.PoName,
                                 LoginDate = loghistory.LoginDate,
                                 LogoutDate = loghistory.LogoutDate
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
