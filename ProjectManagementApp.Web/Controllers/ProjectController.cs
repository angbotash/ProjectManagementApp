using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Web.ViewModels;

namespace ProjectManagementApp.Web.Controllers
{
    [Route("Project")]
    [Authorize(Roles = "Supervisor, Manager, Employee")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IUserService userService, IMapper mapper)
        {
            this._projectService = projectService;
            this._userService = userService;
            this._mapper = mapper;
        }

        [HttpGet("CreateProject")]
        [Authorize(Roles = "Supervisor")]
        public async Task<ActionResult> CreateProject()
        {
            var allManagers = await this._userService.GetManagers();

            var selectListManagers = allManagers.Select(x => new SelectListItem(x.Email, x.Id.ToString())).ToList();

            var model = new CreateProjectViewModel() { Managers = selectListManagers};

            return View(model);
        }

        [HttpPost("CreateProject")]
        [Authorize(Roles = "Supervisor")]
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
        [Authorize(Roles = "Supervisor")]
        public IActionResult GetAllProjects(string sortOrder)
        {
            var projects = this._projectService.GetAll();
            var result = new List<ProjectViewModel>();

            foreach (var proj in projects)
            {
                var tempProject = this._mapper.Map<Project, ProjectViewModel>(proj);

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

        [HttpGet("GetManagerProjects")]
        [Authorize(Roles = "Manager")]
        public IActionResult GetManagerProjects(int? managerId)
        {
            if (managerId is null)
            {
                return BadRequest();
            }

            var projects = this._projectService.GetManagerProjects((int)managerId);
            var result = new List<ProjectViewModel>();

            foreach (var proj in projects)
            {
                var tempProject = this._mapper.Map<Project, ProjectViewModel>(proj);

                result.Add(tempProject);
            }

            return View(result);
        }

        [HttpGet("GetEmployeeProjects")]
        [Authorize(Roles = "Employee")]
        public IActionResult GetEmployeeProjects(int? employeeId)
        {
            if (employeeId is null)
            {
                return BadRequest();
            }

            var projects = this._userService.GetProjects((int)employeeId);
            var result = new List<ProjectViewModel>();

            foreach (var proj in projects)
            {
                var tempProject = this._mapper.Map<Project, ProjectViewModel>(proj);

                result.Add(tempProject);
            }

            return View(result);
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
            var users = this._projectService.GetUsers((int)id);

            foreach (var empl in users)
            {
                var tempUser = this._mapper.Map<User, UserViewModel>(empl);

                result.Users.Add(tempUser);
            }

            return View(result);
        }

        [HttpPost("Delete")]
        [Authorize(Roles = "Supervisor")]
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
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Edit(int? id)
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

            var model = this._mapper.Map<Project, EditProjectViewModel>(project);
            var managers = await this._userService.GetManagers();

            var selectList = managers.Select(x => new SelectListItem(x.Email, x.Id.ToString())).ToList();
            model.Managers = selectList;

            return View(model);
        }

        [HttpPost("Edit")]
        [Authorize(Roles = "Supervisor")]
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
        [Authorize(Roles = "Supervisor, Manager")]
        public async Task<IActionResult> EditProjectEmployees(int? id)
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

            var allEmployees = await this._userService.GetEmployees();
            var projectEmployees = this._projectService.GetUsers((int) id);
            var model = new EditProjectEmployeesViewModel()
                { Project = this._mapper.Map<Project, ProjectViewModel>(project) };

            foreach (var user in allEmployees)
            {
                var tempUser = this._mapper.Map<User, UserViewModel>(user);
                model.AllEmployees.Add(tempUser);
            }

            foreach (var user in projectEmployees)
            {
                var tempUser = this._mapper.Map<User, UserViewModel>(user);
                model.ProjectEmployees.Add(tempUser);
            }

            return View(model);
        }
    }
}
