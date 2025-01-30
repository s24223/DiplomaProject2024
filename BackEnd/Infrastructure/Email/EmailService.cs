using Application.Shared.Interfaces.Email;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        //Properties
        private static readonly string _host = "smtp.gmail.com";
        private static readonly int _port = 587;
        private readonly string _email;
        private readonly string _password;

        //Constructor
        public EmailService(IConfiguration configuration)
        {
            _email = configuration.GetSection("EmailStrings")["Email"] ??
                throw new NotImplementedException();
            _password = configuration.GetSection("EmailStrings")["Password"] ??
                throw new NotImplementedException();
        }


        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Public Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="title"></param>
        /// <param name="message">As HTML </param>
        /// <returns></returns>
        public async Task SendAsync(string email, string title, string message)
        {
            MailMessage mail = new MailMessage(_email, email)
            {
                Subject = title,
                Body = message,
                IsBodyHtml = true // Jeśli chcesz wysłać wiadomość w formacie HTML
            };

            using (SmtpClient smtp = new SmtpClient(_host, _port))
            {
                smtp.Credentials = new NetworkCredential(_email, _password);
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }
        }

        //==================================================================================================
        //==================================================================================================
        //==================================================================================================
        //Private Methods
    }
}
