﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontProjectInAsp.Net.ViewModels.Admin
{
    public class ProductListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal  Price { get; set; }
        public string CatagoryName { get; set; }
        public string Image { get; set; }
    }
}
