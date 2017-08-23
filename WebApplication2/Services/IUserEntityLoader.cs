using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.UserEntities;

namespace WebApplication2.Services
{
    public interface IUserEntityLoader
    {
        Task<ApplicationUser> GetCurrentApplicationUser(ClaimsPrincipal user);
        Task<IUserEntity> GetCurrentUserEntity(ClaimsPrincipal user);
        Task<UserEntityType> GetCurrentUserEntityType(ClaimsPrincipal user);
        Task<ApplicationUser> GetApplicationUserByUserEntity(IUserEntity entity);
    }

    public class UserEntityServices : IUserEntityLoader
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        private ApplicationUser _CurrentUser;

        public UserEntityServices([FromServices]UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<ApplicationUser> GetCurrentApplicationUser(ClaimsPrincipal user)
        {
            if (_CurrentUser == null)
            {
                _CurrentUser = await _userManager.GetUserAsync(user);
            }
            return _CurrentUser;
        }

        public async Task<IUserEntity> GetCurrentUserEntity(ClaimsPrincipal user)
        {
            ApplicationUser appUser = await GetCurrentApplicationUser(user);
            if (appUser == null)
                return null;

            UserEntityType type = appUser.EntityType;
            switch (type)
            {
                case UserEntityType.NormalUser:
                    return _context.NormalUser
                        .Include(n => n.LicenseKeys)
                        .Include(n => n.Plugins)
                            .ThenInclude(p => p.Plugin)
                        .Include(n => n.Plugins)
                            .ThenInclude(p => p.Software)
                        .SingleOrDefault(e => e.Id == appUser.EntityId);
                case UserEntityType.Company:
                    return _context.CompanyUser
                        .Include(c => c.Editors)
                        .Include(c => c.Plugins)
                        .Include(c => c.Softwares)
                        .SingleOrDefault(e => e.Id == appUser.EntityId);
                case UserEntityType.Editor:
                    return _context.EditorUser
                        .Include(e => e.Company)
                        .SingleOrDefault(e => e.Id == appUser.EntityId);
            }
            return null;

        }

        public async Task<UserEntityType> GetCurrentUserEntityType(ClaimsPrincipal user)
        {
            ApplicationUser appUser = await GetCurrentApplicationUser(user);
            if (appUser == null)
                return (UserEntityType)10;

            UserEntityType type = appUser.EntityType;
            return type;
        }

        public async Task<ApplicationUser> GetApplicationUserByUserEntity(IUserEntity entity)
        {
            return await _context.Users.Where(u => u.EntityId == entity.Id).SingleOrDefaultAsync();
        }
    }
}
