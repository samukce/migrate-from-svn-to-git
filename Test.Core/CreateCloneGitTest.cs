using System;
using MigrateFromSvnToGit;
using NSubstitute;
using NUnit.Framework;

namespace Test.Core {
    [TestFixture]
    public class CreateCloneGitTest {
        [Test]
        public void ShouldCallExternalProcessWithGitSvnClone() {
            var processCaller = Substitute.For<IProcessCaller>();

            var createCloneGit = new CreateCloneGit(processCaller);

            createCloneGit.Create("https://svn.com/project/svn", "c:\\users.txt", "projectName");

            processCaller.Received(1)
                         .Execute(@"git.exe",
                                   "svn clone \"https://svn.com/project/svn\" --authors-file=c:\\users.txt --no-metadata projectName");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNotInformedUrlSvnShouldThrowArgumentException() {
            var createCloneGit = new CreateCloneGit(Substitute.For<IProcessCaller>());

            createCloneGit.Create(string.Empty, "c:\\users.txt", "projectName");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNotInformedUserFilePathShouldThrowArgumentException() {
            var createCloneGit = new CreateCloneGit(Substitute.For<IProcessCaller>());

            createCloneGit.Create("https://svn.com/project/svn", string.Empty, "projectName");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNotInformedProjetcNameShouldThrowArgumentException() {
            var createCloneGit = new CreateCloneGit(Substitute.For<IProcessCaller>());

            createCloneGit.Create("https://svn.com/project/svn", "c:\\users.txt", string.Empty);
        }
    }
}
