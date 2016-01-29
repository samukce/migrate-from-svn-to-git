using System;

namespace MigrateFromSvnToGit.Core.Exceptions {
    public class FileUsersNotFoundException : Exception {
        private const string MessageFormat = "Users' file {0} not found.";

        public FileUsersNotFoundException(string fileUsers) : base(string.Format(MessageFormat, fileUsers)) {

        }
    }
}
