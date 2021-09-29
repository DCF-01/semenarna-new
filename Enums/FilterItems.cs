using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace application.Enums {
    public enum FilterItems {
        [Display(Name = "Име на производ: (A-Z)")]
        NameAscending,
        [Display(Name = "Име на производ: (Z-A)")]
        NameDescending,
        [Display(Name = "Цена (ниска > висока)")]
        PriceAscending,
        [Display(Name = "Цена (висока > ниска)")]
        PriceDescending
    }
}
