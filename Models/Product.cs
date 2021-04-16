using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace semenarna_id2.Models
{
    public class Product {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string SalePrice { get; set; }
        public bool OnSale { get; set; }
        public bool InStock { get; set; }
        public List<Category> Categories { get; set; }
        public Spec Spec { get; set; }
        public byte[] Img { get; set; }
    }
}
