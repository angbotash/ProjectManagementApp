using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Web.ViewModels;

namespace ProjectManagementApp.Web.Controllers
{
    [Route("Project")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IMapper mapper)
        {
            this._projectService = projectService;
            this._mapper = mapper;
        }

        [HttpGet("CreateProject")]
        public ActionResult CreateProject()
        {
            return View();
        }

        [HttpPost("CreateProject")]
        public ActionResult CreateProject(CreateProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = this._mapper.Map<CreateProjectViewModel, Project>(model);
                this._projectService.Create(project);
            }

            return View(model);
        }

        [HttpGet("GetAllProjects")]
        public IActionResult GetAllProjects()
        {
            var projects = this._projectService.GetAll();
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

            return View(result);
        }

        [HttpPost("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var project = this._projectService.Get((int) id);

            if (project is null)
            {
                return NotFound();
            }

            this._projectService.Delete((int) id);

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

            if (project is null)
            {
                return NotFound();
            }

            var result = this._mapper.Map<Project, EditProjectViewModel>(project);

            return View(result);
        }

        [HttpPost("Edit")]
        public IActionResult Edit(EditProjectViewModel model)
        {
            var temp = model;

            if (ModelState.IsValid)
            {
                var updatedProject = this._mapper.Map<EditProjectViewModel, Project>(model);

                this._projectService.Edit(updatedProject);
            }

            return RedirectToAction("ViewProject", new { model.Id });
        }
    }
}
