using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AddressBook.Models
{
    public class Address
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Street address is required")]
        [StringLength(maximumLength: 30, ErrorMessage = "Street address is too long")]
        public string Street { get; set; }
        /// <summary>
        /// Apartment/Suite number, unit, building, floor, etc. Not required
        /// </summary>
        [StringLength(maximumLength: 10, ErrorMessage = "Unit/Apt Number is too long")]
        public string Unit { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(maximumLength:20, ErrorMessage = "City is too long")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(maximumLength: 2, MinimumLength = 2, ErrorMessage = "State is too long")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zip code is required")]
        [StringLength(maximumLength: 5, MinimumLength = 5, ErrorMessage = "Zip code must be a 5-digit number")]
        [Range(0, 99999, ErrorMessage = "Zip code must be a 5-digit number")]
        public string ZipCode { get; set; }
    }
}
