using Castle.Core;
using Core;

namespace FrontEnd.JsObjects {
    [CastleComponent("FrontEnd.JsObjects.IndexJsObject", Lifestyle = LifestyleType.Transient)]
    public class IndexJsObject {
        private readonly MigrationOrchestrator migrationOrchestrator;

        public IndexJsObject(MigrationOrchestrator migrationOrchestrator) {
            this.migrationOrchestrator = migrationOrchestrator;
        }

        public void Execute(string svnAdress, string usersFile, string projectName) {
            migrationOrchestrator.Migrate(svnAdress, usersFile, projectName);
        }
    }
}
