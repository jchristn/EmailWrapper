using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailWrapper
{
    class Email
    {
        public bool IsHtml { get; set; }
        public string FromAddress { get; set; }
        public string ReplyAddress { get; set; }
        public string ToAddress { get; set; }
        public string CcAddress { get; set; }
        public string BccAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentData { get; set; }
        public string AttachmentContentType { get; set; }

        public Email()
        {

        }

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
