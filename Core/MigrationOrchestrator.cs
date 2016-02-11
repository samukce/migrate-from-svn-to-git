using System;
using System.IO;
using Castle.Core;
using Core.Exceptions;
using Core.Interfaces;

namespace Core {
    [CastleComponent("Core.MigrationOrchestrator", Lifestyle = LifestyleType.Singleton)]
    public class MigrationOrchestrator {
        private readonly ICreateBareGit createBareGit;
        private readonly ICreateCloneGit createCloneGit;

        public MigrationOrchestrator(ICreateCloneGit createCloneGit, ICreateBareGit createBareGit) {
            this.createCloneGit = createCloneGit;
            this.createBareGit = createBareGit;
        }

        public void Migrate(string svnUrl, string usersAuthorsFullPathFile, string projectNameFolder) {
            if (!File.Exists(usersAuthorsFullPathFile))
                throw new ArgumentException(usersAuthorsFullPathFile);

            if (Directory.Exists(projectNameFolder))
                throw new ProjectFolderAlreadyExistsException(projectNameFolder);

            CreateDirectoryBase(projectNameFolder);

            var fileNameUsers = Path.GetFileName(usersAuthorsFullPathFile);
            File.Copy(usersAuthorsFullPathFile, Path.Combine(projectNameFolder, fileNameUsers));

            createCloneGit.Create(svnUrl, fileNameUsers, projectNameFolder);
            createBareGit.Create(projectNameFolder);
        }

        private static void CreateDirectoryBase(string projectName) {
            Directory.CreateDirectory(projectName);
        }
    }
}
