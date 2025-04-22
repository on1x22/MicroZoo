using MimeKit;
using MailKit.Net.Smtp;

namespace MicroZoo.EmailService
{
    /// <summary>
    /// Provides sending a message to a mailing list
    /// </summary>
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfiguration;

        /// <summary>
        /// Initialize a new instance of <see cref="EmailSender"/> class
        /// </summary>
        /// <param name="emailConfiguration"></param>
        public EmailSender(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        /// <summary>
        /// Sends a message to a mailing list
        /// </summary>
        /// <param name="message"></param>
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            
            Send(emailMessage);
        }


        /// <summary>
        /// Asynchronous sends a message to a mailing list
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(Message message) 
        {
            var emailMessage = CreateEmailMessage(message);

            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message) 
        {
            var emailMessage = new MimeMessage();            
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.UserName, _emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format("<h2 style='color:red'>{0}</h2>", message.Content)
            };
            
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);

                client.Send(mailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);

                await client.SendAsync(mailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }

    }
}
