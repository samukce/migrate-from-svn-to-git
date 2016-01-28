namespace MigrateFromSvnToGit {
    public interface IProcessCaller {
        void Execute(string fileName, string arguments);
    }
}
