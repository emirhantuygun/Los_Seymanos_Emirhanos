using CafeApp.Data;
using CafeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProject.Models;

namespace CafeApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _context;
        public ProductController (DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Product/Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Product/Create")]
        public async Task<IActionResult> Create([Bind("ProductId,Name,ImageSrc,Option,Type,Details,PriceTall,PriceGrande,PriceVenti,PriceDessert")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync(); 
                return RedirectToAction("ProductList","Barista"); 
            }

            return View(product);
        }


       [HttpGet]
        public async Task<IActionResult> Edit(int? id)
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

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,ImageSrc,Option,Type,Details,PriceTall,PriceGrande,PriceVenti,PriceDessert")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ProductList","Barista");
            }
            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
        
       // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ProductList","Barista");
        }
    }
}