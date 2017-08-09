using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using Microsoft.AspNetCore.Authorization;
using WebApplication2.Services;
using WebApplication2.Models.UserEntities;

namespace WebApplication2.Controllers
{
    public class LicenseKeysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserEntityLoader _userEntityServices;

        public LicenseKeysController(ApplicationDbContext context, IUserEntityLoader userEntityServices)
        {
            _context = context;
            _userEntityServices = userEntityServices;
        }


        // GET: UserLicenses
        public async Task<IActionResult> NormalUserIndex()
        {
            NormalUser user = (await _userEntityServices.GetCurrentUserEntity(HttpContext.User)) as NormalUser;
            if (user == null)
                return NotFound();
            List<LicenseKey> ownLicenseKeys = await _context.LicenseKey.Where(s => s.User == user).ToListAsync();
            return View(ownLicenseKeys);
        }

        // GET: Softwares/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licenseKey = await _context.LicenseKey.Include(l => l.User).Include(l => l.Software)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (licenseKey == null)
            {
                return NotFound();
            }

            return View(licenseKey);
        }


        private bool LicenseKeyExists(string id)
        {
            return _context.LicenseKey.Any(e => e.Id == id);
        }
    }
}
