using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    class ClientDbConnection
    {
        public List<M_ClientDsp> GetClientData()
        {
            
            try
            {
                using (var context = new SalesManagement_DevContext())
                {

                    var tb = from client in context.M_Clients
                             join salesoffice in context.M_SalesOffices on client.SoID equals salesoffice.SoID

                             where client.ClFlag == 0

                             select new M_ClientDsp
                             {
                                 ClID = client.ClID,
                                 ClName = client.ClName,
                                 SoID=client.SoID,
                                 SoName = salesoffice.SoName,
                                 ClAddress = client.ClAddress,
                                 ClPhone = client.ClPhone,
                                 ClPostal = client.ClPostal,
                                 ClFAX = client.ClFAX,
                                 ClFlag = client.ClFlag,
                                 ClBoolFlag = client.ClFlag == 0 ? false : true,
                                 ClHidden = client.ClHidden

                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<M_ClientDsp> GetClientData(M_ClientDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {

                    var tb = from client in context.M_Clients
                             join salesoffice in context.M_SalesOffices on client.SoID equals salesoffice.SoID
                    
                             where (selectCondition.ClID == 0 || client.ClID == selectCondition.ClID) &&
                             client.ClName.Contains(selectCondition.ClName) &&
                             (selectCondition.SoID == 0 ||(selectCondition.SoID==-1&&salesoffice.SoFlag!=0)|| salesoffice.SoID == selectCondition.SoID) &&
                             client.ClAddress.Contains(selectCondition.ClAddress) &&
                             client.ClPhone.Contains(selectCondition.ClPhone) &&
                             client.ClPostal.Contains(selectCondition.ClPostal) &&
                             client.ClFAX.Contains(selectCondition.ClFAX) &&
                             (selectCondition.ClFlag==-1||client.ClFlag == selectCondition.ClFlag) &&
                             (string.IsNullOrEmpty(selectCondition.ClHidden) || client.ClHidden.Contains(selectCondition.ClHidden))

                             select new M_ClientDsp
                             {
                                 ClID = client.ClID,
                                 ClName = client.ClName,
                                 SoID=client.SoID,
                                 SoName = salesoffice.SoName,
                                 ClAddress = client.ClAddress,
                                 ClPhone = client.ClPhone,
                                 ClPostal = client.ClPostal,
                                 ClFAX = client.ClFAX,
                                 ClFlag = client.ClFlag,
                                 ClBoolFlag = client.ClFlag == 0 ? false : true,
                                 ClHidden = client.ClHidden

                             };
                    return tb.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public void RegistClientData(M_Client regDate)
        {
            //顧客情報をDBに登録する
            
            using(var context = new SalesManagement_DevContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                    
                        context.M_Clients.Add(regDate);
                        context.SaveChanges();
                        if (regDate.ClID > NumericRange.ID)
                        {
                            transaction.Rollback();
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('M_Client',RESEED,0)");
                            context.Database.ExecuteSqlCommand("DBCC CHECKIDENT('M_Client')");
                            throw new Exception("顧客IDが既定の数字を超えています。");
                        }
                        transaction.Commit();
                   
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    } 
                }    
            }       
        }

        public bool UpdateClientData(M_Client updClient)
        {
            //既存の顧客情報を更新する
            try
            {
                var context = new SalesManagement_DevContext();
                var client = context.M_Clients.Single(x => x.ClID == updClient.ClID);

                client.ClName = updClient.ClName;
                client.SoID = updClient.SoID;
                client.ClAddress = updClient.ClAddress;
                client.ClPhone = updClient.ClPhone;
                client.ClPostal = updClient.ClPostal;
                client.ClFAX = updClient.ClFAX;
                client.ClFlag = updClient.ClFlag;
                client.ClHidden = updClient.ClHidden;

                context.SaveChanges();
                context.Dispose();

                return true;
            }
            catch
            {
                throw;
            }
        }

        public bool CheckClientExsistence(int clID)
        {
            //ClIDの存在を判定する
            bool flg = false;
            try
            {
                var context = new SalesManagement_DevContext();
                //顧客IDで一致するデータが存在するか
                flg = context.M_Clients.Any(x => x.ClID == clID);
                context.Dispose();
            }
            catch
            {
                throw;
            }
            return flg;
        }

        public bool CheckClientIsActive(int clID)
        {
            //顧客が論理削除されていないかを判定する
            bool flg = true;
            try
            {
                var context = new SalesManagement_DevContext();
                var client = context.M_Clients.Single(x => x.ClID == clID);

                if (client.ClFlag == 2)//論理削除されていたらfalse
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
