using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ActionMailerNext.Standalone.Helpers;
using RazorEngine.Templating;

namespace MessageServices.ActionMailer
{
    /// <summary>
    /// Copied from ActionMailerNext in order to use custom UrlHelper.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExtendedTemplateBase<T> : TemplateBase<T>
    {
        private StandaloneHtmlHelpers<T> _html;
        private UrlHelpers<T> _url;

        public StandaloneHtmlHelpers<T> Html
        {
            get
            {
                this._html = this._html ?? new StandaloneHtmlHelpers<T>(TemplateService, ViewBag, Model);
                return this._html;
            }
        }

        public UrlHelpers<T> Url
        {
            get
            {
                this._url = this._url ?? new UrlHelpers<T>(ViewBag, Model);
                return this._url;
            }
        }
    }

    public class ExtendedTemplateBase : TemplateBase
    {
        private StandaloneHtmlHelpers<object> _html;
        private UrlHelpers<object> _url;

        public StandaloneHtmlHelpers<object> Html
        {
            get
            {
                this._html = this._html ?? new StandaloneHtmlHelpers<object>(TemplateService, ViewBag, null);
                return this._html;
            }
        }

        public UrlHelpers<object> Url
        {
            get
            {
                this._url = this._url ?? new UrlHelpers<object>(ViewBag, null);
                return this._url;
            }
        }

    }
}
