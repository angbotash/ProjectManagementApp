using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManagementApp.Domain.Entities;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Web.ViewModels;

namespace ProjectManagementApp.Web.Controllers
{
    public class IssueController : Controller
    {
        private readonly IIssueService _issueService;
        private readonly IEmployeeService _employeeService;
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public IssueController(IIssueService issueService, IEmployeeService employeeService, IProjectService projectService, IMapper mapper)
        {
            this._issueService = issueService;
            this._employeeService = employeeService;
            this._projectService = projectService;
            this._mapper = mapper;
        }

        [HttpGet("Create")]
        public ActionResult Create(int? projectId)
        {
            if (projectId is null)
            {
                return BadRequest();
            }

            if (this._projectService.Get((int)projectId) == null)
            {
                return NotFound();
            }

            var allEmployees = this._employeeService.GetAll();
            var employees = new List<EmployeeViewModel>();

            foreach (var empl in allEmployees)
            {
                var tempEmployee = this._mapper.Map<Employee, EmployeeViewModel>(empl);

                employees.Add(tempEmployee);
            }

            var selectList = employees.Select(x => new SelectListItem(x.Email, x.Id.ToString())).ToList();
            var model = new CreateIssueViewModel
            {
                Employees = selectList,
                ProjectId = (int)projectId,
                Statuses = new List<SelectListItem>()
                {
                    new SelectListItem(IssueStatus.ToDo.ToString(), IssueStatus.ToDo.ToString()),
                    new SelectListItem(IssueStatus.InProgress.ToString(), IssueStatus.InProgress.ToString()),
                    new SelectListItem(IssueStatus.Done.ToString(), IssueStatus.Done.ToString())
                }
            };

            return View(model);
        }

        [HttpPost("Create")]
        public async Task<ActionResult> Create(CreateIssueViewModel model)
        {
            if (ModelState.IsValid)
            {
                var issue = this._mapper.Map<CreateIssueViewModel, Issue>(model);

                await this._issueService.Create(issue);

                return RedirectToAction("ViewProject", "Project", new { issue.ProjectId });
            }

            return View(model);
        }

        [HttpGet("ViewIssue")]
        public IActionResult ViewIssue(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var issue = this._issueService.Get((int)id);

            if (issue is null)
            {
                return NotFound();
            }

            var result = this._mapper.Map<Issue, IssueViewModel>(issue);

            return View(result);
        }

        //[HttpGet("ViewAllIssues")]
        //public IActionResult ViewAllIssues(int? employeeId, string sortOrder, string issueKind = "")
        //{
        //    if (employeeId is null)
        //    {
        //        return BadRequest();
        //    }

        //    var employee = this._employeeService.Get((int) employeeId);

        //    if (employee is null)
        //    {
        //        return NotFound();
        //    }

        //    var tempEmployee = this._mapper.Map<Employee, EmployeeViewModel>(employee);

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
        public IActionResult ViewAssignedIssues(int? employeeId)
        {
            if (employeeId is null)
            {
                return BadRequest();
            }

            var employee = this._employeeService.Get((int)employeeId);

            if (employee is null)
            {
                return NotFound();
            }

            var tempEmployee = this._mapper.Map<Employee, EmployeeViewModel>(employee);
            var issues = tempEmployee.AssignedIssues;

            return View(issues);
        }

        [HttpGet("ViewReportedIssues")]
        public IActionResult ViewReportedIssues(int? employeeId)
        {
            if (employeeId is null)
            {
                return BadRequest();
            }

            var employee = this._employeeService.Get((int)employeeId);

            if (employee is null)
            {
                return NotFound();
            }

            var tempEmployee = this._mapper.Map<Employee, EmployeeViewModel>(employee);
            var issues = tempEmployee.ReportedIssues;

            return View(issues);
        }

        [HttpGet("ViewProjectIssues")]
        public IActionResult ViewProjectIssues(int? projectId)
        {
            if (projectId is null)
            {
                return BadRequest();
            }

            var project = this._projectService.Get((int)projectId);

            if (project is null)
            {
                return NotFound();
            }

            var tempProject = this._mapper.Map<Project, ProjectViewModel>(project);
            var issues = tempProject.Issues;

            return View(issues);
        }

        [HttpGet("Edit")]
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var issue = this._issueService.Get((int)id);
            var allEmployees = this._employeeService.GetAll();

            if (issue is null)
            {
                return NotFound();
            }

            var model = this._mapper.Map<Issue, EditIssueViewModel>(issue);
            var employees = new List<EmployeeViewModel>();

            foreach (var empl in allEmployees)
            {
                var tempEmployee = this._mapper.Map<Employee, EmployeeViewModel>(empl);

                employees.Add(tempEmployee);
            }

            var selectList = employees.Select(x => new SelectListItem(x.Email, x.Id.ToString())).ToList();
            model.Employees = selectList;
            model.Statuses = new List<SelectListItem>()
            {
                new SelectListItem(IssueStatus.ToDo.ToString(), IssueStatus.ToDo.ToString()),
                new SelectListItem(IssueStatus.InProgress.ToString(), IssueStatus.InProgress.ToString()),
                new SelectListItem(IssueStatus.Done.ToString(), IssueStatus.Done.ToString())
            };

            return View(model);
        }

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(EditIssueViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updatedIssue = this._mapper.Map<EditIssueViewModel, Issue>(model);

                await this._issueService.Edit(updatedIssue);
            }

            return RedirectToAction("ViewIssue", new { model.Id });
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var issue = this._issueService.Get((int)id);

            if (issue is null)
            {
                return NotFound();
            }

            await this._issueService.Delete((int)id);

            return RedirectToAction("GetAllProjects", "Project");
        }
    }
}
