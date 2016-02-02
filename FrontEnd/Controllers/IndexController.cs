using System;
using Castle.Core;
using Core;
using FrontEnd.BrowserForm;

namespace FrontEnd.Controllers {
    [CastleComponent("FrontEnd.Controllers.IndexController", Lifestyle = LifestyleType.Transient)]
    public class IndexController {
        private static string HtmlPage { get { return "local://web/index.html"; } }

        private readonly INavigator navigator;
        private readonly MigrationOrchestrator migrationOrchestrator;

        public IndexController(INavigator navigator, MigrationOrchestrator migrationOrchestrator) {
            this.navigator = navigator;
            this.migrationOrchestrator = migrationOrchestrator;
        }

        public void Index() {
            navigator.LoadUrl(HtmlPage, this);
        }

        public void Execute(string svnAdress, string usersFile, string projectName) {
            try {
                migrationOrchestrator.Migrate(svnAdress, usersFile, projectName);

                SuccessMigrate();

            } catch (Exception ex) {
                ErrorMigrate(ex.Message);
            }
        }

        private void ErrorMigrate(string errorMessage) {
            navigator.ExecuteJavaScript(string.Format("error(\"{0}\");", errorMessage));
        }

        private void SuccessMigrate() {
            navigator.ExecuteJavaScript("success();");
        }
    }
}
