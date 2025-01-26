using MimeKit;

namespace MicroZoo.EmailService
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; }
        public string Content { get; }

        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();

            To.AddRange(to.Select(x => new MailboxAddress("Test mailbox", x)));
            Subject = subject;
            Content = content;
        }

        
    }
}
