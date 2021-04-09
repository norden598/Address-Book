using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AddressBook.Models;
using AddressBook.DAL;

namespace AddressBook.Controllers
{
    public class ContactsController : Controller
    {
        private AddressBookContext _dbContext;

        public ContactsController(AddressBookContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public IActionResult ViewAll(string sortOrder)
        {
            var contacts = from c in this._dbContext.Contacts 
                           select c;

            if (String.IsNullOrEmpty(sortOrder))
                ViewBag.NameSortParm = "name_desc";
            else
                ViewBag.NameSortParm = String.Empty;

            switch (sortOrder)
            {
                case "name_desc":
                    contacts = contacts.OrderByDescending(c => c.LastName);        
                    break;
                default:
                    contacts = contacts.OrderBy(c => c.LastName);
                    break;
            }

            return View(contacts);
        }
    }
}
