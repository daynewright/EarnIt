using System.Collections.Generic;
using EarnIt.Models;

namespace EarnIt.ViewModels
{
    public class EventListViewModel
    {
        public IEnumerable<Event> Events { get; set; }
        //public EventListViewModel(ApplicationDbContext ctx, ApplicationUser user) : base(ctx, user) {}
    }
}