using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// EntityFrameworkを利用する
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration.Configuration;

namespace SalesManagement_SysDev
{
    class SalesManagement_DevContext : DbContext
    {
        public DbSet<M_Client> M_Clients { get; set; }
        public DbSet<M_Employee> M_Employees { get; set; }
        public DbSet<M_MajorClassification> M_MajorCassifications { get; set; }
        public DbSet<M_Maker> M_Makers { get; set; }
        public DbSet<M_Position> M_Positions { get; set; }
        public DbSet<M_Product> M_Products { get; set; }
        public DbSet<M_SalesOffice> M_SalesOffices { get; set; }
        public DbSet<M_SmallClassification> M_SmallClassifications { get; set; }
        public DbSet<T_Arrival> T_Arrivals { get; set; }
        public DbSet<T_ArrivalDetail> T_ArrivalDetails { get; set; }
        public DbSet<T_Chumon> T_Chumons { get; set; }
        public DbSet<T_ChumonDetail> T_ChumonDetails { get; set; }
        public DbSet<T_Hattyu> T_Hattyus { get; set; }
        public DbSet<T_HattyuDetail> T_HattyuDetails { get; set; }
        public DbSet<T_LoginHistory> T_LoginHistorys { get; set; }
        public DbSet<T_OperationHistory> T_OperationHistorys { get; set; }
        public DbSet<T_Order> T_Orders { get; set; }
        public DbSet<T_OrderDetail> T_OrderDetails { get; set; }
        public DbSet<T_Sale> T_Sale { get; set; }
        public DbSet<T_SaleDetail> T_SaleDetails { get; set; }
        public DbSet<T_Shipment> T_Shipments { get; set; }
        public DbSet<T_ShipmentDetail> T_ShipmentDetails { get; set; }
        public DbSet<T_Stock> T_Stocks { get; set; }
        public DbSet<T_Syukko> T_Syukkos { get; set; }
        public DbSet<T_SyukkoDetail> T_SyukkoDetails { get; set; }
        public DbSet<T_Warehousing> T_Warehousings { get; set; }
        public DbSet<T_WarehousingDetail> T_WarehousingDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            {
                modelBuilder.Entity<M_Client>()
                    .HasRequired<M_SalesOffice>(x => x.M_SalesOffice)
                    .WithMany(x => x.M_Client)
                    .HasForeignKey(x => x.SoID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<M_Employee>()
                    .HasRequired<M_Position>(x => x.M_Position)
                    .WithMany(x => x.M_Employee)
                    .HasForeignKey(x => x.PoID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<M_Employee>()
                    .HasRequired<M_SalesOffice>(x => x.M_SalesOffice)
                    .WithMany(x => x.M_Employee)
                    .HasForeignKey(x => x.SoID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<M_Product>()
                    .HasRequired<M_Maker>(x => x.M_Maker)
                    .WithMany(x => x.M_Product)
                    .HasForeignKey(x => x.MaID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<M_Product>()
                    .HasRequired<M_SmallClassification>(x => x.M_SmallClassification)
                    .WithMany(x => x.M_Product)
                    .HasForeignKey(x => x.ScID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<M_SmallClassification>()
                    .HasRequired<M_MajorClassification>(x => x.M_MajorClassification)
                    .WithMany(x => x.M_SmallClassification)
                    .HasForeignKey(x => x.McID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_Arrival>()
                    .HasRequired<M_SalesOffice>(x => x.M_SalesOffice)
                    .WithMany(x => x.T_Arrival)
                    .HasForeignKey(x => x.SoID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Arrival>()
                    .HasOptional<M_Employee>(x => x.M_Employee)
                    .WithMany(x => x.T_Arrival)
                    .HasForeignKey(x => x.EmID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Arrival>()
                    .HasRequired<M_Client>(x => x.M_Client)
                    .WithMany(x => x.T_Arrival)
                    .HasForeignKey(x => x.ClID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Arrival>()
                    .HasRequired<T_Order>(x => x.T_Order)
                    .WithMany(x => x.T_Arrival)
                    .HasForeignKey(x => x.OrID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_ArrivalDetail>()
                    .HasRequired<M_Product>(x => x.M_Product)
                    .WithMany(x => x.T_ArrivalDetail)
                    .HasForeignKey(x => x.PrID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_ArrivalDetail>()
                    .HasRequired<T_Arrival>(x => x.T_Arrival)
                    .WithMany(x => x.T_ArrivalDetail)
                    .HasForeignKey(x => x.ArID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_Chumon>()
                    .HasRequired<M_SalesOffice>(x => x.M_SalesOffice)
                    .WithMany(x => x.T_Chumon)
                    .HasForeignKey(x => x.SoID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Chumon>()
                    .HasOptional<M_Employee>(x => x.M_Employee)
                    .WithMany(x => x.T_Chumon)
                    .HasForeignKey(x => x.EmID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Chumon>()
                    .HasRequired<M_Client>(x => x.M_Client)
                    .WithMany(x => x.T_Chumon)
                    .HasForeignKey(x => x.ClID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Chumon>()
                    .HasRequired<T_Order>(x => x.T_Order)
                    .WithMany(x => x.T_Chumon)
                    .HasForeignKey(x => x.OrID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_ChumonDetail>()
                    .HasRequired<T_Chumon>(x => x.T_Chumon)
                    .WithMany(x => x.T_ChumonDetail)
                    .HasForeignKey(x => x.ChID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_ChumonDetail>()
                    .HasRequired<M_Product>(x => x.M_Product)
                    .WithMany(x => x.T_ChumonDetail)
                    .HasForeignKey(x => x.PrID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_Hattyu>()
                    .HasRequired<M_Maker>(x => x.M_Maker)
                    .WithMany(x => x.T_Hattyu)
                    .HasForeignKey(x => x.MaID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Hattyu>()
                    .HasRequired<M_Employee>(x => x.M_Employee)
                    .WithMany(x => x.T_Hattyu)
                    .HasForeignKey(x => x.EmID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_HattyuDetail>()
                    .HasRequired<T_Hattyu>(x => x.T_Hattyu)
                    .WithMany(x => x.T_HattyuDetail)
                    .HasForeignKey(x => x.HaID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_HattyuDetail>()
                    .HasRequired<M_Product>(x => x.M_Product)
                    .WithMany(x => x.T_HattyuDetail)
                    .HasForeignKey(x => x.PrID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_LoginHistory>()
                    .HasRequired<M_Employee>(x => x.M_Employee)
                    .WithMany(x => x.T_LoginHistory)
                    .HasForeignKey(x => x.EmID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_OperationHistory>()
                    .HasRequired<M_Employee>(x => x.M_Employee)
                    .WithMany(x => x.T_OperationHistory)
                    .HasForeignKey(x => x.EmID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_Order>()
                    .HasRequired<M_SalesOffice>(x => x.M_SalesOffice)
                    .WithMany(x => x.T_Order)
                    .HasForeignKey(x => x.SoID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Order>()
                    .HasRequired<M_Employee>(x => x.M_Employee)
                    .WithMany(x => x.T_Order)
                    .HasForeignKey(x => x.EmID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Order>()
                    .HasRequired<M_Client>(x => x.M_Client)
                    .WithMany(x => x.T_Order)
                    .HasForeignKey(x => x.ClID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_OrderDetail>()
                    .HasRequired<T_Order>(x => x.T_Order)
                    .WithMany(x => x.T_OrderDetail)
                    .HasForeignKey(x => x.OrID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_OrderDetail>()
                    .HasRequired<M_Product>(x => x.M_Product)
                    .WithMany(x => x.T_OrderDetail)
                    .HasForeignKey(x => x.PrID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_Shipment>()
                    .HasRequired<M_Client>(x => x.M_Client)
                    .WithMany(x => x.T_Shipment)
                    .HasForeignKey(x => x.ClID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Shipment>()
                    .HasRequired<M_Employee>(x => x.M_Employee)
                    .WithMany(x => x.T_Shipment)
                    .HasForeignKey(x => x.EmID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Shipment>()
                    .HasRequired<M_SalesOffice>(x => x.M_SalesOffice)
                    .WithMany(x => x.T_Shipment)
                    .HasForeignKey(x => x.SoID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Shipment>()
                    .HasRequired<T_Order>(x => x.T_Order)
                    .WithMany(x => x.T_Shipment)
                    .HasForeignKey(x => x.OrID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_ShipmentDetail>()
                    .HasRequired<T_Shipment>(x => x.T_Shipment)
                    .WithMany(x => x.T_ShipmentDetail)
                    .HasForeignKey(x => x.ShID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_ShipmentDetail>()
                    .HasRequired<M_Product>(x => x.M_Product)
                    .WithMany(x => x.T_ShipmentDetail)
                    .HasForeignKey(x => x.PrID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_Stock>()
                    .HasRequired<M_Product>(x => x.M_Product)
                    .WithMany(x => x.T_Stock)
                    .HasForeignKey(x => x.PrID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_Syukko>()
                    .HasOptional<M_Employee>(x => x.M_Employee)
                    .WithMany(x => x.T_Syukko)
                    .HasForeignKey(x => x.EmID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Syukko>()
                    .HasRequired<M_Client>(x => x.M_Client)
                    .WithMany(x => x.T_Syukko)
                    .HasForeignKey(x => x.ClID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Syukko>()
                    .HasRequired<M_SalesOffice>(x => x.M_SalesOffice)
                    .WithMany(x => x.T_Syukko)
                    .HasForeignKey(x => x.SoID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Syukko>()
                    .HasRequired<T_Order>(x => x.T_Order)
                    .WithMany(x => x.T_Syukko)
                    .HasForeignKey(x => x.OrID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_SyukkoDetail>()
                    .HasRequired<T_Syukko>(x => x.T_Syukko)
                    .WithMany(x => x.T_SyukkoDetail)
                    .HasForeignKey(x => x.SyID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_SyukkoDetail>()
                    .HasRequired<M_Product>(x => x.M_Product)
                    .WithMany(x => x.T_SyukkoDetail)
                    .HasForeignKey(x => x.PrID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_Sale>()
                    .HasRequired<M_Client>(x => x.M_Client)
                    .WithMany(x => x.T_Sale)
                    .HasForeignKey(x => x.ClID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Sale>()
                    .HasRequired<M_SalesOffice>(x => x.M_SalesOffice)
                    .WithMany(x => x.T_Sale)
                    .HasForeignKey(x => x.SoID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Sale>()
                    .HasRequired<M_Employee>(x => x.M_Employee)
                    .WithMany(x => x.T_Sale)
                    .HasForeignKey(x => x.EmID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Sale>()
                    .HasRequired<T_Chumon>(x => x.T_Chumon)
                    .WithMany(x => x.T_Sale)
                    .HasForeignKey(x => x.ChID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_SaleDetail>()
                    .HasRequired<T_Sale>(x => x.T_Sale)
                    .WithMany(x => x.T_SaleDetail)
                    .HasForeignKey(x => x.SaID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_SaleDetail>()
                    .HasRequired<M_Product>(x => x.M_Product)
                    .WithMany(x => x.T_SaleDetail)
                    .HasForeignKey(x => x.PrID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_Warehousing>()
                    .HasRequired<T_Hattyu>(x => x.T_Hattyu)
                    .WithMany(x => x.T_Warehousing)
                    .HasForeignKey(x => x.HaID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_Warehousing>()
                    .HasRequired<M_Employee>(x => x.M_Employee)
                    .WithMany(x => x.T_Warehousing)
                    .HasForeignKey(x => x.EmID)
                    .WillCascadeOnDelete(false);
            }
            {
                modelBuilder.Entity<T_WarehousingDetail>()
                    .HasRequired<T_Warehousing>(x => x.T_Warehousing)
                    .WithMany(x => x.T_WarehousingDetail)
                    .HasForeignKey(x => x.WaID)
                    .WillCascadeOnDelete(false);
                modelBuilder.Entity<T_WarehousingDetail>()
                    .HasRequired<M_Product>(x => x.M_Product)
                    .WithMany(x => x.T_WarehousingDetail)
                    .HasForeignKey(x => x.PrID)
                    .WillCascadeOnDelete(false);
            }
        }
    }
}
