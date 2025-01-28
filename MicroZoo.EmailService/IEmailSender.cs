namespace MicroZoo.EmailService
{
    /// <summary>
    /// Provides sending a message to a mailing list
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Sends a message to a mailing list
        /// </summary>
        /// <param name="message"></param>
        void SendEmail(Message message);

        /// <summary>
        /// Asynchronous sends a message to a mailing list
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendEmailAsync(Message message);
    }
}
