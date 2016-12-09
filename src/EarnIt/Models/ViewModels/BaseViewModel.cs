using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EarnIt.Data;
using EarnIt.Models;

namespace EarnIt.ViewModels
{
    public class BaseViewModel
    {
        protected ApplicationDbContext context;
        private ApplicationUser _user;

        public BaseViewModel(ApplicationDbContext ctx, ApplicationUser user)
        {
            context = ctx;

            _user = user;
        }
        public BaseViewModel() { }
        
        public string getLoggedInUserId() {
            return _user.Id;
        }
    }
}