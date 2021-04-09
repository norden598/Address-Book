using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AddressBook.Models;

namespace AddressBook.DAL
{
    public class DbInitializer
    {
        public static void Initialize(AddressBookContext context)
        {
            context.Database.EnsureCreated();

            if (context.Contacts.Any())
                return;

            var contacts = new Contact[]
            {
                new Contact{FirstName="Test", Address = new Address{ } }
            };

            foreach (Contact contact in contacts)
                context.Contacts.Add(contact);

            context.SaveChanges();

            return;
        }
    }
}
