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
            var isAuthenticated = HttpContext.Session.GetString("IsAuth");

            if (Convert.ToBoolean(isAuthenticated))
            {
                return RedirectToAction("OrderList");
            }

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
                    barista.IsAuthenticated = true;
                    HttpContext.Session.SetString("IsAuth", barista.IsAuthenticated.ToString());
                    return RedirectToAction("OrderList");
                }
                else
                {
                    TempData["Error Message"] = "Invalid email or password!";
                }
            }
            return View();
        }

        //create new admin

        public IActionResult Signup()
        {
            var isAuthenticated = HttpContext.Session.GetString("IsAuth");

            if (!Convert.ToBoolean(isAuthenticated))
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup([Bind("Email,Password")] Barista barista)
        {
            if (ModelState.IsValid)
            {
                var Barista = _context.FindByEmail(barista.Email!);

                if (Barista == null)
                {
                    _context.Add(barista);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("IsAuth", "false");
                    return RedirectToAction(nameof(Login));

                }
                else
                {
                    TempData["Error Message"] = "Barista is already added";
                }
            }
            return View(barista);
        }

        //SHOW ORDERS
        [HttpGet]
        public async Task<IActionResult> OrderList()
        {
            var isAuthenticated = HttpContext.Session.GetString("IsAuth");

            if (!Convert.ToBoolean(isAuthenticated))
            {
                return RedirectToAction("Login");
            }

            _context.ClearNullOrders();

            var orders = await _context.Orders.ToListAsync();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetails(int? id)
        {
            var isAuthenticated = HttpContext.Session.GetString("IsAuth");

            if (!Convert.ToBoolean(isAuthenticated))
            {
                return RedirectToAction("Login");
            }

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
            var isAuthenticated = HttpContext.Session.GetString("IsAuth");

            if (!Convert.ToBoolean(isAuthenticated))
            {
                return RedirectToAction("Login");
            }

            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var order = await _context.Orders.FindAsync(id);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int orderId)
        {
            List<OrderProduct> orderProducts = await _context.OrderProducts
            .Where(op => op.OrderId == orderId)
            .ToListAsync();
            _context.OrderProducts.RemoveRange(orderProducts);

            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
                _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            return RedirectToAction("OrderList", "Barista");
        }
    }
}