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
                new Contact{FirstName="Test", LastName="User", Address = new Address{Street = "TestStreet", City = "TestCity", State = "TE", ZipCode = "12345" } },
                new Contact{FirstName="Austin", LastName="Norden", Address = new Address{Street = "6317 Reo Street", City = "Toledo", State = "OH", ZipCode = "43615" } }
            };

            foreach (Contact contact in contacts)
                context.Contacts.Add(contact);

            context.SaveChanges();

            return;
        }
    }
}
