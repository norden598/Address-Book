using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models
{
    public class Contact
    {
        /// <summary>
        /// Contact ID (Primary Key)
        /// </summary>
        public int ID { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(30, ErrorMessage = "First Name is too long")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(30, ErrorMessage = "Last Name is too long")]
        public string LastName { get; set; }

        public Address Address { get; set; }

        /// <summary>
        /// Contact address ID (Foreign Key)
        /// </summary>
        public int AddressID { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = false)]
        //public DateTime BirthDay { get; set; }

    }
}
