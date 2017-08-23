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
            return View(await _context.Plugin.Include(p => p.RelatedSoftware).Include(p => p.Company).ToListAsync());
        }

        // GET: OwnPlugins
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
            }
            else if (entity is EditorUser)
            {
                await _context.CompanyUser.ToListAsync();
                company = (entity as EditorUser).Company;
            }
            List<Plugin> ownPlugins = await _context.Plugin.Include(p => p.RelatedSoftware).Include(p => p.Company).Where(p => p.Company == company).ToListAsync();
            return View(ownPlugins);
        }

        // GET: ExternalPluginsForOwnSoftware
        public async Task<IActionResult> ExternalPluginsForOwnSoftware()
        {
            IUserEntity entity = (await _userEntityServices.GetCurrentUserEntity(HttpContext.User));
            if (entity is NormalUser)
                return Forbid();

            CompanyUser company = await GetCompanyUser();
            List<Plugin> plugins = await _context.Plugin.Include(p => p.RelatedSoftware).Include(p => p.Company).Where(p => p.Company != company && p.RelatedSoftware.Company == company).ToListAsync();
            return View(plugins);
        }

        private async Task<CompanyUser> GetCompanyUser()
        {
            IUserEntity entity = (await _userEntityServices.GetCurrentUserEntity(HttpContext.User));
            if (entity is NormalUser)
                return null;

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

        public async Task<IActionResult> UserPluginsIndex()
        {
            NormalUser user = (await _userEntityServices.GetCurrentUserEntity(HttpContext.User)) as NormalUser;
            if (user == null)
                return NotFound();

            ICollection<UsersPlugins> userPlugins = await _context.UserPlugin
                    .Include(p => p.Plugin)
                        .ThenInclude(p => p.Company)
                    .Include(p => p.Software)
                    .Include(p => p.User)
                    .Where(p => p.User == user)
                    .ToListAsync();
            List<Plugin> plugins = userPlugins.Select(p => p.Plugin).ToList();

            return View(plugins);
        }


        // GET: Plugins/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plugin = await _context.Plugin
                .Include(p => p.Company)
                .Include(p => p.RelatedSoftware)
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
            string sid = HttpContext.Session.GetString("SoftwareId");
            if (ModelState.IsValid && files != null && files.Count == 1)
            {

                plugin.Company = await GetCompanyUser();
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
                    return RedirectToAction("Details", "Softwares", new { id = sid });
                }
            }

            Software software = await _context.Software.Where(s => s.Id == sid).SingleOrDefaultAsync();
            HttpContext.Session.SetString("SoftwareId", software.Id);
            ViewData["Software"] = software;
            if (files == null || files.Count == 0)
                ModelState.AddModelError("data", "You need to select a plugin file.");
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description")] Plugin plugin, ICollection<IFormFile> files)
        {
            if (id != plugin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (files != null && files.Count == 1)
                    {
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
                        }
                    }
                    else
                    {
                        plugin = await _context.Plugin
                            .Include(p => p.RelatedSoftware)
                            .Include(p => p.Company)
                            .SingleOrDefaultAsync(p => p.Id == plugin.Id);
                    }

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


        //public IActionResult Subscribe()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> Subscribe(string id, string returnview)
        {
            var plugin = await _context.Plugin.Include(p => p.RelatedSoftware).SingleOrDefaultAsync(m => m.Id == id);
            try
            {
                var entity = await _userEntityServices.GetCurrentUserEntity(HttpContext.User);
                if (!(entity is NormalUser))
                    return Forbid();
                NormalUser user = entity as NormalUser;
                Software software = plugin.RelatedSoftware;

                UsersPlugins userPlugin = new UsersPlugins
                {
                    Id = Guid.NewGuid().ToString(),
                    User = user,
                    Plugin = plugin,
                    Software = software
                };


                _context.Add(userPlugin);
                //_context.Update(user);
                await _context.SaveChangesAsync();
                //return RedirectToAction("NormalUserIndex");
                user.Plugins.Add(userPlugin);
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
            return RedirectToAction(returnview, new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> Unsubscribe(string id, string returnview)
        {
            var entity = await _userEntityServices.GetCurrentUserEntity(HttpContext.User);
            if (!(entity is NormalUser))
                return Forbid();
            NormalUser user = entity as NormalUser;

            try
            {
                var userPlugin = await _context.UserPlugin
                    .Include(up => up.User)
                    .Include(up => up.Plugin)
                    .Include(up => up.Software)
                    .SingleOrDefaultAsync(up => up.User == user && up.Plugin.Id == id);

                _context.Remove(userPlugin);
                user.Plugins.Remove(userPlugin);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PluginExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(returnview, new { id = id });
        }







        private bool PluginExists(string id)
        {
            return _context.Plugin.Any(e => e.Id == id);
        }
    }
}
