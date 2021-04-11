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

            ViewBag.FirstNameSortParam = String.IsNullOrEmpty(sortOrder) || sortOrder != "FirstName_Desc" ? "FirstName_Desc" : "FirstName";
            ViewBag.LastNameSortParam = String.IsNullOrEmpty(sortOrder) || sortOrder != "LastName_Desc" ? "LastName_Desc" : "LastName";
            ViewBag.AddressSortParam = String.IsNullOrEmpty(sortOrder) || sortOrder != "Address_Desc" ? "Address_Desc" : "Address";
            ViewBag.PhoneSortParam = String.IsNullOrEmpty(sortOrder) || sortOrder != "PhoneNumber_Desc" ? "PhoneNumber_Desc" : "PhoneNumber";

            switch (sortOrder)
            {
                case "FirstName":
                    contacts = contacts.OrderBy(c => c.FirstName);
                    break;
                case "FirstName_Desc":
                    contacts = contacts.OrderByDescending(c => c.FirstName);
                    break;
                case "LastName":
                    contacts = contacts.OrderBy(c => c.LastName);
                    break;
                case "LastName_Desc":
                    contacts = contacts.OrderByDescending(c => c.LastName);
                    break;
                case "Address":
                    contacts = contacts.OrderBy(c => c.Address.Street);
                    break;
                case "Address_Desc":
                    contacts = contacts.OrderByDescending(c => c.Address.Street);
                    break;
                case "PhoneNumber":
                    contacts = contacts.OrderBy(c => c.PhoneNumber);
                    break;
                case "PhoneNumber_Desc":
                    contacts = contacts.OrderByDescending(c => c.PhoneNumber);
                    break;
                default:
                    contacts = contacts.OrderBy(c => c.LastName);
                    break;
            }

            return View(contacts);
        }
    }
}
