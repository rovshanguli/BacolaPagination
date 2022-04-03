using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontProjectInAsp.Net.ViewModels.Admin
{
    public class BannerVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu hisseni bos buraxmayin"), MaxLength(50, ErrorMessage = "Uzunluq 50-den cox ola bilmez")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu hisseni bos buraxmayin"), MaxLength(50, ErrorMessage = "Uzunluq 50-den cox ola bilmez")]
        public string Discount { get; set; }
        public IFormFile Photo { get; set; }
    }
}
