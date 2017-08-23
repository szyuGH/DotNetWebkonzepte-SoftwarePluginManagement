using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SPM.Data;
using SPM.Models.UserEntities;
using SPM.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace SPM.Controllers
{
    [Authorize("CompanyUser")]
    public class EditorUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditorUsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: EditorUsers
        public async Task<IActionResult> Index()
        {
            ApplicationUser appUser = await _userManager.GetUserAsync(HttpContext.User);
            CompanyUser companyUser = await _context.CompanyUser.FirstOrDefaultAsync(d => d.Id == appUser.EntityId);
            IEnumerable<EditorUser> editors = _context.EditorUser.Where(e => e.Company == companyUser);

            List<Tuple<ApplicationUser, EditorUser>> editorUsers = new List<Tuple<ApplicationUser, EditorUser>>();
            foreach (EditorUser editor in editors)
            {
                ApplicationUser editorUser = await _context.Users.FirstOrDefaultAsync(u => u.EntityType == UserEntityType.Editor && editor.Id == u.EntityId);
                if (editorUser != null)
                    editorUsers.Add(new Tuple<ApplicationUser, EditorUser>(editorUser, editor));
            }

            return View(editorUsers);
        }
        

        // GET: EditorUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EditorUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EditorUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = await _userManager.GetUserAsync(HttpContext.User);
                CompanyUser companyUser = await _context.CompanyUser.FirstOrDefaultAsync(d => d.Id == appUser.EntityId);

                EditorUser editor = new EditorUser { Company = companyUser, FirstName = model.FirstName, LastName = model.LastName };
                _context.Add<EditorUser>(editor);
                await _context.SaveChangesAsync();
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email, EntityType = Models.UserEntities.UserEntityType.Editor, EntityId = editor.Id };
                var result = await _userManager.CreateAsync(user, model.Password);
                AddErrors(result);

                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: EditorUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser appUser = await _userManager.FindByIdAsync(id);
            var editorUser = await _context.EditorUser.SingleOrDefaultAsync(m => m.Id == appUser.EntityId);
            if (editorUser == null)
            {
                return NotFound();
            }

            EditEditorUserViewModel model = new EditEditorUserViewModel
            {
                AppUserId = appUser.Id,
                Username = appUser.UserName,
                Email = appUser.Email,
                FirstName = editorUser.FirstName,
                LastName = editorUser.LastName
            };
            return View(model);
        }

        // POST: EditorUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditEditorUserViewModel model)
        {
            if (id != model.AppUserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ApplicationUser appUser = await _userManager.FindByIdAsync(id);
                var editorUser = await _context.EditorUser.SingleOrDefaultAsync(m => m.Id == appUser.EntityId);
                if (editorUser == null)
                {
                    return NotFound();
                }

                appUser.UserName = model.Username;
                appUser.Email = model.Email;
                editorUser.FirstName = model.FirstName;
                editorUser.LastName = model.LastName;

                try
                {
                    await _userManager.UpdateAsync(appUser);
                    _context.Update(editorUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EditorUserExists(editorUser.Id))
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
            return View(model);
        }

        // GET: EditorUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ApplicationUser appUser = await _userManager.FindByIdAsync(id);
            var editorUser = await _context.EditorUser.SingleOrDefaultAsync(m => m.Id == appUser.EntityId);
            if (editorUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: EditorUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            ApplicationUser appUser = await _userManager.FindByIdAsync(id);
            var editorUser = await _context.EditorUser.SingleOrDefaultAsync(m => m.Id == appUser.EntityId);

            _context.EditorUser.Remove(editorUser);
            await _context.SaveChangesAsync();
            await _userManager.DeleteAsync(appUser);

            return RedirectToAction("Index");
        }

        private bool EditorUserExists(string id)
        {
            return _context.EditorUser.Any(e => e.Id == id);
        }



        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
