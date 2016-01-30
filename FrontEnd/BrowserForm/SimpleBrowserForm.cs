using System;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.WinForms.Internals;

namespace FrontEnd.BrowserForm {
    public partial class SimpleBrowserForm : Form {
        private ChromiumWebBrowser browser;

        public SimpleBrowserForm() {
            InitializeComponent();

            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            var version = String.Format("Chromium: {0}, CEF: {1}, CefSharp: {2}, Environment: {3}", Cef.ChromiumVersion, Cef.CefVersion, Cef.CefSharpVersion, bitness);
            DisplayOutput(version);

            //Only perform layout when control has completly finished resizing
            ResizeBegin += (s, e) => SuspendLayout();
            ResizeEnd += (s, e) => ResumeLayout(true);

            Load += OnLoad;
        }

        private void OnLoad(object sender, EventArgs e) {
            CreateBrowser();
        }

        private void CreateBrowser() {
            browser = new ChromiumWebBrowser("local://web/index.html") {
                Dock = DockStyle.Fill,
            };
            toolStripContainer.ContentPanel.Controls.Add(browser);

            browser.ConsoleMessage += OnBrowserConsoleMessage;
            browser.StatusMessage += OnBrowserStatusMessage;
            browser.TitleChanged += OnBrowserTitleChanged;
            
            //browser.RegisterJsObject("bound", new BoundObject());
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

        private void LoadUrl(string url) {
            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute)) {
                browser.Load(url);
            }
        }

        private void SimpleBrowserForm_FormClosed(object sender, FormClosedEventArgs e) {
            browser.Dispose();
            Cef.Shutdown();
        }
    }
}
