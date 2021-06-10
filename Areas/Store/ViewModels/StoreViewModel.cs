using semenarna_id2.Areas.Store.ViewModels;
using semenarna_id2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Areas.Store.ViewModels {
    public class StoreViewModel {
        public List<ProductViewModel> Products { get; set; }
        public List<Category> Categories { get; set; }
        public string CurrentCategory { get; set; }
        public string BaseURL { get; set; }
        public int Page_number { get; set; }
        public int Number_of_pages { get; set; }
        public int AllProductsCount { get; set; }
        public int ProductsShowingCount { get; set; }
        public string URLParameters { get; set; }
        public int Products_on_page { get; set; }
    }
}
