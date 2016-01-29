using System;
using MigrateFromSvnToGit;
using MigrateFromSvnToGit.Core;
using MigrateFromSvnToGit.Core.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Test.Core {
    [TestFixture]
    public class CreateBareGitTest {
        [Test]
        public void ShouldCallExternalProcessWithGitSvnClone() {
            var processCaller = Substitute.For<IProcessCaller>();

            var createBareGit = new CreateBareGit(processCaller);

            createBareGit.Create("projectName");

            processCaller.Received(1)
                         .Execute(@"git.exe",
                                   "clone --bare projectName");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNotInformedProjectNameShouldThrowArgumentException() {
            var createBareGit = new CreateBareGit(Substitute.For<IProcessCaller>());

            createBareGit.Create(string.Empty);
        }
    }
}
