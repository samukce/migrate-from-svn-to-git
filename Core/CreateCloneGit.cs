using System;
using MigrateFromSvnToGit.Exceptions;
using MigrateFromSvnToGit.Interfaces;

namespace MigrateFromSvnToGit {
    public class CreateCloneGit : ICreateCloneGit {
        private const string FileExecute = "git.exe";
        private const string Arguments = "svn clone \"{0}\" --authors-file={1} --no-metadata {2}";

        private readonly IProcessCaller processCaller;
        private readonly IValidateFile validateFile;

        public CreateCloneGit(IProcessCaller processCaller, IValidateFile validateFile) {
            this.processCaller = processCaller;
            this.validateFile = validateFile;
        }

        public void Create(string svnUrl, string usersAuthorsPathFile, string projectName) {
            if (string.IsNullOrWhiteSpace(svnUrl))
                throw new ArgumentException("svnUrl");

            if (string.IsNullOrWhiteSpace(usersAuthorsPathFile))
                throw new ArgumentException("usersAuthorsPathFile");

            if (string.IsNullOrWhiteSpace(projectName))
                throw new ArgumentException("projectName");

            if (!validateFile.Exist(usersAuthorsPathFile))
                throw new FileUsersNotFoundException(usersAuthorsPathFile);

            var argumentsFormat = string.Format(Arguments, svnUrl, usersAuthorsPathFile, projectName);
            processCaller.Execute(FileExecute, argumentsFormat);
        }
    }
}
