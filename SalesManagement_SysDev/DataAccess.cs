using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    class DataAccess
    {
        public List<M_PositionDsp> GetPositionDspData()
        {
            try
            {
                using(var context=new SalesManagement_DevContext())
                {
                    var tb = from t1 in context.M_Positions
                             where t1.PoFlag == 0

                             select new M_PositionDsp
                             {
                                 PoID = t1.PoID,
                                 PoName = t1.PoName

                             };
                    return tb.ToList();
                }
            }
            catch 
            {
                throw;
            }
        }
        public List<M_SalesOfficeDsp> GetSalesOfficeDspData()
        {
            try
            {
                using(var context=new SalesManagement_DevContext())
                {
                    var tb = from t1 in context.M_SalesOffices
                             where t1.SoFlag == 0

                             select new M_SalesOfficeDsp
                             {
                                 SoID = t1.SoID,
                                 SoName = t1.SoName
                             };
                    return tb.ToList();
                }
            }
            catch 
            {
                throw;
            }
        }
        public List<M_MakerDsp> GetMakerDspData()
        {
            try
            {
                using(var context=new SalesManagement_DevContext())
                {
                    var tb = from t1 in context.M_Makers
                             where t1.MaFlag == 0

                             select new M_MakerDsp
                             {
                                 MaID=t1.MaID,
                                 MaName=t1.MaName

                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }

        }
        public bool CheckMakerIsActive(int Maid)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var target = context.M_Makers.Single(x => x.MaID == Maid);
                    if (target.MaFlag == 0)
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
        public List<M_MajorClassificationDsp> GetMajorClassificationDspData()
        {
            try
            {
                using(var context=new SalesManagement_DevContext())
                {
                    var tb = from t1 in context.M_MajorCassifications
                             where t1.McFlag == 0
                             select new M_MajorClassificationDsp
                             {
                                 McID = t1.McID,
                                 McName = t1.McName
                             };
                    return tb.ToList();
                }
            }
            catch 
            {
                throw;
            }
        }
        public List<M_SmallClassificationDsp> GetSmallClassificationDspData()
        {
            try
            {
                using(var context=new SalesManagement_DevContext())
                {
                    var tb = from t1 in context.M_SmallClassifications
                             where t1.ScFlag == 0

                             select new M_SmallClassificationDsp
                             {
                                 ScID = t1.ScID,
                                 ScName = t1.ScName,
                                 McID = t1.McID
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<M_SmallClassificationDsp> GetSmallClassificationDspData(int mcID)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {
                    var tb = from t1 in context.M_SmallClassifications
                             join majorCategory in context.M_MajorCassifications on t1.McID equals majorCategory.McID
                             where (mcID == 0 || (mcID == -1 && majorCategory.McFlag != 0) || mcID == t1.McID) &&
                             t1.ScFlag == 0

                             select new M_SmallClassificationDsp
                             {
                                 ScID = t1.ScID,
                                 ScName = t1.ScName,
                                 McID = t1.McID
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<M_ClientDsp> GetClientDspData(int soID)
        {
            try
            {
                using(var context=new SalesManagement_DevContext())
                {
                    var tb = from client in context.M_Clients
                             join salesoffice in context.M_SalesOffices on client.SoID equals salesoffice.SoID
                             where client.ClFlag == 0 &&
                             (soID == 0 || (soID == -1 && salesoffice.SoFlag != 0) || soID == client.SoID)

                             select new M_ClientDsp
                             {
                                 ClID = client.ClID,
                                 ClName = client.ClName,
                                 SoID = client.SoID
                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }
        public List<M_EmployeeDsp> GetEmployeeDspData(int soID)
        {
            try
            {
                using(var context=new SalesManagement_DevContext())
                {
                    var tb = from employee in context.M_Employees
                             join salesoffice in context.M_SalesOffices on employee.SoID equals salesoffice.SoID
                             where employee.EmFlag == 0 &&
                             (soID == 0 || (soID == -1 && salesoffice.SoFlag != 0) || soID == employee.SoID)

                             select new M_EmployeeDsp
                             {
                                 EmID = employee.EmID,
                                 EmName = employee.EmName,
                                 SoID = employee.SoID
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
