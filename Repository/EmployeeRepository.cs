using DAL;
using Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IUnitOfWork<PlayGround_DbContext> unitOfWork)
        : base(unitOfWork)
        {
        }
        public EmployeeRepository(PlayGround_DbContext context)
        : base(context)
        {
        }
        public IEnumerable<Employee> GetEmployeesByGender(string Gender)
        {
            return Context.Employees.Where(emp => emp.Gender == Gender).ToList();
        }
        public IEnumerable<Employee> GetEmployeesByDepartment(string Dept)
        {
            return Context.Employees.Where(emp => emp.Department == Dept).ToList();
        }
    }
}
