using Microsoft.AspNetCore.Mvc;
using ProjectManagementApp.Domain.ServiceInterfaces;
using ProjectManagementApp.Web.ViewModels;
using AutoMapper;
using ProjectManagementApp.Domain.Entities;

namespace ProjectManagementApp.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            this._employeeService = employeeService;
            this._mapper = mapper;
        }

        [HttpGet("CreateEmployee")]
        public IActionResult CreateEmployee()
        {
            return View();
        }

        [HttpPost("CreateEmployee")]
        public IActionResult CreateEmployee(CreateEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<CreateEmployeeViewModel, Employee>(model);
                _employeeService.Create(employee);
            }

            return View(model);
        }
    }
}
