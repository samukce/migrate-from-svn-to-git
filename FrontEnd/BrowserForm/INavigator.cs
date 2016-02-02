using System.Windows.Forms;
using CefSharp.WinForms;

namespace FrontEnd.BrowserForm {
    public interface INavigator {
        void LoadUrl(string url, object jsObject = null);
        Form WinForm();
        ChromiumWebBrowser Browser();
    }
}