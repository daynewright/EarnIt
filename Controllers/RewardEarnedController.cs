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
     * Class: RewardEarned
     * Purpose: Used to query for all rewards earned for a child and if a reward has been earned
     * Methods:
     *   Task<IActionResult> All(int id) - Gets all the event points for the passed in event id if authorized
     *       id - Child id needed to get all rewards earned
     *   Task<IActionResult> Create(RewardEarned rewardEarned) - Creates a reward earned record for the reward id sent
     *       rewardEarned - The reward earned object to add to the database
     **/
    [Produces("application/json")]
    public class RewardEarnedController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext context;
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public RewardEarnedController(UserManager<ApplicationUser> UserManager, ApplicationDbContext ctx)
        {
            _userManager = UserManager;
            context = ctx;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            RewardEarned rewardEarned = new RewardEarned();

            try
            {
                Child child = await context.Child.Where(c => c.UserId == user.Id).SingleAsync();
                Reward reward = await context.Reward.Where(r => r.RewardId == id).SingleAsync();
                rewardEarned.RewardId = id; 
                context.Add(rewardEarned);
                await context.SaveChangesAsync();  
                await context.Entry(rewardEarned).GetDatabaseValuesAsync();

                List<Event> events = new List<Event>();
                List<EventPoint> eventPoints = new List<EventPoint>();

                events = await context.Event.Where(e => e.RewardId == id).ToListAsync();

                foreach(var snglEvent in events)
                {
                    eventPoints = await context.EventPoint.Where(ep => ep.EventId == snglEvent.EventId).ToListAsync();
                    
                    foreach(var point in eventPoints)
                    {
                        point.RewardEarnedId = rewardEarned.RewardEarnedId;
                        context.Update(point);
                    }
                    await context.SaveChangesAsync();
                }

                return Json(new { success = "Reward earned saved!", rewardEarned } );
            }
            catch
            {
                return BadRequest(new { error = "Unable to add reward earned" } );            
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> All([FromRoute] int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            List<Event> events = new List<Event>();
            List<RewardEarned> rewardsEarned = new List<RewardEarned>();
            Child child = new Child();

            try
            {
                child = await context.Child.Where(c => c.ChildId == id).SingleAsync();
            }
            catch
            {
                return BadRequest(new { error = $"There is no child with the id #{id}" } );
            }

            if(child.UserId != user.Id)
            {
                return BadRequest( new { error = $"This user is not authorized to view the child with id #{id}" } );
            }

            events = await context.Event.Where(e => e.ChildId == child.ChildId).ToListAsync();

            foreach(var snglEvent in events)
            {
                Reward reward = await context.Reward.Where(r => r.RewardId == snglEvent.RewardId).SingleOrDefaultAsync();
                if(reward != null)
                {
                    RewardEarned rewardEarned = await context.RewardEarned.Where(re => re.RewardId == reward.RewardId).SingleOrDefaultAsync();
                    if(rewardEarned != null && !rewardsEarned.Contains(rewardEarned))
                    {
                        rewardsEarned.Add(rewardEarned);
                    }
                }
            }
            return Json( new { success = "Found rewards earned!", rewardsEarned } );
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Single([FromRoute] int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();
            try
            {
                RewardEarned rewardEarned = await context.RewardEarned.Where(re => re.RewardId == id).SingleAsync();

                return Json( new { success= "Reward earned found!", rewardEarned } ); 
            }
            catch
            {
                return BadRequest( new { error = $"No reward earned can be found for the reward id #{id}" } );
            }   
        }

        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> Redeemed([FromRoute] int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();

            try
            {
                Child child = await context.Child.Where(c => c.UserId == user.Id).SingleAsync();
            }
            catch
            {
                return BadRequest( new { error = "The current user is not authorized to update this reward" } );
            }

            try
            {
                RewardEarned rewardEarned = await context.RewardEarned.Where(re => re.RewardEarnedId == id).SingleAsync();
                rewardEarned.IsRedeemed = true;

                context.Update(rewardEarned);
                await context.SaveChangesAsync();
                await context.Entry(rewardEarned).GetDatabaseValuesAsync();

                return Json( new { success = "Reward earned has been marked as redeemed!", rewardEarned } );
            }
            catch
            {
                return BadRequest( new { error = $"There is not a reward earned with the id #{id}.  Did you forget to add the rewardEarned id? ex: rewardearned/redeemed/{{id}}" } );
            }
        }
    }
}