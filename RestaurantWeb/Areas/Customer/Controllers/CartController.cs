using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantWeb.DataAccess.Data;
using RestaurantWeb.Web.Models;
using System.Security.Claims;
using Web.DataAccess.Repository.IRepository;
using Web.Models;

namespace RestaurantWeb.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        [Area("Customer")]
       public IActionResult Index()
       {
           return View();
       }
    }
}
