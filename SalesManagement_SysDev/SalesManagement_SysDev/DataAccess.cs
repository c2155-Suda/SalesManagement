using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesManagement_SysDev
{
    class DataAccess
    {
        public List<M_PositionDsp> GetPositionDspData()
        {
            List<M_PositionDsp> position = new List<M_PositionDsp>();

            try
            {
                var context = new SalesManagement_DevContext();
                // tbはIEnumerable型
                var tb = from t1 in context.M_Positions

                         select new
                         {
                             t1.PoID,
                             t1.PoName

                         };

                // IEnumerable型のデータをList型へ
                foreach (var p in tb)
                {
                    position.Add(new M_PositionDsp()
                    {
                        PoID = p.PoID,
                        PoName = p.PoName
                    });
                }
                context.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return position;
        }
        public List<M_SalesOfficeDsp> GetSalesOfficeDspData()
        {
            List<M_SalesOfficeDsp> salesoffice = new List<M_SalesOfficeDsp>();

            try
            {
                var context = new SalesManagement_DevContext();
                // tbはIEnumerable型
                var tb = from t1 in context.M_SalesOffices

                         select new
                         {
                             t1.SoID,
                             t1.SoName
                         };

                // IEnumerable型のデータをList型へ
                foreach (var p in tb)
                {
                    salesoffice.Add(new M_SalesOfficeDsp()
                    {
                        SoID = p.SoID,
                        SoName = p.SoName
                    });
                }
                context.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return salesoffice;
        }
        public List<M_MakerDsp> GetMakerDspData()
        {
            List<M_MakerDsp> maker = new List<M_MakerDsp>();

            try
            {
                var context = new SalesManagement_DevContext();
                // tbはIEnumerable型
                var tb = from t1 in context.M_Makers

                         select new
                         {
                             t1.MaID,
                             t1.MaName

                         };

                // IEnumerable型のデータをList型へ
                foreach (var p in tb)
                {
                    maker.Add(new M_MakerDsp()
                    {
                        MaID = p.MaID,
                        MaName = p.MaName
                    });
                }
                context.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return maker;
        }
        public List<M_MajorClassificationDsp> GetMajorClassificationDspData()
        {
            List<M_MajorClassificationDsp> major = new List<M_MajorClassificationDsp>();

            try
            {
                var context = new SalesManagement_DevContext();
                // tbはIEnumerable型
                var tb = from t1 in context.M_MajorCassifications

                         select new
                         {
                             t1.McID,
                             t1.McName

                         };

                // IEnumerable型のデータをList型へ
                foreach (var p in tb)
                {
                    major.Add(new M_MajorClassificationDsp()
                    {
                        McID = p.McID,
                        McName = p.McName
                    });
                }
                context.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return major;
        }
        public List<M_SmallClassificationDsp> GetSmallClassificationDspData()
        {
            List<M_SmallClassificationDsp> small = new List<M_SmallClassificationDsp>();

            try
            {
                var context = new SalesManagement_DevContext();
                // tbはIEnumerable型
                var tb = from t1 in context.M_SmallClassifications                         

                         select new
                         {
                             t1.ScID,
                             t1.ScName,
                             t1.McID
                         };

                // IEnumerable型のデータをList型へ
                foreach (var p in tb)
                {
                    small.Add(new M_SmallClassificationDsp()
                    {
                        ScID = p.ScID,
                        ScName = p.ScName,
                        McID = p.McID
                    });
                }
                context.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return small;
        }
        public List<M_SmallClassificationDsp> GetSmallClassificationDspData(int mcID)
        {
            List<M_SmallClassificationDsp> small = new List<M_SmallClassificationDsp>();
            try
            {
                var context = new SalesManagement_DevContext();
                // tbはIEnumerable型
                var tb = from t1 in context.M_SmallClassifications
                         join majorCategory in context.M_MajorCassifications on mcID equals majorCategory.McID
                         where t1.McID == mcID

                         select new 
                         {
                             t1.ScID,
                             t1.ScName,
                             t1.McID
                         };

                // IEnumerable型のデータをList型へ
                foreach (var p in tb)
                {
                    small.Add(new M_SmallClassificationDsp()
                    {
                        ScID = p.ScID,
                        ScName = p.ScName,
                        McID = p.McID
                    });
                }
                context.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "例外エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return small;
        }
    }
}
