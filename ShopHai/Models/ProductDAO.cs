namespace ShopHai.Models
{
    public class ProductDAO
    {
        private ShopClothesContext _ctx;
        public ProductDAO(ShopClothesContext ctx)
        {
            _ctx =ctx;
        }
        public Product getProductById(int id)
        {
            using (var context = new ShopClothesContext())
            {
                return context.Products.FirstOrDefault(t => t.ProductId == id);
            }
        }

        public List<Product> getAllProduct()
        {
            return _ctx.Products.ToList();
        }
        public List<Product> getTop5Price()
        {
            return _ctx.Products.OrderByDescending(x => x.Price).Take(5).ToList();
        }
        public Boolean CreateProduct(Product product)
        {
            _ctx.Products.Add(product);
            _ctx.SaveChanges();
            return true;
        }
        public Boolean UpdateProduct(Product updateProduct)
        {
            //1 findByID
            Product product = _ctx.Products.Where(x => x.ProductId == updateProduct.ProductId).SingleOrDefault();
            //2 change data
            product.ProductName = updateProduct.ProductName;
            product.ProductImage = updateProduct.ProductImage;
            product.Price = updateProduct.Price;
            product.Brand = updateProduct.Brand;
            product.Color = updateProduct.Color;
            product.Size = updateProduct.Size;
            product.CateId = updateProduct.CateId;
            product.Rating = updateProduct.Rating;
            product.Description = updateProduct.Description;
            //3 
            _ctx.Products.Update(product);
            _ctx.SaveChanges();
            return true;
        }
        public Boolean DeleteProduct(Int16 ProductId)
        {
            Product product = _ctx.Products.Where(x => x.ProductId == ProductId).SingleOrDefault();
            _ctx.Products.Remove(product);
            _ctx.SaveChanges();
            return true;
        }
        public List<Product> searchProductByName(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return _ctx.Products.ToList();
            }
            else
            {
                return _ctx.Products.Where(p => p.ProductName.Contains(productName)).ToList();

            }
        }

        public List<Product> GetAllProductById(Int16 id)
        {
            return _ctx.Products.Where(x => x.CateId == id).ToList();
        }
        public List<Product> SortProductsByPriceDescending()
        {
            return _ctx.Products.OrderByDescending(x => x.Price).ToList();
        }

    }
}

