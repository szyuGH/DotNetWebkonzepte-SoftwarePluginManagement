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
    [Authorize]
    public class SoftwaresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserEntityLoader _userEntityServices;

        public SoftwaresController(ApplicationDbContext context, IUserEntityLoader userEntityServices)
        {
            _context = context;
            _userEntityServices = userEntityServices;
        }

        // GET: Softwares
        public async Task<IActionResult> Index()
        {
            return View(await _context.Software.Include(s => s.Company).Include(s => s.Plugins).ToListAsync());
        }

        // GET: OwnSoftwares
        public async Task<IActionResult> Own()
        {
            IUserEntity entity = (await _userEntityServices.GetCurrentUserEntity(HttpContext.User));
            if (entity is NormalUser)
                return Forbid();

            CompanyUser company = null;
            if (entity is CompanyUser)
            {
                company = entity as CompanyUser;
                if (company == null)
                    return NotFound();
            } else if (entity is EditorUser)
            {
                await _context.CompanyUser.ToListAsync();
                company = (entity as EditorUser).Company;
            }
            List<Software> ownSoftware = await _context.Software.Include(s => s.Company).Include(s => s.Plugins).Where(s => s.Company == company).ToListAsync();
            return View(ownSoftware);
        }
        

        // GET: Softwares/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await _context.Software
                .Include(s => s.LicenseKeys).ThenInclude(lk => lk.Software)
                .Include(s => s.LicenseKeys).ThenInclude(l => l.User)
                .Include(s => s.Plugins).ThenInclude(p => p.Company)
                .SingleOrDefaultAsync(m => m.Id == id);
            //await _context.NormalUser.LoadAsync();

            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }

        // GET: Softwares/Create
        public async Task<IActionResult> Create()
        {
            IUserEntity entity = (await _userEntityServices.GetCurrentUserEntity(HttpContext.User));
            if (entity is NormalUser)
                 return Forbid();
            return View();
        }

        // POST: Softwares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Software software)
        {
            if (ModelState.IsValid)
            {
                IUserEntity entity = (await _userEntityServices.GetCurrentUserEntity(HttpContext.User));
                if (entity is NormalUser)
                    return Forbid();
                else if (entity is CompanyUser)
                    software.Company = entity as CompanyUser;
                else if (entity is EditorUser)
                {
                    _context.EditorUser.Include(e => e.Company).Load();
                    software.Company = (entity as EditorUser).Company;
                }

                
                _context.Add(software);
                await _context.SaveChangesAsync();
                return RedirectToAction("Own");
            }
            return View(software);
        }


        public IActionResult CreatePlugin(string id)
        {
            return RedirectToAction("Create", "Plugins", new { softwareId = id});
        }

        public IActionResult CreateLicenseKey(string id)
        {
            return RedirectToAction("Create", "LicenseKeys", new { softwareId = id });
        }


        // GET: Softwares/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await _context.Software.SingleOrDefaultAsync(m => m.Id == id);
            if (software == null)
            {
                return NotFound();
            }
            return View(software);
        }

        // POST: Softwares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description")] Software software)
        {
            if (id != software.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(software);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoftwareExists(software.Id))
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
            return View(software);
        }

        // GET: Softwares/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await _context.Software
                .SingleOrDefaultAsync(m => m.Id == id);
            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }

        // POST: Softwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var software = await _context.Software.SingleOrDefaultAsync(m => m.Id == id);
            _context.Software.Remove(software);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool SoftwareExists(string id)
        {
            return _context.Software.Any(e => e.Id == id);
        }
    }
}
