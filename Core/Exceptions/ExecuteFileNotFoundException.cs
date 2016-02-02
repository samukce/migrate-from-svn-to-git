using System;

namespace Core.Exceptions {
    public class ExecuteFileNotFoundException : Exception {
        private const string MessageFormat = "File execute {0} not found.";

        public ExecuteFileNotFoundException(string fileName) : base(string.Format(MessageFormat, fileName)) {

        }
    }
}
