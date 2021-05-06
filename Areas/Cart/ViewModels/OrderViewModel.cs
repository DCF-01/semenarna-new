using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Cart.ViewModels {
    public class OrderViewModel {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        [Required]
        public string Address { get; set; }
        public string ZipCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public string DeliveryMethod { get; set; }

    }
}
