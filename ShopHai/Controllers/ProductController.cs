
using Microsoft.AspNetCore.Mvc;
using ShopHai.Models;
using System.Diagnostics;
namespace ShopHai.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ShopClothesContext _ctx;
        private ProductDAO _productDAO;

        public ProductController(ILogger<HomeController> logger, ShopClothesContext ctx)
        {
            _logger = logger;
            _ctx = ctx;
            _productDAO = new ProductDAO(_ctx);
        }

        public IActionResult Index()
        {
            //1.Call dao
            List<Product> ds = _productDAO.SortProductsByPriceDescending();
            //2.passing data
            return View(ds);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
