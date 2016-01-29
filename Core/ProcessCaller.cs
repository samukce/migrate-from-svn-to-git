using System;
using System.ComponentModel;
using System.Diagnostics;
using Castle.Core;
using Core.Exceptions;
using Core.Interfaces;

namespace Core {
    [CastleComponent("Core.ProcessCaller", typeof(IProcessCaller), Lifestyle = LifestyleType.Singleton)]
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
