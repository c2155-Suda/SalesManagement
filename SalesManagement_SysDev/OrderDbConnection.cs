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
        public List<T_OrderDsp> GetOrderData()
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {

                    var tb = from order in context.T_Orders
                             join salesoffice in context.M_SalesOffices on order.OrID equals salesoffice.SoID
                             join employes in context.M_Employees on order.OrID equals employes.EmID
                             join client in context.M_Clients on order.OrID equals client.ClID

                             where order.OrFlag == 0

                             select new T_OrderDsp
                             {
                                 OrID = order.OrID,
                                 SoID = salesoffice.SoID,
                                 EmID = employes.EmID,
                                 ClID = client.ClID,
                                 ClCharge = order.ClCharge,
                                 OrData = order.OrDate,
                                 OrStateFlag = order.OrStateFlag,
                                 OrFlag = order.OrFlag,
                                 OrHidden = order.OrHidden
                             };
                    return tb.ToList();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        public List<T_OrderDsp> GetOrderData(T_OrderDsp selectCondition)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {

                    var tb = from order in context.T_Orders
                             join salesoffice in context.M_SalesOffices on order.OrID equals salesoffice.SoID
                             join employes in context.M_Employees on order.OrID equals employes.EmID
                             join client in context.M_Clients on order.OrID equals client.ClID

                             where selectCondition.OrID != 0 ? order.OrID == selectCondition.OrID : true &&
                             selectCondition.SoID != 0 ? salesoffice.SoID == selectCondition.SoID :
                             selectCondition.EmID != 0 ? employes.EmID == selectCondition.EmID : true &&
                             selectCondition.ClID != 0 ? client.ClID == selectCondition.ClID : true &&
                             order.ClCharge.Contains(selectCondition.ClCharge) &&
                             selectCondition.OrStateFlag != 0 ? order.OrStateFlag == selectCondition.OrStateFlag : true &&
                             selectCondition.OrFlag != 0 ? order.OrFlag == selectCondition.OrFlag : true &&
                             !string.IsNullOrEmpty(selectCondition.OrHidden) ? order.OrHidden.Contains(selectCondition.OrHidden) : true

                             select new T_OrderDsp
                             {
                                 OrID = order.OrID,
                                 SoID = salesoffice.SoID,
                                 EmID = employes.EmID,
                                 ClID = client.ClID,
                                 ClCharge = order.ClCharge,
                                 OrData = order.OrDate,
                                 OrStateFlag = order.OrStateFlag,
                                 OrFlag = order.OrFlag,
                                 OrHidden = order.OrHidden

                             };
                    return tb.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        public List<T_OrderDsp> GetOrderData(T_OrderDsp selectCondition, DateTime? starDay, DateTime? endDay)
        {
            try
            {
                using (var context = new SalesManagement_DevContext())
                {

                    var tb = from order in context.T_Orders
                             join salesoffice in context.M_SalesOffices on order.OrID equals salesoffice.SoID
                             join employes in context.M_Employees on order.OrID equals employes.EmID
                             join client in context.M_Clients on order.OrID equals client.ClID

                             where selectCondition.OrID != 0 ? order.OrID == selectCondition.OrID : true &&
                             selectCondition.SoID != 0 ? salesoffice.SoID == selectCondition.SoID :
                             selectCondition.EmID != 0 ? employes.EmID == selectCondition.EmID : true &&
                             selectCondition.ClID != 0 ? client.ClID == selectCondition.ClID : true &&
                             order.ClCharge.Contains(selectCondition.ClCharge) &&
                             selectCondition.OrStateFlag != 0 ? order.OrStateFlag == selectCondition.OrStateFlag : true &&
                             selectCondition.OrFlag != 0 ? order.OrFlag == selectCondition.OrFlag : true &&
                             !string.IsNullOrEmpty(selectCondition.OrHidden) ? order.OrHidden.Contains(selectCondition.OrHidden) : true &&

                             starDay == null ? order.OrDate <= endDay : endDay == null ? order.OrDate >= starDay : (order.OrDate >= starDay && order.OrDate <= endDay)

                             select new T_OrderDsp
                             {
                                 OrID = order.OrID,
                                 SoID = salesoffice.SoID,
                                 EmID = employes.EmID,
                                 ClID = client.ClID,
                                 ClCharge = order.ClCharge,
                                 OrData = order.OrDate,
                                 OrStateFlag = order.OrStateFlag,
                                 OrFlag = order.OrFlag,
                                 OrHidden = order.OrHidden

                             };
                    return tb.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        public bool RegistOrData(T_Order regOrder)
        {
            //DBに登録する
            try
            {
                var context = new SalesManagement_DevContext();
                context.T_Orders.Add(regOrder);
                context.SaveChanges();
                context.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool UpdateOrderData(T_Order updOrder)
        {
            //を更新する
            try
            {
                var context = new SalesManagement_DevContext();
                var order = context.T_Orders.Single(x => x.OrID == updOrder.EmID);

                order.OrID = updOrder.OrID;
                order.SoID = updOrder.SoID;
                order.EmID = updOrder.EmID;
                order.ClID = updOrder.ClID;
                order.ClCharge = updOrder.ClCharge;
                order.OrDate = updOrder.OrDate;
                order.OrStateFlag = updOrder.OrStateFlag;
                order.OrFlag = updOrder.OrFlag;


                context.SaveChanges();
                context.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool CheckOrderExsistence(int orID)
        {

            bool flg = false;
            try
            {
                var context = new SalesManagement_DevContext();
                //データが存在するか
                flg = context.T_Orders.Any(x => x.OrID == orID);
                context.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return flg;
        }

        public bool CheckOrderIsActive(int orID)
        {

            bool flg = true;
            try
            {
                var context = new SalesManagement_DevContext();
                var order = context.T_Orders.Single(x => x.OrID == orID);

                if (order.OrFlag == 2)
                {
                    flg = false;
                }
                context.Dispose();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return flg;
        }


    }
}
