using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class PluginsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PluginsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Plugins
        public async Task<IActionResult> Index()
        {
            return View(await _context.Plugin.ToListAsync());
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Plugins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Plugin plugin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plugin);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
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
