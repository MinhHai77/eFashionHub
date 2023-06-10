using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopHai.Models;
using ShopHai.Repository;

namespace ShopHai.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IActionResult ViewAllUser()
        {
            //1 get data
            List<User> lst = _userRepository.GetAll();
            //2 data to view
            return View("ViewAllUser", lst);
        }
        [HttpGet]
        public IActionResult CreateUser()
        {
            
            return View("CreateUser", new User());
        }
        [HttpPost]
        public IActionResult SaveUser(User user)
        {
            if (ModelState.IsValid)
            {
                bool isUserNameExist = _userRepository.checkname(user.UserName);
                if (isUserNameExist)
                {
                    ModelState.AddModelError(string.Empty, "tên đã có");
                    return View("CreateUser");
                }
                _userRepository.Create(user);

                return RedirectToAction("ViewAllUser");

            }
            else
            {
                return View("CreateUser");
            }
        }
        public IActionResult EditUser(Int16 id)
        {
           

            return View("EditUser", _userRepository.FindById(id));
        }
        [HttpPost]
        public IActionResult UpdateUser(User user)
        {

            if (ModelState.IsValid)
            {
                bool isUserNameExist = _userRepository.checkname(user.UserName);
                if (isUserNameExist)
                {
                    ModelState.AddModelError(string.Empty, "tên đã có");
                    return View("EditUser");


                }
                _userRepository.Update(user);

                return RedirectToAction("ViewAllUser");

            }
            else
            {
                return View("EditUser");
            }
        }
        
        public IActionResult Delete(Int16 userId)
        {
            _userRepository.Delete(userId);
            return RedirectToAction("ViewAllUser");
        }
        public IActionResult DeleteUser(Int16 id)
        {
            return View("DeleteUser", _userRepository.FindById(id));
        }

    }
}
