using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coresite.Data;
using Coresite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coresite.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET
        public async Task<IActionResult> Index()
        {
            return View(await _db.Category.ToListAsync());
        }

        //GET Create
        public IActionResult Create()
        {
            return View();
        }

        //Post -create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                //if valid
                _db.Category.Add(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        //get edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }

            var category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();

            }

            return View(category);
        }

        //post edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Update(category);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }

            var category = await _db.Category.FindAsync(id);
            _db.Remove(category);
            await _db.SaveChangesAsync();

            if (category == null)
            {
                return NotFound();

            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }

            var category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();

            }

            return View(category);

        }

        /*
        //get edit
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }

            var category = await _db.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();

            }

            return View();
        }
        */

        //post delete


    }
}
