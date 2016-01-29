using System.IO;
using MigrateFromSvnToGit.Interfaces;

namespace MigrateFromSvnToGit {
    public class ValidateFile : IValidateFile {
        public bool Exist(string fullPath) {
            return File.Exists(fullPath);
        }
    }
}
