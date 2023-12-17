using CafeApp.Data;
using Microsoft.AspNetCore.Mvc;
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
                    TempData["Error Message"] = "Invalid email or password";

                    
                }

            }

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