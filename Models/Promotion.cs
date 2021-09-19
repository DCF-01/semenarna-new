using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace application.Models {
    public class Promotion {
        public int PromotionId { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Price { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public byte[] Img { get; set; }
        public bool Active { get; set; }
    }
}
