using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Models {
    public class Variation {
        public int VariationId { get; set; }
        public string Name { get; set; }
        public string[] Options { get; set; }
        public ICollection<Product> Products { get; set; }
     }
}
