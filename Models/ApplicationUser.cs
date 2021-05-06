using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Models {
    public class ApplicationUser : IdentityUser {
        public Cart Cart { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
