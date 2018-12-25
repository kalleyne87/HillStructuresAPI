using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HillStructuresAPI.Models
{
    public class HillStructuresContext : DbContext
    {
        public HillStructuresContext (DbContextOptions<HillStructuresContext> options)
            : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<Job> Job { get; set; }
        public DbSet<Year> Year { get; set; }
        public DbSet<Month> Month { get; set; }
        public DbSet<Week> Week { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<SubContractor> SubContractor { get; set; }
        public DbSet<EmployeeJob> EmployeeJobs {get; set;}
        public DbSet<SubContractorJob> SubContractorJobs {get; set;}
        public DbSet<SupplierJob> SupplierJobs {get; set;}
        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<TimeSheetDetail> TimeSheetDetails { get; set; }        
        public DbSet<PaymentSheet> PaymentSheets { get; set; }
        public DbSet<PaymentSheetDetail> PaymentSheetDetails { get; set; }
        public DbSet<SuperUser> SuperUsers { get; set; }
        public DbSet<IdentityUserClaim<string>> IdentityUserClaims { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // primary keys
            modelBuilder.Entity<User>()
                .HasKey(c => c.UserID);
            modelBuilder.Entity<Job>()
                .HasKey(j => j.JobID);
            modelBuilder.Entity<TimeSheet>()
                .HasKey(t => t.TimeSheetID);
            modelBuilder.Entity<Year>()
                .HasKey(c => c.YearID);
            modelBuilder.Entity<Month>()
                .HasKey(j => j.MonthID);
            modelBuilder.Entity<Week>()
                .HasKey(t => t.WeekID);
            modelBuilder.Entity<TimeSheetDetail>()
                .HasKey(t => t.TimeSheetDetailID);
            modelBuilder.Entity<PaymentSheet>()
                .HasKey(t => t.PaymentSheetID);
            modelBuilder.Entity<PaymentSheetDetail>()
                .HasKey(t => t.PaymentSheetDetailID);
            modelBuilder.Entity<Company>()
                .HasKey(e => e.CompanyID);
            modelBuilder.Entity<SubContractorJob>()
                .HasKey(ej => new {ej.CompanyID, ej.JobID});
            modelBuilder.Entity<SupplierJob>()
                .HasKey(ej => new {ej.CompanyID, ej.JobID});

            // relationships
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Client);

            /*modelBuilder.Entity<Client>()
                .HasOne()
                .WithMany(c => c.Job);*/

            modelBuilder.Entity<EmployeeJob>(entity =>
            {
                entity.HasKey(ej => new { ej.UserID, ej.JobID })
                    .HasName("PK_EmployeeJobs");

                entity.HasOne(e => e.Employee)
                    .WithMany(ej => ej.EmployeeJobs)
                    .HasForeignKey(f => f.UserID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_EmployeeJobs_User_UserID");

                entity.HasOne(e => e.Job)
                    .WithMany(ej => ej.EmployeeJobs)
                    .HasForeignKey(f => f.JobID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_EmployeeJobs_Job_JobID");
            });
            
            modelBuilder.Entity<SubContractorJob>()
                .HasOne(ej => ej.SubContractor)
                .WithMany(ej => ej.SubContractorJobs)
                .HasForeignKey(ej => ej.CompanyID);
            modelBuilder.Entity<SubContractorJob>()
                .HasOne(ej => ej.Job)
                .WithMany(ej => ej.SubContractorJobs)
                .HasForeignKey(ej => ej.JobID);

            modelBuilder.Entity<SupplierJob>()
                .HasOne(ej => ej.Supplier)
                .WithMany(ej => ej.SupplierJobs)
                .HasForeignKey(ej => ej.CompanyID);
            modelBuilder.Entity<SupplierJob>()
                .HasOne(ej => ej.Job)
                .WithMany(ej => ej.SupplierJobs)
                .HasForeignKey(ej => ej.JobID);

            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasKey(p => new { p.Id });
        }
    }
}
