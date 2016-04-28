using System;
using Castle.Core;
using SvnToGit.Core.Interfaces;

namespace SvnToGit.Core {
    [CastleComponent("SvnToGit.Core.CreateBareGit", typeof(ICreateBareGit), Lifestyle = LifestyleType.Singleton)]
    public class CreateBareGit : ICreateBareGit {
        private const string FileExecute = "git.exe";
        private const string Arguments = "clone --bare svnclone";

        private readonly IProcessCaller processCaller;

        public CreateBareGit(IProcessCaller processCaller) {
            this.processCaller = processCaller;
        }

        public void Create(string projectNameFolder) {
            if (string.IsNullOrWhiteSpace(projectNameFolder))
                throw new ArgumentException("projectNameFolder");

            var argumentsFormat = string.Format(Arguments);

            processCaller.ExecuteSync(FileExecute, argumentsFormat, projectNameFolder);
        }
    }
}
