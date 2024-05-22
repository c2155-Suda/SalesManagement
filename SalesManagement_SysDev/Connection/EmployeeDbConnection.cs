using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace SalesManagement_SysDev
{
    class EmployeeDbConnection
    {
        public List<M_EmployeeDsp> GetEmployeeData()
        {
            //社員情報をDBから取得する
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from employee in context.M_Employees
                             join salesoffice in context.M_SalesOffices on employee.SoID equals salesoffice.SoID
                             join position in context.M_Positions on employee.PoID equals position.PoID

                             where employee.EmFlag == 0

                             select new M_EmployeeDsp
                             {
                                 EmID = employee.EmID,
                                 EmName = employee.EmName,
                                 SoID=employee.SoID,
                                 SoName = salesoffice.SoName,
                                 PoID=employee.PoID,
                                 PoName = position.PoName,
                                 EmHiredate = employee.EmHiredate,
                                 EmPassword = employee.EmPassword,
                                 EmPhone = employee.EmPhone,
                                 EmFlag = employee.EmFlag,
                                 EmBoolFlag = employee.EmFlag == 0? false:true,
                                 EmHidden = employee.EmHidden
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<M_EmployeeDsp> GetEmployeeData(M_EmployeeDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from employee in context.M_Employees
                             join salesoffice in context.M_SalesOffices on employee.SoID equals salesoffice.SoID
                             join position in context.M_Positions on employee.PoID equals position.PoID

                             where (selectCondition.EmID == 0 || employee.EmID == selectCondition.EmID) &&
                             employee.EmName.Contains(selectCondition.EmName) &&
                             (selectCondition.SoID == 0 ||(selectCondition.SoID==-1&&salesoffice.SoFlag!=0)|| salesoffice.SoID == selectCondition.SoID) &&
                             (selectCondition.PoID == 0 ||(selectCondition.PoID==-1&&position.PoFlag!=0)|| position.PoID == selectCondition.PoID) &&
                             employee.EmPhone.Contains(selectCondition.EmPhone) &&
                             (selectCondition.EmFlag==-1||employee.EmFlag == selectCondition.EmFlag) &&
                             (string.IsNullOrEmpty(selectCondition.EmHidden) || employee.EmHidden.Contains(selectCondition.EmHidden))

                             select new M_EmployeeDsp
                             {
                                 EmID = employee.EmID,
                                 EmName = employee.EmName,
                                 SoID=employee.SoID,
                                 SoName = salesoffice.SoName,
                                 PoID=employee.PoID,
                                 PoName = position.PoName,
                                 EmHiredate = employee.EmHiredate,
                                 EmPassword = employee.EmPassword,
                                 EmPhone = employee.EmPhone,
                                 EmFlag = employee.EmFlag,
                                 EmBoolFlag = employee.EmFlag == 0 ? false : true,
                                 EmHidden = employee.EmHidden
                             }; 
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<M_EmployeeDsp> GetEmployeeData(M_EmployeeDsp selectCondition, DateTime? starDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from employee in context.M_Employees
                             join salesoffice in context.M_SalesOffices on employee.SoID equals salesoffice.SoID
                             join position in context.M_Positions on employee.PoID equals position.PoID

                             where (selectCondition.EmID == 0 || employee.EmID == selectCondition.EmID) &&
                             employee.EmName.Contains(selectCondition.EmName) &&
                             (selectCondition.SoID == 0 || (selectCondition.SoID == -1 && salesoffice.SoFlag != 0) || salesoffice.SoID == selectCondition.SoID) &&
                             (selectCondition.PoID == 0 || (selectCondition.PoID == -1 && position.PoFlag != 0) || position.PoID == selectCondition.PoID) &&
                             employee.EmPhone.Contains(selectCondition.EmPhone) &&
                             (selectCondition.EmFlag == -1 || employee.EmFlag == selectCondition.EmFlag) &&
                             (string.IsNullOrEmpty(selectCondition.EmHidden) || employee.EmHidden.Contains(selectCondition.EmHidden)) &&

                             //starDay == null ? employee.EmHiredate <= endDay : endDay == null ? employee.EmHiredate >= starDay : (employee.EmHiredate >= starDay && employee.EmHiredate <= endDay)

                             (starDay == null && employee.EmHiredate <= endDay || 
                             endDay == null && employee.EmHiredate >= starDay || 
                             (starDay <= employee.EmHiredate && employee.EmHiredate <= endDay))


                             select new M_EmployeeDsp
                             {
                                 EmID = employee.EmID,
                                 EmName = employee.EmName,
                                 SoID=employee.SoID,
                                 SoName = salesoffice.SoName,
                                 PoID=employee.PoID,
                                 PoName = position.PoName,
                                 EmHiredate = employee.EmHiredate,
                                 EmPassword = employee.EmPassword,
                                 EmPhone = employee.EmPhone,
                                 EmFlag = employee.EmFlag,
                                 EmBoolFlag = employee.EmFlag == 0 ? false : true,
                                 EmHidden = employee.EmHidden
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public bool RegistEmployeeData(M_Employee regEmployee)
        {
            //社員情報をDBに登録する
            try
            {
                var context = new SalesManagement_DevContext();
                context.M_Employees.Add(regEmployee);
                context.SaveChanges();
                
                context.Dispose();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool UpdateEmployeeData(M_Employee updEmployee)
        {
            //既存の社員情報を更新する
            try
            {
                var context = new SalesManagement_DevContext();
                var employee = context.M_Employees.Single(x => x.EmID == updEmployee.EmID);

                employee.EmName = updEmployee.EmName;
                employee.SoID = updEmployee.SoID;
                employee.PoID = updEmployee.PoID;
                employee.EmHiredate = updEmployee.EmHiredate;
                employee.EmPassword = updEmployee.EmPassword;
                employee.EmPhone = updEmployee.EmPhone;
                employee.EmFlag = updEmployee.EmFlag;
                employee.EmHidden = updEmployee.EmHidden;

                context.SaveChanges();
                context.Dispose();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool CheckEmployeeExsistence(int emID)
        {
            //EmIDの存在を判定する
            bool flg = false;
            try
            {
                var context = new SalesManagement_DevContext();
                //社員IDで一致するデータが存在するか
                flg = context.M_Employees.Any(x => x.EmID == emID);
                context.Dispose();
            }
            catch
            {
                throw;
            }
            return flg;
        }

        public bool CheckEmployeeIsActive(int emID)
        {
            //社員が論理削除されていないかを判定する
            bool flg = true;
            try
            {
                var context = new SalesManagement_DevContext();
                var employee = context.M_Employees.Single(x => x.EmID == emID);
                                
                if(employee.EmFlag == 2)//論理削除されていたらfalse
                {
                    flg = false;
                }
                context.Dispose();

            }
            catch
            {
                throw;
            }
            return flg;
        }
    }
}
