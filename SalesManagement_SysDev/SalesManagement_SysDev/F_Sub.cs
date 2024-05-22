using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.Entity.Core.Metadata.Edm;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace SalesManagement_SysDev
{
    public partial class F_Sub : Form
    {
        public F_Sub()
        {
            InitializeComponent();
        }

        private void btn_CleateDabase_Click(object sender, EventArgs e)
        {
            //データベースの生成を行います．
            //再度実行する場合には，必ずデータベースの削除をしてから実行してください．

            SalesManagement_DevContext context = new SalesManagement_DevContext();

            List<M_Position> po = new List<M_Position>();

            {
                po.Add(new M_Position()
                {
                    PoName = "管理者",
                    PoFlag = 0,
                });
                po.Add(new M_Position()
                {
                    PoName = "営業",
                    PoFlag = 0,
                });
                po.Add(new M_Position()
                {
                    PoName = "物流",
                    PoFlag = 0,
                });
                context.M_Positions.AddRange(po);
                context.SaveChanges();
            }

            context.Dispose();

            MessageBox.Show("テーブル作成完了");
        }

        private void btn_InsertSampleData_Click(object sender, EventArgs e)
        {
            SalesManagement_DevContext context = new SalesManagement_DevContext();

            List<M_Position> po = context.M_Positions.OrderBy(x => x.PoID).ToList();
            List<M_Maker> ma = new List<M_Maker>();
            List<M_SalesOffice> so = new List<M_SalesOffice>();
            List<M_Client> cl = new List<M_Client>();
            Dictionary<int, M_Employee> em = new Dictionary<int, M_Employee>();
            List<M_MajorClassification> mc = new List<M_MajorClassification>();
            List<M_SmallClassification> sc = new List<M_SmallClassification>();
            List<M_Product> pr = new List<M_Product>();

            {
                ma.Add(new M_Maker()
                {
                    MaName = "Aメーカ",
                    MaAddress = "大阪",
                    MaPhone = "000-0000-0000",
                    MaPostal = "0000000",
                    MaFAX = "000-0000-0000",
                    MaFlag = 0,
                });
                ma.Add(new M_Maker()
                {
                    MaName = "Bメーカ",
                    MaAddress = "京都",
                    MaPhone = "000-0000-0000",
                    MaPostal = "0000000",
                    MaFAX = "000-0000-0000",
                    MaFlag = 0,
                });
                ma.Add(new M_Maker()
                {
                    MaName = "Cメーカ",
                    MaAddress = "和歌山",
                    MaPhone = "000-0000-0000",
                    MaPostal = "0000000",
                    MaFAX = "000-0000-0000",
                    MaFlag = 0,
                });
                ma.Add(new M_Maker()
                {
                    MaName = "Dメーカ",
                    MaAddress = "滋賀",
                    MaPhone = "000-0000-0000",
                    MaPostal = "0000000",
                    MaFAX = "000-0000-0000",
                    MaFlag = 0,
                });
                context.M_Makers.AddRange(ma);
                context.SaveChanges();
            }
            {
                so.Add(new M_SalesOffice()
                {
                    SoName = "北大阪営業所",
                    SoAddress = "大阪府吹田市寿町3-4-40",
                    SoPhone = "06-7011-6123",
                    SoPostal = "5600046",
                    SoFAX = "06-6562-2740",
                    SoFlag = 0,
                });
                so.Add(new M_SalesOffice()
                {
                    SoName = "兵庫営業所",
                    SoAddress = "兵庫県姫路市東辻井2-5-20",
                    SoPhone = "079-669-4326",
                    SoPostal = "6700994",
                    SoFAX = "079-669-4327",
                    SoFlag = 0,
                });
                so.Add(new M_SalesOffice()
                {
                    SoName = "鹿営業所",
                    SoAddress = "奈良県生駒郡三郷町勢野8-7-50",
                    SoPhone = "0745-99-0084",
                    SoPostal = "6360814",
                    SoFAX = "0746-0-1160",
                    SoFlag = 0,
                });
                so.Add(new M_SalesOffice()
                {
                    SoName = "京都営業所",
                    SoAddress = "京都府京都市山科区東野南井ノ上町10-3-7",
                    SoPhone = "077-672-6006",
                    SoPostal = "6078143",
                    SoFAX = "0771-85-2574",
                    SoFlag = 0,
                });
                so.Add(new M_SalesOffice()
                {
                    SoName = "和歌山営業所",
                    SoAddress = "和歌山県和歌山市柳丁4-19",
                    SoPhone = "073-887-1927",
                    SoPostal = "6408336",
                    SoFAX = "0735-78-4874",
                    SoFlag = 0,
                });
                context.M_SalesOffices.AddRange(so);
                context.SaveChanges();
            }
            {
                cl.Add(new M_Client()
                {
                    ClName = "上村電機",
                    ClAddress = "京都府京都市伏見区塩屋町3-9-95",
                    ClPhone = "077-672-6006",
                    ClPostal = "6128046",
                    ClFAX = "077-581-0164",
                    ClFlag = 0,
                    M_SalesOffice = so[3],
                });
                cl.Add(new M_Client()
                {
                    ClName = "萬田金融",
                    ClAddress = "大阪府大阪市西区北堀江1丁目22-3",
                    ClPhone = "06-8757-6267",
                    ClPostal = "5500014",
                    ClFAX = "06-8757-6267",
                    ClFlag = 0,
                    M_SalesOffice = so[0],
                });
                cl.Add(new M_Client()
                {
                    ClName = "宝田電機",
                    ClAddress = "大阪府大阪市中央区和泉町2-5-46",
                    ClPhone = "06-1423-1895",
                    ClPostal = "5720806",
                    ClFAX = "06-1374-4358",
                    ClFlag = 0,
                    M_SalesOffice = so[0],
                });
                cl.Add(new M_Client()
                {
                    ClName = "INATUGI",
                    ClAddress = "大阪府茨木市横江2-5-60",
                    ClPhone = "072-02-5171",
                    ClPostal = "5670044",
                    ClFAX = "072-018-0116",
                    ClFlag = 0,
                    M_SalesOffice = so[0],
                });
                cl.Add(new M_Client()
                {
                    ClName = "水野電機",
                    ClAddress = "大阪府豊中市末広町2-6-13",
                    ClPhone = "06-2096-0974",
                    ClPostal = "5600024",
                    ClFAX = "06-2434-2434",
                    ClFlag = 0,
                    M_SalesOffice = so[1],
                });
                cl.Add(new M_Client()
                {
                    ClName = "ショップ赤川",
                    ClAddress = "大阪府大阪市天王寺区上本町",
                    ClPhone = "090-1111-1111",
                    ClPostal = "5430001",
                    ClFAX = "06-1111-1111",
                    ClFlag = 0,
                    M_SalesOffice = so[0],
                });
                cl.Add(new M_Client()
                {
                    ClName = "成田",
                    ClAddress = "奈良県御所市船路2-8-87",
                    ClPhone = "0746-0-1160",
                    ClPostal = "6392268",
                    ClFAX = "0746-0-1160",
                    ClFlag = 0,
                    M_SalesOffice = so[2],
                });
                context.M_Clients.AddRange(cl);
                context.SaveChanges();
            }
            {
                if (context.M_Employees.Where(x => x.EmID == 116).Count() == 0)
                {
                    em.Add(116, new M_Employee()
                    {
                        EmID = 116,
                        EmName = "坂口郁美",
                        EmHiredate = new DateTime(1980, 6, 17),
                        EmPassword = "0116",
                        EmPhone = "06-6813-5485",
                        M_SalesOffice = so[1],
                        M_Position = po[2],
                    });
                }
                if (context.M_Employees.Where(x => x.EmID == 310).Count() == 0)
                {
                    em.Add(310, new M_Employee()
                    {
                        EmID = 310,
                        EmName = "高谷春男",
                        EmHiredate = new DateTime(1973, 3, 21),
                        EmPassword = "0310",
                        EmPhone = "06-6356-8742",
                        M_SalesOffice = so[0],
                        M_Position = po[1],
                    });
                }
                if (context.M_Employees.Where(x => x.EmID == 1002).Count() == 0)
                {
                    em.Add(1002, new M_Employee()
                    {
                        EmID = 1002,
                        EmName = "日下部俊夫",
                        EmHiredate = new DateTime(1990, 9, 4),
                        EmPassword = "1002",
                        EmPhone = "06-6579-0622",
                        M_SalesOffice = so[0],
                        M_Position = po[1],
                    });
                }
                if (context.M_Employees.Where(x => x.EmID == 1007).Count() == 0)
                {
                    em.Add(1007, new M_Employee()
                    {
                        EmID = 1007,
                        EmName = "岸本芽生",
                        EmHiredate = new DateTime(1997, 2, 4),
                        EmPassword = "1007",
                        EmPhone = "075-425-3371",
                        M_SalesOffice = so[2],
                        M_Position = po[1],
                    });
                }
                if (context.M_Employees.Where(x => x.EmID == 1111).Count() == 0)
                {
                    em.Add(1111, new M_Employee()
                    {
                        EmID = 1111,
                        EmName = "奥村敦彦",
                        EmHiredate = new DateTime(1985, 3, 17),
                        EmPassword = "999",
                        EmPhone = "079-145-6121",
                        M_SalesOffice = so[3],
                        M_Position = po[2],
                    });
                }
                if (context.M_Employees.Where(x => x.EmID == 1208).Count() == 0)
                {
                    em.Add(1208, new M_Employee()
                    {
                        EmID = 1208,
                        EmName = "渋谷秋昴",
                        EmHiredate = new DateTime(1994, 1, 31),
                        EmPassword = "1208",
                        EmPhone = "0790-68-8043",
                        M_SalesOffice = so[4],
                        M_Position = po[1],
                    });
                }
                if (context.M_Employees.Where(x => x.EmID == 1227).Count() == 0)
                {
                    em.Add(1227, new M_Employee()
                    {
                        EmID = 1227,
                        EmName = "生田徳次郎",
                        EmHiredate = new DateTime(1964, 3, 20),
                        EmPassword = "1227",
                        EmPhone = "06-3021-1630",
                        M_SalesOffice = so[0],
                        M_Position = po[0],
                    });
                }
                context.M_Employees.AddRange(em.Values);
                context.SaveChanges();
                foreach (var emp in context.M_Employees)
                {
                    em[emp.EmID] = emp;
                }
            }
            {
                mc.Add(new M_MajorClassification()
                {
                    McName = "テレビ・レコーダー",
                    McFlag = 0,
                });
                mc.Add(new M_MajorClassification()
                {
                    McName = "エアコン・冷蔵庫・洗濯機",
                    McFlag = 0,
                });
                mc.Add(new M_MajorClassification()
                {
                    McName = "オーディオ・イヤホン・ヘッドホン",
                    McFlag = 0,
                });
                mc.Add(new M_MajorClassification()
                {
                    McName = "携帯電話・スマートフォン",
                    McFlag = 0,
                });
                context.M_MajorCassifications.AddRange(mc);
                context.SaveChanges();
            }
            {
                sc.Add(new M_SmallClassification()
                {
                    M_MajorClassification = mc[0],
                    ScName = "テレビ",
                    ScFlag = 0,
                });
                sc.Add(new M_SmallClassification()
                {
                    M_MajorClassification = mc[0],
                    ScName = "レコーダー",
                    ScFlag = 0,
                });
                sc.Add(new M_SmallClassification()
                {
                    M_MajorClassification = mc[1],
                    ScName = "エアコン",
                    ScFlag = 0,
                });
                sc.Add(new M_SmallClassification()
                {
                    M_MajorClassification = mc[1],
                    ScName = "冷蔵庫",
                    ScFlag = 0,
                });
                sc.Add(new M_SmallClassification()
                {
                    M_MajorClassification = mc[1],
                    ScName = "洗濯機",
                    ScFlag = 0,
                });
                sc.Add(new M_SmallClassification()
                {
                    M_MajorClassification = mc[2],
                    ScName = "オーディオ",
                    ScFlag = 0,
                });
                sc.Add(new M_SmallClassification()
                {
                    M_MajorClassification = mc[2],
                    ScName = "イヤホン",
                    ScFlag = 0,
                });
                sc.Add(new M_SmallClassification()
                {
                    M_MajorClassification = mc[2],
                    ScName = "ヘッドホン",
                    ScFlag = 0,
                });
                sc.Add(new M_SmallClassification()
                {
                    M_MajorClassification = mc[3],
                    ScName = "携帯電話",
                    ScFlag = 0,
                });
                sc.Add(new M_SmallClassification()
                {
                    M_MajorClassification = mc[3],
                    ScName = "スマートフォン",
                    ScFlag = 0,
                });
                context.M_SmallClassifications.AddRange(sc);
                context.SaveChanges();
            }
            {
                pr.Add(new M_Product()
                {
                    M_Maker = ma[0],
                    PrName = "テレビA",
                    Price = 100000,
                    PrSafetyStock = 100,
                    M_SmallClassification = sc[0],
                    PrModelNumber = "1",
                    PrColor = "黒",
                    PrReleaseDate = new DateTime(2019, 5, 1),
                    PrFlag = 0,
                });
                pr.Add(new M_Product()
                {
                    M_Maker = ma[0],
                    PrName = "テレビB",
                    Price = 98000,
                    PrSafetyStock = 100,
                    M_SmallClassification = sc[0],
                    PrModelNumber = "1",
                    PrColor = "黒",
                    PrReleaseDate = new DateTime(2019, 5, 10),
                    PrFlag = 0,
                });
                pr.Add(new M_Product()
                {
                    M_Maker = ma[0],
                    PrName = "レコーダーA",
                    Price = 5000,
                    PrSafetyStock = 50,
                    M_SmallClassification = sc[1],
                    PrModelNumber = "1",
                    PrColor = "黒",
                    PrReleaseDate = new DateTime(2019, 10, 1),
                    PrFlag = 0,
                });
                pr.Add(new M_Product()
                {
                    M_Maker = ma[1],
                    PrName = "エアコンA",
                    Price = 160000,
                    PrSafetyStock = 50,
                    M_SmallClassification = sc[2],
                    PrModelNumber = "1",
                    PrColor = "白",
                    PrReleaseDate = new DateTime(2020, 10, 1),
                    PrFlag = 0,
                });
                pr.Add(new M_Product()
                {
                    M_Maker = ma[1],
                    PrName = "冷蔵庫A",
                    Price = 200000,
                    PrSafetyStock = 50,
                    M_SmallClassification = sc[3],
                    PrModelNumber = "1",
                    PrColor = "白",
                    PrReleaseDate = new DateTime(2020, 1, 1),
                    PrFlag = 0,
                });
                pr.Add(new M_Product()
                {
                    M_Maker = ma[1],
                    PrName = "洗濯機A",
                    Price = 150000,
                    PrSafetyStock = 50,
                    M_SmallClassification = sc[4],
                    PrModelNumber = "1",
                    PrColor = "白",
                    PrReleaseDate = new DateTime(2019, 3, 1),
                    PrFlag = 0,
                });
                pr.Add(new M_Product()
                {
                    M_Maker = ma[2],
                    PrName = "オーディオA",
                    Price = 6000,
                    PrSafetyStock = 10,
                    M_SmallClassification = sc[5],
                    PrModelNumber = "1",
                    PrColor = "黒",
                    PrReleaseDate = new DateTime(2020, 8, 1),
                    PrFlag = 0,
                });
                pr.Add(new M_Product()
                {
                    M_Maker = ma[2],
                    PrName = "イヤホンA",
                    Price = 5000,
                    PrSafetyStock = 100,
                    M_SmallClassification = sc[6],
                    PrModelNumber = "1",
                    PrColor = "赤",
                    PrReleaseDate = new DateTime(2019, 5, 1),
                    PrFlag = 0,
                });
                pr.Add(new M_Product()
                {
                    M_Maker = ma[3],
                    PrName = "iphone8",
                    Price = 78800,
                    PrSafetyStock = 50,
                    M_SmallClassification = sc[8],
                    PrModelNumber = "1",
                    PrColor = "ゴールド",
                    PrReleaseDate = new DateTime(2017, 9, 22),
                    PrFlag = 0,
                });
                pr.Add(new M_Product()
                {
                    M_Maker = ma[3],
                    PrName = "スマートフォンA",
                    Price = 30000,
                    PrSafetyStock = 50,
                    M_SmallClassification = sc[9],
                    PrModelNumber = "1",
                    PrColor = "シルバー",
                    PrReleaseDate = new DateTime(2019, 5, 1),
                    PrFlag = 0,
                });
                pr.Add(new M_Product()
                {
                    M_Maker = ma[3],
                    PrName = "スマートフォンB",
                    Price = 40000,
                    PrSafetyStock = 50,
                    M_SmallClassification = sc[9],
                    PrModelNumber = "1",
                    PrColor = "黒",
                    PrReleaseDate = new DateTime(2020, 11, 1),
                    PrFlag = 0,
                });
                context.M_Products.AddRange(pr);
                context.SaveChanges();
            }
            List<T_Stock> st = new List<T_Stock>();
            {
                st.Add(new T_Stock()
                {
                    M_Product = pr[0],
                    StQuantity = 100,
                    StFlag = 0,
                });
                st.Add(new T_Stock()
                {
                    M_Product = pr[1],
                    StQuantity = 120,
                    StFlag = 0,
                });
                st.Add(new T_Stock()
                {
                    M_Product = pr[2],
                    StQuantity = 199,
                    StFlag = 0,
                });
                st.Add(new T_Stock()
                {
                    M_Product = pr[3],
                    StQuantity = 50,
                    StFlag = 0,
                });
                st.Add(new T_Stock()
                {
                    M_Product = pr[4],
                    StQuantity = 60,
                    StFlag = 0,
                });
                st.Add(new T_Stock()
                {
                    M_Product = pr[5],
                    StQuantity = 32,
                    StFlag = 0,
                });
                st.Add(new T_Stock()
                {
                    M_Product = pr[9],
                    StQuantity = 240,
                    StFlag = 0,
                });
                context.T_Stocks.AddRange(st);
                context.SaveChanges();
            }
            List<T_Order> or = new List<T_Order>();
            {
                or.Add(new T_Order
                {
                    M_SalesOffice = so[0],
                    M_Employee = em[310],
                    M_Client = cl[1],
                    ClCharge = "萬田銀次郎",
                    OrDate = new DateTime(2020, 12, 10),
                    OrStateFlag = 1,
                    OrFlag = 0,
                });
                or.Add(new T_Order
                {
                    M_SalesOffice = so[1],
                    M_Employee = em[116],
                    M_Client = cl[4],
                    ClCharge = "水野勝成",
                    OrDate = new DateTime(2021, 1, 5),
                    OrStateFlag = 0,
                    OrFlag = 0,
                });
                context.T_Orders.AddRange(or);
                context.SaveChanges();
            }
            List<T_OrderDetail> ord = new List<T_OrderDetail>();
            {
                ord.Add(new T_OrderDetail()
                {
                    T_Order = or[0],
                    M_Product = pr[2],
                    OrQuantity = 40,
                    OrTotalPrice = 200000,
                });
                ord.Add(new T_OrderDetail()
                {
                    T_Order = or[0],
                    M_Product = pr[9],
                    OrQuantity = 30,
                    OrTotalPrice = 900000,
                });
                ord.Add(new T_OrderDetail()
                {
                    T_Order = or[1],
                    M_Product = pr[3],
                    OrQuantity = 20,
                    OrTotalPrice = 3200000,
                });
                ord.Add(new T_OrderDetail()
                {
                    T_Order = or[1],
                    M_Product = pr[4],
                    OrQuantity = 15,
                    OrTotalPrice = 3000000,
                });
                ord.Add(new T_OrderDetail()
                {
                    T_Order = or[1],
                    M_Product = pr[5],
                    OrQuantity = 15,
                    OrTotalPrice = 2250000,
                });
                context.T_OrderDetails.AddRange(ord);
                context.SaveChanges();
            }
            List<T_Chumon> ch = new List<T_Chumon>();
            {
                ch.Add(new T_Chumon()
                {
                    M_SalesOffice = so[0],
                    M_Employee = em[1002],
                    M_Client = cl[1],
                    T_Order = or[0],
                    ChDate = new DateTime(2020, 12, 11),
                    ChStateFlag = 1,
                    ChFlag = 0,
                });
                context.T_Chumons.AddRange(ch);
                context.SaveChanges();
            }
            List<T_ChumonDetail> chd = new List<T_ChumonDetail>();
            {
                chd.Add(new T_ChumonDetail()
                {
                    T_Chumon = ch[0],
                    M_Product = pr[2],
                    ChQuantity = 40,
                });
                chd.Add(new T_ChumonDetail()
                {
                    T_Chumon = ch[0],
                    M_Product = pr[9],
                    ChQuantity = 30,
                });
                context.T_ChumonDetails.AddRange(chd);
                context.SaveChanges();
            }
            List<T_Syukko> sy = new List<T_Syukko>();
            {
                sy.Add(new T_Syukko()
                {
                    M_Client = cl[1],
                    M_SalesOffice = so[0],
                    T_Order = or[0],
                    SyStateFlag = 0,
                    SyFlag = 0,
                });
                context.T_Syukkos.AddRange(sy);
                context.SaveChanges();
            }
            List<T_SyukkoDetail> syd = new List<T_SyukkoDetail>();
            {
                syd.Add(new T_SyukkoDetail()
                {
                    T_Syukko = sy[0],
                    M_Product = pr[2],
                    SyQuantity = 40,
                });
                syd.Add(new T_SyukkoDetail()
                {
                    T_Syukko = sy[0],
                    M_Product = pr[9],
                    SyQuantity = 30,
                });
                context.T_SyukkoDetails.AddRange(syd);
                context.SaveChanges();
            }


            context.Dispose();

            MessageBox.Show("サンプルデータ登録完了");
        }
    }
}
