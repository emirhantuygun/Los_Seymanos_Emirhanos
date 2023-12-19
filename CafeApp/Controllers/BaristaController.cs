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
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
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