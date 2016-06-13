using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;
using System.IO;
using System.Net.Mail;

using Microsoft.AspNet.Identity;

using ActionMailerNext.Standalone;
using ActionMailerNext.Standalone.Helpers;

using BusinessLogic.Models;

namespace MessageServices
{
    using Identity;
    using ViewModels;
    using ActionMailer;

    public class ConfigKey
    {
        public const string GlobalViewPath = "EmailService_GlobalViewPath";
        public const string From = "EmailService_From";
        public const string FromDisplayName = "EmailService_FromDisplayName";
        public const string SubjectPrefix = "EmailService_SubjectPrefix";
        public const string BaseUri = "EmailService_BaseUri";
    }

    public class EmailService : RazorMailerBase, IEmailService, IIdentityEmailService
    {
        private readonly string globalViewPath;
        private readonly ExtendedViewSettings viewSettings;
        private readonly string subjectPrefix;

        public override string GlobalViewPath
        {
            get { return globalViewPath; }
        }

        public override ViewSettings ViewSettings
        {
            get { return viewSettings; }
        }

        public EmailService()
        {
            globalViewPath = GetConfigValue(ConfigKey.GlobalViewPath, Path.Combine(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath, "..", "Views"));

            viewSettings = new ExtendedViewSettings();

            var baseUri = new Uri(GetConfigValue(ConfigKey.BaseUri));

            viewSettings.Hostname = baseUri.Host;
            viewSettings.Protocol = baseUri.Scheme;
            viewSettings.UrlPattern = "{controller}/{action}";
            viewSettings.Port = baseUri.Port;

            MailAttributes.From = new MailAddress(GetConfigValue(ConfigKey.From), GetConfigValue(ConfigKey.FromDisplayName, ""));
            MailAttributes.PostProcessors.Add(new InlineCssPostProcessor(baseUri));

            subjectPrefix = GetConfigValue(ConfigKey.SubjectPrefix, "");
        }

        public Task SendAsync(IdentityMessage message)
        {
            SetSubject(message.Subject);
            SetRecipient(message.Destination);

            MailAttributes.HtmlBody = message.Body;

            return MailSender.SendAsync(MailAttributes);
        }

        public void SendPasswordResetLink(User user, string code)
        {
            SetSubject("password recovery");
            SetRecipient(user.Email);
            SetGreetingName(user.FullName);

            ViewBag.ViewSettings = viewSettings;

            var model = new ForgotPasswordViewModel();
            model.UserId = user.Id;
            model.Code = code;

            MailSender.Deliver(Email("ForgotPassword", model));
        }

        private void SetSubject(string subject)
        {
            if (!String.IsNullOrEmpty(subjectPrefix))
                ViewBag.Subject = subjectPrefix + " " + subject;
            else
                ViewBag.Subject = subject;

            MailAttributes.Subject = ViewBag.Subject;
        }

        private void SetRecipient(string email)
        {
            MailAttributes.To.Add(new MailAddress(email));
        }

        private void SetGreetingName(string name)
        {
            ViewBag.GreetingName = name;
        }

        private string GetConfigValue(string key)
        {
            return GetConfigValue(key, null);
        }

        private string GetConfigValue(string key, string defaultValue)
        {
            var val = ConfigurationManager.AppSettings[key];

            if (val != null && !String.IsNullOrEmpty(val))
                return val;

            if (defaultValue != null)
                return defaultValue;

            throw new ArgumentException(String.Format(@"[EmailService] The setting with the key ""{0}"" is not found in the config file and default value is not provided.", key));
        }
    }

    public interface IEmailService
    {
        void SendPasswordResetLink(User user, string code);
    }
}
