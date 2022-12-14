using Microsoft.AspNetCore.Mvc;
using ProjectManagementApp.Web.Models;
using System.Diagnostics;
using ProjectManagementApp.Domain.ServiceInterfaces;

namespace ProjectManagementApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IUserService userService, ILogger<HomeController> logger)
        {
            this._userService = userService;
            this._logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = await _userService.GetCurrentUserId(User);

            if (currentUserId is null)
            {
                return BadRequest();
            }

            return View((int)currentUserId);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}