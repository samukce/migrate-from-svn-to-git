using System;
using System.Windows.Forms;
using Castle.Core;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;

namespace FrontEnd.BrowserForm {
    [CastleComponent("FrontEnd.BrowserForm.SimpleBrowserForm", typeof(INavigator), Lifestyle = LifestyleType.Singleton)]
    public partial class SimpleBrowserForm : Form, INavigator {
        private ChromiumWebBrowser browser;

        public SimpleBrowserForm() {
            InitializeComponent();

            ResizeBegin += (s, e) => SuspendLayout();
            ResizeEnd += (s, e) => ResumeLayout(true);
        }

        private void CreateBrowser(string address) {
            browser = new ChromiumWebBrowser(address) {
                Dock = DockStyle.Fill,
            };
            toolStripContainer.ContentPanel.Controls.Add(browser);

            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
        }

        private void OnBrowserConsoleMessage(object sender, ConsoleMessageEventArgs args) {
            DisplayOutput(string.Format("Line: {0}, Source: {1}, Message: {2}", args.Line, args.Source, args.Message));
        }

        private void OnBrowserStatusMessage(object sender, StatusMessageEventArgs args) {
            this.InvokeOnUiThreadIfRequired(() => statusLabel.Text = args.Value);
        }

        private void OnBrowserTitleChanged(object sender, TitleChangedEventArgs args) {
            this.InvokeOnUiThreadIfRequired(() => Text = args.Title);
        }

        public void DisplayOutput(string output) {
            this.InvokeOnUiThreadIfRequired(() => outputLabel.Text = output);
        }

        private void SimpleBrowserForm_FormClosed(object sender, FormClosedEventArgs e) {
            browser.Dispose();
            Cef.Shutdown();
        }

        private void RegisterJsObject(object jsObject) {
            if (jsObject == null)
                return;

            if (browser == null)
                return;

            browser.RegisterAsyncJsObject("bound", jsObject);
        }

        public ChromiumWebBrowser Browser() {
            return browser;
        }

        public Form WinForm() {
            return this;
        }

        public void LoadUrl(string url, object jsObject = null) {
            if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                return;

            if (browser == null)
                CreateBrowser(url);
            else
                browser.Load(url);

            RegisterJsObject(jsObject);
        }
    }
}
