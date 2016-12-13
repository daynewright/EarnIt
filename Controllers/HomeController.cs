using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EarnIt.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return BadRequest( new { error = "You need to login or hit an API endpoint." } );
        }
        public IActionResult Error()
        {
            return BadRequest( new { error = "Something went wrong." } );
        }
    }
}
