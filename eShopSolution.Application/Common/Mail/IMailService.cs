namespace eShopSolution.Application.Common.Mail
{
    public interface IMailService
    {
        void SentMail(string content, string emailTo);
    }
}
