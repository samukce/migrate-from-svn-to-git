namespace MigrateFromSvnToGit {
    public interface ICreateCloneGit {
        void Create(string svnUrl, string usersAuthorsPathFile, string projectName);
    }
}
