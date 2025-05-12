using Microsoft.EntityFrameworkCore;
using Week2.Data;
using Week2.Models;
using Week2.Repositories.Interfaces;

namespace Week2.Repositories
{
    public class EFBookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public EFBookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAll()
        {
            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.Images)
                .ToList();
        }

        public Book GetById(int id)
        {
            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.Images)
                .FirstOrDefault(b => b.Id == id);
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
        }

        public void Update(Book book)
        {
            _context.Entry(book).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book != null)
            {
                _context.Books.Remove(book);
            }
        }

        public bool Save()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
