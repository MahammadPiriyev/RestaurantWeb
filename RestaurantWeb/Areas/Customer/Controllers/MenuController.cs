using Microsoft.AspNetCore.Mvc;
using RestaurantWeb.DataAccess.Data;
using RestaurantWeb.Web.Models;

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
            List<Product> productList = _db.Products.ToList();
            return View(productList);
        }
    }
}
