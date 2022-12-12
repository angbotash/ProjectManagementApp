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
        public async Task<IActionResult> CreateEmployee(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = this._mapper.Map<CreateEmployeeViewModel, Employee>(model);
                await this._employeeService.Create(employee);

                return RedirectToAction("GetAllEmployees");
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
        public async Task<IActionResult> ViewEmployee(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var employee = await this._employeeService.Get((int)id);

            if (employee is null)
            {
                return NotFound();
            }

            var result = this._mapper.Map<Employee, EmployeeViewModel>(employee);
            var projects = await this._employeeService.GetProjects((int)id);

            foreach (var proj in projects)
            {
                var tempProject = this._mapper.Map<Project, ProjectViewModel>(proj);

                result.Projects.Add(tempProject);
            }

            return View(result);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var employee = await this._employeeService.Get((int)id);

            if (employee is null)
            {
                return NotFound();
            }

            await this._employeeService.Delete((int)id);

            return RedirectToAction("GetAllEmployees");
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var employee = await this._employeeService.Get((int)id);

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
            if (ModelState.IsValid)
            {
                var updatedEmployee = this._mapper.Map<EditEmployeeViewModel, Employee>(model);

                this._employeeService.Edit(updatedEmployee);
            }

            return RedirectToAction("ViewEmployee", new { model.Id });
        }
    }
}
