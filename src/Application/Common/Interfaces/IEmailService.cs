namespace KoiAuction.Application.Common.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmEmailAsync(string email, string confirmationLink);
        Task SendResetPasswordEmailAsync(string email, string resetLink);
        Task SendWinningEmail(string userId, string koiName, decimal bidAmount);
    }

}
