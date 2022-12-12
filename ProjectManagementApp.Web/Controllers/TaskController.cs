using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApp.Domain.ServiceInterfaces;

namespace ProjectManagementApp.Web.Controllers
{
    public class TaskController : Controller
    {

        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TaskController(ITaskService taskService, IMapper mapper)
        {
            this._taskService = taskService;
            this._mapper = mapper;
        }
    }
}
