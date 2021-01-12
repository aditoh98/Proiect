using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StoreModel.Data;
using StoreModel.Models;

namespace Proiect.Controllers
{
    public class SmartphonesController : Controller
    {
        private readonly StoreContext _context;

        public SmartphonesController(StoreContext context)
        {
            _context = context;
        }

        // GET: Smartphones
        public async Task<IActionResult> Index(
            string sortOrder,
            string searchString,
            string currentFilter,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var smartphones = from b in _context.Smartphones
                              select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                smartphones = smartphones.Where(s => s.Manufacturer.Contains(searchString));
            }

            smartphones = sortOrder switch
            {
                "title_desc" => smartphones.OrderByDescending(b => b.Manufacturer),
                "Price" => smartphones.OrderBy(b => b.Price),
                "price_desc" => smartphones.OrderByDescending(b => b.Price),
                _ => smartphones.OrderBy(b => b.Manufacturer),
            };
            int pageSize = 2;
            return View(await PaginatedList<Smartphone>.CreateAsync(smartphones.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        // GET: Smartphones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smartphone = await _context.Smartphones
                .Include(s => s.Orders)
                .ThenInclude(e => e.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);


            if (smartphone == null)
            {
                return NotFound();
            }

            return View(smartphone);
        }

        // GET: Smartphones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Smartphones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Manufacturer,Model,Price")] Smartphone smartphone)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(smartphone);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex*/)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                "Try again, and if the problem persists ");
            }

            return View(smartphone);
        }



        // GET: Smartphones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smartphone = await _context.Smartphones.FindAsync(id);
            if (smartphone == null)
            {
                return NotFound();
            }
            return View(smartphone);
        }

        // POST: Smartphones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if ((id == null))
            {
                var studentToUpdate = await _context.Smartphones.FirstOrDefaultAsync(s => s.ID == id);
                if (await TryUpdateModelAsync<Smartphone>(studentToUpdate, "", s => s.Manufacturer, s => s.Model, s => s.Price))
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateException /* ex */)
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists");
                    }
                }
                return View(studentToUpdate);
            }
            return NotFound();
        }


        // GET: Smartphones/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var smartphone = await _context.Smartphones
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            if (smartphone == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                "Delete failed. Try again";
            }
            return View(smartphone);
        }

        // POST: Smartphones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var smartphone = await _context.Smartphones.FindAsync(id);
            if (smartphone == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Smartphones.Remove(smartphone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}


