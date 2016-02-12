namespace Core.Interfaces {
    public interface IProcessCaller {
        void ExecuteSync(string fileName, string arguments, string workingDirectory);
    }
}
