using GodCF.Data;
using GodCF.Models;
using Microsoft.EntityFrameworkCore;

namespace GodCF.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products
                .Include(p => p.Category)
                .ToList();
        }

        public IEnumerable<Product> GetAllWithImages()
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToList();
        }

        public Product GetById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId)
                .ToList();
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            // Handle collection navigation property
            foreach (var image in product.Images)
            {
                if (image.Id == 0)
                    _context.ProductImages.Add(image);
                else
                    _context.Entry(image).State = EntityState.Modified;
            }

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products
                .Include(p => p.Images)
                .FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                // Remove associated images first
                _context.ProductImages.RemoveRange(product.Images);

                // Then remove the product
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}