using System;

namespace MigrateFromSvnToGit {
    public class CreateBareGit : ICreateBareGit {
        private const string FileExecute = "git.exe";
        private const string Arguments = "clone --bare {0}";

        private readonly IProcessCaller processCaller;

        public CreateBareGit(IProcessCaller processCaller) {
            this.processCaller = processCaller;
        }

        public void Create(string projectName) {
            if (string.IsNullOrWhiteSpace(projectName))
                throw new ArgumentException("projectName");

            var argumentsFormat = string.Format(Arguments, projectName);

            processCaller.Execute(FileExecute, argumentsFormat);
        }
    }
}
