using System.Diagnostics;

namespace MigrateFromSvnToGit {
    public class ProcessCaller : IProcessCaller {
        private const int MillisecondsTimeout = 1000 * 60 * 5;

        public void Execute(string fileName, string arguments) {
            var process = new Process {
                StartInfo = {
                    FileName = fileName,
                    Arguments = arguments,
                    ErrorDialog = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                }
            };

            process.Start();
            process.WaitForExit(MillisecondsTimeout);
        }
    }
}
