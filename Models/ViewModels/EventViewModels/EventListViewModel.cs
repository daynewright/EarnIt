using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EarnIt.Data;
using EarnIt.Models;

namespace EarnIt.ViewModels
{
    public class EventListViewModel : BaseViewModel
    {
        public IEnumerable<Event> Events { get; set; }

        public EventListViewModel(ApplicationDbContext ctx, ApplicationUser user) : base(ctx, user) {}
    }
}