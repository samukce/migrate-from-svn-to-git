using System;
using System.ComponentModel;
using System.Diagnostics;
using MigrateFromSvnToGit.Exceptions;
using MigrateFromSvnToGit.Interfaces;

namespace MigrateFromSvnToGit {
    public class ProcessCaller : IProcessCaller {
        private const int MillisecondsTimeout = 1000 * 60 * 5;

        public void Execute(string fileName, string arguments) {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName");

            var process = new Process {
                StartInfo = {
                    FileName = fileName,
                    Arguments = arguments,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            try {
                process.Start();

                process.WaitForExit(MillisecondsTimeout);
            } catch (Win32Exception) {
                throw new ExecuteFileNotFoundException(fileName);
            }
        }
    }
}
