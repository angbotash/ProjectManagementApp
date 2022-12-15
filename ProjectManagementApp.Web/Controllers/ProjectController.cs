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
            _projectService = projectService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("CreateProject")]
        [Authorize(Roles = "Supervisor")]
        public async Task<ActionResult> CreateProject()
        {
            var allManagers = await _userService.GetManagers();
            var selectListManagers = allManagers.Select(u => new SelectListItem(u.Email, u.Id.ToString())).ToList();
            var model = new CreateProjectViewModel() { Managers = selectListManagers};

            return View(model);
        }

        [HttpPost("CreateProject")]
        [Authorize(Roles = "Supervisor")]
        public async Task<ActionResult> CreateProject(CreateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = _mapper.Map<CreateProjectViewModel, Project>(model);
                await _projectService.Create(project);

                return RedirectToAction("GetAllProjects");
            }

            return View(model);
        }

        [HttpGet("Edit")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = await _projectService.GetById((int)id);

            if (project is null)
            {
                return NotFound();
            }

            var model = _mapper.Map<Project, EditProjectViewModel>(project);
            var managers = await _userService.GetManagers();
            var selectList = managers.Select(u => new SelectListItem(u.Email, u.Id.ToString())).ToList();
            model.Managers = selectList;

            return View(model);
        }

        [HttpPost("Edit")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Edit(EditProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updatedProject = _mapper.Map<EditProjectViewModel, Project>(model);

                await _projectService.Edit(updatedProject);
            }

            return RedirectToAction("ViewProject", new { id = model.Id });
        }

        [HttpPost("Delete")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = await _projectService.GetById((int)id);

            if (project is null)
            {
                return NotFound();
            }

            await _projectService.Delete((int)id);

            return RedirectToAction("GetAllProjects");
        }

        [HttpGet("GetAllProjects")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetAllProjects(string sortOrder)
        {
            var projects = await _projectService.GetAll();
            var result = new List<ProjectViewModel>();

            foreach (var proj in projects)
            {
                var tempProject = _mapper.Map<Project, ProjectViewModel>(proj);

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
        public async Task<IActionResult> GetManagerProjects(int? managerId)
        {
            if (managerId is null)
            {
                return BadRequest();
            }

            var projects = await _projectService.GetManagerProjects((int)managerId);
            var result = new List<ProjectViewModel>();

            foreach (var proj in projects)
            {
                var tempProject = _mapper.Map<Project, ProjectViewModel>(proj);

                result.Add(tempProject);
            }

            return View(result);
        }

        [HttpGet("GetEmployeeProjects")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetEmployeeProjects(int? employeeId)
        {
            if (employeeId is null)
            {
                return BadRequest();
            }

            var user = await _userService.GetById((int)employeeId);

            if (user is null)
            {
                return BadRequest();
            }

            var result = new List<ProjectViewModel>();

            foreach (var proj in user.UserProjects)
            {
                var tempProject = _mapper.Map<Project, ProjectViewModel>(proj.Project);

                result.Add(tempProject);
            }

            return View(result);
        }

        [HttpGet("ViewProject")]
        public async Task<IActionResult> ViewProject(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = await _projectService.GetById((int)id);

            if (project is null)
            {
                return NotFound();
            }

            var result = _mapper.Map<Project, ProjectViewModel>(project);

            foreach (var user in project.UserProjects)
            {
                var tempUser = _mapper.Map<User, UserViewModel>(user.User);

                result.Users.Add(tempUser);
            }

            return View(result);
        }

        [HttpGet("EditProjectEmployees")]
        [Authorize(Roles = "Supervisor, Manager")]
        public async Task<IActionResult> EditProjectEmployees(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = await _projectService.GetById((int)id);

            if (project is null)
            {
                return NotFound();
            }

            var allEmployees = await _userService.GetEmployees();
            var model = new EditProjectEmployeesViewModel()
            {
                Project = _mapper.Map<Project, ProjectViewModel>(project)
            };

            foreach (var proj in project.UserProjects)
            {
                var tempUser = _mapper.Map<User, UserViewModel>(proj.User);

                model.ProjectEmployees.Add(tempUser);
            }

            foreach (var user in allEmployees)
            {
                var tempUser = _mapper.Map<User, UserViewModel>(user);

                if (model.ProjectEmployees.Contains(tempUser))
                {
                    continue;
                }

                model.AllEmployees.Add(tempUser);
            }

            return View(model);
        }
    }
}
