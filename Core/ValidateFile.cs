using System;
using System.IO;
using MigrateFromSvnToGit.Core.Interfaces;

namespace MigrateFromSvnToGit.Core {
    public class ValidateFile : IValidateFile {
        public bool Exist(string fullPath) {
            if (string.IsNullOrWhiteSpace(fullPath))
                throw new ArgumentException("fullPath");

            return File.Exists(fullPath);
        }
    }
}
