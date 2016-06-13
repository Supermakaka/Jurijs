using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

using ActionMailerNext.Interfaces;
using ActionMailerNext;
using PreMailer.Net;

namespace MessageServices.ActionMailer
{
    /// <summary>
    /// Copied from ActionMailerNext, added support for ignoreElements option of PreMailer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InlineCssPostProcessor : IPostProcessor
    {
        private System.Uri baseUri;

        public InlineCssPostProcessor(System.Uri baseUri) : base()
        {
            this.baseUri = baseUri;
        }

        public MailAttributes Execute(MailAttributes mailAttributes)
        {
            var newMailAttributes = new MailAttributes(mailAttributes);

            foreach (var view in mailAttributes.AlternateViews)
            {
                using (var reader = new StreamReader(view.ContentStream))
                {
                    var body = reader.ReadToEnd();

                    if (view.ContentType.MediaType == MediaTypeNames.Text.Html)
                    {
                        var inlinedCssString = PreMailer.Net.PreMailer.MoveCssInline(baseUri, body, ignoreElements: ".non-inline");

                        byte[] byteArray = Encoding.UTF8.GetBytes(inlinedCssString.Html);
                        var stream = new MemoryStream(byteArray);

                        var newAlternateView = new AlternateView(stream, MediaTypeNames.Text.Html);
                        newMailAttributes.AlternateViews.Add(newAlternateView);
                    }
                    else
                    {
                        newMailAttributes.AlternateViews.Add(view);
                    }
                }
            }

            return newMailAttributes;
        }
    }
}
