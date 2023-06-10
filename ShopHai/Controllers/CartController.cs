using Microsoft.AspNetCore.Mvc;

namespace ShopHai.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
