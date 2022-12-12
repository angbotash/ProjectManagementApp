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

            ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["StartDateSortParam"] = sortOrder == "Date" ? "start_date_desc" : "Date";
            ViewData["EndDateSortParam"] = sortOrder == "Date" ? "end_date_desc" : "Date";
            ViewData["PrioritySortParam"] = sortOrder == "Priority" ? "priority_desc" : "Priority";

            IEnumerable<ProjectViewModel> temp = result;

            switch (sortOrder)
            {
                case "name_desc":
                    temp = temp.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    temp = temp.OrderBy(s => s.StartDate);
                    break;
                case "start_date_desc":
                    temp = temp.OrderByDescending(s => s.StartDate);
                    break;
                case "end_date_desc":
                    temp = temp.OrderByDescending(s => s.StartDate);
                    break;
                case "Priority":
                    temp = temp.OrderBy(s => s.Priority);
                    break;
                case "priority_desc":
                    temp = temp.OrderByDescending(s => s.Priority);
                    break;
                default:
                    temp = temp.OrderBy(s => s.Name);
                    break;
            }

            return View(temp.ToList());
        }

        [HttpGet("ViewProject")]
        public async Task<IActionResult> ViewProject(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = await this._projectService.Get((int)id);

            if (project is null)
            {
                return NotFound();
            }

            var result = this._mapper.Map<Project, ProjectViewModel>(project);
            var employees = await this._projectService.GetEmployees((int)id);

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

            var project = await this._projectService.Get((int) id);

            if (project is null)
            {
                return NotFound();
            }

            await this._projectService.Delete((int) id);

            return RedirectToAction("GetAllProjects");
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = await this._projectService.Get((int)id);
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

            return RedirectToAction("ViewProject", new { model.Id });
        }

        [HttpGet("EditProjectEmployees")]
        public async Task<IActionResult> EditProjectEmployees(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = await this._projectService.Get((int) id);

            if (project is null)
            {
                return NotFound();
            }

            var allEmployees = this._employeeService.GetAll();
            var projectEmployees = await this._projectService.GetEmployees((int) id);
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

        [HttpPost("AddToProject")]
        public async Task<IActionResult> AddToProject(int? projectId, int? employeeId)
        {
            if (projectId is null || employeeId is null)
            {
                return BadRequest();
            }

            if (await this._projectService.Get((int)projectId) is null)
            {
                return NotFound();
            }

            if (await this._employeeService.Get((int)employeeId) is null)
            {
                return NotFound();
            }

            await this._projectService.AddToProject((int)projectId, (int)employeeId);

            return RedirectToAction("ViewProject", new { projectId });
        }

        [HttpPost("RemoveFromProject")]
        public async Task<IActionResult> RemoveFromProject(int? projectId, int? employeeId)
        {
            if (projectId is null || employeeId is null)
            {
                return BadRequest();
            }

            if (await this._projectService.Get((int)projectId) is null)
            {
                return NotFound();
            }

            if (await this._employeeService.Get((int)employeeId) is null)
            {
                return NotFound();
            }

            await this._projectService.RemoveFromProject((int)projectId, (int)employeeId);

            return RedirectToAction("ViewProject", new { projectId });
        }
    }
}
