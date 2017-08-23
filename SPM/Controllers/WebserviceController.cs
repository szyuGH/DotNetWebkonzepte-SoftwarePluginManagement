using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SPM.Data;
using SPM.Models;
using SPM.Models.UserEntities;
using SPM.Services;

namespace SPM.Controllers
{
    [Route("api/[controller]")]
    public class WebserviceController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserEntityLoader _userEntityServices;

        public WebserviceController(ApplicationDbContext context, IUserEntityLoader userEntityServices)
        {
            _context = context;
            _userEntityServices = userEntityServices;
        }

        [HttpGet]
        public string GetAll()
        {
            return "HELLO";
        }

        [HttpGet("{userId}/{softwareId}")]
        public async Task<string> GetPlugins(string userId, string softwareId)
        {
            try
            {
                NormalUser normalUser = await _context.NormalUser.FirstOrDefaultAsync(nu => nu.Id == userId);
                if (normalUser == null)
                {
                    return null;
                }

                Software software = await _context.Software.FirstOrDefaultAsync(s => s.Id == softwareId);
                if (software == null)
                {
                    return null;
                }

                ICollection<UsersPlugins> userPlugins = await _context.UserPlugin
                    .Include(p => p.Plugin)
                        .ThenInclude(p => p.Company)
                    .Include(p => p.Software)
                    .Include(p => p.User)
                    .Where(p => p.User == normalUser && p.Software == software)
                    .ToListAsync();

                List<Plugin> plugins = userPlugins.Select(p => p.Plugin).ToList();
                List<Object> returnObjects = new List<object>();
                foreach (Plugin plugin in plugins){
                    returnObjects.Add(new {
                        Id = plugin.Id,
                        Name = plugin.Name,
                        Description = plugin.Description,
                        Company = plugin.Company.Name,
                        Data = plugin.Data
                    });
                }
                string json = JsonConvert.SerializeObject(returnObjects);
                return json;
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
