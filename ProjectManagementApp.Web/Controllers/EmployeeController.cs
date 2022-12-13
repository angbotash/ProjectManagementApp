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
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IProjectService projectService, IMapper mapper)
        {
            this._employeeService = employeeService;
            this._projectService = projectService;
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
            var projects = this._employeeService.GetProjects((int)id);

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

            var employee = this._employeeService.Get((int)id);

            if (employee is null)
            {
                return NotFound();
            }

            await this._employeeService.Delete((int)id);

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
        public async Task<IActionResult> Edit(EditEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updatedEmployee = this._mapper.Map<EditEmployeeViewModel, Employee>(model);

                await this._employeeService.Edit(updatedEmployee);
            }

            return RedirectToAction("ViewEmployee", new { model.Id });
        }

        [HttpPost("AddToProject")]
        public async Task<IActionResult> AddToProject(int? projectId, int? employeeId)
        {
            if (projectId is null || employeeId is null)
            {
                return BadRequest();
            }

            if (this._projectService.Get((int)projectId) is null)
            {
                return NotFound();
            }

            if (this._employeeService.Get((int)employeeId) is null)
            {
                return NotFound();
            }

            await this._employeeService.AddToProject((int)projectId, (int)employeeId);

            return RedirectToAction("ViewProject", "Project" , new { projectId });
        }

        [HttpPost("RemoveFromProject")]
        public async Task<IActionResult> RemoveFromProject(int? projectId, int? employeeId)
        {
            if (projectId is null || employeeId is null)
            {
                return BadRequest();
            }

            if (this._projectService.Get((int)projectId) is null)
            {
                return NotFound();
            }

            if (this._employeeService.Get((int)employeeId) is null)
            {
                return NotFound();
            }

            await this._employeeService.RemoveFromProject((int)projectId, (int)employeeId);

            return RedirectToAction("ViewProject", "Project", new { projectId });
        }
    }
}
