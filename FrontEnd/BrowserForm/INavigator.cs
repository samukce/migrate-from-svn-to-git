using System.Windows.Forms;

namespace SvnToGit.FrontEnd.BrowserForm {
    public interface INavigator {
        void LoadUrl(string url, object jsObject = null);
        void ExecuteJavaScript(string javaScript);
        Form WinForm();
    }
}