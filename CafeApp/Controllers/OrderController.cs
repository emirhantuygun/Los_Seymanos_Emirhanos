using WebProject.Data;
using WebProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebProject.Controllers
{

    public class OrderController : Controller
    {
        private readonly DataContext _context;
        public OrderController(DataContext cont)
        {
            _context = cont;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order model)
        {
            if (ModelState.IsValid)
            {
                model.Items = new List<string> { "Item1", "Item2", "Item3" };
                _context.Orders.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction("MyOrder");
            }

            return View(model);
        }

        public async Task<IActionResult> MyOrder()
        {
            var std = await _context.Orders.ToListAsync();
            return View(std);
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
            var st = await _context.Orders.FindAsync(id);
            return View(st);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            var st = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(st);
            await _context.SaveChangesAsync();
            return RedirectToAction("Create");
        }
    }
}