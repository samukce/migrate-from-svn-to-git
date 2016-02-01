using System;

namespace Core.Interfaces {
    public interface IProcessCaller {
        void Execute(string fileName, string arguments);
        Action<string> OutputLog { get; set; }
    }
}
