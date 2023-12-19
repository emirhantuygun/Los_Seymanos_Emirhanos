using CafeApp.Data;
using CafeApp.Models;
using Microsoft.AspNetCore.Components;
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

        [HttpPost]
        public async Task<IActionResult> AddToBag(int productId, int orderId)
        {

            var existingOrderProduct = _context.OrderProducts.FirstOrDefault(op => op.OrderId == orderId && op.ProductId == productId);

            if (existingOrderProduct != null)
            {
                existingOrderProduct.Quantity++;
            }
            else
            {
                var newOrderProduct = new OrderProduct
                {
                    OrderId = orderId,
                    ProductId = productId,
                    Quantity = 1
                };

                _context.OrderProducts.Add(newOrderProduct);
            }

            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            List<Product> products = await _context.Products.ToListAsync();
            Order order = new Order();
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            var tuple = Tuple.Create(products, order);
            return View(tuple);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int orderId, int tableNo, string customerName, decimal totalPrice)
        {

            if (ModelState.IsValid)
            {

                var order = await _context.Orders.FindAsync(orderId);

                if (order != null)
                {
                    order.TableNo = tableNo;
                    order.CustomerName = customerName;
                    order.OrderDate = DateTime.Now;
                    order.TotalPrice = totalPrice;
                }
                else
                {
                    return Json(new { success = false });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("MyOrder", new { id = orderId });
            }
            else
            {
                return Json(new { success = false });
            }

        }

        [HttpGet]
        public async Task<IActionResult> MyOrder(int id)
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


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            var order = await _context.Orders.FindAsync(id);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int? id, Order model)
        {
            if (id == null)
                return NotFound();

            Console.WriteLine("");
            Console.WriteLine(id);
            Console.WriteLine("");

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


        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(Order order)
        {
            if (ModelState.IsValid)
            {
                var existingOrder = await _context.Orders.FindAsync(order.OrderId);
                if (existingOrder != null)
                {
                    existingOrder.IsServed = order.IsServed;
                    existingOrder.IsPaid = order.IsPaid;
                    // Diğer güncellemeler...

                    _context.Update(existingOrder);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("OrderList", "Barista");
                }
            }
            return View(order);
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

            List<OrderProduct> orderProducts = _context.OrderProducts
            .Where(op => op.OrderId == orderId)
            .ToList();
            _context.OrderProducts.RemoveRange(orderProducts);

            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
                _context.Orders.Remove(order);

            await _context.SaveChangesAsync();

            return RedirectToAction("Success");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}