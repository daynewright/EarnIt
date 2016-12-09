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
            ApplicationUser user = await GetCurrentUserAsync();
            var childList = new ChildListViewModel();
            childList.Children = await context.Child.Where(c => c.UserId == user.Id).OrderBy(c => c.Name).ToListAsync();

            return Json( new { children = childList.Children});
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

                return Json(new {success = "Child added!"});
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();

            //remove the child all events, points, earned rewards and rewards
            Child child = await context.Child.Where(c => c.ChildId == id).FirstOrDefaultAsync();
            List<Event> events = await context.Event.Where(e => e.ChildId == id).ToListAsync();

            List<Reward> rewards = new List<Reward>();
            List<EventPoint> points = new List<EventPoint>();
            List<RewardEarned> earnedList = new List<RewardEarned>();
    
            //checks each event for rewards ands adds it to the reward list
            foreach (var singleEvent in events)
            {
                Reward reward = await context.Reward.Where(r => r.EventId == singleEvent.EventId).SingleOrDefaultAsync();
                
                if(!rewards.Contains(reward))
                {
                    rewards.Add(reward);
                }

                EventPoint point = await context.EventPoint.Where(p => p.EventId == singleEvent.EventId).SingleOrDefaultAsync();
                points.Add(point);
            }

            //gets each reward earned and adds it to the rewards earned list
            foreach (var singleReward in rewards)
            {
                RewardEarned earned = await context.RewardEarned.Where(e => e.RewardId == singleReward.RewardId).SingleOrDefaultAsync();
                earnedList.Add(earned);
            }

            try
            {
                ForEachContextRemove(points.Cast<object>().ToList());
                ForEachContextRemove(earnedList.Cast<object>().ToList());
                ForEachContextRemove(rewards.Cast<object>().ToList());
                ForEachContextRemove(events.Cast<object>().ToList());
                context.Remove(child);

                await context.SaveChangesAsync();

                return Json(new {success = "The child was removed!"});
            }
            catch
            {
                return BadRequest(ModelState);
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