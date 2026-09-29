using Common.Models.Template;
using Common.Repositories.Template;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Common.Services.Email
{
    internal partial class EmailService
    {
        public async Task SendEmail(
           IEnumerable<MailAddress> to,
            string templateString,
            bool bodyAsHtml,
            string culture,
            MailAddress from,
            Dictionary<string, string>? bodyParams = null,
            Dictionary<string, string>? subjectParams = null,
            IEnumerable<Attachment>? attachmets = null,
            IEnumerable<MailAddress>? cc = null,
            IEnumerable<MailAddress>? bcc = null,
            IEnumerable<MailAddress>? replyTo = null,
            MailPriority emailPriority = MailPriority.Normal
            )
        {
            TemplateModel templateMailAddress = await _templateService.Get(templateString)
                ?? throw new Exception("Nie znaleziono szablonu");

            if (templateMailAddress.Body is null || templateMailAddress.Subject is null) throw new Exception("Subject or body not provided");

            //PODMIANA PARAMETRÓW BODY
            if (bodyParams is not null)
                foreach (KeyValuePair<string, string> parameter in bodyParams)
                    templateMailAddress.Body = templateMailAddress.Body.Replace("{{" + parameter.Key + "}}", parameter.Value);

            //PODMIANA PARAMETRÓW SUBJECT
            if (subjectParams is not null)
                foreach (KeyValuePair<string, string> parameter in subjectParams)
                    templateMailAddress.Subject = templateMailAddress.Subject.Replace("{{" + parameter.Key + "}}", parameter.Value);

            //using SmtpClient smtpClient = new()
            //{
            //    Host = _configuration["Email:Server"] ?? throw new Exception("Nie ustawiono serwera dla e-maila"),
            //    Port = int.Parse(_configuration["Email:Port"] ?? throw new Exception("Nie ustawiono portu dla e-maila")),
            //    Timeout = 12000,
            //    EnableSsl = bool.Parse(_configuration["Enable:SSL"] ?? throw new Exception("Nie ustawiono SSL")),
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    UseDefaultCredentials = false,
            //    Credentials = new NetworkCredential(_configuration["Email:User"] ?? throw new Exception("Nie ustawiono user'a dla e-maila"), _configuration["Email:Password"] ?? throw new Exception("Nie ustawiono hasła dla e-maila"))
            //};
            using SmtpClient smtpClient = new();

            try
            {
               

                smtpClient.Host = _configuration["Email:Server"] ?? throw new Exception("Nie ustawiono serwera dla e-maila");
                smtpClient.Port = int.Parse(_configuration["Email:Port"] ?? throw new Exception("Nie ustawiono portu dla e-maila"));
                smtpClient.Timeout = 12000;
                smtpClient.EnableSsl = bool.Parse(_configuration["Enable:SSL"] ?? throw new Exception("Nie ustawiono SSL"));
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(
                    _configuration["Email:User"] ?? throw new Exception("Nie ustawiono user'a dla e-maila"),
                    _configuration["Email:Password"] ?? throw new Exception("Nie ustawiono hasła dla e-maila")
                );

                // Tutaj możesz wysłać e-mail
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Błąd parsowania konfiguracji: {ex.Message}");
            }
            catch (SmtpException ex)
            {
                Console.WriteLine($"Błąd SMTP: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Nieoczekiwany błąd: {ex.Message}");
            }


            using MailMessage message = new()
            {
                SubjectEncoding = Encoding.UTF8,
                IsBodyHtml = bodyAsHtml,
                BodyEncoding = Encoding.UTF8,
                Priority = emailPriority,
                Subject = templateMailAddress.Subject,
                Body = templateMailAddress.Body,
                From = from,
            };

            foreach (MailAddress receiver in to) message.To.Add(receiver);
            if (cc is not null) foreach (MailAddress item in cc) message.CC.Add(item);
            if (bcc is not null) foreach (MailAddress item in bcc) message.Bcc.Add(item);
            if (replyTo is not null) foreach (MailAddress item in replyTo) message.ReplyToList.Add(item);

            //DODANIE ZAŁĄCZNIKÓW
            if (attachmets is not null)
                foreach (Attachment attachmet in attachmets)
                    message.Attachments.Add(new Attachment(attachmet.ContentStream, attachmet.Name));

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.DefaultConnectionLimit = 100;
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => { return true; };
                smtpClient.Send(message);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException innerEx in ex.InnerExceptions)
                {
                    SmtpStatusCode status = innerEx.StatusCode;
                    if (status is SmtpStatusCode.MailboxBusy or SmtpStatusCode.MailboxUnavailable)
                    {
                        Thread.Sleep(5000);
                        smtpClient.Send(message);
                    }
                    else
                    {
                        Console.WriteLine($"Failed to deliver message to {innerEx.FailedRecipient}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mail sending error: {ex.Message}");
            }
        }
    }
}
