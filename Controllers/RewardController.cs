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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSingle([FromRoute] int id)
        {
            //finish get all method.
            ApplicationUser user = await GetCurrentUserAsync();
            Reward reward = await context.Reward.Where(r => r.EventId == id).SingleAsync();

            RewardViewModel model = new RewardViewModel();

            model.Name = reward.Name;
            model.Description = reward.Description;
            model.ImageURL = reward.ImageURL;
            model.PointsNeeded = reward.PointsNeeded;

            return Json(new {reward = model});
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Reward reward)
        {
            if(ModelState.IsValid)
            {
                ApplicationUser user = await GetCurrentUserAsync();

                context.Add(reward);
                await context.SaveChangesAsync();

                return Json(new {success = "Reward created!"} );
            }
            return BadRequest(ModelState);
        }
    }
}