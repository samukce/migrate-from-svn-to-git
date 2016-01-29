using System;
using System.Windows.Forms;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;

namespace FrontEnd {
    static class Program {
        private static IWindsorContainer container;

        [STAThread]
        static void Main() {
            container = new WindsorContainer(new XmlInterpreter());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
