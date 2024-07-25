using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assignment.Data;
using Assignment.Models;

namespace Assignment.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }


        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 5, string searchQuery = "")
        {
            // Store search query in ViewBag to retain value after search
            ViewBag.SearchQuery = searchQuery;

            // Filter products based on search query
            var products = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                products = products.Where(p => p.Name.Contains(searchQuery));
            }

            // Pagination
            var totalProducts = await products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalProducts / (double)pageSize);
            var productsList = await products
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(productsList);
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var role= HttpContext.Session.GetString("UserEmail");
            if (role=="admin@gmail.com")
            {
                return View();
            }
            return RedirectToAction("AccessDenied", "Home");
            
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, IFormFile? ImageFile)
        {
            var role = HttpContext.Session.GetString("UserEmail");
            if (role == "admin@gmail.com")
            {
                if (ModelState.IsValid)
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        // Get the path to the "wwwroot" folder
                        var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                        // Create the "images" folder if it does not exist
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Generate a unique file name
                        var fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                        var fileExtension = Path.GetExtension(ImageFile.FileName);
                        var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";

                        // Combine the folder path and file name
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        // Set the ImagePath property
                        product.ImagePath = $"/images/{uniqueFileName}";
                    }

                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(product);
            }
            return RedirectToAction("AccessDenied", "Home");

        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var role = HttpContext.Session.GetString("UserEmail");
            if (role == "admin@gmail.com")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            return RedirectToAction("AccessDenied", "Home");  
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Product product, IFormFile? ImageFile)
        {
            var role = HttpContext.Session.GetString("UserEmail");

            if (role == "admin@gmail.com")
            {
                if (id != product.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        // Get the path to the "wwwroot" folder
                        var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");

                        // Create the "images" folder if it does not exist
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // Generate a unique file name
                        var fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                        var fileExtension = Path.GetExtension(ImageFile.FileName);
                        var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{fileExtension}";

                        // Combine the folder path and file name
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // Save the file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(fileStream);
                        }

                        // Set the ImagePath property
                        product.ImagePath = $"/images/{uniqueFileName}";
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            return RedirectToAction("AccessDenied", "Home");
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var role = HttpContext.Session.GetString("UserEmail");
            if (role=="admin@gmail.com")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var product = await _context.Products
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (product == null)
                {
                    return NotFound();
                }

                return View(product);
            }
            return RedirectToAction("AccessDenied", "Home");

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var role = HttpContext.Session.GetString("UserEmail");
            if (role == "admin@gmail.com")
            {
                var product = await _context.Products.FindAsync(id);
                if (product != null)
                {
                    // Delete the image file if it exists
                    if (!string.IsNullOrEmpty(product.ImagePath))
                    {
                        var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, product.ImagePath.TrimStart('/'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("AccessDenied", "Home");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
