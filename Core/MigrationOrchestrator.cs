using Core.Interfaces;

namespace Core {
    public class MigrationOrchestrator {
        private readonly ICreateBareGit createBareGit;
        private readonly ICreateCloneGit createCloneGit;

        public MigrationOrchestrator(ICreateCloneGit createCloneGit, ICreateBareGit createBareGit) {
            this.createCloneGit = createCloneGit;
            this.createBareGit = createBareGit;
        }

        public void Migrate(string svnUrl, string usersAuthorsPathFile, string projectName) {
            createCloneGit.Create(svnUrl, usersAuthorsPathFile, projectName);
            createBareGit.Create(projectName);
        }
    }
}
