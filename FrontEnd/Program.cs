using System;
using System.Windows.Forms;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using CefSharp;
using SvnToGit.FrontEnd.BrowserForm;
using SvnToGit.FrontEnd.Controllers;

namespace SvnToGit.FrontEnd {
    static class Program {
        private static IWindsorContainer container;

        [STAThread]
        static void Main() {
            container = new WindsorContainer(new XmlInterpreter());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            StartChromiumBrowser();

            container.Resolve<IndexController>().Index();
            Application.Run(container.Resolve<INavigator>().WinForm());
        }

        private static void StartChromiumBrowser() {
            Cef.EnableHighDPISupport();

            var settings = new CefSettings();

            settings.RegisterScheme(new CefCustomScheme {
                SchemeName = LocalSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new LocalSchemeHandlerFactory()
            });

            Cef.Initialize(settings);
        }
    }
}
