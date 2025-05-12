using Microsoft.AspNetCore.Mvc;
using Week2.Models;
using Week2.Repositories.Interfaces;

namespace Week2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var books = _bookRepository.GetAll();
            return View(books);
        }

        public IActionResult Detail(int id)
        {
            ShoppingCart cartObj = new()
            {
                Book = _bookRepository.GetById(id),
                Count = 1
            };
            return View(cartObj);
        }
    }
}
