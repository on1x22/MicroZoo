using MimeKit;

namespace MicroZoo.EmailService
{
    /// <summary>
    /// Provides main information about message
    /// </summary>
    public class Message
    {
        /// <summary>
        /// List of emails for sending message
        /// </summary>
        public List<MailboxAddress> To { get; set; }

        /// <summary>
        /// The subject of the message
        /// </summary>
        public string Subject { get; }

        /// <summary>
        /// Message content
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Initialize a new instance of <see cref="Message"/> class
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="content"></param>
        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress("Test mailbox", x)));
            Subject = subject;
            Content = content;
        }

        
    }
}
