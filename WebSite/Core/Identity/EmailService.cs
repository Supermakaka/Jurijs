using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace WebSite.Core.Identity
{
    public interface IEmailService : IIdentityMessageService { }

    public class EmailService : IEmailService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}