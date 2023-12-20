using CafeApp.Data;
using CafeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProject.Models;
namespace CafeApp.Controllers
{

    public class BaristaController : Controller
    {
        private readonly DataContext _context;
        public BaristaController(DataContext cont)
        {
            _context = cont;
        }


        //ADMİN LOGIN

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Barista model)
        {
            if (model.Email != null)
            {
                var barista = _context.FindByEmail(model.Email);

                if (barista != null && barista.Password == model.Password)
                {
                    return RedirectToAction("OrderList");
                }
                else{

                    TempData["Error Message"] = "Invalid email or password!";
                }

            }

            return View();
        }

        //create new admin

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup([Bind("Email,Password")] Barista barista)
        {
            if (ModelState.IsValid)
            {
                _context.Add(barista);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Login)); 
            }
            return View(barista);
        }


        //SHOW ORDERS
        [HttpGet]
        public async Task<IActionResult> OrderList()
        {
            var orders = await _context.Orders.ToListAsync();
            return View(orders);
        }


        [HttpGet]
        public async Task<IActionResult> OrderDetails(int? id)
        {
             var order = await _context.Orders.FindAsync(id);

            List<OrderProduct> orderProducts = _context.OrderProducts
            .Where(op => op.OrderId == id)
            .ToList();

            List<int> productIds = orderProducts.Select(op => op.ProductId).ToList();

            List<Product> products = _context.Products
                .Where(p => productIds.Contains(p.ProductId))
                .ToList();

            Dictionary<int, int> productIdQuantityPairs = orderProducts
                .ToDictionary(op => op.ProductId, op => op.Quantity);

            var tuple = Tuple.Create(order, products, productIds, productIdQuantityPairs);
            return View(tuple);
        }
        
         // GET: Barista/ProductList
         [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
       

    }
}