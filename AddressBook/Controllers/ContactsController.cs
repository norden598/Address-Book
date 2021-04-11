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

        /// <summary>
        /// GET: /Contacts/ViewAll
        /// </summary>
        public IActionResult ViewAll(string sortOrder, string searchString)
        {
            var contacts = from c in this._dbContext.Contacts 
                           select c;

            // format contacts' individual address components into a string to be displayed in the view
            foreach (Contact contact in contacts)
            {
                Address address = this._dbContext.Addresses.Where(a => a.ID == contact.AddressID).FirstOrDefault();
                if (String.IsNullOrEmpty(address.Unit))
                    address.TextValue = $"{address.Street} {address.City}, {address.State}, {address.ZipCode}";
                else
                    address.TextValue = $"{address.Street} {address.Unit} {address.City}, {address.State}, {address.ZipCode}";

                contact.Address = address;
            }

            // filter contacts based on the input search string, if applicable
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.searchString = searchString;

                contacts = contacts.Where(c => c.FirstName.Contains(searchString) ||
                                               c.LastName.Contains(searchString) ||
                                               c.Address.Street.Contains(searchString) ||
                                               c.Address.Unit.Contains(searchString) ||
                                               c.Address.City.Contains(searchString) ||
                                               c.Address.State.Contains(searchString) ||
                                               c.Address.ZipCode.Contains(searchString));
            }

            // handle ascending/descending column sorting
            ViewBag.FirstNameSortParam = String.IsNullOrEmpty(sortOrder) || sortOrder != "FN" ? "FN" : "FN_Desc";
            ViewBag.LastNameSortParam = String.IsNullOrEmpty(sortOrder) || sortOrder != "LN" ? "LN" : "LN_Desc";
            ViewBag.AddressSortParam = String.IsNullOrEmpty(sortOrder) || sortOrder != "Add" ? "Add" : "Add_Desc";

            switch (sortOrder)
            {
                case "FN":
                    contacts = contacts.OrderByDescending(c => c.FirstName);
                    break;
                case "FN_Desc":
                    contacts = contacts.OrderBy(c => c.FirstName);
                    break;
                case "LN":
                    contacts = contacts.OrderByDescending(c => c.LastName);
                    break;
                case "LN_Desc":
                    contacts = contacts.OrderBy(c => c.LastName);
                    break;
                case "Add":
                    contacts = contacts.OrderByDescending(c => c.Address.Street);
                    break;
                case "Add_Desc":
                    contacts = contacts.OrderBy(c => c.Address.Street);
                    break;
                default:
                    contacts = contacts.OrderBy(c => c.LastName);
                    break;
            }

            return View(contacts);
        }
    }
}
