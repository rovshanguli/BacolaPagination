using FrontProjectInAsp.Net.Data;
using FrontProjectInAsp.Net.Models;
using FrontProjectInAsp.Net.Utilities.Pagination;
using FrontProjectInAsp.Net.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontProjectInAsp.Net.Areas.AdminArea.Controllers
{   [Area("AdminArea")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1,int take = 10)
        {
            var products = await _context.Products
                .AsNoTracking()
                .OrderByDescending(m => m.Id)
                .Skip((page-1)*take)
                .Take(take)
                .ToListAsync();
            var productsVM = GetMapDatas(products);
            int count = await GetPageCount(take);

            Paginate<ProductListVM> result = new Paginate<ProductListVM>(productsVM, page, count);
            return View(result);
        }

        private async Task<int> GetPageCount(int take)
        {
            var count = await _context.Products.CountAsync();
            return (int)Math.Ceiling((decimal)count / take);
        }
        private List<ProductListVM> GetMapDatas(List<Product> products)
        {
            List<ProductListVM> productList = new List<ProductListVM>();
            foreach (var product in products)
            {
                ProductListVM newProduct = new ProductListVM
                {
                    Id = product.Id,
                    Image = product.Image,
                    Name = product.Name,
                    Price = product.NewPrice
                };
                productList.Add(newProduct);
            }
            return productList;
        }
    }
}
