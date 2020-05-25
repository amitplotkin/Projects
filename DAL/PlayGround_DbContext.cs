using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace DAL
{
    public class PlayGround_DbContext : DbContext
    {
        public PlayGround_DbContext() 
        {
        }
       

        public PlayGround_DbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee() { EmployeeId = 1, Name = "John", Designation = "Developer", Address = "New York", CompanyName = "XYZ Inc", Salary = 30000 },
                new Employee() { EmployeeId = 2, Name = "Chris", Designation = "Manager", Address = "New York", CompanyName = "ABC Inc", Salary = 50000 },
                new Employee() { EmployeeId = 3, Name = "Mukesh", Designation = "Consultant", Address = "New Delhi", CompanyName = "XYZ Inc", Salary = 20000 });
        }


        public override int SaveChanges()
        {

            var entities = from e in ChangeTracker.Entries()
                           let state = e.State
                           where e.State == EntityState.Added
                               || state == EntityState.Modified
                           select e.Entity;

            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);
                //Validator.ValidateObject will throw a ValidationException if validation fails, which you can handle accordingly.
                Validator.ValidateObject(entity, validationContext);
            }

            return base.SaveChanges();

        }

    }
      
}  

