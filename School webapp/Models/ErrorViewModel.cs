using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentitySchoolWebap.Models
{
    public class CustomEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}