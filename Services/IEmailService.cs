using System.Threading.Tasks;

namespace Week2.Services
{
    public interface IEmailService
    {
        Task SendOrderConfirmationEmailAsync(string email, string customerName, int orderId, decimal totalAmount, string template = "send2");
    }
}
