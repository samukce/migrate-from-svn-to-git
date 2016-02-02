using Castle.Core;
using CefSharp;
using Core.Interfaces;
using FrontEnd.BrowserForm;

namespace FrontEnd.JsObjects {
    [CastleComponent("FrontEnd.JsObjects.LoggerBrowser", typeof(ILogger), Lifestyle = LifestyleType.Transient)]
    public class LoggerBrowser : ILogger {
        private readonly INavigator navigator;

        public LoggerBrowser(INavigator navigator) {
            this.navigator = navigator;
        }

        public void Info(string message) {
            navigator.Browser().ExecuteScriptAsync(JsLogMessages(message));
        }

        private static string JsLogMessages(string message) {
            return string.Format("addlog('{0}');", message);
        }
    }
}
