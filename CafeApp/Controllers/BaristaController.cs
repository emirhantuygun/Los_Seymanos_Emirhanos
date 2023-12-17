using CafeApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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


        //SHOW ORDERS

        public IActionResult OrderList()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }



    }
}