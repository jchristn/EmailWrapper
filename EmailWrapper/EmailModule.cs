using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;
using RestSharp.Authenticators;

namespace EmailWrapper
{
    class EmailModule
    {
        #region Constructor

        public EmailModule(string mailgunApiKey, string mailgunDomain)
        {
            if (String.IsNullOrEmpty(mailgunApiKey)) throw new ArgumentNullException(nameof(mailgunApiKey));
            if (String.IsNullOrEmpty(mailgunDomain)) throw new ArgumentNullException(nameof(mailgunDomain));

            MailgunApiKey = mailgunApiKey;
            MailgunDomain = mailgunDomain;
            Provider = "mailgun";
        }

        public EmailModule(
            string smtpServer,
            int smtpPort,
            string smtpUsername,
            string smtpPassword,
            bool smtpSsl)
        {
            if (String.IsNullOrEmpty(smtpServer)) throw new ArgumentNullException(nameof(smtpServer));
            if (String.IsNullOrEmpty(smtpUsername)) throw new ArgumentNullException(nameof(smtpUsername));
            if (String.IsNullOrEmpty(smtpPassword)) throw new ArgumentNullException(nameof(smtpPassword));
            if (smtpPort <= 0) throw new ArgumentOutOfRangeException(nameof(smtpPort));

            SmtpServer = smtpServer;
            SmtpUsername = smtpUsername;
            SmtpPassword = smtpPassword;
            SmtpPort = smtpPort;
            SmtpSsl = smtpSsl;
            Provider = "smtp";
        }

        #endregion
        
        #region Private-Members
        
        private string Provider { get; set; }
        private string SmtpServer { get; set; }
        private int SmtpPort { get; set; }
        private string SmtpUsername { get; set; }
        private string SmtpPassword { get; set; }
        private bool SmtpSsl { get; set; }

        private string MailgunDomain { get; set; }
        private string MailgunApiKey { get; set; }

        #endregion

        #region Public-Methods

        public bool Send(Email email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            if (!email.IsValid()) throw new Exception("Email object is not valid");

            switch (Provider)
            {
                case "smtp":
                    return SendSmtp(email);

                case "mailgun":
                    return SendMailgun(email);

                default:
                    throw new ArgumentOutOfRangeException(nameof(Provider));
            }
        }

        #endregion

        #region Private-Methods

        private bool SendSmtp(Email email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(email.FromAddress);
            msg.To.Add(email.ToAddress);
            msg.Subject = email.Subject;
            if (!String.IsNullOrEmpty(email.CcAddress)) msg.CC.Add(email.CcAddress);
            if (!String.IsNullOrEmpty(email.BccAddress)) msg.Bcc.Add(email.BccAddress);
            if (!String.IsNullOrEmpty(email.ReplyAddress)) msg.ReplyToList.Add(email.ReplyAddress);
            
            msg.Body = email.Body;

            SmtpClient smtp = new SmtpClient(SmtpServer, SmtpPort);
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = SmtpSsl;
            smtp.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword, null);

            if (email.IsHtml) msg.IsBodyHtml = true;
            else msg.IsBodyHtml = false;

            if (!String.IsNullOrEmpty(email.AttachmentData))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(email.AttachmentData);
                MemoryStream attachmentStream = new MemoryStream(bytes);
                msg.Attachments.Add(new Attachment(attachmentStream, email.AttachmentName, email.AttachmentContentType));
                attachmentStream.Dispose();
            }

            smtp.Send(msg);
            return true;
        }

        private bool SendMailgun(Email email)
        {
            RestClient client = new RestClient();
            client.BaseUrl = new Uri("https://api.mailgun.net/v3");
            client.Authenticator = new HttpBasicAuthenticator("api", MailgunApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain", MailgunDomain, ParameterType.UrlSegment);
            request.Resource = MailgunDomain + "/messages";
            request.AddParameter("from", email.FromAddress);
            request.AddParameter("to", email.ToAddress);

            if (!String.IsNullOrEmpty(email.CcAddress)) request.AddParameter("cc", email.CcAddress);
            if (!String.IsNullOrEmpty(email.BccAddress)) request.AddParameter("bcc", email.BccAddress);

            request.AddParameter("subject", email.Subject);

            if (email.IsHtml) request.AddParameter("html", email.Body);
            else request.AddParameter("text", email.Body);

            if (!String.IsNullOrEmpty(email.AttachmentData))
                request.AddFileBytes(
                    "attachment",
                    Encoding.UTF8.GetBytes(email.AttachmentData),
                    email.AttachmentName,
                    email.AttachmentContentType);

            request.Method = Method.POST;
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK) return true;
            else return false;
        }

        #endregion
    }
}
