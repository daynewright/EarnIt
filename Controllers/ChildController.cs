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
    public class ChildController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private ApplicationDbContext context;
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public ChildController(UserManager<ApplicationUser> UserManager, ApplicationDbContext ctx)
        {
            _userManager = UserManager;
            context = ctx;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> All()
        {
            try 
            {
                ApplicationUser user = await GetCurrentUserAsync();
                var childList = new ChildListViewModel();
                childList.Children = await context.Child.Where(c => c.UserId == user.Id).OrderBy(c => c.Name).ToListAsync();

                return Json( new { children = childList.Children});
            }
            catch
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] ChildCreateViewModel child)
        {
            ApplicationUser user = await GetCurrentUserAsync();

            if(ModelState.IsValid)
            {
                var model = new Child();

                model.Name = child.Name;
                model.Age = child.Age;
                model.ImageURL = child.ImageURL;
                model.UserId = user.Id;

                context.Add(model);
                await context.SaveChangesAsync();
                await context.Entry(model).GetDatabaseValuesAsync();

                return Json(new { success = "Child added!", child = model });
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();

            List<Event> events = new List<Event>();
            List<Reward> rewards = new List<Reward>();
            List<EventPoint> allPoints = new List<EventPoint>();
            List<RewardEarned> rewardsEarned = new List<RewardEarned>();

            //checks that the logged in user is authorized to delete this child record
            try 
            {
                Child child = await context.Child.Where(c => c.ChildId == id && c.UserId == user.Id).SingleAsync();

                //get all event ids attached to the passed in child
                events = await context.Event.Where(e => e.ChildId == id).ToListAsync();
        
                //get add the rewards and event points for the passed in child from the reward ids and event ids
                foreach (var singleEvent in events)
                {
                    List<EventPoint> tempList = new List<EventPoint>();

                    Reward reward = await context.Reward.Where(r => r.RewardId == singleEvent.RewardId).SingleOrDefaultAsync();

                    if(!rewards.Contains(reward) && reward != null)
                    {
                        rewards.Add(reward);
                    }

                    tempList = await context.EventPoint.Where(p => p.EventId == singleEvent.EventId).ToListAsync();

                    if(tempList.Any())
                    {
                        foreach(var item in tempList)
                        {
                            allPoints.Add(item);
                        }
                    }
                }
                //get all the rewards earned for the passed in child from the reward ids
                foreach(var reward in rewards)
                {
                    RewardEarned rewardEarned = await context.RewardEarned.Where(re => re.RewardId == reward.RewardId).SingleOrDefaultAsync();

                    if(rewardEarned != null)
                    {
                        rewardsEarned.Add(rewardEarned);
                    }
                }

                //attempt to remove all of the rewards, points, events, earned rewards and the child
                try
                {
                    ForEachContextRemove(allPoints.Cast<object>().ToList());
                    ForEachContextRemove(rewardsEarned.Cast<object>().ToList());
                    ForEachContextRemove(rewards.Cast<object>().ToList());
                    ForEachContextRemove(events.Cast<object>().ToList());
                    context.Remove(child); 

                    await context.SaveChangesAsync();

                    return Json(new {success = "The child was removed!"});
                }
                catch
                {
                    return BadRequest(new { error = "Not able to remove the child" } );
                }
            }
            //returns a bad request if logged in user cannot delete the child passed in
            catch
            {
              return BadRequest( new { error = "This user is not authorized to delete this child or the child record does not exist" } );   
            }
        }

        //Helper to loop through context remove
        private void ForEachContextRemove(List<object> list)
        {
            foreach(var item in list)
            {
                context.Remove(item);
            }
        }
    }
}