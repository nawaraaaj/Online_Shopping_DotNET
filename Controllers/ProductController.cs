using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShopping_BIT_2025.Data;
using OnlineShopping_BIT_2025.Models;

namespace OnlineShopping_BIT_2025.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly OnlineShopping_BIT_2025Context _context;

        public ProductController(OnlineShopping_BIT_2025Context context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var onlineShopping_BIT_2025Context = _context.Product.Include(p => p.Category);
            return View(await onlineShopping_BIT_2025Context.ToListAsync());
        }
        [AllowAnonymous]
        public async Task<IActionResult> ProductDashboard(string? title)
        {
            if (string.IsNullOrEmpty(title))
            {
                var onlineShopping_BIT_2025Context = _context.Product.Include(p => p.Category);
                return View(await onlineShopping_BIT_2025Context.ToListAsync());
            }
            else
            {
                var searchProducts = _context.Product.Include(p=>p.Category).Where(p=>p.Title.Contains(title));
                return View(await searchProducts.ToListAsync());
            }
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Price,Description,ProductPhoto,CategoryId")] Product product,IFormFile Photo)
        {
            if (ModelState.IsValid)
            {
                string path = Environment.CurrentDirectory + "/wwwroot/ProductImages/";
                string file = Photo.FileName;
                FileStream fs = new FileStream(path + file, FileMode.Create);
                await Photo.CopyToAsync(fs);
                product.ProductPhoto = "ProductImages/" + file; 

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,Description,ProductPhoto,CategoryId")] Product product, IFormFile Photo)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Photo != null && Photo.Length > 0)
                    {
                        // Validate file extension
                        var ext = Path.GetExtension(Photo.FileName).ToLower();
                        var allowedExts = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                        if (!allowedExts.Contains(ext))
                        {
                            ModelState.AddModelError("Photo", "Only image files (.jpg, .png, .gif) are allowed.");
                            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
                            return View(product);
                        }

                        // Create path and unique filename
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");
                        string uniqueFileName = Guid.NewGuid().ToString() + ext;
                        string fullPath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the file
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await Photo.CopyToAsync(stream);
                        }

                        // Set path to save in DB
                        product.ProductPhoto = "ProductImages/" + uniqueFileName;
                    }
                    else
                    {
                        // Preserve existing image if not changed
                        var existingProduct = await _context.Product.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                        if (existingProduct != null)
                        {
                            product.ProductPhoto = existingProduct.ProductPhoto;
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }


        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
