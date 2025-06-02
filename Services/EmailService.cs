using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Week2.Data;
using Week2.Models;

namespace Week2.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public EmailService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task SendOrderConfirmationEmailAsync(string email, string customerName, int orderId, decimal totalAmount, string template = "send2")
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Book)
                    .Include(o => o.User)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    throw new Exception($"Order with ID {orderId} not found");
                }

                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "templates", $"{template}.html");
                string templateContent = await File.ReadAllTextAsync(templatePath);

                // Replace placeholders
                templateContent = templateContent.Replace("{{TenKhachHang}}", customerName);
                templateContent = templateContent.Replace("{{MaDon}}", orderId.ToString());
                templateContent = templateContent.Replace("{{NgayDatHang}}", order.OrderDate.ToString("dd/MM/yyyy HH:mm"));
                
                // Format products
                StringBuilder productsHtml = new StringBuilder();
                decimal subtotal = 0;
                
                foreach (var item in order.OrderDetails)
                {
                    decimal lineTotal = item.Price * item.Quantity;
                    subtotal += lineTotal;
                    
                    productsHtml.AppendLine($@"<tr>
                        <td style=""color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif;word-wrap:break-word"">
                            {item.Book.Title}
                        </td>
                        <td style=""color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif"">
                            {item.Quantity}
                        </td>
                        <td style=""color:#636363;border:1px solid #e5e5e5;padding:12px;text-align:left;vertical-align:middle;font-family:'Helvetica Neue',Helvetica,Roboto,Arial,sans-serif"">
                            <span>{item.Price.ToString("N0")}&nbsp;<span>₫</span></span>
                        </td>
                    </tr>");
                }
                
                templateContent = templateContent.Replace("{{SanPham}}", productsHtml.ToString());
                templateContent = templateContent.Replace("{{ThanhTien}}", subtotal.ToString("N0"));
                templateContent = templateContent.Replace("{{TongTien}}", order.TotalPrice.ToString("N0"));
                  // Customer info
                string addressInfo = string.Join(", ", new[]
                {
                    order.User.StreetAddress,
                    order.User.City,
                    order.User.State,
                    order.User.PostalCode
                }.Where(x => !string.IsNullOrEmpty(x)));
                
                templateContent = templateContent.Replace("{{DiaChi}}", 
                    !string.IsNullOrEmpty(addressInfo) ? addressInfo : "Không có địa chỉ");
                templateContent = templateContent.Replace("{{Phone}}", 
                    !string.IsNullOrEmpty(order.User.PhoneNumber) ? order.User.PhoneNumber : "Không có số điện thoại");
                templateContent = templateContent.Replace("{{Email}}", order.User.Email);

                // Send email
                await SendEmailAsync(email, $"Đơn hàng #{orderId} - Xác nhận đặt hàng thành công", templateContent);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error sending order confirmation email: {ex.Message}");
                throw;
            }
        }        private async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");
            var smtpServer = emailSettings["SmtpServer"] ?? "smtp.gmail.com";
            var smtpPortStr = emailSettings["SmtpPort"] ?? "587";
            var smtpPort = int.Parse(smtpPortStr);
            var smtpUsername = emailSettings["SmtpUsername"] ?? "";
            var smtpPassword = emailSettings["SmtpPassword"] ?? "";
            var senderEmail = emailSettings["SenderEmail"] ?? "no-reply@example.com";
            var senderName = emailSettings["SenderName"] ?? "Web Bán Sách";

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(smtpUsername, smtpPassword);

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(senderEmail, senderName);
                    message.To.Add(new MailAddress(to));
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;

                    await client.SendMailAsync(message);
                }
            }
        }
    }
}
