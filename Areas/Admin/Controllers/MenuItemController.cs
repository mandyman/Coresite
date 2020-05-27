using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Coresite.Data;
using Coresite.Models;
using Coresite.Models.ViewModel;
using Coresite.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Coresite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        
        [BindProperty]
        public MenuItemViewModel MenuItemVM { get; set; }

        public MenuItemController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
            MenuItemVM = new MenuItemViewModel()
            {
                Categories = _db.Category,
                MenuItem = new Models.MenuItem()
            };


        }
        public async Task<IActionResult> Index()
        {
            var menuItems = await _db.MenuItem.Include(m=>m.Category).Include(m=>m.SubCategory).ToListAsync();

            return View(menuItems);
        }

        //get - create
        public IActionResult Create()
        {
            return View(MenuItemVM);
        }


        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }

            _db.MenuItem.Add(MenuItemVM.MenuItem);
            await _db.SaveChangesAsync();

            //images

            string webRooPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var menuItemfromdb = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id);

            if (files.Count() > 0) 
            {
                //File(uploaded);
                var uploads = Path.Combine(webRooPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filestream = new FileStream(Path.Combine(uploads,MenuItemVM.MenuItem.Id+extension), FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }

                menuItemfromdb.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension;
            }
            else
            {
                //no file was uploaded

                var uploads = Path.Combine(webRooPath, @"images\" + SD.DefaultFoodImage);
                System.IO.File.Copy(uploads, webRooPath+@"\images\"+MenuItemVM.MenuItem.Id+".png");
                menuItemfromdb.Image = @"\images\" + MenuItemVM.MenuItem.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id==id);

            MenuItemVM.SubCategories = await _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId)
                .ToListAsync();

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return View(MenuItemVM);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MenuItemVM.MenuItem.SubCategoryId = Convert.ToInt32(Request.Form["SubCategoryId"].ToString());

            if (!ModelState.IsValid)
            {
                //has to be poputated cause of in the view is populated by JS
                MenuItemVM.SubCategories = await _db.SubCategory
                    .Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId).ToListAsync();
                return View(MenuItemVM);
            }


            //images

            string webRooPath = _hostEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var menuItemfromdb = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id);

            if (files.Count() > 0)
            {
                //New Image has been uploaded
                var uploads = Path.Combine(webRooPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //delete original image
                var imagePath = Path.Combine(webRooPath, menuItemfromdb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                using (var filestream = new FileStream(Path.Combine(uploads, MenuItemVM.MenuItem.Id + extension_new),
                    FileMode.Create))
                {
                    files[0].CopyTo(filestream);
                }

                menuItemfromdb.Image = @"\images\" + MenuItemVM.MenuItem.Id + extension_new;
            }

            menuItemfromdb.Name = MenuItemVM.MenuItem.Name;
            menuItemfromdb.Description = MenuItemVM.MenuItem.Description;
            menuItemfromdb.Price = MenuItemVM.MenuItem.Price;
            menuItemfromdb.Spicyness = MenuItemVM.MenuItem.Spicyness;
            menuItemfromdb.CategoryId = MenuItemVM.MenuItem.CategoryId;
            menuItemfromdb.SubCategoryId = MenuItemVM.MenuItem.SubCategoryId;


            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemVM.MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);

            MenuItemVM.SubCategories = await _db.SubCategory.Where(s => s.CategoryId == MenuItemVM.MenuItem.CategoryId)
                .ToListAsync();

            if (MenuItemVM.MenuItem == null)
            {
                return NotFound();
            }

            return View(MenuItemVM);

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            //images
            MenuItemVM.MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);


            string webRooPath = _hostEnvironment.WebRootPath;


            var menuItemfromdb = await _db.MenuItem.FindAsync(MenuItemVM.MenuItem.Id);


            //delete original image
            var imagePath = Path.Combine(webRooPath, menuItemfromdb.Image.TrimStart('\\'));

            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }


            _db.Remove(menuItemfromdb);


            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
