using Microsoft.EntityFrameworkCore;
using Week2.Data;
using Week2.Models;
using Week2.Repositories.Interfaces;

namespace Week2.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Category> GetAll()
        {
            return _context.Categories
                .Include(c => c.Books)
                .ToList();
        }

        public Category GetById(int id)
        {
            return _context.Categories
                .Include(c => c.Books)
                .FirstOrDefault(c => c.Id == id);
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public void Update(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
