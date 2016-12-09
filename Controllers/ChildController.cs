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

                return Json(new {success= "Child added!"});
            }
            return Json(new {error = "Unable to add child."});
        }
    }
}