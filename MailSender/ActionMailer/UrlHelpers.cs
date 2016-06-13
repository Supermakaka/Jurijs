using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RazorEngine.Text;

namespace MessageServices.ActionMailer
{
    /// <summary>
    /// Copied from ActionMailerNext, added support for port.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class UrlHelpers<TModel>
    {
        private readonly ExtendedViewSettings _viewSettings;
        private readonly TModel _model;

        public UrlHelpers() { }

        public UrlHelpers(dynamic viewBag, TModel model)
        {
            _model = model;
            _viewSettings = viewBag.ViewSettings as ExtendedViewSettings;
        }

        /// <summary>
        /// Generates a fully qualified URL for an action method by using the specified action name, controller name and route values
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>The fully qualified URL to an action method..</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEncodedString Action(string actionName, string controllerName)
        {
            return Action(actionName, controllerName, null);
        }

        /// <summary>
        /// Generates a fully qualified URL for an action method by using the specified action name, controller name and route values
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">Dictionary that contains the parameters for a route.</param>
        /// <returns>The fully qualified URL to an action method..</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEncodedString Action(string actionName, string controllerName, object routeValues)
        {
            var routerValuesDict = UtilHelper.ObjectToDictionary(routeValues);
            return Action(actionName, controllerName, routerValuesDict, null);
        }

        /// <summary>
        /// Generates a fully qualified URL for an action method by using the specified action name, controller name and route values
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">Dictionary that contains the parameters for a route.</param>
        /// <returns>The fully qualified URL to an action method..</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEncodedString Action(string actionName, string controllerName, IDictionary<string, string> routeValues)
        {
            return Action(actionName, controllerName, routeValues, null);
        }

        /// <summary>
        /// Generates a fully qualified URL for an action method by using the specified action name, controller name, route values and protocol to use.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">Dictionary that contains the parameters for a route.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <returns>The fully qualified URL to an action method..</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEncodedString Action(string actionName, string controllerName, object routeValues,
            string protocol)
        {
            var routerValuesDict = UtilHelper.ObjectToDictionary(routeValues);
            return Action(actionName, controllerName, routerValuesDict, protocol, null);
        }

        /// <summary>
        /// Generates a fully qualified URL for an action method by using the specified action name, controller name, route values and protocol to use.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">Dictionary that contains the parameters for a route.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <returns>The fully qualified URL to an action method..</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEncodedString Action(string actionName, string controllerName, IDictionary<string, string> routeValues,
            string protocol)
        {
            return Action(actionName, controllerName, routeValues, protocol, null);
        }

        /// <summary>
        /// Generates a fully qualified URL for an action method by using the specified action name, controller name, route values, protocol to use and host name.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeValues">Dictionary that contains the parameters for a route.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <returns>The fully qualified URL to an action method..</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEncodedString Action(string actionName, string controllerName, IDictionary<string, string> routeValues, string protocol, string hostName)
        {
            if (string.IsNullOrWhiteSpace(actionName))
            {
                throw new ArgumentNullException("actionName");
            }

            if (string.IsNullOrWhiteSpace(controllerName))
            {
                throw new ArgumentNullException("controllerName");
            }

            //var uriBuilder = new UriBuilder(protocol ?? _viewSettings.Protocol, hostName ?? _viewSettings.Hostname);
            Uri uri = null;
            if (routeValues != null && routeValues.Count != 0)
            {
                routeValues.Add("action", actionName);
                routeValues.Add("controller", controllerName);

                uri = UtilHelper.BuildURI(protocol ?? _viewSettings.Protocol, hostName ?? _viewSettings.Hostname, _viewSettings.Port, _viewSettings.UrlPattern, routeValues);
            }
            else
            {
                var uriBuilder = new UriBuilder(protocol ?? _viewSettings.Protocol, hostName ?? _viewSettings.Hostname, _viewSettings.Port)
                {
                    Path = controllerName + "/" + actionName
                };
                uri = uriBuilder.Uri;
            }

            var link = uri.ToString();

            return new RawString(link);
        }

        /// <summary>
        /// Converts a virtual (relative) path to an application absolute path.
        /// </summary>
        /// <param name="actionName">The virtual path of the content.</param>
        /// <returns>The application absolute path.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public IEncodedString Content(string contentPath)
        {
            var uriBuilder = new UriBuilder(_viewSettings.Protocol, _viewSettings.Hostname)
            {
                Path = contentPath
            };

            var link = uriBuilder.Uri.ToString();

            return new RawString(link);
        }
    }
}
