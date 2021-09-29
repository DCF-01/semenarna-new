using application.Areas.Store.ViewModels;
using application.Enums;
using application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Areas.Store.ViewModels {
    public class StoreViewModel {
        public IEnumerable<ProductViewModel> Products { get; set; }
        public List<Category> Categories { get; set; }
        public int CurrentCategoryId { get; set; }
        public string BaseURL { get; set; } 
        public int Page { get; set; } = 1;
        public int TotalPages { 
            get {
                return (int)Math.Ceiling(Total / (double)PageSize);
            }
        }
        public int Total { get; set; } = 0;
        public int PageSize { get; set; } = 24;
        public bool HasPrevious {
            get {
                return Page > 1;
            }
        }
        public bool HasNext {
            get {
                return Page < TotalPages;
            }
        }
        public string Next {
            get {
                return $"{BaseURL}/{Page - 1}";
            }
        }
        public string Previous {
            get {
                return $"{BaseURL}/{Page - 1}";
            }
        }
        public FilterItems FilterItems { get; set; }
    }

}
