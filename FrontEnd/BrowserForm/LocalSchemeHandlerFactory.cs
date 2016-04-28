using CefSharp;

namespace SvnToGit.FrontEnd.BrowserForm {
    public class LocalSchemeHandlerFactory : ISchemeHandlerFactory {
        public static string SchemeName { get { return "local"; } }

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request) {
            return new LocalSchemeHandler();
        }
    }
}
