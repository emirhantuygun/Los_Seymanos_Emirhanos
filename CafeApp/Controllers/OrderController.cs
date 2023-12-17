using CafeApp.Data;
using CafeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;

namespace CafeApp.Controllers
{

    public class OrderController : Controller
    {
        private readonly DataContext _context;
        public OrderController(DataContext cont)
        {
            _context = cont;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return View(products);
        }


        [HttpPost]
        public async Task<IActionResult> AddToBag(int productId)
        {

            Console.WriteLine("HEYYYYY");

            Product? product = await _context.Products.FindAsync(productId);

            var selectedProducts = await _context.SelectedProducts.Include(sp => sp.Product).ToListAsync();


            if (selectedProducts != null)
            {
                SelectedProduct? existingSP = selectedProducts.FirstOrDefault(p => p.Product != null && p.Product.ProductId == productId);

                if (existingSP != null)
                {
                    existingSP.Quantity++;
                }
                else
                {
                    SelectedProduct newSelectedProduct = new SelectedProduct() { Product = product, Quantity = 1 };
                    _context.SelectedProducts.Add(newSelectedProduct);
                }
            }
            else
            {
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


        [HttpGet]
        [Route("Order/Create")]
        public async Task<IActionResult> Create()
        {

            List<Product> products = await _context.Products.ToListAsync();
            Order order = new Order();
            var tuple = Tuple.Create(products, order);
            return View(tuple);
        }

        [HttpPost]
        [Route("Order/Create")]
        public async Task<IActionResult> Create(int number, string name)
        {

            if (ModelState.IsValid)
            {
                List<SelectedProduct> selectedProducts = await _context.SelectedProducts.Include(sp => sp.Product).ToListAsync();
                if (selectedProducts.Count() != 0)
                {
                    Order newOrder = new Order()
                    {
                        CustomerName = name,
                        TableNo = number,
                        Products = selectedProducts
                    };

                    Console.WriteLine("");
                    Console.WriteLine(selectedProducts.First().Product?.Name);
                    Console.WriteLine("");

                    _context.Orders.Add(newOrder);


                    await _context.SaveChangesAsync();
                    return RedirectToAction("MyOrder");
                }
                else
                {
                    List<Product> products = await _context.Products.ToListAsync();
                    Order order = new Order();
                    var tuple = Tuple.Create(products, order);
                    return View(tuple);
                }
            }
            else
            {
                List<Product> products = await _context.Products.ToListAsync();
                Order order = new Order();
                var tuple = Tuple.Create(products, order);
                return View(tuple);
            }

        }

        [HttpGet]
        public async Task<IActionResult> MyOrder()
        {
            List<Order>? ordersWithItems = await _context.Orders.Include(o => o.Products!).ThenInclude(p => p.Product).ToListAsync();

            return View(ordersWithItems.Last());
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();  //404 error

            var st = await _context.Orders.FindAsync(id);

            if (st == null)
                return NotFound();

            return View(st);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Order model)
        {
            if (id == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction("MyOrder");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var o = await _context.Orders.FindAsync(id);
            return View(o);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var allSP = await _context.SelectedProducts.Include(sp => sp.Product).ToListAsync();
            _context.SelectedProducts.RemoveRange(allSP); 
            var o = await _context.Orders.FindAsync(id);
            if (o != null)
                _context.Orders.Remove(o);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create");
        }
    }
}