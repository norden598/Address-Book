using AddressBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AddressBook.DAL;

namespace AddressBook.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        private AddressBookContext _dbContext;

        public HomeController(AddressBookContext dbContext)
        {
            this._dbContext = dbContext;
        }

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        /// <summary>
        /// GET: Home page
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/About
        /// </summary>
        public ActionResult About()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/ViewAll
        /// </summary>
        public ActionResult ViewAll()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
