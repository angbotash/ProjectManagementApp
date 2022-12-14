using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Web.ViewModels;

namespace ProjectManagementApp.Web.Controllers
{
    [Route("Issue")]
    [Authorize(Roles = "Supervisor, Manager, Employee")]
    public class IssueController : Controller
    {
        private readonly IIssueService _issueService;
        private readonly IUserService _userService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public IssueController(IIssueService issueService, IUserService userService, IProjectService projectService, IMapper mapper)
        {
            _issueService = issueService;
            _userService = userService;
            _projectService = projectService;
            _mapper = mapper;
        }

        [HttpGet("Create")]
        [Authorize(Roles = "Supervisor, Manager")]
        public async Task<ActionResult> Create(int? projectId)
        {
            if (projectId is null)
            {
                return BadRequest();
            }

            if (_projectService.Get((int)projectId) == null)
            {
                return NotFound();
            }

            var allEmployees = await _userService.GetEmployees();
            var allManagers = await _userService.GetManagers();

            var selectListEmployees = allEmployees .Select(u => new SelectListItem(u.Email, u.Id.ToString())).ToList();
            var selectListManagers = allManagers.Select(u => new SelectListItem(u.Email, u.Id.ToString())).ToList();
            var model = new CreateIssueViewModel
            {
                Employees = selectListEmployees,
                Managers = selectListManagers,
                ProjectId = (int)projectId,
                Statuses = new List<SelectListItem>()
                {
                    new SelectListItem(
                        IssueStatus.ToDo.ToString(),
                        IssueStatus.ToDo.ToString()),
                    new SelectListItem(
                        IssueStatus.InProgress.ToString(),
                        IssueStatus.InProgress.ToString()),
                    new SelectListItem(
                        IssueStatus.Done.ToString(),
                        IssueStatus.Done.ToString())
                }
            };

            return View(model);
        }

        [HttpPost("Create")]
        [Authorize(Roles = "Supervisor, Manager")]
        public async Task<ActionResult> Create(CreateIssueViewModel model)
        {
            if (ModelState.IsValid)
            {
                var issue = _mapper.Map<CreateIssueViewModel, Issue>(model);

                await _issueService.Create(issue);

                return RedirectToAction("ViewProject", "Project", new { id = issue.ProjectId });
            }

            return View(model);
        }

        [HttpGet("Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var issue = _issueService.Get((int)id);

            if (issue is null)
            {
                return NotFound();
            }

            var model = _mapper.Map<Issue, EditIssueViewModel>(issue);
            var allEmployees = await _userService.GetEmployees();

            model.Employees = allEmployees.Select(u => new SelectListItem(u.Email, u.Id.ToString())).ToList();
            model.Statuses = new List<SelectListItem>()
            {
                new SelectListItem(
                    IssueStatus.ToDo.ToString(),
                    IssueStatus.ToDo.ToString()),
                new SelectListItem(
                    IssueStatus.InProgress.ToString(),
                    IssueStatus.InProgress.ToString()),
                new SelectListItem(
                    IssueStatus.Done.ToString(),
                    IssueStatus.Done.ToString())
            };

            return View(model);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(EditIssueViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updatedIssue = _mapper.Map<EditIssueViewModel, Issue>(model);

                await _issueService.Edit(updatedIssue);

                return RedirectToAction("ViewIssue", new { id = model.Id });
            }

            return View(model);
        }

        [HttpPost("Delete")]
        [Authorize(Roles = "Supervisor, Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var issue = _issueService.Get((int)id);

            if (issue is null)
            {
                return NotFound();
            }

            var projectId = issue.ProjectId;

            await _issueService.Delete((int)id);

            return RedirectToAction("ViewProject", "Project", new { id = projectId });
        }

        [HttpGet("ViewIssue")]
        public IActionResult ViewIssue(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var issue = _issueService.Get((int)id);

            if (issue is null)
            {
                return NotFound();
            }

            var result = _mapper.Map<Issue, IssueViewModel>(issue);

            return View(result);
        }

        //[HttpGet("ViewAllIssues")]
        //public IActionResult ViewAllIssues(int? userId, string sortOrder, string issueKind = "")
        //{
        //    if (userId is null)
        //    {
        //        return BadRequest();
        //    }

        //    var employee = this._userService.Get((int) userId);

        //    if (employee is null)
        //    {
        //        return NotFound();
        //    }

        //    var tempEmployee = this._mapper.Map<User, UserViewModel>(employee);

        //    ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : string.Empty;
        //    ViewData["StatusSortParam"] = sortOrder == "Status" ? "status_desc" : "Status";
        //    ViewData["PrioritySortParam"] = sortOrder == "Priority" ? "priority_desc" : "Priority";

        //    IEnumerable<IssueViewModel> sortedIssues = issueKind == "assignee"
        //        ? tempEmployee.AssignedIssues
        //        : tempEmployee.ReportedIssues;

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            sortedIssues = sortedIssues.OrderByDescending(s => s.Name);
        //            break;
        //        case "Status":
        //            sortedIssues = sortedIssues.OrderBy(s => s.Status);
        //            break;
        //        case "status_desc":
        //            sortedIssues = sortedIssues.OrderByDescending(s => s.Status);
        //            break;
        //        case "Priority":
        //            sortedIssues = sortedIssues.OrderBy(s => s.Priority);
        //            break;
        //        case "priority_desc":
        //            sortedIssues = sortedIssues.OrderByDescending(s => s.Priority);
        //            break;
        //        default:
        //            sortedIssues = sortedIssues.OrderBy(s => s.Name);
        //            break;
        //    }

        //    return View(sortedIssues.ToList());
        //}

        [HttpGet("ViewAssignedIssues")]
        public IActionResult ViewAssignedIssues(int? userId)
        {
            if (userId is null)
            {
                return BadRequest();
            }

            var user = _userService.Get((int)userId);

            if (user is null)
            {
                return NotFound();
            }

            var tempUser = _mapper.Map<User, UserViewModel>(user);
            var issues = tempUser.AssignedIssues;

            return View(issues);
        }

        [HttpGet("ViewReportedIssues")]
        public IActionResult ViewReportedIssues(int? userId)
        {
            if (userId is null)
            {
                return BadRequest();
            }

            var user = _userService.Get((int)userId);

            if (user is null)
            {
                return NotFound();
            }

            var tempUser = _mapper.Map<User, UserViewModel>(user);
            var issues = tempUser.ReportedIssues;

            return View(issues);
        }

        [HttpGet("ViewProjectIssues")]
        public IActionResult ViewProjectIssues(int? projectId)
        {
            if (projectId is null)
            {
                return BadRequest();
            }

            var project = _projectService.Get((int)projectId);

            if (project is null)
            {
                return NotFound();
            }

            var tempProject = _mapper.Map<Project, ProjectViewModel>(project);
            var issues = tempProject.Issues;

            return View(issues);
        }
    }
}
