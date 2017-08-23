using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Services;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserEntityLoader _userEntityServices;

        public HomeController(ApplicationDbContext context, IUserEntityLoader userEntityServices)
        {
            _context = context;
            _userEntityServices = userEntityServices;
        }

        public IActionResult Index()
        {
            HomeInfo info = new HomeInfo
            {
                CompanyCount = _context.CompanyUser.Count(),
                UserCount = _context.NormalUser.Count(),
                SoftwareCount = _context.Software.Count(),
                PluginCount = _context.Plugin.Count()
            };
            return View(info);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
