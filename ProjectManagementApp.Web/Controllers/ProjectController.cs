using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Web.ViewModels;

namespace ProjectManagementApp.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService, IMapper mapper)
        {
            this._projectService = projectService;
            this._mapper = mapper;
        }

        // GET: ProjectController/Create
        [HttpGet("CreateProject")]
        public ActionResult CreateProject()
        {
            return View();
        }

        // POST: ProjectController/Create
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
    }
}
