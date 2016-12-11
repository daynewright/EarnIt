using System.Collections.Generic;
using EarnIt.Models;

namespace EarnIt.ViewModels
{
    public class EventListViewModel
    {
        public List<EventViewModel> Events { get; set; } = new List<EventViewModel>(); 
    }
}
