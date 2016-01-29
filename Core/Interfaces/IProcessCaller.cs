namespace MigrateFromSvnToGit.Interfaces {
    public interface IProcessCaller {
        void Execute(string fileName, string arguments);
    }
}
