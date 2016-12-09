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
            IEnumerable<Event> events = await context.Event.Where(e => e.ChildId == id).OrderBy(e => e.Name).ToListAsync();

            if(events.Any())
            {
                EventListViewModel viewEvents = new EventListViewModel();
                EventViewModel viewEvent = new EventViewModel();

                foreach (var sglEvent in events)
                {
                    viewEvent.Name = sglEvent.Name;
                    viewEvent.Description = sglEvent.Description;
                    viewEvent.Type = sglEvent.Type;
                    viewEvent.ImageURL = sglEvent.ImageURL;
                    viewEvent.AutoRefresh = sglEvent.AutoRefresh;
                    viewEvent.IsActive = sglEvent.IsActive;
                    viewEvent.Frequency = sglEvent.Frequency;
                
                    viewEvents.Events.Add(viewEvent);
                }

                return Json(new {viewEvents.Events});
            }
                return Json(new {Error= "Unable to find events for the current id"});
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] EventCreateViewModel newEvent)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            
            if(ModelState.IsValid)
            {
                Event model = new Event();
                    model.Name = newEvent.Name;
                    model.Description = newEvent.Description;
                    model.ImageURL = newEvent.ImageURL;
                    model.Type = newEvent.Type;
                    model.AutoRefresh = newEvent.AutoRefresh;
                    model.IsActive = newEvent.IsActive;
                    model.Frequency = newEvent.Frequency;
                    model.ChildId = newEvent.ChildId;

                context.Add(model);
                await context.SaveChangesAsync();
                return Json(new { saved = "New Event saved!"});
            }

            return Json(new {error = "unable to save this event"});
        }
    }
}