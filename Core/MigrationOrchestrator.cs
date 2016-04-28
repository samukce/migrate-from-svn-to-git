using System;
using System.IO;
using Castle.Core;
using SvnToGit.Core.Exceptions;
using SvnToGit.Core.Interfaces;

namespace SvnToGit.Core {
    [CastleComponent("Core.MigrationOrchestrator", Lifestyle = LifestyleType.Singleton)]
    public class MigrationOrchestrator {
        private readonly ICreateBareGit createBareGit;
        private readonly IOpenFolder openFolder;
        private readonly ICreateCloneGit createCloneGit;

        public MigrationOrchestrator(ICreateCloneGit createCloneGit, ICreateBareGit createBareGit, IOpenFolder openFolder) {
            this.createCloneGit = createCloneGit;
            this.createBareGit = createBareGit;
            this.openFolder = openFolder;
        }

        public void Migrate(string svnUrl, string usersAuthorsFullPathFile, string projectNameFolder, int retryTimes = 0) {
            if (string.IsNullOrWhiteSpace(usersAuthorsFullPathFile))
                throw new ArgumentException("usersAuthorsFullPathFile");

            if (Directory.Exists(projectNameFolder))
                throw new ProjectFolderAlreadyExistsException(projectNameFolder);

            CreateDirectoryBase(projectNameFolder);

            var fileNameUsers = Path.GetFileName(usersAuthorsFullPathFile);
            CopyUserFileToProjectFolder(usersAuthorsFullPathFile, projectNameFolder, fileNameUsers);

            var countClone = 0;
            do {
                try {
                    createCloneGit.Create(svnUrl, fileNameUsers, projectNameFolder);
                } catch (CloneErrorException) {

                }
            } while (countClone++ < retryTimes);

            createBareGit.Create(projectNameFolder);
            openFolder.Folder(projectNameFolder);
        }

        private static void CopyUserFileToProjectFolder(string usersAuthorsFullPathFile, string projectNameFolder,
            string fileNameUsers) {
            File.Copy(usersAuthorsFullPathFile, Path.Combine(projectNameFolder, fileNameUsers));
        }

        private static void CreateDirectoryBase(string projectName) {
            Directory.CreateDirectory(projectName);
        }
    }
}
