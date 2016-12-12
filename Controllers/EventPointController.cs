using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EarnIt.Data;
using EarnIt.Models;
using EarnIt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EarnIt.Controllers
{
    [Produces("application/json")]
    public class EventPointController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext context;
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public EventPointController(UserManager<ApplicationUser> UserManager, ApplicationDbContext ctx)
        {
            _userManager = UserManager;
            context = ctx;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromRoute] int id)
        {
            List<EventPoint> EventPoints = new List<EventPoint>();

            EventPoints = await context.EventPoint.Where(ep => ep.EventId == id).ToListAsync();

            return Json( new { success = "Event Points found!", EventPoints});

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            
        }
    }
}