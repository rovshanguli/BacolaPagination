using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontProjectInAsp.Net.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public bool IsDelete { get; set; } = false;
        public string Name { get; set; }
        public int rateing { get; set; }
        public decimal OldPrice { get; set; }
        public decimal  NewPrice { get; set; }
    }
}