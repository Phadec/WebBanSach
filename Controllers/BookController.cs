using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Week2.Models;
using Week2.Repositories.Interfaces;

namespace Week2.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookController(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var books = _bookRepository.GetAll();
            return View(books);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Book book)
        {
            if (ModelState.IsValid)
            {
                // Ensure the cover path starts with the correct folder
                if (!string.IsNullOrEmpty(book.Cover) && !book.Cover.StartsWith("images/"))
                {
                    book.Cover = "images/" + book.Cover.TrimStart('/');
                }

                _bookRepository.Add(book);
                _bookRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View(book);
        }

        public IActionResult Update(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", book.CategoryId);
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Ensure the cover path starts with the correct folder
                if (!string.IsNullOrEmpty(book.Cover) && !book.Cover.StartsWith("images/"))
                {
                    book.Cover = "images/" + book.Cover.TrimStart('/');
                }

                _bookRepository.Update(book);
                _bookRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name", book.CategoryId);
            return View(book);
        }

        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _bookRepository.Delete(id);
            _bookRepository.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}
