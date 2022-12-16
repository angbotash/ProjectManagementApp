using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.QueryOrder;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Web.ViewModels;
using IssueStatus = ProjectManagementApp.Web.ViewModels.IssueStatus;

namespace ProjectManagementApp.Web.Controllers
{
    [Route("Issue")]
    [Authorize(Roles = "Supervisor, Manager, Employee")]
    public class IssueController : Controller
    {
        private readonly IIssueService _issueService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public IssueController(IIssueService issueService, IUserService userService, IMapper mapper)
        {
            _issueService = issueService;
            _userService = userService;
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

            var allEmployees = await _userService.GetEmployeesAsync();
            var allManagers = await _userService.GetManagersAsync();

            var selectListEmployees = allEmployees.Select(u => new SelectListItem(u.Email, u.Id.ToString())).ToList();
            var selectListManagers = allManagers.Select(u => new SelectListItem(u.Email, u.Id.ToString())).ToList();
            var model = new CreateIssueViewModel
            {
                Employees = selectListEmployees,
                Managers = selectListManagers,
                ProjectId = projectId.Value,
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

                await _issueService.CreateAsync(issue);

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

            var issue = await _issueService.GetByIdAsync(id.Value);

            if (issue is null)
            {
                return NotFound();
            }

            var model = _mapper.Map<Issue, EditIssueViewModel>(issue);
            var allEmployees = await _userService.GetEmployeesAsync();

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

                await _issueService.EditAsync(updatedIssue);

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

            var issue = await _issueService.GetByIdAsync(id.Value);

            if (issue is null)
            {
                return NotFound();
            }

            var projectId = issue.ProjectId;

            await _issueService.DeleteAsync(id.Value);

            return RedirectToAction("ViewProject", "Project", new { id = projectId });
        }

        [HttpGet("ViewIssue")]
        public async Task<IActionResult> ViewIssueAsync(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var issue = await _issueService.GetByIdAsync(id.Value);

            if (issue is null)
            {
                return NotFound();
            }

            var result = _mapper.Map<Issue, IssueViewModel>(issue);

            return View(result);
        }

        [HttpGet("ViewReportedIssues")]
        public async Task<IActionResult> ViewReportedIssues(int? userId,
            SortDirection direction = SortDirection.Ascending,
            string? order = null)
        {
            if (userId is null)
            {
                return BadRequest();
            }

            var issues = await _issueService.GetReportedIssuesAsync(userId.Value, direction, order);

            var model = new IssuesViewModel()
            {
                Issues = _mapper.Map<IList<IssueViewModel>>(issues),
                ReporterId = userId.Value,
                Direction = direction,
                Order = order,
            };

            return View(model);
        }

        [HttpGet("ViewAssignedIssues")]
        public async Task<IActionResult> ViewAssignedIssuesAsync(int? userId,
            SortDirection direction = SortDirection.Ascending,
            string? order = null)
        {
            if (userId is null)
            {
                return BadRequest();
            }

            var issues = await _issueService.GetAssignedIssuesAsync(userId.Value, direction, order);

            var model = new IssuesViewModel()
            {
                Issues = _mapper.Map<IList<IssueViewModel>>(issues),
                AsigneeId = userId.Value,
                Direction = direction,
                Order = order,
            };

            return View(model);
        }

        [HttpGet("ViewProjectIssues")]
        public async Task<IActionResult> ViewProjectIssues(int? projectId,
            SortDirection direction = SortDirection.Ascending,
            string? order = null)
        {
            if (projectId is null)
            {
                return BadRequest();
            }

            var issues = await _issueService.GetProjectIssuesAsync(projectId.Value, direction, order);

            var model = new IssuesViewModel()
            {
                Issues = _mapper.Map<IList<IssueViewModel>>(issues),
                ProjectId = projectId.Value,
                Direction = direction,
                Order = order,
            };

            return View(model);
        }
    }
}
