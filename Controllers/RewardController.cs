using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using EarnIt.Data;
using EarnIt.Models;
using EarnIt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EarnIt.Controllers
{
    /**
     * Class: Reward
     * Purpose: Used to create, update and read rewards from the database
     * Methods:
     *   Task<IActionResult> GetSingle(int id) - Returns the reward object of the id provided
     *       id - The id of the rewards object in the database
     *   Task<IActionResult> Create(Reward reward, int id) - Creates a reward for the event id provided and adds the id to the event
     *       reward - The reward object passed in
     *       id - The id of the event that needs the reward id added
     **/
    [Produces("application/json")]
    public class RewardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext context;
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public RewardController(UserManager<ApplicationUser> UserManager, ApplicationDbContext ctx)
        {
            _userManager = UserManager;
            context = ctx;
        }
        /**
         * Purpose: Gets a single reward from the reward id passed in
         * Arguments:
         *      id - The reward id that is passed in to get the information
         * Return:
         *      Returns json object with the reward data requested
         *      If no reward is found then returns a bad request
         */
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSingle([FromRoute] int id)
        {
            try
            {
                ApplicationUser user = await GetCurrentUserAsync();
                Event curEvent = await context.Event.Where(e => e.EventId == id).SingleAsync();
                Reward reward = await context.Reward.Where(r => r.RewardId == curEvent.RewardId).SingleAsync();

                RewardViewModel model = new RewardViewModel();

                model.Name = reward.Name;
                model.Description = reward.Description;
                model.ImageURL = reward.ImageURL;
                model.PointsNeeded = reward.PointsNeeded;
                model.RewardId = reward.RewardId;

                return Json(new {reward = model});
            }
            catch
            {
                return BadRequest(new { error = $"A reward could not be found for the id #{id}" });
            }
        }

        /**
         * Purpose: Creates a new reward for the passed event if one has not been created
         * Arguments:
         *      reward - json object of the passed in reward
         *      id - The id of the event attached to the reward
         * Return:
         *      If created it returns the event with the reward attached
         *      If not created it returns a bad request with the appropriate json object
         */
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Reward reward,[FromRoute] int id)
        {
            if(id < 1)
            { 
                 return BadRequest(new { error = new { message = "Event id is required to save this reward", reward } });
            } 
            if(ModelState.IsValid)
            {
                try
                {
                    Event passedEvent = await context.Event.Where(e => e.EventId == id).SingleAsync();
                    try
                    {
                        passedEvent = await context.Event.Where(e => e.EventId == id && e.RewardId == null).SingleAsync();
                        context.Add(reward);
                        await context.SaveChangesAsync();
                        context.Entry(reward).GetDatabaseValues();

                        passedEvent.RewardId = reward.RewardId;
                        context.Update(passedEvent);
                        await context.SaveChangesAsync();
                        context.Entry(passedEvent).GetDatabaseValues();

                        return Json(new {success = "Reward created!", data = passedEvent} );
                    }
                    catch
                    {
                        reward = await context.Reward.Where(r => r.RewardId == passedEvent.RewardId).SingleAsync();
                        return BadRequest(new { error = $"The event with id #{id} is already connected to a reward", data = passedEvent });
                    }
                    }
                catch
                {
                    return BadRequest(new {error = $"No event with id #{id} can be found"});
                }
            }
            return BadRequest(ModelState);
        }
    }
}