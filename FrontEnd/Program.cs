using System;
using System.Windows.Forms;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using CefSharp;
using FrontEnd.BrowserForm;
using FrontEnd.Controllers;

namespace FrontEnd {
    static class Program {
        private static IWindsorContainer container;

        [STAThread]
        static void Main() {
            container = new WindsorContainer(new XmlInterpreter());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            StartChromiumBrowser();
        }

        private static void StartChromiumBrowser() {
            Cef.EnableHighDPISupport();

            var settings = new CefSettings();

            settings.RegisterScheme(new CefCustomScheme {
                SchemeName = LocalSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new LocalSchemeHandlerFactory()
            });
            Cef.Initialize(settings);

            container.Resolve<IndexController>().Index();
            Application.Run(container.Resolve<INavigator>().WinForm());
        }
    }
}
