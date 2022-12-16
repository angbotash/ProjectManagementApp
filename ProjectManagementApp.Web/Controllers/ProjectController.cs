using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.QueryOrder;
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
            var allManagers = await _userService.GetManagersAsync();
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
                await _projectService.CreateAsync(project);

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

            var project = await _projectService.GetByIdAsync(id.Value);

            if (project is null)
            {
                return NotFound();
            }

            var model = _mapper.Map<Project, EditProjectViewModel>(project);
            var managers = await _userService.GetManagersAsync();
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

                await _projectService.EditAsync(updatedProject);
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

            var project = await _projectService.GetByIdAsync(id.Value);

            if (project is null)
            {
                return NotFound();
            }

            await _projectService.DeleteAsync(id.Value);

            return RedirectToAction("GetAllProjects");
        }

        [HttpGet("GetAllProjects")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetAllProjects(
            SortDirection direction = SortDirection.Ascending,
            string? order = null)
        {
            var projects = await _projectService.GetOrderedListAsync(direction, order);
            var model = new ProjectsViewModel
            {
                Projects = _mapper.Map<IList<ProjectViewModel>>(projects),
                Direction = direction,
                Order = order
            };

            return View(model);
        }

        [HttpGet("GetManagerProjects")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetManagerProjects(
            int? managerId,
            SortDirection direction = SortDirection.Ascending,
            string? order = null,
            string? filter = null)
        {
            if (managerId is null)
            {
                return BadRequest();
            }

            var projects = await _projectService.GetManagerProjectsAsync(managerId.Value, direction, order);
            var model = new ProjectsViewModel()
            {
                Projects = _mapper.Map<IList<ProjectViewModel>>(projects),
                ManagerId = managerId,
                Direction = direction,
                Order = order
            };

            return View(model);
        }

        [HttpGet("GetEmployeeProjects")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetEmployeeProjects(
            int? employeeId,
            SortDirection direction = SortDirection.Ascending,
            string? order = null)
        {
            if (employeeId is null)
            {
                return BadRequest();
            }

            var projects = await _projectService.GetEmployeeProjectsAsync(employeeId.Value, direction, order);
            var model = new ProjectsViewModel()
            {
                Projects = _mapper.Map<IList<ProjectViewModel>>(projects),
                EmployeeId = employeeId,
                Direction = direction,
                Order = order
            };

            return View(model);
        }

        [HttpGet("ViewProject")]
        public async Task<IActionResult> ViewProject(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = await _projectService.GetByIdAsync(id.Value);

            if (project is null)
            {
                return NotFound();
            }

            var model = _mapper.Map<Project, ProjectViewModel>(project);
            var users = project.UserProjects.Select(x => x.User).ToList();
            model.Users = _mapper.Map<IList<UserViewModel>>(users);

            return View(model);
        }

        [HttpGet("EditProjectEmployees")]
        [Authorize(Roles = "Supervisor, Manager")]
        public async Task<IActionResult> EditProjectEmployees(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = await _projectService.GetByIdAsync(id.Value);

            if (project is null)
            {
                return NotFound();
            }

            var allEmployees = await _userService.GetEmployeesAsync();
            var projectEmployees = project.UserProjects.Select(x => x.User).ToList();
            var model = new EditProjectEmployeesViewModel()
            {
                Project = _mapper.Map<Project, ProjectViewModel>(project),
                ProjectEmployees = _mapper.Map<IList<UserViewModel>>(projectEmployees),
            };

            foreach (var user in allEmployees)
            {
                if (!projectEmployees.Contains(user))
                {
                    var tempUser = _mapper.Map<User, UserViewModel>(user);

                    model.AllEmployees.Add(tempUser);
                }
            }

            return View(model);
        }
    }
}
