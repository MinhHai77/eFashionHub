using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopHai.Models;
using ShopHai.Repository;

namespace ShopHai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;

        public HomeController(ICategoryRepository categoryRepository,
            IProductRepository productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ViewAllCategories()
        {
            //1 get data
            List<Category> lst = _categoryRepository.GetAll();
            //2 data to view
            return View("ViewAllCategories", lst);
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View("CreateCategory", new Category());
        }
        [HttpGet]
        public IActionResult CreateToy()
        {
            var q1 = from c in _categoryRepository.GetAll()
                     select new SelectListItem()
                     {
                         Text = c.CateName,
                         Value = c.CateId.ToString()
                     };
            var q2 = from c in _productRepository.GetAll()
                     select new SelectListItem()
                     {
                         Text = c.ProductName,
                         Value = c.ProductId.ToString()
                     };
            ViewBag.CateId = q1.ToList();
            ViewBag.ProductId = q2.ToList();
            return View("CreateToy", new Product());
        }
        [HttpPost]
        public IActionResult SaveCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                bool isCategoryNameExist = _categoryRepository.checkname(category.CateName);
                if (isCategoryNameExist)
                {
                    ModelState.AddModelError(string.Empty, "tên đã có");
                    return View("CreateCategory");
                }
                _categoryRepository.Create(category);

                return RedirectToAction("ViewAllCategories");

            }
            else
            {
                return View("CreateCategory");
            }
        }
        public IActionResult EditCategory(Int16 id)
        {
            return View("EditCategory", _categoryRepository.FindById(id));
        }
        [HttpPost]
        public IActionResult UpdateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                bool isCategoryNameExist = _categoryRepository.checkname(category.CateName);
                if (isCategoryNameExist)
                {
                    ModelState.AddModelError(string.Empty, "tên đã có");
                    return View("EditCategory");


                }
                _categoryRepository.Update(category);

                return RedirectToAction("ViewAllCategories");

            }
            else
            {
                return View("EditCategory");
            }
        }
        public IActionResult Delete(Int16 cateId)
        {
            _categoryRepository.Delete(cateId);
            return RedirectToAction("ViewAllCategories");
        }
        public IActionResult DeleteCategory(Int16 id)
        {
            return View("DeleteCategory", _categoryRepository.FindById(id));
        }
    }
}
