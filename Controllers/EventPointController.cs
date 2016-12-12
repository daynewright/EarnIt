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
     * Class: EventPoint
     * Purpose: Used to add an event point for the event id passed in
     * Methods:
     *   Task<IActionResult> All(int id) - Gets all the event points for the passed in event id if authorized
     *       id - Event id needed to get all event points
     *   Task<IActionResult> Create(EventPoint eventPoint) - Creates an event point for the event id
     *       eventPoint - A json object with the information to add the event point to the database
     **/
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

        /**
         * Purpose: Gets all the event points for the passed in event id
         * Arguments:
         *      id - The event id for the points needed
         * Return:
         *      If successful then an object with the event points for the id otherwise a custom bad request object
         */
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> All([FromRoute] int id)
        {
            List<EventPoint> EventPoints = new List<EventPoint>();

            EventPoints = await context.EventPoint.Where(ep => ep.EventId == id).ToListAsync();

            if(EventPoints.Any())
            {
                return Json( new { success = "Event Points found!", EventPoints});
            }

            return BadRequest(new { error = $"Unable to find any points for event #{id}" });
        }

       /**
         * Purpose: Creates a new event point for the event id passed in the model if authorized
         * Arguments:
         *      eventPoint - An object containing what is needed to post the event point
         * Return:
         *      If successful then an object with the created event point otherwise a custom bad request object
         */
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] EventPoint eventPoint)
        {
            if(ModelState.IsValid)
            {
                context.Add(eventPoint);
                await context.SaveChangesAsync();
                await context.Entry(eventPoint).GetDatabaseValuesAsync();
                return Json( new {success = "Event point returned!", eventPoint});
            }

            return BadRequest(new { error = "Unable to save the event point", eventPoint });
        }
    }
}