using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopHai.Models;
using ShopHai.Repository;

namespace ShopHai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InvoiceController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private IUserRepository _userRepository;
        public InvoiceController( IUserRepository userRepository, IInvoiceRepository invoiceRepository)
        {
            _userRepository = userRepository;
            _invoiceRepository = invoiceRepository;
        }

        public IActionResult ViewAllInvoice()
        {
            var q1 = from c in _userRepository.GetAll()
                     select new SelectListItem()
                     {
                         Text = c.UserId.ToString(),
                         Value = c.UserId.ToString(),
                     };

            ViewBag.UserId = q1.ToList();
            //1 get data
            List<Invoice> lst = _invoiceRepository.GetAll();
            //2 data to view
            return View("ViewAllInvoice", lst);
        }
        [HttpGet]
        public IActionResult CreateInvoice()
        {
            var q1 = from c in _userRepository.GetAll()
                     select new SelectListItem()
                     {
                         Text = c.UserId.ToString(),
                         Value = c.UserId.ToString(),
                     };

            ViewBag.UserId = q1.ToList();
            return View("CreateInvoice", new Invoice());
        }
        [HttpPost]
        public IActionResult SaveInvoice(Invoice invoice)
        {
            
                return View("CreateInvoice");
            
        }
        public IActionResult EditInvoice(Int16 id)
        {
            var q1 = from c in _userRepository.GetAll()
                     select new SelectListItem()
                     {
                         Text = c.UserId.ToString(),
                         Value = c.UserId.ToString(),
                     };
          

            ViewBag.UserId = q1.ToList();
            
            return View("EditInvoice", _invoiceRepository.FindById(id));
        }
        [HttpPost]
        public IActionResult UpdateInvoice(Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật trạng thái của hóa đơn
                switch (invoice.Status)
                {
                    case 0:
                        invoice.Status = 0;
                        break;
                    case 1:
                        invoice.Status = 1; ;
                        break;
                    case 2:
                        invoice.Status = 2; ;
                        break;
                    default:
                        break;
                }

                _invoiceRepository.Update(invoice); // Cập nhật hóa đơn trong cơ sở dữ liệu
                return RedirectToAction("ViewAllInvoice");
            }

            // Nếu dữ liệu không hợp lệ, quay trở lại trang sửa hóa đơn với dữ liệu hiện tại
            var q1 = from c in _userRepository.GetAll()
                     select new SelectListItem()
                     {
                         Text = c.UserId.ToString(),
                         Value = c.UserId.ToString(),
                     };

            ViewBag.UserId = q1.ToList();
            return View("EditInvoice", invoice);
        }

        public IActionResult Delete(Int16 invoiceId)
        {
            _invoiceRepository.Delete(invoiceId);
            return RedirectToAction("ViewAllInvoice");
        }
        public IActionResult DeleteInvoice(Int16 id)
        {
            return View("DeleteInvoice", _invoiceRepository.FindById(id));
        }
    }
}
