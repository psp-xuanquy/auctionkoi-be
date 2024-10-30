namespace KoiAuction.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmEmailAsync(string email, string confirmationLink);
        Task SendResetPasswordEmailAsync(string email, string resetLink);
    }

}
