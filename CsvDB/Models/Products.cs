using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CsvDB.Models
{
    public class Products
    {
        public string SKU { get; set; }
        public string Manufacture { get; set; }
        public string Cost { get; set; }
        public string Weight { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Shipping { get; set; }

    }
}