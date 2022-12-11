using Microsoft.AspNetCore.Mvc;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Web.ViewModels;
using AutoMapper;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Web.Controllers
{
    [Route("Employee")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            this._employeeService = employeeService;
            this._mapper = mapper;
        }

        [HttpGet("CreateEmployee")]
        public IActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost("CreateEmployee")]
        public IActionResult CreateEmployee(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = this._mapper.Map<CreateEmployeeViewModel, Employee>(model);
                this._employeeService.Create(employee);
            }

            return View(model);
        }

        [HttpGet("GetAllEmployees")]
        public IActionResult GetAllEmployees()
        {
            var employees = this._employeeService.GetAll();
            var result = new List<EmployeeViewModel>();

            foreach (var empl in employees)
            {
                var tempEmployee = this._mapper.Map<Employee, EmployeeViewModel>(empl);

                result.Add(tempEmployee);
            }

            return View(result);
        }

        [HttpGet("ViewEmployee")]
        public IActionResult ViewEmployee(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var employee = this._employeeService.Get((int)id);

            if (employee is null)
            {
                return NotFound();
            }

            var result = this._mapper.Map<Employee, EmployeeViewModel>(employee);

            return View(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var employee = this._employeeService.Get((int)id);

            if (employee is null)
            {
                return NotFound();
            }

            this._employeeService.Delete((int)id);

            return RedirectToAction("GetAllEmployees");
        }

        [HttpGet("Edit")]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var employee = this._employeeService.Get((int)id);

            if (employee is null)
            {
                return NotFound();
            }

            var result = this._mapper.Map<Employee, EditEmployeeViewModel>(employee);

            return View(result);
        }

        [HttpPost("Edit")]
        public IActionResult Edit(EditEmployeeViewModel model)
        {
            var temp = model;

            if (ModelState.IsValid)
            {
                var updatedEmployee = this._mapper.Map<EditEmployeeViewModel, Employee>(model);

                this._employeeService.Edit(updatedEmployee);
            }

            return RedirectToAction("ViewEmployee", new { model.Id });
        }
    }
}
