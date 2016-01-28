using System;

namespace MigrateFromSvnToGit {
    public class CreateCloneGit : ICreateCloneGit {
        private const string FileExecute = "git.exe";
        private const string Arguments = "svn clone \"{0}\" --authors-file={1} --no-metadata {2}";

        private readonly IProcessCaller processCaller;

        public CreateCloneGit(IProcessCaller processCaller) {
            this.processCaller = processCaller;
        }

        public void Create(string svnUrl, string usersAuthorsPathFile, string projectName) {
            if (string.IsNullOrWhiteSpace(svnUrl))
                throw new ArgumentException("svnUrl");

            if (string.IsNullOrWhiteSpace(usersAuthorsPathFile))
                throw new ArgumentException("usersAuthorsPathFile");

            if (string.IsNullOrWhiteSpace(projectName))
                throw new ArgumentException("projectName");

            var argumentsFormat = string.Format(Arguments, svnUrl, usersAuthorsPathFile, projectName);
            processCaller.Execute(FileExecute, argumentsFormat);
        }
    }
}
