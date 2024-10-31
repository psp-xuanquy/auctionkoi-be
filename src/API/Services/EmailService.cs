using KoiAuction.Application.Common.Interfaces;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using KoiAuction.Application.Common.Email;

namespace KoiAuction.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendConfirmEmailAsync(string email, string confirmationLink)
        {
            var fromAddress = new MailAddress(_emailSettings.SmtpUser, "Koi Auction");
            var toAddress = new MailAddress(email);
            const string subject = "Email Confirmation";
            var body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8' />
                <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                <title>Email Confirmation</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                        color: #333;
                    }}
                    .email-container {{
                        max-width: 600px;
                        margin: 0 auto;
                        background-color: #ffffff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                    }}
                    .email-header {{
                        text-align: center;
                        padding-bottom: 20px;
                        border-bottom: 1px solid #ddd;
                    }}
                    .email-header h1 {{
                        font-size: 24px;
                        margin: 0;
                    }}
                    .email-body {{
                        padding: 20px;
                        line-height: 1.6;
                    }}
                    .email-footer {{
                        text-align: center;
                        font-size: 12px;
                        color: #888;
                        padding: 20px;
                        border-top: 1px solid #ddd;
                    }}
                    .button {{
                        background-color: #007BFF;
                        color: white;
                        padding: 10px 20px;
                        text-decoration: none;
                        border-radius: 4px;
                    }}
                </style>
            </head>
            <body>
                <div class='email-container'>
                    <div class='email-header'>
                        <h1>Email Confirmation</h1>
                    </div>
                    <div class='email-body'>
                        <p>Hello,</p>
                        <p>Thank you for registering an account on Koi Auction. Please click the link below to confirm your email address:</p>
                        <p style='text-align: center;'>
                            <a href='{confirmationLink}' class='button'>Confirm Email</a>
                        </p>
                        <p>If you did not request this, please disregard this email.</p>
                        <p>Best regards,<br />The Koi Auction Support Team</p>
                    </div>
                    <div class='email-footer'>
                        <p>&copy; 2024 Koi Auction. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>";

            var smtpClient = new SmtpClient(_emailSettings.SmtpHost)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendResetPasswordEmailAsync(string email, string resetLink)
        {
            var fromAddress = new MailAddress(_emailSettings.SmtpUser, "Koi Auction");
            var toAddress = new MailAddress(email);
            const string subject = "Reset Your Password";
            var body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8' />
                <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                <title>Password Reset</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                        color: #333;
                    }}
                    .email-container {{
                        max-width: 600px;
                        margin: 0 auto;
                        background-color: #ffffff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                    }}
                    .email-header {{
                        text-align: center;
                        padding-bottom: 20px;
                        border-bottom: 1px solid #ddd;
                    }}
                    .email-header h1 {{
                        font-size: 24px;
                        margin: 0;
                    }}
                    .email-body {{
                        padding: 20px;
                        line-height: 1.6;
                    }}
                    .email-footer {{
                        text-align: center;
                        font-size: 12px;
                        color: #888;
                        padding: 20px;
                        border-top: 1px solid #ddd;
                    }}
                    .button {{
                        background-color: #007BFF;
                        color: white;
                        padding: 10px 20px;
                        text-decoration: none;
                        border-radius: 4px;
                    }}
                </style>
            </head>
            <body>
                <div class='email-container'>
                    <div class='email-header'>
                        <h1>Reset Your Password</h1>
                    </div>
                    <div class='email-body'>
                        <p>Hello,</p>
                        <p>We received a request to reset your password. Please click the link below to proceed:</p>
                        <p style='text-align: center;'>
                            <a href='{resetLink}' class='button'>Reset Password</a>
                        </p>
                        <p>If you did not request a password reset, please ignore this email.</p>
                        <p>Best regards,<br />The Koi Auction Support Team</p>
                    </div>
                    <div class='email-footer'>
                        <p>&copy; 2024 Koi Auction. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>";

            var smtpClient = new SmtpClient(_emailSettings.SmtpHost)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendWinningEmail(string userEmail, string koiName, decimal bidAmount)
        {
            var fromAddress = new MailAddress(_emailSettings.SmtpUser, "Koi Auction");
            var toAddress = new MailAddress(userEmail);
            const string subject = "Congratulations! You've won the auction!";
            var body = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset='utf-8' />
                <meta name='viewport' content='width=device-width, initial-scale=1.0' />
                <title>Auction Win Notification</title>
                <style>
                    body {{
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                        color: #333;
                    }}
                    .email-container {{
                        max-width: 600px;
                        margin: 0 auto;
                        background-color: #ffffff;
                        padding: 20px;
                        border-radius: 8px;
                        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                    }}
                    .email-header {{
                        text-align: center;
                        padding-bottom: 20px;
                        border-bottom: 1px solid #ddd;
                    }}
                    .email-header h1 {{
                        font-size: 24px;
                        margin: 0;
                    }}
                    .email-body {{
                        padding: 20px;
                        line-height: 1.6;
                    }}
                    .email-footer {{
                        text-align: center;
                        font-size: 12px;
                        color: #888;
                        padding: 20px;
                        border-top: 1px solid #ddd;
                    }}
                </style>
            </head>
            <body>
                <div class='email-container'>
                    <div class='email-header'>
                        <h1>Congratulations!</h1>
                    </div>
                    <div class='email-body'>
                        <p>Hello,</p>
                        <p>You have won the auction for the <strong>{koiName}</strong> fish with the amount of <strong>{bidAmount}</strong>.</p>
                        <p>Thank you for participating in our auction!</p>
                        <p>Best regards,<br />The Koi Auction Support Team</p>
                    </div>
                    <div class='email-footer'>
                        <p>&copy; 2024 Koi Auction. All rights reserved.</p>
                    </div>
                </div>
            </body>
            </html>";

            var smtpClient = new SmtpClient(_emailSettings.SmtpHost)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPass),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
