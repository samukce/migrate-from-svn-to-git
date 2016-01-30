using System;
using System.Windows.Forms;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using CefSharp;
using FrontEnd.BrowserForm;

namespace FrontEnd {
    static class Program {
        private static IWindsorContainer container;

        [STAThread]
        static void Main() {
            container = new WindsorContainer(new XmlInterpreter());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            StartChromiumBroser();
        }

        private static void StartChromiumBroser() {
            Cef.EnableHighDPISupport();

            var settings = new CefSettings();

            settings.RegisterScheme(new CefCustomScheme {
                SchemeName = LocalSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new LocalSchemeHandlerFactory()
            });
            Cef.Initialize(settings);

            var browser = new SimpleBrowserForm();
            Application.Run(browser);
        }
    }
}
