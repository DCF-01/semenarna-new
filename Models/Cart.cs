using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Models {
    public class Cart {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<Product> Products { get; set; }
    }
}
