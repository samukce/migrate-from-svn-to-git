﻿using Castle.Core;
using SvnToGit.Core.Interfaces;
using SvnToGit.FrontEnd.BrowserForm;

namespace SvnToGit.FrontEnd {
    [CastleComponent("SvnToGit.FrontEnd.LoggerBrowser", typeof(ILogger), Lifestyle = LifestyleType.Transient)]
    public class LoggerBrowser : ILogger {
        private readonly INavigator navigator;

        public LoggerBrowser(INavigator navigator) {
            this.navigator = navigator;
        }

        public void Info(string message) {
            RegisterMessageLog(message);
        }

        public void Error(string message) {
            RegisterMessageLog(message);
        }

        private void RegisterMessageLog(string message) {
            navigator.ExecuteJavaScript(JsLogMessages(message));
        }

        private static string JsLogMessages(string message) {
            return string.IsNullOrWhiteSpace(message) ? string.Empty :
                                                        string.Format("addlog(\"{0}\");", message.Replace("\"", "'"));
        }
    }
}
