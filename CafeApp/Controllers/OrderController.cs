using CafeApp.Data;
using CafeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> UpdateDBBefore(int orderId)
        {

            var existingOrderProductList = await _context.OrderProducts.Where(op => op.OrderId == orderId).ToListAsync();

            if (existingOrderProductList != null)
                _context.OrderProducts.RemoveRange(existingOrderProductList);


            var order = await _context.Orders.FindAsync(orderId);

            if (order != null)
                _context.Orders.Remove(order);


            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> AddToBag(int productId, int orderId)
        {

            var existingOrderProduct = await _context.OrderProducts.FirstOrDefaultAsync(op => op.OrderId == orderId && op.ProductId == productId);

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

                await _context.OrderProducts.AddAsync(newOrderProduct);
            }

            await _context.SaveChangesAsync();

            var updatedQuantity = await _context.OrderProducts
                .Where(op => op.OrderId == orderId && op.ProductId == productId)
                .Select(op => op.Quantity)
                .FirstOrDefaultAsync();

            return Json(new { success = true, quantity = updatedQuantity });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromBag(int productId, int orderId)
        {

            var existingOrderProduct = await _context.OrderProducts.FirstOrDefaultAsync(op => op.OrderId == orderId && op.ProductId == productId);

            if (existingOrderProduct != null)
            {

                if (existingOrderProduct.Quantity > 0)
                {
                    existingOrderProduct.Quantity--;
                }

                if (existingOrderProduct.Quantity == 0)
                {
                    _context.OrderProducts.Remove(existingOrderProduct);
                }

                await _context.SaveChangesAsync();

                var updatedQuantity = existingOrderProduct.Quantity;

                return Json(new { success = true, quantity = updatedQuantity });
            }

            return Json(new { success = true, quantity = 0 });
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

                var existingOrderList = await _context.Orders.Where(o => o.TableNo == null).ToListAsync();

                if (existingOrderList != null)
                {

                    var orderIds = existingOrderList.Select(op => op.OrderId).ToList();

                    var orderProductWiththeOrderIdList = await _context.OrderProducts
                    .Where(op => orderIds.Contains(op.OrderId))
                    .ToListAsync();

                    _context.OrderProducts.RemoveRange(orderProductWiththeOrderIdList);
                    _context.Orders.RemoveRange(existingOrderList);
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
            List<OrderProduct> orderProducts = _context.OrderProducts
           .Where(op => op.OrderId == id)
           .ToList();

            List<Product> products = await _context.Products.ToListAsync();
            var order = await _context.Orders.FindAsync(id);

            Dictionary<int, int> productIdQuantityPairs = orderProducts
                .ToDictionary(op => op.ProductId, op => op.Quantity);

            var tuple = Tuple.Create(products, order, productIdQuantityPairs);


            return View(tuple);
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


        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, bool isServed, bool isPaid)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }


            order.IsServed = isServed;
            order.IsPaid = isPaid;


            _context.Update(order);
            await _context.SaveChangesAsync();


            return RedirectToAction("OrderList", "Barista");
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

            return RedirectToAction("Success", "Order");
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
    }
}