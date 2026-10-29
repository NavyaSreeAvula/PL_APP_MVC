namespace PL_APP_MVC.Services
{
    public interface IMailService
    {
        public string SendEmail(string sendername, string sendermail, string toname, string tomail, string subject, string message);

    }
}
