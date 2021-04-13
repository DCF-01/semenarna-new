using semenarna_id2.Areas.Store.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Store.ViewModels {
    public class StoreViewModel {
        public List<ProductViewModel> Product_list { get; set; }
        public int Page_number { get; set; }
        public int Number_of_pages { get; set; }
    }
}
