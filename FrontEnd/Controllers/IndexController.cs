using Castle.Core;
using FrontEnd.BrowserForm;
using FrontEnd.JsObjects;

namespace FrontEnd.Controllers {
    [CastleComponent("FrontEnd.Controllers.IndexController", Lifestyle = LifestyleType.Transient)]
    public class IndexController {
        private static string HtmlPage { get { return "local://web/index.html"; } }
        
        private readonly INavigator navigator;
        private readonly IndexJsObject indexJsObject;

        public IndexController(INavigator navigator, IndexJsObject indexJsObject) {
            this.navigator = navigator;
            this.indexJsObject = indexJsObject;

            indexJsObject.Success += SuccessMigrate;
            indexJsObject.Error += ErrorMigrate;
        }

        public void Index() {
            navigator.LoadUrl(HtmlPage, indexJsObject);
        }

        private void ErrorMigrate(string errorMessage) {
            //navigator.LoadUrl(ErrorPage);
        }

        private void SuccessMigrate() {
            //navigator.LoadUrl(SuccessPage);
        }
    }
}
