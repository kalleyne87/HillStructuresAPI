using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HillStructuresAPI.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }
        [Display(Name = "Name")]
        public string CompanyName { get; set; }
        public string Address { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{4})$", ErrorMessage = "The Phone Number field is not a valid phone number")]
        public string PhoneNumber { get; set; }
        public ICollection<PaymentSheet> PaymentSheets { get; set; }
    }
}