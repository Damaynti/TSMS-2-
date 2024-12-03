using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TSMS_2_.EF
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model111")
        {
        }

        public virtual DbSet<categories> categories { get; set; }
        public virtual DbSet<client> client { get; set; }
        public virtual DbSet<discount> discount { get; set; }
        public virtual DbSet<element_sale> element_sale { get; set; }
        public virtual DbSet<element_supply> element_supply { get; set; }
        public virtual DbSet<loanAgreement> loanAgreement { get; set; }
        public virtual DbSet<products> products { get; set; }
        public virtual DbSet<sale> sale { get; set; }
        public virtual DbSet<salesman> salesman { get; set; }
        public virtual DbSet<status> status { get; set; }
        public virtual DbSet<supplier> supplier { get; set; }
        public virtual DbSet<supply> supply { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<categories>()
                .HasMany(e => e.products)
                .WithRequired(e => e.categories)
                .HasForeignKey(e => e.categoris_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<client>()
                .HasMany(e => e.sale)
                .WithOptional(e => e.client)
                .HasForeignKey(e => e.client_id);

            modelBuilder.Entity<discount>()
                .HasMany(e => e.client)
                .WithRequired(e => e.discount)
                .HasForeignKey(e => e.discount_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<products>()
                .HasMany(e => e.element_sale)
                .WithRequired(e => e.products)
                .HasForeignKey(e => e.products_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<products>()
                .HasMany(e => e.element_supply)
                .WithRequired(e => e.products)
                .HasForeignKey(e => e.products_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<sale>()
                .HasMany(e => e.element_sale)
                .WithRequired(e => e.sale)
                .HasForeignKey(e => e.sale_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<salesman>()
                .HasMany(e => e.sale)
                .WithRequired(e => e.salesman)
                .HasForeignKey(e => e.salesmn_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<status>()
                .HasMany(e => e.loanAgreement)
                .WithRequired(e => e.status)
                .HasForeignKey(e => e.status_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<supplier>()
                .HasMany(e => e.loanAgreement)
                .WithRequired(e => e.supplier)
                .HasForeignKey(e => e.supplier_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<supplier>()
                .HasMany(e => e.supply)
                .WithRequired(e => e.supplier)
                .HasForeignKey(e => e.supplier_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<supply>()
                .HasMany(e => e.element_supply)
                .WithRequired(e => e.supply)
                .HasForeignKey(e => e.supply_id)
                .WillCascadeOnDelete(false);
        }
    }
}
