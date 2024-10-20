using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantWeb.DataAccess.Data;
using RestaurantWeb.Web.Models;
using RestaurantWeb.Web.Utility;
using System.Reflection;
using Web.DataAccess.Repository.IRepository;

namespace RestaurantWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IProductRepository productRepo, ICategoryRepository categoryRepo, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> ProductList = _productRepo.GetWithInclude(c => c.Category).ToList();
            return View(ProductList);
        }
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> ProductList = _categoryRepo.Select(u => new
            SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.ProductList = ProductList;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj, IFormFile? file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    // Generate unique file name
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Construct path to the images/product folder inside wwwroot
                    string productPath = Path.Combine(wwwRootPath, "images/product");

                    // Ensure the directory exists
                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }

                    // Save the file to the constructed path
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Save the relative image path to the database
                    obj.ImageUrl = @"\images\product\" + fileName;
                }
                _productRepo.Add(obj);
                _productRepo.Save();
                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction("Index");
                
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            IEnumerable<SelectListItem> ProductList = _categoryRepo.Select(u => new
            SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
            ViewBag.ProductList = ProductList;

            Product productFromDb = _productRepo.Get(i => i.Id == id);
            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj, IFormFile? file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    // Generate unique file name
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    // Construct path to the images/product folder inside wwwroot
                    string productPath = Path.Combine(wwwRootPath, "images/product");

                    // Ensure the directory exists
                    if (!Directory.Exists(productPath))
                    {
                        Directory.CreateDirectory(productPath);
                    }
                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImageUrl = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImageUrl))
                        {
                            System.IO.File.Delete(oldImageUrl);
                        }
                    }

                    // Save the file to the constructed path
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Save the relative image path to the database
                    obj.ImageUrl = @"\images\product\" + fileName;
                }
                _productRepo.Update(obj);
                _productRepo.Save();
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpGet]
        public IActionResult GetAll(int? id)
        {
            List<Product> ProductList = _productRepo.GetWithInclude(c => c.Category).ToList();
            return Json(ProductList);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var productToBeDeleted = _productRepo.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Was not deleted!" });
            }

            if (!string.IsNullOrEmpty(productToBeDeleted.ImageUrl))
            {
                var oldImageUrl = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImageUrl))
                {
                    System.IO.File.Delete(oldImageUrl);
                }
            }

            _productRepo.Remove(productToBeDeleted);
            _productRepo.Save();

            return Json(new { success = true, message = "Successfully deleted!" });

        }
    }
}
