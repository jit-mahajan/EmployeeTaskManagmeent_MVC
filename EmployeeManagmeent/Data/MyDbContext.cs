using EmployeeManagmeent.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagmeent.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeTask> EmployeeTasks { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Employee
            modelBuilder.Entity<Employee>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Employee>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Employee>()
                .Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<Employee>()
                .Property(x => x.Department)
                .IsRequired()
                .HasMaxLength(100);

            // EmployeeTask
            modelBuilder.Entity<EmployeeTask>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<EmployeeTask>()
                .Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<EmployeeTask>()
                .Property(x => x.Description)
                .HasMaxLength(1000);

            // Store enum as integer
            modelBuilder.Entity<EmployeeTask>()
                .Property(x => x.Status)
                .HasConversion<int>();

            // Employee -> EmployeeTask relationship
            modelBuilder.Entity<Employee>()
                .HasMany(x => x.Tasks)
                .WithOne(x => x.Employee)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
