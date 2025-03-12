using GodCF.Data;
using GodCF.Models;
using Microsoft.EntityFrameworkCore;

namespace GodCF.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories
                .Include(c => c.Products)
                .Where(c => !c.IsDeleted)
                .ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                category.IsDeleted = true;
                _context.Categories.Update(category);
                _context.SaveChanges();
            }
        }
    }
}