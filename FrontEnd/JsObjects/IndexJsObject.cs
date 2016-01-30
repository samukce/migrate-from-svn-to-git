using System;
using Castle.Core;
using Core;

namespace FrontEnd.JsObjects {
    [CastleComponent("FrontEnd.JsObjects.IndexJsObject", Lifestyle = LifestyleType.Transient)]
    public class IndexJsObject {
        private readonly MigrationOrchestrator migrationOrchestrator;

        public event Action Success;
        public event Action<string> Error;

        public string LastError { get; set; }

        public IndexJsObject(MigrationOrchestrator migrationOrchestrator) {
            this.migrationOrchestrator = migrationOrchestrator;
        }

        public void Execute(string svnAdress, string usersFile, string projectName) {
            try {
                migrationOrchestrator.Migrate(svnAdress, usersFile, projectName);

                if (Success != null)
                    Success();

            } catch (Exception ex) {
                LastError = ex.Message;

                if (Error != null)
                    Error(ex.Message);
            }
        }
    }
}
