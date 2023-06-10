using Microsoft.EntityFrameworkCore;
using ShopHai.Models;
namespace ShopHai.Repository
{
    public interface IProductRepository
    {
        bool Create(Product product);
        bool Update(Product product);
        bool Delete(Int16 productId);
        List<Product> GetAll();
        public List<Product> GetAllProductsById(Int16 id);
        public bool checkname(string name);
        public Product FindById(Int16 id);
    }
    public class ProductRepository : IProductRepository
    {
        private ShopClothesContext _ctx;

        public ProductRepository(ShopClothesContext ctx)
        {
            _ctx = ctx;
        }

        public bool Create(Product product)
        {
            _ctx.Products.Add(product);
            _ctx.SaveChanges();
            return true;
        }

        public bool Update(Product product)
        {
            Product existingProduct = _ctx.Products.FirstOrDefault(t => t.ProductId == product.ProductId);
            if (existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.Description = product.Description;
                existingProduct.Rating = product.Rating;
                existingProduct.Color = product.Color;
                existingProduct.Brand = product.Brand;
                existingProduct.Size = product.Size;
                existingProduct.ProductImage = product.ProductImage;
                existingProduct.CateId = product.CateId;
                existingProduct.Price = product.Price;

                _ctx.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Delete(Int16 productId)
        {
            Product product = _ctx.Products.FirstOrDefault(t => t.ProductId == productId);
           
                _ctx.Products.Remove(product);
                _ctx.SaveChanges();
                return true;
           
        }

        public List<Product> GetAll()
        {
            return _ctx.Products.ToList();
        }

        public List<Product> GetAllProductsById(Int16 id)
        {
            return _ctx.Products.Where(x => x.CateId == id).ToList();
        }

        public bool checkname(string name)
        {
            Product c = _ctx.Products.Where(x => x.ProductName.Trim() == name.Trim()).FirstOrDefault();
            if (c == null)
                return false;
            else
                return true;
        }

        public Product FindById(short id)
        {
            Product c = _ctx.Products.FirstOrDefault(x => x.ProductId == id);
            return c;
        }
    }
}

