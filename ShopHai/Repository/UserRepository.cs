using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ShopHai.Models;
namespace ShopHai.Repository
{
    public interface IUserRepository
    {
        bool Create(User user);
        bool Update(User user);
        bool Delete(Int16 userId);
        List<User> GetAll();
        public List<User> GetAllUsersById(Int16 id);
        public bool checkname(string name);
        public User FindById(Int16 id);
    }
    public class UserRepository : IUserRepository
    {
        private ShopClothesContext _ctx;

        public UserRepository(ShopClothesContext ctx)
        {
            _ctx = ctx;
        }
        public bool checkname(string name)
        {
            User c = _ctx.Users.Where(x => x.UserName.Trim() == name.Trim()).FirstOrDefault();
            if (c == null)
                return false;
            else
                return true;
        }

        public bool Create(User user)
        {
            _ctx.Users.Add(user);
            _ctx.SaveChanges();
            return true;
        }

        public bool Delete(Int16 userId)
        {
            User user = _ctx.Users.FirstOrDefault(t => t.UserId == userId);
                _ctx.Users.Remove(user);
                _ctx.SaveChanges();
                return true;
          
        }

        public User FindById(short id)
        {
            User c = _ctx.Users.FirstOrDefault(x => x.UserId == id);
            return c;
        }

        public List<User> GetAll()
        {
            return _ctx.Users.ToList();
        }

        public List<User> GetAllUsersById(short id)
        {
            return _ctx.Users.Where(x => x.UserId == id).ToList();
        }

        public bool Update(User user)
        {
            User existingUser = _ctx.Users.FirstOrDefault(t => t.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Phone = user.Phone;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.Role = user.Role;
                existingUser.IsAdmin = user.IsAdmin;

                _ctx.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
