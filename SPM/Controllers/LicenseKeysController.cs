using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPM.Data;
using SPM.Models;
using Microsoft.AspNetCore.Authorization;
using SPM.Services;
using SPM.Models.UserEntities;
using Microsoft.AspNetCore.Http;

namespace SPM.Controllers
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
            List<LicenseKey> ownLicenseKeys = await _context.LicenseKey
                .Include(l => l.Software)
                .Where(s => s.User == user).ToListAsync();
            return View(ownLicenseKeys);
        }

        // GET: Softwares/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licenseKey = await _context.LicenseKey
                .Include(l => l.Software)
                .SingleOrDefaultAsync(m => m.Id == id);
            await _context.NormalUser.LoadAsync();

            if (licenseKey == null)
            {
                return NotFound();
            }

            return View(licenseKey);
        }

        // GET: LicenseKeys/Create
        public async Task<IActionResult> Create(string softwareId)
        {
            Software software = await _context.Software.Where(s => s.Id == softwareId).SingleOrDefaultAsync();
            HttpContext.Session.SetString("SoftwareId", software.Id);
            ViewData["Software"] = software;
            return View();
        }

        // POST: LicenseKeys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Key")] LicenseKey lk)
        {
            string sid = HttpContext.Session.GetString("SoftwareId");
            if (ModelState.IsValid)
            {
                lk.Software = (await _context.Software.Where(s => s.Id == sid).SingleOrDefaultAsync());

                _context.Add(lk);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Softwares", new { id = sid });
            }

            // bind software to next view
            Software software = await _context.Software.Where(s => s.Id == sid).SingleOrDefaultAsync();
            HttpContext.Session.SetString("SoftwareId", software.Id);
            ViewData["Software"] = software;
            return View(lk);
        }

        public IActionResult AddUserKey()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserKey([Bind("Id,Key")] LicenseKey lk)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var licenseKey = await _context.LicenseKey
                        .Include(l => l.Software)
                        .SingleOrDefaultAsync(l => l.Key.Equals(lk.Key));
                    if (licenseKey == null)
                    {
                        ModelState.AddModelError(string.Empty, "License Key not related to any software!");
                        return View(lk);
                    }
                    IUserEntity entity = (await _userEntityServices.GetCurrentUserEntity(HttpContext.User));
                    if (!(entity is NormalUser))
                        return Forbid();
                    licenseKey.User = entity as NormalUser;
                    _context.Update(licenseKey);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("NormalUserIndex");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LicenseKeyExists(lk.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(lk);
        }

        // GET: LicenseKeys/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licenseKey = await _context.LicenseKey
                .Include(l => l.Software)
                .SingleOrDefaultAsync(m => m.Id == id);
            await _context.NormalUser.LoadAsync();
            if (licenseKey == null)
            {
                return NotFound();
            }
            return View(licenseKey);
        }

        // POST: LicenseKeys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Key")] LicenseKey licenseKey)
        {
            if (id != licenseKey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(licenseKey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LicenseKeyExists(licenseKey.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(licenseKey);
        }

        // GET: LicenseKeys/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licenseKey = await _context.LicenseKey
                .Include(m => m.Software)
                .SingleOrDefaultAsync(m => m.Id == id);
            var lazyLoad = await _context.LicenseKey
                .Include(m => m.User)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (licenseKey == null)
            {
                return NotFound();
            }

            return View(licenseKey);
        }

        // POST: LicenseKeys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var licenseKey = await _context.LicenseKey
                .Include(l => l.Software)
                .SingleOrDefaultAsync(m => m.Id == id);
            _context.LicenseKey.Remove(licenseKey);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Softwares", new { id = licenseKey.Software.Id });
        }


        public async Task<IActionResult> Unsubscribe(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var licenseKey = await _context.LicenseKey
                .Include(l => l.Software)
                .Include(l => l.User)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (licenseKey == null)
            {
                return NotFound();
            }

            return View(licenseKey);
        }

        [HttpPost, ActionName("Unsubscribe")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnsubscribeConfirmed(string id)
        {
            var licenseKey = await _context.LicenseKey
                .Include(l => l.User)
                .Include(l => l.Software)
                .SingleOrDefaultAsync(m => m.Id == id);
            licenseKey.User = null;
            _context.Update(licenseKey);
            await _context.SaveChangesAsync();
            return RedirectToAction("NormalUserIndex");
        }


        private bool LicenseKeyExists(string id)
        {
            return _context.LicenseKey.Any(e => e.Id == id);
        }
    }

}
