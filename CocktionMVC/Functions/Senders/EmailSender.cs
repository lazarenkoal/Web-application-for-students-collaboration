using AegisImplicitMail;

namespace CocktionMVC.Functions
{
    /// <summary>
    /// Занимается отправкой писем
    /// </summary>
    public static class EmailSender
    {
        /// <summary>
        /// Содержит поле с текстом для подтверждения имейла
        /// </summary>
        public const string VERIFY_EMAIL_MESSAGE = "Please, click this link to verify your email :)";

        public const string VERIFY_EMAIL_SUBJECT = "Email validation";

        public const string RESET_PASSWORD_MESSAGE = "Please, help us to reset your password!";

        public const string RESET_PASSWORD_SUBJECT = "Some problems with password...";

        /// <summary>
        /// Отправляет имейл на указанный адрес с указанными данными.
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="subject">Тема сообщения</param>
        /// <param name="adressToMail">Адрес, на который нужно отправить сообщение</param>
        public static void SendEmail(string message, string subject, string adressToMail)
        {
            const string MAIL = "cocky@cocktion.com";
            const string HOST = "smtp.yandex.ru";
            const int PORT = 465;
            const string USER = "cocky@cocktion.com";
            const string PASSWORD = "IloveCocktion290996";

            //Generate Message 
            var mymessage = new MimeMailMessage();
            mymessage.From = new MimeMailAddress(MAIL);
            mymessage.To.Add(adressToMail);
            mymessage.Subject = subject;
            mymessage.Body = message;

            //Create Smtp Client
            var mailer = new MimeMailer(HOST, PORT);
            mailer.User = USER;
            mailer.Password = PASSWORD;
            mailer.SslType = SslMode.Ssl;
            mailer.AuthenticationMode = AuthenticationType.Base64;
            mailer.EnableImplicitSsl = true;
            mailer.SendMail(mymessage);
        }
    }
}