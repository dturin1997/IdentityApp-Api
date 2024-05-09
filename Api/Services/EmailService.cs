using Api.DTOs.Account;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Net;
using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net.Http;

namespace Api.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        /*
        public async Task<bool> SendEmailAsync(EmailSendDto emailSend)
        {
            MailjetClient client = new MailjetClient(_config["MailJet:ApiKey"], _config["MailJet:SecretKey"]);

            var email = new TransactionalEmailBuilder()
                 .WithFrom(new SendContact(_config["Email:From"], _config["Email:ApplicationName"]))
                 .WithSubject(emailSend.Subject)
                 .WithHtmlPart(emailSend.Body)
                 .WithTo(new SendContact(emailSend.To))
                 .Build();

            var response = await client.SendTransactionalEmailAsync(email);
            if (response.Messages != null)
            {
                if (response.Messages[0].Status == "success")
                {
                    return true;
                }
            }

            return false;
        }
        */

        public async Task<bool> SendEmailAsync(EmailSendDto emailSend)
        {
            try
            {
                var username = _config["SMTP:Username"];
                var password = _config["SMTP:Password"];
                var client = new SmtpClient("smtp-mail.outlook.com", 587)
                {

                    EnableSsl = true,
                    Credentials = new NetworkCredential(username, password)
                };

                var message = new MailMessage(from: username, to: emailSend.To, subject: emailSend.Subject, body: emailSend.Body);

                message.IsBodyHtml = true;
                await client.SendMailAsync(message);
                return true;
            }
            catch (Exception ex) {

                Console.WriteLine(ex.ToString());
                return false;
            }

        }
    }

}
