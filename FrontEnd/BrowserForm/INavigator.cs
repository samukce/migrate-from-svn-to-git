using System.Windows.Forms;

namespace FrontEnd.BrowserForm {
    public interface INavigator {
        void LoadUrl(string url, object jsObject = null);
        Form WinForm();
    }
}