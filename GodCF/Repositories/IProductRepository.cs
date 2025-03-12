using GodCF.Models;

namespace GodCF.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAllWithImages();
        Product GetById(int id);
        IEnumerable<Product> GetByCategory(int categoryId);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}