using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailWrapper
{
    /// <summary>
    /// An email object.
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Indicates whether or not the email is HTML.
        /// </summary>
        public bool IsHtml { get; set; }

        /// <summary>
        /// The from email address.
        /// </summary>
        public string FromAddress { get; set; }

        /// <summary>
        /// The reply-to email address.
        /// </summary>
        public string ReplyAddress { get; set; }

        /// <summary>
        /// The to email address.
        /// </summary>
        public string ToAddress { get; set; }

        /// <summary>
        /// Email addresses to be carbon copied, separated by a semicolon.
        /// </summary>
        public string CcAddress { get; set; }

        /// <summary>
        /// Email addresses to be blind carbon copied, separated by a semicolon.
        /// </summary>
        public string BccAddress { get; set; }

        /// <summary>
        /// The subject of the message.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The body of the message.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The name of the attachment.
        /// </summary>
        public string AttachmentName { get; set; }

        /// <summary>
        /// The attachment data.
        /// </summary>
        public string AttachmentData { get; set; }

        /// <summary>
        /// The content type of the attachment.
        /// </summary>
        public string AttachmentContentType { get; set; }

        /// <summary>
        /// Instantiates the object.
        /// </summary>
        public Email()
        {

        }

        /// <summary>
        /// Tests the validity of the object.
        /// </summary>
        /// <returns>True if valid.</returns>
        public bool IsValid()
        {
            if (String.IsNullOrEmpty(FromAddress)) throw new ArgumentNullException(nameof(FromAddress));
            if (String.IsNullOrEmpty(ToAddress)) throw new ArgumentNullException(nameof(ToAddress));
            if (String.IsNullOrEmpty(ReplyAddress)) throw new ArgumentNullException(nameof(ReplyAddress));
            if (String.IsNullOrEmpty(Subject)) throw new ArgumentNullException(nameof(Subject));
            if (String.IsNullOrEmpty(Body)) throw new ArgumentNullException(nameof(Body));
            return true;
        }
    }
}
