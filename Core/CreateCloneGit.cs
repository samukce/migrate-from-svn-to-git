using System;
using System.IO;
using Castle.Core;
using Core.Exceptions;
using Core.Interfaces;

namespace Core {
    [CastleComponent("Core.CreateCloneGit", typeof(ICreateCloneGit), Lifestyle = LifestyleType.Singleton)]
    public class CreateCloneGit : ICreateCloneGit {
        private const string FileExecute = "git.exe";
        private const string Arguments = "svn clone \"{0}\" --authors-file={1} --no-metadata svnclone";
        private const string FileNameErrorClone = "perl.exe.stackdump";

        private readonly IProcessCaller processCaller;
        private readonly IValidateFile validateFile;

        public CreateCloneGit(IProcessCaller processCaller, IValidateFile validateFile) {
            this.processCaller = processCaller;
            this.validateFile = validateFile;
        }

        public void Create(string svnUrl, string usersAuthorsPathFile, string projectNameFolder) {
            if (string.IsNullOrWhiteSpace(svnUrl))
                throw new ArgumentException("svnUrl");

            if (string.IsNullOrWhiteSpace(usersAuthorsPathFile))
                throw new ArgumentException("usersAuthorsPathFile");

            if (string.IsNullOrWhiteSpace(projectNameFolder))
                throw new ArgumentException("projectNameFolder");

            var fileUsersPath = Path.Combine(projectNameFolder, usersAuthorsPathFile);

            if (!validateFile.Exist(fileUsersPath))
                throw new FileUsersNotFoundException(fileUsersPath);

            var argumentsFormat = string.Format(Arguments, svnUrl, usersAuthorsPathFile);
            processCaller.ExecuteSync(FileExecute, argumentsFormat, projectNameFolder);

            var fullFileNameError = GetFullFileNameError(projectNameFolder);

            if (validateFile.Exist(fullFileNameError))
                throw new CloneErrorException();
        }

        private string GetFullFileNameError(string projectNameFolder) {
            return Path.Combine(projectNameFolder, "svnclone", FileNameErrorClone);
        }
    }
}
