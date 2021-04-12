using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

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
    }
}
