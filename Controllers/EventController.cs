using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EarnIt.Data;
using EarnIt.Models;
using EarnIt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EarnIt.Controllers
{
    [Produces("application/json")]
    // [Route("api/[controller]/[action]")]
    public class EventController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext context;
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public EventController(UserManager<ApplicationUser> UserManager, ApplicationDbContext ctx)
        {
            _userManager = UserManager;
            context = ctx;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> All([FromRoute] int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            var eventsJson = new EventListViewModel(context, user);

            eventsJson.Events = await context.Event.Where(e => e.ChildId == id).OrderBy(e => e.Name).ToListAsync();
            return Json(eventsJson);
        }
        
    }
}