using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopHai.Models;
using ShopHai.Repository;

namespace ShopHai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;

        public ProductController(ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        
        public IActionResult ViewAllProduct()
        {
            var q1 = from c in _categoryRepository.GetAll()
                     select new SelectListItem()
                     {
                         Text = c.CateName,
                         Value = c.CateId.ToString(),
                     };

            ViewBag.CateId = q1.ToList();
            //1 get data
            List<Product> lst = _productRepository.GetAll();
            //2 data to view
            return View("ViewAllProduct", lst);
        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            var q1 = from c in _categoryRepository.GetAll()
                     select new SelectListItem()
                     {
                         Text = c.CateId.ToString(),
                         Value = c.CateId.ToString(),
                     };
           
            ViewBag.CateId = q1.ToList();
            return View("CreateProduct", new Product());
        }
        [HttpPost]
        public IActionResult SaveProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                bool isProductNameExist = _productRepository.checkname(product.ProductName);
                if (isProductNameExist)
                {
                    ModelState.AddModelError(string.Empty, "tên đã có");
                    return View("CreateProduct");
                }
                _productRepository.Create(product);

                return RedirectToAction("ViewAllProduct");

            }
            else
            {
                return View("CreateProduct");
            }
        }
        public IActionResult EditProduct(Int16 id)
        {
            var q1 = from c in _categoryRepository.GetAll()
                     select new SelectListItem()
                     {
                         Text = c.CateName,
                         Value = c.CateId.ToString(),
                     };

            ViewBag.CateId = q1.ToList();

            return View("EditProduct", _productRepository.FindById(id));
        }
        [HttpPost]
        public IActionResult UpdateProduct(Product product)
        {
           
            if (ModelState.IsValid)
            {
                bool isProductNameExist = _productRepository.checkname(product.ProductName);
                if (isProductNameExist)
                {
                    ModelState.AddModelError(string.Empty, "tên đã có");
                    return View("EditProduct");


                }
                _productRepository.Update(product);

                return RedirectToAction("ViewAllProduct");

            }
            else
            {
                return View("EditProduct");
            }
        }
        public IActionResult Delete(Int16 productId)
        {
            _productRepository.Delete(productId);
            return RedirectToAction("ViewAllProduct");
        }
        public IActionResult DeleteProduct(Int16 id)
        {
            return View("DeleteProduct", _productRepository.FindById(id));
        }

    }
}
