using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace HospitalManagementSystem.Web.Helpers
{
    public static class EmailHelper
    {
        public static void SendEmail(string recipient, string subject, string body)
        {
            SendEmail(recipient, subject, body, null);
        }

        public static void SendEmail(string recipient, string subject, string body, string replyTo)
        {
            if (string.IsNullOrWhiteSpace(recipient))
            {
                throw new InvalidOperationException("Recipient is required.");
            }

            string host = GetSetting("smtpHost");
            string portText = GetSetting("smtpPort");
            string user = GetSetting("smtpUser");
            string password = GetSetting("smtpPassword");
            string enableSslText = GetSetting("smtpEnableSsl");
            string fromAddress = GetSetting("smtpFrom");

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(portText) ||
                string.IsNullOrWhiteSpace(fromAddress))
            {
                throw new InvalidOperationException("SMTP configuration is incomplete.");
            }

            int port;
            if (!int.TryParse(portText, out port) || port <= 0)
            {
                throw new InvalidOperationException("SMTP port is invalid.");
            }

            bool enableSsl = true;
            if (!string.IsNullOrWhiteSpace(enableSslText))
            {
                bool.TryParse(enableSslText, out enableSsl);
            }

            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(fromAddress.Trim());
                message.To.Add(recipient.Trim());
                message.Subject = subject ?? string.Empty;
                message.Body = body ?? string.Empty;
                message.IsBodyHtml = false;

                if (!string.IsNullOrWhiteSpace(replyTo))
                {
                    message.ReplyToList.Add(new MailAddress(replyTo.Trim()));
                }

                using (SmtpClient client = new SmtpClient(host.Trim(), port))
                {
                    client.EnableSsl = enableSsl;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;

                    if (!string.IsNullOrWhiteSpace(user))
                    {
                        client.Credentials = new NetworkCredential(user.Trim(), password ?? string.Empty);
                    }
                    else
                    {
                        client.UseDefaultCredentials = false;
                    }

                    client.Send(message);
                }
            }
        }

        private static string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
