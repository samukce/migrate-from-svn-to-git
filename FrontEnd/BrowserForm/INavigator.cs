using System.Windows.Forms;

namespace FrontEnd.BrowserForm {
    public interface INavigator {
        void LoadUrl(string url, object jsObject = null);
        void ExecuteJavaScript(string javaScript);
        Form WinForm();
    }
}