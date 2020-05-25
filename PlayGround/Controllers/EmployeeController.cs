using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Repository;
using Repository.UnitOfWork;

namespace PlayGround.Controllers
{
    public class EmployeeController : Controller
    {
        private UnitOfWork<PlayGround_DbContext> unitOfWork = new UnitOfWork<PlayGround_DbContext>();
        private GenericRepository<Employee> repository;
        private IEmployeeRepository employeeRepository;
        public EmployeeController()
        {
            //If you want to use Generic Repository with Unit of work
            repository = new GenericRepository<Employee>(unitOfWork);
            //If you want to use Specific Repository with Unit of work
            employeeRepository = new EmployeeRepository(unitOfWork);
        }
        [HttpGet]
        public ActionResult Index()
        {
            var model = repository.GetAll();
            //Using Specific Repository
            //var model = employeeRepository.GetEmployeesByDepartment(1);
            return View(model);
        }
        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddEmployee(Employee model)
        {
            try
            {
                unitOfWork.CreateTransaction();
                if (ModelState.IsValid)
                {
                    repository.Insert(model);
                    unitOfWork.Save();
                    //Do Some Other Task with the Database
                    //If everything is working then commit the transaction else rollback the transaction
                    unitOfWork.Commit();
                    return RedirectToAction("Index", "Employee");
                }
            }
            catch (Exception ex)
            {
                //Log the exception and rollback the transaction
                unitOfWork.Rollback();
            }
            return View();
        }
        [HttpGet]
        public ActionResult EditEmployee(int EmployeeId)
        {
            Employee model = repository.GetById(EmployeeId);
            return View(model);
        }
        [HttpPost]
        public ActionResult EditEmployee(Employee model)
        {
            if (ModelState.IsValid)
            {
                repository.Update(model);
                unitOfWork.Save();
                return RedirectToAction("Index", "Employee");
            }
            else
            {
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult DeleteEmployee(int EmployeeId)
        {
            Employee model = repository.GetById(EmployeeId);
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(int EmployeeID)
        {
            Employee model = repository.GetById(EmployeeID);
            repository.Delete(model);
            unitOfWork.Save();
            return RedirectToAction("Index", "Employee");
        }
    }
}