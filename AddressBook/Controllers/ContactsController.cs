using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using AddressBook.Models;
using AddressBook.DAL;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public ActionResult ViewAll(string sortOrder, string searchString)
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

            this._dbContext.SaveChanges();

            // filter contacts based on the input search string, if applicable
            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.searchString = searchString;

                contacts = contacts.Where(c => c.FirstName.Contains(searchString) ||
                                               c.LastName.Contains(searchString) ||
                                               c.Address.TextValue.Contains(searchString));
            }

            // work out sorting ascending/descending columns
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

        // GET: /Contacts/Create
        public ActionResult Create()
        {
            ViewBag.CategoryList = GetStates();
            //Contact newContact = new Contact()
            //{
            //    FirstName = String.Empty,
            //    LastName = String.Empty,
            //    Address = new Address(),
            //    PhoneNumber = String.Empty
            //};

            return View();
        }

        // POST: /Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this._dbContext.Add(contact);
                    await this._dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(ViewAll));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes.");
            }

            ViewBag.CategoryList = GetStates();

            return View(contact);
        }

        // GET: /Contacts/Edit/<ContactID>
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);

            Contact contact = this._dbContext.Contacts.Where(c => c.ID == id).FirstOrDefault();
            if (contact == null)
                return NotFound();

            contact.Address = this._dbContext.Addresses.Where(a => a.ID == contact.AddressID).FirstOrDefault();
            if (contact.Address == null)
                return NotFound();

            ViewBag.CategoryList = GetStates();

            return View(contact);
        }

        // POST: /Contacts/Edit/<ContactID>
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditPost(int? id)
        {
            if (id == null)
                return NotFound();

            var contactToUpdate = await this._dbContext.Contacts.FirstOrDefaultAsync(c => c.ID == id);
            if (await TryUpdateModelAsync<Contact>(contactToUpdate, "", c => c.FirstName, c => c.LastName, c => c.Address, c => c.PhoneNumber))
            {
                try
                {
                    await this._dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(ViewAll));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes.");
                }
            }

            return View(contactToUpdate);
        }

        // GET: /Contacts/Delete/<ContactID>
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new StatusCodeResult((int)HttpStatusCode.BadRequest);

            Contact contact = this._dbContext.Contacts.Where(c => c.ID == id).FirstOrDefault();
            if (contact == null)
                return NotFound();

            contact.Address = this._dbContext.Addresses.Where(a => a.ID == contact.AddressID).FirstOrDefault();
            if (contact.Address == null)
                return NotFound();

            return View(contact);
        }

        // POST: /Contacts/Delete/<ContactID>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactToDelete = await this._dbContext.Contacts.FirstOrDefaultAsync(c => c.ID == id);
            if (contactToDelete == null)
                return RedirectToAction(nameof(ViewAll));

            try
            {
                int addressID = contactToDelete.AddressID;

                this._dbContext.Contacts.Remove(contactToDelete);
                await this._dbContext.SaveChangesAsync();

                // if no other contacts have this addess, delete the address from the database too
                // *** is this the correct way to handle this scenario?
                if (!this._dbContext.Contacts.Any(c => c.AddressID == addressID))
                {
                    this._dbContext.Addresses.Remove(this._dbContext.Addresses.Find(addressID));
                    await this._dbContext.SaveChangesAsync();
                }
                
                return RedirectToAction(nameof(ViewAll));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to delete contact.");
            }

            return View(contactToDelete);
        }

        // Builds a list of states for pre-populating the state dropdown field when editing a contact
        private List<SelectListItem> GetStates()
        {
            List<SelectListItem> states = new List<SelectListItem>()
            {
                new SelectListItem { Value = "AL", Text = "AL" },
                new SelectListItem { Value = "AK", Text = "AK" },
                new SelectListItem { Value = "AZ", Text = "AZ" },
                new SelectListItem { Value = "AR", Text = "AR" },
                new SelectListItem { Value = "CA", Text = "CA" },
                new SelectListItem { Value = "CO", Text = "CO" },
                new SelectListItem { Value = "CT", Text = "CT" },
                new SelectListItem { Value = "DE", Text = "DE" },
                new SelectListItem { Value = "FL", Text = "FL" },
                new SelectListItem { Value = "GA", Text = "GA" },
                new SelectListItem { Value = "HI", Text = "HI" },
                new SelectListItem { Value = "ID", Text = "ID" },
                new SelectListItem { Value = "IL", Text = "IL" },
                new SelectListItem { Value = "IN", Text = "IN" },
                new SelectListItem { Value = "IA", Text = "IA" },
                new SelectListItem { Value = "KS", Text = "KS" },
                new SelectListItem { Value = "KY", Text = "KY" },
                new SelectListItem { Value = "LA", Text = "LA" },
                new SelectListItem { Value = "ME", Text = "ME" },
                new SelectListItem { Value = "MD", Text = "MD" },
                new SelectListItem { Value = "MA", Text = "MA" },
                new SelectListItem { Value = "MI", Text = "MI" },
                new SelectListItem { Value = "MN", Text = "MN" },
                new SelectListItem { Value = "MS", Text = "MS" },
                new SelectListItem { Value = "MO", Text = "MO" },
                new SelectListItem { Value = "MT", Text = "MT" },
                new SelectListItem { Value = "NE", Text = "NE" },
                new SelectListItem { Value = "NV", Text = "NV" },
                new SelectListItem { Value = "NH", Text = "NH" },
                new SelectListItem { Value = "NJ", Text = "NJ" },
                new SelectListItem { Value = "NM", Text = "NM" },
                new SelectListItem { Value = "NY", Text = "NY" },
                new SelectListItem { Value = "NC", Text = "NC" },
                new SelectListItem { Value = "ND", Text = "ND" },
                new SelectListItem { Value = "OH", Text = "OH" },
                new SelectListItem { Value = "OK", Text = "OK" },
                new SelectListItem { Value = "OR", Text = "OR" },
                new SelectListItem { Value = "PA", Text = "PA" },
                new SelectListItem { Value = "RI", Text = "RI" },
                new SelectListItem { Value = "SC", Text = "SC" },
                new SelectListItem { Value = "SD", Text = "SD" },
                new SelectListItem { Value = "TN", Text = "TN" },
                new SelectListItem { Value = "TX", Text = "TX" },
                new SelectListItem { Value = "UT", Text = "UT" },
                new SelectListItem { Value = "VT", Text = "VT" },
                new SelectListItem { Value = "VA", Text = "VA" },
                new SelectListItem { Value = "WA", Text = "WA" },
                new SelectListItem { Value = "WV", Text = "WV" },
                new SelectListItem { Value = "WI", Text = "WI" },
                new SelectListItem { Value = "WY", Text = "WY" }
            };

            return states;
        }
    }
}
