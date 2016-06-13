using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ActionMailerNext.Standalone.Helpers;

namespace MessageServices.ActionMailer
{
    /// <summary>
    /// Extended from ActionMailerNext's ViewSettings, added support for port.
    /// </summary>
    /// <typeparam name="T"></typeparam>

    public class ExtendedViewSettings : ViewSettings
    {
        public int Port { get; set; }
    }
}
