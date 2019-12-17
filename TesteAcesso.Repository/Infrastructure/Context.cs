using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TesteAcesso.Model.Entity;

namespace TesteAcesso.Repository.Infrastructure
{
    public class Context : DbContext
    {

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }

        public DbSet<Transaction> Transaction { get; set; }

        internal int Count()
        {
            throw new NotImplementedException();
        }

        public DbSet<Status> Status { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnType("varchar(38)");
                entity.Property(e => e.StatusId).IsRequired();
                entity.Property(e => e.AccountOrigin).IsRequired();
                entity.Property(e => e.AccountDestination).IsRequired();
                entity.Property(e => e.CreateDate).IsRequired();
                entity.Property(e => e.Value).IsRequired().HasColumnType("decimal(10,6)");

                entity.HasOne(d => d.Status).WithMany(p => p.Transfers);
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(50);
            });


        }

    }
}
