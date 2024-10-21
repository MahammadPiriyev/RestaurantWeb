using Microsoft.AspNetCore.Mvc;
using RestaurantWeb.DataAccess.Data;
using RestaurantWeb.Web.Models;
using Web.Models;

namespace RestaurantWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _db;
        public MenuController(ApplicationDbContext db)
        {
            _db = db; 
        }
        public IActionResult Index()
        {
            var viewModel = new MenuViewModel
            {
                Categories = _db.Categories.ToList(),
                Products = _db.Products.ToList()
            };

            return View(viewModel);
        }
    }
}
