using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Web.ViewModels;

namespace ProjectManagementApp.Web.Controllers
{
    [Route("Project")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IEmployeeService employeeService, IMapper mapper)
        {
            this._projectService = projectService;
            this._employeeService = employeeService;
            this._mapper = mapper;
        }

        [HttpGet("CreateProject")]
        public ActionResult CreateProject()
        {
            var allEmployees = this._employeeService.GetAll();
            var employees = new List<EmployeeViewModel>();

            foreach (var empl in allEmployees)
            {
                var tempEmployee = this._mapper.Map<Employee, EmployeeViewModel>(empl);

                employees.Add(tempEmployee);
            }

            var selectList = employees.Select(x => new SelectListItem(x.Email, x.Id.ToString())).ToList();
            var model = new CreateProjectViewModel() { Employees = selectList };

            return View(model);
        }

        [HttpPost("CreateProject")]
        public async Task<ActionResult> CreateProject(CreateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = this._mapper.Map<CreateProjectViewModel, Project>(model);
                await this._projectService.Create(project);

                return RedirectToAction("GetAllProjects");
            }

            return View(model);
        }

        [HttpGet("GetAllProjects")]
        public IActionResult GetAllProjects(string sortOrder)
        {
            var projects = this._projectService.GetAll();
            var result = new List<ProjectViewModel>();

            foreach (var proj in projects)
            {
                var tempProject = this._mapper.Map<Project, ProjectViewModel>(proj);

                if (proj.Manager != null)
                {
                    var tempManager = this._mapper.Map<Employee, EmployeeViewModel>(proj.Manager);

                    tempProject.Manager = tempManager;
                }

                result.Add(tempProject);
            }

            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : String.Empty;
            ViewData["StartDateSortParam"] = sortOrder == "Date" ? "start_date_desc" : "Date";
            ViewData["EndDateSortParam"] = sortOrder == "Date" ? "end_date_desc" : "Date";
            ViewData["PrioritySortParam"] = sortOrder == "Priority" ? "priority_desc" : "Priority";

            IEnumerable<ProjectViewModel> sortedProjects = result;

            switch (sortOrder)
            {
                case "name_desc":
                    sortedProjects = sortedProjects.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    sortedProjects = sortedProjects.OrderBy(s => s.StartDate);
                    break;
                case "start_date_desc":
                    sortedProjects = sortedProjects.OrderByDescending(s => s.StartDate);
                    break;
                case "end_date_desc":
                    sortedProjects = sortedProjects.OrderByDescending(s => s.StartDate);
                    break;
                case "Priority":
                    sortedProjects = sortedProjects.OrderBy(s => s.Priority);
                    break;
                case "priority_desc":
                    sortedProjects = sortedProjects.OrderByDescending(s => s.Priority);
                    break;
                default:
                    sortedProjects = sortedProjects.OrderBy(s => s.Name);
                    break;
            }

            return View(sortedProjects.ToList());
        }

        [HttpGet("ViewProject")]
        public IActionResult ViewProject(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = this._projectService.Get((int)id);

            if (project is null)
            {
                return NotFound();
            }

            var result = this._mapper.Map<Project, ProjectViewModel>(project);
            var employees = this._projectService.GetEmployees((int)id);

            foreach (var empl in employees)
            {
                var tempEmployee = this._mapper.Map<Employee, EmployeeViewModel>(empl);

                result.Employees.Add(tempEmployee);
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

            var project = this._projectService.Get((int)id);

            if (project is null)
            {
                return NotFound();
            }

            await this._projectService.Delete((int) id);

            return RedirectToAction("GetAllProjects");
        }

        [HttpGet("Edit")]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = this._projectService.Get((int)id);
            var allEmployees = this._employeeService.GetAll();

            if (project is null)
            {
                return NotFound();
            }

            var model = this._mapper.Map<Project, EditProjectViewModel>(project);
            var managers = new List<EmployeeViewModel>();

            foreach (var empl in allEmployees)
            {
                var tempEmployee = this._mapper.Map<Employee, EmployeeViewModel>(empl);

                managers.Add(tempEmployee);
            }

            var selectList = managers.Select(x => new SelectListItem(x.Email, x.Id.ToString())).ToList();
            model.Managers = selectList;

            return View(model);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(EditProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updatedProject = this._mapper.Map<EditProjectViewModel, Project>(model);

                await this._projectService.Edit(updatedProject);
            }

            return RedirectToAction("ViewProject", new { id = model.Id });
        }

        [HttpGet("EditProjectEmployees")]
        public IActionResult EditProjectEmployees(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = this._projectService.Get((int)id);

            if (project is null)
            {
                return NotFound();
            }

            var allEmployees = this._employeeService.GetAll();
            var projectEmployees = this._projectService.GetEmployees((int) id);
            var model = new EditProjectEmployeesViewModel()
                {Project = this._mapper.Map<Project, ProjectViewModel>(project)};

            foreach (var employee in allEmployees)
            {
                var tempEmployee = this._mapper.Map<Employee, EmployeeViewModel>(employee);
                model.AllEmployees.Add(tempEmployee);
            }

            foreach (var employee in projectEmployees)
            {
                var tempEmployee = this._mapper.Map<Employee, EmployeeViewModel>(employee);
                model.ProjectEmployees.Add(tempEmployee);
            }

            return View(model);
        }
    }
}
