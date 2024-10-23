using Microsoft.AspNetCore.Mvc;
using RestaurantWeb.DataAccess.Data;
using RestaurantWeb.Web.Models;
using Web.DataAccess.Repository.IRepository;
using Web.Models;

namespace RestaurantWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IProductRepository _productRepo;
        public MenuController(ApplicationDbContext db, IProductRepository _productRepo)
        {
            _db = db;
            _productRepo = _productRepo;
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
        public IActionResult Detail(int id)
        {
            Product productFromDb = _productRepo.Get(c=>c.Id == id);    
            return View(productFromDb); 
        }
    }
}
