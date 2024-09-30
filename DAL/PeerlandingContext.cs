using System;
using System.Collections.Generic;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public partial class PeerlandingContext : DbContext
{
    public PeerlandingContext()
    {
    }

    public PeerlandingContext(DbContextOptions<PeerlandingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<MstUser> MstUsers { get; set; }
    public virtual DbSet<MstLoans> MstLoans { get; set; }
    public virtual DbSet<TrnFunding> TrnFundings { get; set; }
    public virtual DbSet<TrnRepayment> TrnRepayments { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MstUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("mst_user_pk");
            entity.ToTable("mst_user");

            entity.Property(e => e.Balance)
                .HasPrecision(18, 2)
                .HasColumnName("balance");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(30)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

	internal async Task<decimal> GetBalanceAsync(int lenderId)
	{
		throw new NotImplementedException();
	}

	internal async Task<IEnumerable<object>> GetLoansAsync(string status)
	{
		throw new NotImplementedException();
	}

	internal async Task UpdateBalanceAsync(int lenderId, object amount)
	{
		throw new NotImplementedException();
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
