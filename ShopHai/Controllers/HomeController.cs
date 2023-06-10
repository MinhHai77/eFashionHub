using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopHai.Models;
using System.Diagnostics;
using System.Security.Claims;
using ShopHai.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShopHai.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private ShopClothesContext _ctx;
        private ProductDAO _productDAO;
        private readonly UserDAO _userDAO;
        private readonly CartDAO _cartDAO;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IInvoiceRepository invoiceRepository, ICategoryRepository categoryRepository,ILogger<HomeController> logger, ShopClothesContext ctx, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _ctx = ctx;
            _productDAO = new ProductDAO(_ctx);
            _userDAO = new UserDAO(_ctx);
            _cartDAO = new CartDAO(_ctx);
            _categoryRepository = categoryRepository;
            _httpContextAccessor = httpContextAccessor;
            _invoiceRepository = invoiceRepository;
        }

        public IActionResult Index()
        {
            //1. Call dao
            List<Product> ds = _productDAO.getAllProduct();
            //2. Passing data
            return View(ds);
        }
        public IActionResult Detail(int id)
        {
           
            //1. Call dao
            Product product = _productDAO.getProductById(id);
            //2. Passing data
            return View(product);
        }
        public IActionResult Search(string productName)
        {
            List<Product> products = _productDAO.searchProductByName(productName);
            return View(products);
        }
        public IActionResult findProductByCategoryId(Int16 id)
        {
            List<Product> products = _productDAO.GetAllProductById(id);
            return View(products);
        }
        private void SetAuthenticationCookie(int userId)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
    };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                // Thiết lập các thuộc tính của cookie (nếu cần)
            };

            _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties).Wait();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Phương thức xử lý đăng ký
        [HttpPost]
        public IActionResult Register(User user)
        {
            // Kiểm tra xem người dùng đã tồn tại hay chưa
            if (_userDAO.GetUserByEmail(user.Email) != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View();
            }

            // Kiểm tra và gán giá trị mặc định cho IsAdmin và Role
            user.IsAdmin = user.IsAdmin ?? 0;
            user.Role = user.Role ?? "khách";

            // Thêm người dùng mới vào cơ sở dữ liệu
            _userDAO.AddUser(user);

            // Chuyển hướng đến trang đăng nhập hoặc trang chính
            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            User existingUser = _userDAO.GetUserByEmail(user.Email);

            if (existingUser == null || existingUser.Password != user.Password)
            {
                ModelState.AddModelError("LoginError", "Invalid email or password");
                return View();
            }

            if (existingUser.IsAdmin == 1)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                SetAuthenticationCookie(existingUser.UserId);
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public IActionResult Logout()
        {
            // Xóa thông tin đăng nhập khỏi session hoặc cookie
            // Ví dụ:
             HttpContext.Session.Clear(); // Xóa tất cả các dữ liệu trong session
             HttpContext.SignOutAsync(); // Đăng xuất người dùng

            // Chuyển hướng đến trang chủ 
            return RedirectToAction("Index");
        }


        private int GetUserId()
        {
            if (_httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity identity && identity.IsAuthenticated)
            {
                // Lấy thông tin user ID từ cookie
                var userIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    return userId;
                }
            }

            return 0;
        }


        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                // Người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login");
            }

            // Lấy sản phẩm từ cơ sở dữ liệu
            Product product = _productDAO.getProductById(productId);

            if (product == null)
            {
                return NotFound();
            }

            // Thêm sản phẩm vào giỏ hàng
            int userId = GetUserId();
            _cartDAO.AddToCart(product, quantity, userId);

            // Chuyển hướng đến trang giỏ hàng
            return RedirectToAction("Cart");
        }


        public IActionResult Cart()
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                // Người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login");
            }

            // Lấy danh sách sản phẩm trong giỏ hàng của người dùng hiện tại
            int userId = GetUserId();
            List<Cart> cartItems = _cartDAO.GetCartItemsByUserId(userId);

            // Passing data
            return View(cartItems);
        }


        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            // Get the current user ID
            int userId = GetUserId();

            // Find the cart item for the user and product
            var cartItem = _ctx.Carts.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);

            if (cartItem != null)
            {
                // Remove the cart item from the database
                _ctx.Carts.Remove(cartItem);
                _ctx.SaveChanges();
            }

            // Redirect back to the Cart page
            return RedirectToAction("Cart");
        }
        private string GenerateInvoiceNumber()
        {
            // Generate a unique invoice number based on your requirements
            // For simplicity, you can use a combination of a static prefix and a random number or a timestamp

            // Example: Prefix + Random number
            string prefix = "INV";
            Random random = new Random();
            string invoiceNumber = prefix + random.Next(10000, 99999).ToString();

            return invoiceNumber;
        }


        public IActionResult PrintInvoice(Int16 invoiceId)
        {
            // Get the current user ID
            int userId = GetUserId();

            // Get the cart items for the current user
            List<Cart> cartItems = _ctx.Carts.Include(c => c.Product).Where(c => c.UserId == userId).ToList();

            // Calculate the total amount
            decimal totalAmount = (decimal)cartItems.Sum(c => c.Price * c.Quantity);

            // Generate the invoice number
            string invoiceNumber = GenerateInvoiceNumber();
            // Cập nhật trạng thái thanh toán cho các mục giỏ hàng
            foreach (var cartItem in cartItems)
            {
                cartItem.IsPaid = true;
            }
            // Create an Invoice object
            Invoice invoice = new Invoice
            {
                CartId = cartItems.FirstOrDefault()?.CartId,
                UserId = userId,
                TotalAmount = totalAmount,
                InvoiceDate = DateTime.Now,
                Status = 0
            };

            // Save the invoice to the database
            _ctx.Invoices.Add(invoice);
            _ctx.SaveChanges();

            // Pass the invoice data to the view
            ViewData["InvoiceNumber"] = invoiceNumber;
            ViewData["CartItems"] = cartItems;
            ViewData["TotalAmount"] = totalAmount;

            return View(invoice);
        }

        public IActionResult Order()
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                // Người dùng chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login");
            }
            // Lấy User ID hiện tại
            int userId = GetUserId();

            // Lấy danh sách Invoice của User từ Repository
            List<Invoice> invoices = _invoiceRepository.GetInvoicesByUserId(userId);

            return View(invoices);
        }

        [HttpPost]

        public IActionResult DeleteCartByInvoiceStatus(int status)
        {
            // Find the carts with the specified invoice status
            var cartsToDelete = _ctx.Carts.Include(c => c.Invoices)
            .Where(c => c.Invoices.Any(i => i.Status == status))
            .ToList();
            // Remove the carts
            _ctx.Carts.RemoveRange(cartsToDelete);

            // Save changes
            _ctx.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult UpdateInvoiceStatus(int invoiceId, int status)
        {
            var invoice = _ctx.Invoices.FirstOrDefault(i => i.InvoiceId == invoiceId);
            if (invoice != null)
            {
                invoice.Status = status;
                _ctx.SaveChanges();
            }

            return RedirectToAction("Index");

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