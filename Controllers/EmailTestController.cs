using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Week2.Services;

namespace Week2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailTestController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailTestController> _logger;

        public EmailTestController(IEmailService emailService, ILogger<EmailTestController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpGet("test/{orderId}")]
        public async Task<IActionResult> TestSendEmail(int orderId, string email = null)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    // Default test email if not provided
                    email = "test@example.com";
                }

                await _emailService.SendOrderConfirmationEmailAsync(
                    email,
                    "Test Customer",
                    orderId,
                    1000000,
                    "send2");

                return Ok($"Test email sent to {email} for order #{orderId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending test email");
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }
    }
}
