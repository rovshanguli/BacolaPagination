using FrontProjectInAsp.Net.Data;
using FrontProjectInAsp.Net.Models;
using FrontProjectInAsp.Net.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LessonMigration.Utilities.File;
using System;
using LessonMigration.Utilities.Helpers;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace FrontProjectInAsp.Net.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BannerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public BannerController(AppDbContext context , IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Banner> banners = await _context.banners.OrderByDescending(m => m.Id).AsNoTracking().ToListAsync();
            return View(banners);
        }

        #region Create 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerVM bannerVM)
        {
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();
            if (!bannerVM.Photo.CheckFileType("image/"))
            {
                ModelState.AddModelError("Photo", "Image type is wrong");
                return View();
            }

            if (!bannerVM.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "Image size is wrong");
                return View();
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = _context.banners.Any(m => m.Name.ToLower().Trim() == bannerVM.Name.ToLower().Trim());

            if (isExist)
            {
                ModelState.AddModelError("catagoryName", "Bu catagory artiq movcuddur");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "_" + bannerVM.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await bannerVM.Photo.CopyToAsync(stream);
            }


            Banner banner = new Banner
            {
                Image = fileName,
                Name = bannerVM.Name,
                Discount = bannerVM.Discount
            };

            await _context.banners.AddAsync(banner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Banner banner = await _context.banners.FindAsync(id);

            if (banner == null) return NotFound();

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img", banner.Image);
            Helper.DeleteFile(path);

            _context.banners.Remove(banner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        public async Task<IActionResult> Update(int id)
        {
            Banner banner = await _context.banners.FindAsync(id);
            return View(banner);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id , Banner banner)
        {
            //Delete Old File From Folder

            Banner dbbanner = await _context.banners.FindAsync(id);
            if (dbbanner == null) return NotFound();
            string oldPath = Helper.GetFilePath(_env.WebRootPath, "assets/img/", dbbanner.Image);
            dbbanner.Image = banner.Image;
            dbbanner.Name = banner.Name;
            dbbanner.Discount = banner.Discount;
            Helper.DeleteFile(oldPath);



            //Add New File To Folder

            string fileName = Guid.NewGuid().ToString() + "_" + banner.Photo.FileName;

            string path = Helper.GetFilePath(_env.WebRootPath, "assets/img/", fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await banner.Photo.CopyToAsync(stream);
            }



            //Replace Image Name in DB
            dbbanner.Image = fileName;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
