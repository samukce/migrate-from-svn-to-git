using System;
using System.IO;
using Castle.Core;
using SvnToGit.Core.Exceptions;
using SvnToGit.Core.Interfaces;

namespace SvnToGit.Core {
    [CastleComponent("SvnToGit.Core.CreateCloneGit", typeof(ICreateCloneGit), Lifestyle = LifestyleType.Singleton)]
    public class CreateCloneGit : ICreateCloneGit {
        private const string SvnCloneFolder = "svnclone";
        private const string FileExecute = "git.exe";
        private const string Arguments = "svn clone \"{0}\" --authors-file={1} --no-metadata " + SvnCloneFolder;
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

            CreateEmptyFolder(SvnCloneFolder);

            var argumentsFormat = string.Format(Arguments, svnUrl, usersAuthorsPathFile);
            processCaller.ExecuteSync(FileExecute, argumentsFormat, projectNameFolder);

            var fullFileNameError = GetFullFileNameError(projectNameFolder);

            if (validateFile.Exist(fullFileNameError))
                throw new CloneErrorException();
        }

        private string GetFullFileNameError(string projectNameFolder) {
            return Path.Combine(projectNameFolder, SvnCloneFolder, FileNameErrorClone);
        }

        public virtual void CreateEmptyFolder(string path) {
            if (Directory.Exists(path))
                Directory.Delete(path, true);

            Directory.CreateDirectory(path);
        }
    }
}
