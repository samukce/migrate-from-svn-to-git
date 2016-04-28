using System;

namespace SvnToGit.Core.Exceptions {
    public class ProjectFolderAlreadyExistsException : Exception {
        private const string MessageFormat = "Project folder {0} already exists.";

        public ProjectFolderAlreadyExistsException(string projectFolder) : base(string.Format(MessageFormat, projectFolder)) {

        }
    }
}
