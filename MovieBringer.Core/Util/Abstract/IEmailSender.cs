namespace MovieBringer.WebApp.Util.Abstract
{
    public interface IEmailSender
    {
        Task SendEmailFromContactAsync(string email,string clientName, string clientEmail, string subject, string message);
        Task SendSimpleAsync(string email, string subject, string body);
    }
}
