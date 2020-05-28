using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Coresite.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Coresite.Models;
using Coresite.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Coresite.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController( ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {

            IndexViewModel IndexVM = new IndexViewModel()
            {
                MenuItem = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory).ToListAsync(),
                Category = await _db.Category.ToListAsync()

            };

            var claimsidentity = (ClaimsIdentity) User.Identity;
            var claim = claimsidentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim!=null)
            {
                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32("ssCartcount", cnt);
            }



            return View(IndexVM);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var menuitemfromdb = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory)
                .Where(m => m.Id == id).FirstOrDefaultAsync();

            ShoppingCart cartobj = new ShoppingCart()
            {
                MenuItem = menuitemfromdb,
                MenuItemId = menuitemfromdb.Id

            };

            return View(cartobj);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShoppingCart CartObject)
        {
            CartObject.Id = 0;

            if (ModelState.IsValid)
            {
                var claimIdentity = (ClaimsIdentity) this.User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                CartObject.ApplicationUserId = claim.Value;

                ShoppingCart cartfromdb = await _db.ShoppingCart.Where(c =>
                    c.ApplicationUserId == CartObject.ApplicationUserId &&
                    c.MenuItemId == CartObject.MenuItemId).FirstOrDefaultAsync();


                if (cartfromdb == null)
                {
                    await _db.ShoppingCart.AddAsync(CartObject);
                }
                else
                {
                    cartfromdb.Count = cartfromdb.Count + CartObject.Count;
                }

                await _db.SaveChangesAsync();


                var count = _db.ShoppingCart.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId).ToList()
                    .Count();

                HttpContext.Session.SetInt32("ssCartcount", count);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var menuitemfromdb = await _db.MenuItem.Include(m => m.Category).Include(m => m.SubCategory)
                    .Where(m => m.Id == CartObject.MenuItemId).FirstOrDefaultAsync();

                ShoppingCart cartobj = new ShoppingCart()
                {
                    MenuItem = menuitemfromdb,
                    MenuItemId = menuitemfromdb.Id

                };

                return View(CartObject);
            }


        }
        


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
