using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace AddressBook.Models
{
    public class Contact
    {
        /// <summary>
        /// Contact ID (Primary Key)
        /// </summary>
        public int ID { get; set; }

        //[DisplayName("First Name")]
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(maximumLength: 30, ErrorMessage = "First Name is too long")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(maximumLength: 30, ErrorMessage = "Last Name is too long")]
        public string LastName { get; set; }

        /// <summary>
        /// Phone number formatted as xxx-xxx-xxxx
        /// </summary>
        [Required(ErrorMessage = "Phone Number is required")]
        [StringLength(maximumLength: 12, MinimumLength = 12, ErrorMessage = "Phone Number must be formatted as xxx-xxx-xxxx")]
        public string PhoneNumber { get; set; }

        public Address Address { get; set; }

        /// <summary>
        /// Contact address ID (Foreign Key)
        /// </summary>
        public int AddressID { get; set; }
    }
}
