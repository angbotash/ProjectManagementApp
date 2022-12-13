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
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IUserService userService, IMapper mapper)
        {
            this._projectService = projectService;
            this._userService = userService;
            this._mapper = mapper;
        }

        [HttpGet("CreateProject")]
        public ActionResult CreateProject()
        {
            var allUsers = this._userService.GetAll();
            var users = new List<UserViewModel>();

            foreach (var empl in allUsers)
            {
                var tempUser = this._mapper.Map<User, UserViewModel>(empl);

                users.Add(tempUser);
            }

            var selectList = users.Select(x => new SelectListItem(x.Email, x.Id.ToString())).ToList();
            var model = new CreateProjectViewModel() { Users = selectList };

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
                    var tempManager = this._mapper.Map<User, UserViewModel>(proj.Manager);

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
            var users = this._projectService.GetUsers((int)id);

            foreach (var empl in users)
            {
                var tempUser = this._mapper.Map<User, UserViewModel>(empl);

                result.Users.Add(tempUser);
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
            var allUsers = this._userService.GetAll();

            if (project is null)
            {
                return NotFound();
            }

            var model = this._mapper.Map<Project, EditProjectViewModel>(project);
            var managers = new List<UserViewModel>();

            foreach (var empl in allUsers)
            {
                var tempUser = this._mapper.Map<User, UserViewModel>(empl);

                managers.Add(tempUser);
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

            var allUsers = this._userService.GetAll();
            var projectUsers = this._projectService.GetUsers((int) id);
            var model = new EditProjectEmployeesViewModel()
                {Project = this._mapper.Map<Project, ProjectViewModel>(project)};

            foreach (var user in allUsers)
            {
                var tempUser = this._mapper.Map<User, UserViewModel>(user);
                model.AllUsers.Add(tempUser);
            }

            foreach (var user in projectUsers)
            {
                var tempUser = this._mapper.Map<User, UserViewModel>(user);
                model.ProjectUsers.Add(tempUser);
            }

            return View(model);
        }
    }
}
