using Xunit;
using System;
using EarnIt.Data;
using Microsoft.AspNetCore.Identity;
using EarnIt.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EarnItTests
{
    public class AccountTest
    {
        private ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> _userManager;

        [Fact]
        public void CanCreateNewUser()
        {
            
        }
                
        [Fact]
        public void CanLoginUser()
        {

        }
        
        [Fact]
        public void CanGetLoggedInUser()
        {

        }

        [Fact]
        public void CanLogOutUser()
        {

        }
    }
}