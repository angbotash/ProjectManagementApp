using Microsoft.AspNetCore.Mvc;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Web.ViewModels;
using AutoMapper;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Web.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IProjectService projectService, IMapper mapper)
        {
            this._userService = userService;
            this._projectService = projectService;
            this._mapper = mapper;
        }

        [HttpGet("CreateUser")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = this._mapper.Map<CreateUserViewModel, User>(model);
                await this._userService.Create(user);

                return RedirectToAction("GetAllUsers");
            }

            return View(model);
        }

        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = this._userService.GetAll();
            var result = new List<UserViewModel>();

            foreach (var empl in users)
            {
                var tempUser = this._mapper.Map<User, UserViewModel>(empl);

                result.Add(tempUser);
            }

            return View(result);
        }

        [HttpGet("ViewUser")]
        public IActionResult ViewUser(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var user = this._userService.Get((int)id);

            if (user is null)
            {
                return NotFound();
            }

            var result = this._mapper.Map<User, UserViewModel>(user);
            var projects = this._userService.GetProjects((int)id);

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

            var user = this._userService.Get((int)id);

            if (user is null)
            {
                return NotFound();
            }

            await this._userService.Delete((int)id);

            return RedirectToAction("GetAllUsers");
        }

        [HttpGet("Edit")]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var user = this._userService.Get((int)id);

            if (user is null)
            {
                return NotFound();
            }

            var result = this._mapper.Map<User, EditUserViewModel>(user);

            return View(result);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updatedUser = this._mapper.Map<EditUserViewModel, User>(model);

                await this._userService.Edit(updatedUser);
            }

            return RedirectToAction("ViewUser", new { id = model.Id });
        }

        [HttpPost("AddToProject")]
        public async Task<IActionResult> AddToProject(int? projectId, int? userId)
        {
            if (projectId is null || userId is null)
            {
                return BadRequest();
            }

            if (this._projectService.Get((int)projectId) is null)
            {
                return NotFound();
            }

            if (this._userService.Get((int)userId) is null)
            {
                return NotFound();
            }

            await this._userService.AddToProject((int)projectId, (int)userId);

            return RedirectToAction("EditProjectEmployees", "Project" , new { id = projectId });
        }

        [HttpPost("RemoveFromProject")]
        public async Task<IActionResult> RemoveFromProject(int? projectId, int? userId)
        {
            if (projectId is null || userId is null)
            {
                return BadRequest();
            }

            if (this._projectService.Get((int)projectId) is null)
            {
                return NotFound();
            }

            if (this._userService.Get((int)userId) is null)
            {
                return NotFound();
            }

            await this._userService.RemoveFromProject((int)projectId, (int)userId);

            return RedirectToAction("EditProjectEmployees", "Project", new { id = projectId });
        }
    }
}
