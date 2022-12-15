using Microsoft.AspNetCore.Mvc;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ProjectManagementApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManagementApp.Web.Controllers
{
    [Route("User")]
    [Authorize(Roles = "Supervisor, Manager, Employee")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IProjectService projectService, IMapper mapper)
        {
            _userService = userService;
            _projectService = projectService;
            _mapper = mapper;
        }

        [HttpGet("CreateUser")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> CreateUser()
        {
            var roles = await _userService.GetRolesAsync();
            var selectList = roles.Select(x => new SelectListItem(x.Name, x.Name.ToString())).ToList();
            var model = new CreateUserViewModel()
            {
                Roles = selectList
            };

            return View(model);
        }

        [HttpPost("CreateUser")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<CreateUserViewModel, User>(model);
                var result = await _userService.CreateAsync(user, model.Password, model.Role);

                if (result.Success)
                {
                    return RedirectToAction("GetAllUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            return View(model);
        }

        [HttpGet("EditAsync")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> EditAsync(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var user = await _userService.GetByIdAsync((int)id);

            if (user is null)
            {
                return NotFound();
            }

            var result = _mapper.Map<User, EditUserViewModel>(user);

            return View(result);
        }

        [HttpPost("EditAsync")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updatedUser = _mapper.Map<EditUserViewModel, User>(model);

                await _userService.EditAsync(updatedUser);
            }

            return RedirectToAction("ViewUser", new { id = model.Id });
        }

        [HttpPost("DeleteAsync")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var user = await _userService.GetByIdAsync((int)id);

            if (user is null)
            {
                return NotFound();
            }

            await _userService.DeleteAsync((int)id);

            return RedirectToAction("GetAllUsers");
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Supervisor")]
        public async Task<IActionResult> GetAllUsers()
        {
            var currentUser = await _userService.GetCurrentUserAsync(User);

            if (currentUser is null)
            {
                return BadRequest();
            }

            var users = await _userService.GetAllAsync();
            var result = new List<UserViewModel>();

            foreach (var user in users)
            {
                var tempUser = _mapper.Map<User, UserViewModel>(user);

                if (tempUser.Id == currentUser.Id)
                {
                    continue;
                }

                result.Add(tempUser);
            }

            return View(result);
        }

        [HttpPost("AddToProjectAsync")]
        [Authorize(Roles = "Supervisor, Manager")]
        public async Task<IActionResult> AddToProject(int? projectId, int? userId)
        {
            if (projectId is null || userId is null)
            {
                return BadRequest();
            }

            if (await _projectService.GetByIdAsync((int)projectId) is null)
            {
                return NotFound();
            }

            if (await _userService.GetByIdAsync((int)userId) is null)
            {
                return NotFound();
            }

            await _userService.AddToProjectAsync((int)projectId, (int)userId);

            return RedirectToAction("EditProjectEmployees", "Project", new { id = projectId });
        }

        [HttpPost("RemoveFromProjectAsync")]
        [Authorize(Roles = "Supervisor, Manager")]
        public async Task<IActionResult> RemoveFromProject(int? projectId, int? userId)
        {
            if (projectId is null || userId is null)
            {
                return BadRequest();
            }

            if (await _projectService.GetByIdAsync((int)projectId) is null)
            {
                return NotFound();
            }

            if (await _userService.GetByIdAsync((int)userId) is null)
            {
                return NotFound();
            }

            await _userService.RemoveFromProjectAsync((int)projectId, (int)userId);

            return RedirectToAction("EditProjectEmployees", "Project", new { id = projectId });
        }

        [HttpGet("ViewUser")]
        public async Task<IActionResult> ViewUser(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var user = await _userService.GetByIdAsync((int)id);

            if (user is null)
            {
                return NotFound();
            }

            var result = _mapper.Map<User, UserViewModel>(user);

            foreach (var proj in user.UserProjects)
            {
                var tempProject = _mapper.Map<Project, ProjectViewModel>(proj.Project);

                result.Projects.Add(tempProject);
            }

            return View(result);
        }
    }
}
