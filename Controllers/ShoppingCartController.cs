using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Week2.Models;
using Week2.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Week2.Data;
using Microsoft.EntityFrameworkCore;
using Week2.Services;

namespace Week2.Controllers
{    public class ShoppingCartController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public ShoppingCartController(
            IBookRepository bookRepository,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService)
        {
            _bookRepository = bookRepository;
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            if (cart.Items == null || cart.Items.Count == 0)
            {
                return View("EmptyCart");
            }
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int id, int quantity)
        {
            var book = _bookRepository.GetById(id);
            if (book == null)
            {
                return NotFound();
            }

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            
            if (cart.Items == null)
            {
                cart.Items = new System.Collections.Generic.List<CartItem>();
            }
            
            cart.AddItem(new CartItem 
            { 
                BookId = book.Id, 
                Title = book.Title, 
                Author = book.Author,
                Price = (decimal)book.Price,
                Quantity = quantity,
                Cover = book.Cover
            });
            
            HttpContext.Session.SetObjectAsJson("Cart", cart);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveItem(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart != null)
            {
                cart.RemoveItem(id);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }
            return RedirectToAction("Index");
        }
        
        [Authorize]
        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || cart.Items == null || cart.Items.Count == 0)
            {
                return RedirectToAction("Index");
            }
            
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || cart.Items == null || cart.Items.Count == 0)
            {
                return RedirectToAction("Index");
            }
            
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Checkout", "ShoppingCart") });
                }
                
                // Tạo Order
                order.UserId = user.Id;
                order.OrderDate = DateTime.Now;
                order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
                
                // Tạo OrderDetails
                if (order.OrderDetails == null)
                {
                    order.OrderDetails = new System.Collections.Generic.List<OrderDetail>();
                }
                
                foreach (var item in cart.Items)
                {
                    var orderDetail = new OrderDetail
                    {
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };
                    order.OrderDetails.Add(orderDetail);
                }                // Lưu vào database
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                
                // Gửi email xác nhận đơn hàng
                try
                {
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        await _emailService.SendOrderConfirmationEmailAsync(
                            user.Email, 
                            user.Name, 
                            order.Id, 
                            order.TotalPrice, 
                            "send2");
                    }
                }
                catch (Exception emailEx)
                {
                    // Log lỗi nhưng không làm gián đoạn quá trình thanh toán
                    Console.WriteLine($"Lỗi khi gửi email: {emailEx.Message}");
                }
                
                // Xóa cart
                HttpContext.Session.Remove("Cart");
                
                return View("OrderComplete", order.Id);
            }
            catch (Exception ex)
            {
                // Log lỗi và hiển thị thông báo
                ModelState.AddModelError(string.Empty, "Lỗi khi xử lý đơn hàng: " + ex.Message);
                return View(order);
            }
        }
    }
}
