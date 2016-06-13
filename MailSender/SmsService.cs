using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using Microsoft.AspNet.Identity;

namespace MessageServices
{
    using Identity;

    public class SmsService : IIdentitySmsService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your sms service here to send an sms.
            return Task.FromResult(0);
        }
    }
}