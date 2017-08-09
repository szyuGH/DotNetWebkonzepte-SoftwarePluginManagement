using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Services;
using WebApplication2.Models.UserEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System.IO;

namespace WebApplication2.Controllers
{
    public class PluginsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserEntityLoader _userEntityServices;

        public PluginsController(ApplicationDbContext context, IUserEntityLoader userEntityServices)
        {
            _context = context;
            _userEntityServices = userEntityServices;
        }

        // GET: Plugins
        public async Task<IActionResult> Index()
        {
            return View(await _context.Plugin.ToListAsync());
        }

        // GET: OwnPlugins
        public async Task<IActionResult> Own()
        {
            IUserEntity entity = (await _userEntityServices.GetCurrentUserEntity(HttpContext.User));
            if (entity is NormalUser)
                Forbid();

            CompanyUser company = null;
            if (entity is CompanyUser)
            {
                company = entity as CompanyUser;
                if (company == null)
                    return NotFound();
            }
            else if (entity is EditorUser)
            {
                await _context.CompanyUser.ToListAsync();
                company = (entity as EditorUser).Company;
            }
            List<Plugin> ownPlugins = await _context.Plugin.Where(p => p.Company == company).ToListAsync();
            return View(ownPlugins);
        }

        // GET: ExternalPluginsForOwnSoftware
        public async Task<IActionResult> ExternalPluginsForOwnSoftware()
        {
            IUserEntity entity = (await _userEntityServices.GetCurrentUserEntity(HttpContext.User));
            if (entity is NormalUser)
                Forbid();

            CompanyUser company = await GetCompanyUser();
            List<Plugin> plugins = await _context.Plugin.Where(p => p.Company != company && p.RelatedSoftware.Company == company).ToListAsync();
            return View(plugins);
        }

        private async Task<CompanyUser> GetCompanyUser()
        {
            IUserEntity entity = (await _userEntityServices.GetCurrentUserEntity(HttpContext.User));
            if (entity is NormalUser)
                Forbid();

            CompanyUser company = null;
            if (entity is CompanyUser)
            {
                company = entity as CompanyUser;
                if (company == null)
                    return null;
            }
            else if (entity is EditorUser)
            {
                await _context.CompanyUser.ToListAsync();
                company = (entity as EditorUser).Company;
            }
            return company;
        }


        // GET: Plugins/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plugin = await _context.Plugin
                .SingleOrDefaultAsync(m => m.Id == id);
            if (plugin == null)
            {
                return NotFound();
            }

            return View(plugin);
        }

        // GET: Plugins/Create
        public async Task<IActionResult> Create(string softwareId)
        {
            Software software = await _context.Software.Where(s => s.Id == softwareId).SingleOrDefaultAsync();
            HttpContext.Session.SetString("SoftwareId", software.Id);
            ViewData["Software"] = software;
            return View();
        }

        // POST: Plugins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Plugin plugin, ICollection<IFormFile> files)
        {
            if (ModelState.IsValid && files.Count == 1)
            { 

                plugin.Company = await GetCompanyUser();
                string sid = HttpContext.Session.GetString("SoftwareId");
                plugin.RelatedSoftware = (await _context.Software.Where(s => s.Id == sid).SingleOrDefaultAsync());

                var file = files.First();
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        string contentAsString = reader.ReadToEnd();
                        plugin.Data = new byte[contentAsString.Length * sizeof(char)];
                        System.Buffer.BlockCopy(contentAsString.ToCharArray(), 0, plugin.Data, 0, plugin.Data.Length);
                    }

                    _context.Add(plugin);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "Softwares", sid);
                }
            }
            return View(plugin);
        }

        

        // GET: Plugins/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plugin = await _context.Plugin.SingleOrDefaultAsync(m => m.Id == id);
            if (plugin == null)
            {
                return NotFound();
            }
            return View(plugin);
        }

        // POST: Plugins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description")] Plugin plugin)
        {
            if (id != plugin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plugin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PluginExists(plugin.Id))
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
            return View(plugin);
        }

        // GET: Plugins/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plugin = await _context.Plugin
                .SingleOrDefaultAsync(m => m.Id == id);
            if (plugin == null)
            {
                return NotFound();
            }

            return View(plugin);
        }

        // POST: Plugins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var plugin = await _context.Plugin.SingleOrDefaultAsync(m => m.Id == id);
            _context.Plugin.Remove(plugin);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }




        private bool PluginExists(string id)
        {
            return _context.Plugin.Any(e => e.Id == id);
        }
    }
}
