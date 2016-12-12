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
    /**
     * Class: Event
     * Purpose: Used to create an event, get all events or disable an event for the authorized user
     * Methods:
     *   Task<IActionResult> All(int id) - Gets all the events for the passed in child id if authorized
     *       id - Child id needed to get all events
     *   Task<IActionResult> Create(event model) - Creates an event for the child id
     *       model - A json object with the information to add the event to the database
     **/
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
        /**
         * Purpose: Gets all of the events for the child id passed in that the user is authorized to get
         * Arguments:
         *      id - Child id for all events
         * Return:
         *      If successful returns object with all events otherwise custom bad request object
         */
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> All([FromRoute] int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            IEnumerable<Event> events = await context.Event.Where(e => e.ChildId == id).OrderBy(e => e.Name).ToListAsync();

            if(events.Any())
            {
                EventListViewModel viewEvents = new EventListViewModel();
                try
                { 
                    Child child = await context.Child.Where(c => c.UserId == user.Id && events.Any(e => e.ChildId == c.ChildId)).FirstAsync();
                    
                    foreach (var sglEvent in events)
                    {
                        EventViewModel viewEvent = new EventViewModel();
                            viewEvent.EventId = sglEvent.EventId;
                            viewEvent.RewardId = sglEvent.RewardId;
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
                catch
                {
                    return BadRequest( new { error = $"The child id #{id} is not authorized for the logged in user"});
                }
            }

            try
            {
                Child child = await context.Child.Where(c => c.ChildId == id && c.UserId == user.Id).SingleAsync();
                return BadRequest(new {Error = $"Unable to find any events for the child id #{id} for this user"});
            }
            catch 
            {
                return BadRequest(new {Error= $"There is no child with the id #{id}. Did you forget to pass an id? ex: event/all/{{id}}"}); 
            }
        }

        /**
         * Purpose: Creates a new event for the child id passed in the model if authorized
         * Arguments:
         *      model - An object containing what is needed to post the event
         * Return:
         *      If successful then an object with the created event otherwise a custom bad request object
         */
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Event model)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            
            if(ModelState.IsValid)
            {
                try
                {
                    Child child = await context.Child.Where(c => c.ChildId == model.ChildId && c.UserId == user.Id).SingleAsync();

                    context.Add(model);
                    await context.SaveChangesAsync();
                    context.Entry(model).GetDatabaseValues();

                    return Json(new { success = "New Event saved!", newEvent = model});
                }
                catch
                {
                    return BadRequest( new { error = $"The current user is not authorized to add events for the child with id #{model.ChildId}" });
                }
            }
            return Json(new {error = "unable to save this event"});
        }
    }
}