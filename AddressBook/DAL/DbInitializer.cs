﻿using System.Collections.Generic;
using System.Linq;
using AddressBook.Models;

namespace AddressBook.DAL
{
    public class DbInitializer
    {
        /// <summary>
        /// Checks for data upon execution. If database doesn't exist, it will pre-populate the data below and save the database file to C:\Users\<username>\AddressBookDb.mdf
        /// </summary>
        public static void Initialize(AddressBookContext context)
        {
            context.Database.EnsureCreated();

            if (context.Contacts.Any())
                return;

            // pre-populate addresses
            var addresses = new List<Address>
            {
                new Address{ Street = "TestStreet", City = "TestCity", Unit = "Apt. 101", State = "OH", ZipCode = "12345" },
                new Address{ Street = "2801 Bancroft Street", City = "Toledo", State = "OH", ZipCode = "43606" },
                new Address{ Street = "151 Page Street", City = "New Bedford", State = "MA", ZipCode = "02740" },
                new Address{ Street = "100 Alfred Lerner Way", City = "Cleveland", State = "OH", ZipCode = "44114" },
                new Address{ Street = "1 Patriot Pl", City = "Foxborough", State = "MA", ZipCode = "02035" }
            };

            foreach (Address address in addresses)
                context.Addresses.Add(address);

            context.SaveChanges();

            // pre-populate contacts
            var contacts = new List<Contact>
            {
                new Contact{ FirstName="Test", LastName="User", Address = addresses.Find(a => a.ID == 1), PhoneNumber= "000-000-0000" },
                new Contact{ FirstName="Austin", LastName="Norden", Address = addresses.Find(a => a.ID == 2), PhoneNumber = "419-123-4567" },
                new Contact{ FirstName="Southcoast", LastName="Health", Address = addresses.Find(a => a.ID == 3), PhoneNumber = "844-744-5544" },
                new Contact{ FirstName="Baker", LastName="Mayfield", Address = addresses.Find(a => a.ID == 4), PhoneNumber = "440-891-5000" },
                new Contact{ FirstName="Nick", LastName="Chubb", Address = addresses.Find(a => a.ID == 4), PhoneNumber = "440-891-6000" },
                new Contact{ FirstName="Bill", LastName="Belichick", Address = addresses.Find(a => a.ID == 5), PhoneNumber = "508-543-8200" }
            };

            foreach (Contact contact in contacts)
                context.Contacts.Add(contact);

            context.SaveChanges();  

            return;
        }
    }
}
