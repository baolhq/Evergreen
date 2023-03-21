using EvergreenAPI.DTO;
using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;

namespace EvergreenAPI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailDTO _emailDTO;
        private readonly ILogger<EmailService> logger;



        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public EmailService (IOptions<EmailDTO> emailDTO, ILogger<EmailService> _logger)
        {
           _emailDTO = emailDTO.Value;
            logger = _logger;
            logger.LogInformation("Create SendMailService");
        }


        public class MailContent
        {
            public string To { get; set; }              // Địa chỉ gửi đến
            public string Subject { get; set; }         // Chủ đề (tiêu đề email)
            public string Body { get; set; }            // Nội dung (hỗ trợ HTML) của email
        }


        // Gửi email, theo nội dung trong mailContent
        public async Task SendMail(MailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_emailDTO.DisplayName, _emailDTO.Mail);
            email.From.Add(new MailboxAddress(_emailDTO.DisplayName, _emailDTO.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(_emailDTO.Host, _emailDTO.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailDTO.Mail, _emailDTO.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await email.WriteToAsync(emailsavefile);

                logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            logger.LogInformation("send mail to " + mailContent.To);
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            await SendMail(new MailContent()
            {
                To = email,
                Subject = subject,
                Body = htmlMessage
            });
        }

       
    }
}
