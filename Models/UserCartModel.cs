using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Models {
    public class UserCartModel {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int[] TestProductIds { get; set; }
    }
}
