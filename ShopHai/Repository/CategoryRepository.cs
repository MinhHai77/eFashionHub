using ShopHai.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace ShopHai.Repository
{
    public interface ICategoryRepository
    {
        bool Create(Category category);
        bool Update(Category category);
        bool Delete(Int16 cateId);
        List<Category> GetAll();
        public Category FindById(Int16 id);
        public bool checkname(string name);

    }
    public class CategoryRepository : ICategoryRepository
    {
        private ShopClothesContext _ctx;

        public CategoryRepository(ShopClothesContext ctx)
        {
            _ctx = ctx;
        }
        public bool Create(Category category)
        {
            _ctx.Categories.Add(category);
            _ctx.SaveChanges();
            return true;
        }

        public bool Delete(Int16 cateId)
        {
            Category c = _ctx.Categories.FirstOrDefault(x => x.CateId == cateId);
                _ctx.Categories.Remove(c);
                _ctx.SaveChanges();
                return true;
           
        }


        public List<Category> GetAll()
        {
            return _ctx.Categories.ToList();
        }

        public bool Update(Category category)
        {
            Category c = _ctx.Categories.FirstOrDefault(x => x.CateId == category.CateId);
            if (c != null)
            {
                _ctx.Entry(c).CurrentValues.SetValues(category);
                _ctx.SaveChanges();
            }
            return true;
        }
        public Category FindById(Int16 id)
        {
            Category c = _ctx.Categories.FirstOrDefault(x => x.CateId == id);
            return c;
        }

        public bool checkname(string name)
        {
            Category c = _ctx.Categories.Where(x => x.CateName.Trim() == name.Trim()).FirstOrDefault();
            if (c == null)
                return false;
            else
                return true;
        }


    }
}
