using System.IO;
using MigrateFromSvnToGit;
using NUnit.Framework;

namespace Test.Core {
    [TestFixture]
    public class ProcessCallerTest {
        private const string DirectoryNameTest = "teste";

        [SetUp]
        public void Setup() {
            if (Directory.Exists(DirectoryNameTest))
                Directory.Delete(DirectoryNameTest, true);
        }

        [Test]
        public void ShouldCreateTheTestDirectoryUsingCmdWindow() {
            new ProcessCaller().Execute("cmd.exe", "/c md " + DirectoryNameTest);

            Assert.That(Directory.Exists(DirectoryNameTest));
        }
    }
}
