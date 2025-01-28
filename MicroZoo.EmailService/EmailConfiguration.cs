namespace MicroZoo.EmailService
{
    /// <summary>
    /// Provides information of email configuration
    /// </summary>
    public class EmailConfiguration
    {
        /// <summary>
        /// Email sender
        /// </summary>
        public string? From { get; set; }

        /// <summary>
        /// Tne name of SMTP server from which emails are sent
        /// </summary>
        public string? SmtpServer { get; set; }

        /// <summary>
        /// Port of SMTP server from which emails are sent
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The name of the user on whose behalf the letters are sent
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        public string? Password { get; set; }
    }
}
