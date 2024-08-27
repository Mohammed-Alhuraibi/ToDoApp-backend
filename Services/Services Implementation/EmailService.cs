using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ToDo.Models;

public class EmailService : IEmailService
{
    private readonly string _email;
    private readonly string _password;

    public EmailService(IConfiguration configuration)
    {
        _email = configuration["EmailSettings:Email"] ?? throw new ArgumentNullException(nameof(configuration), "Email not found in configuration");
        _password = configuration["EmailSettings:Password"] ?? throw new ArgumentNullException(nameof(configuration), "Password not found in configuration");
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            var client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential(_email, _password),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_email),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await client.SendMailAsync(mailMessage);
            // Schedule confirmation code deletion after 2 minutes

        }
        catch (SmtpException ex)
        {
            // Log detailed SMTP error message
            Console.WriteLine($"SMTP Error: {ex.StatusCode} - {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            // Log general error message
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

}
