using System;
using Core;
using Core.Exceptions;
using Core.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Test.Core {
    [TestFixture]
    public class CreateCloneGitTest {
        private CreateCloneGit createCloneGit;
        private IProcessCaller processCaller;
        private IValidateFile validateFile;

        [SetUp]
        public void Setup() {
            processCaller = Substitute.For<IProcessCaller>();
            validateFile = Substitute.For<IValidateFile>();

            createCloneGit = new CreateCloneGit(processCaller, validateFile);
        }

        [Test]
        public void ShouldCallExternalProcessWithGitSvnClone() {
            validateFile.Exist(Arg.Any<string>())
                        .Returns(true);

            createCloneGit.Create("https://svn.com/project/svn", "users.txt", "projectName");

            processCaller.Received(1)
                         .Execute(@"git.exe",
                                   "svn clone \"https://svn.com/project/svn\" --authors-file=users.txt --no-metadata svnclone",
                                   "projectName");
        }

        [Test]
        public void ShouldToValidateFileUserProjectFolder() {
            validateFile.Exist("projectName\\users.txt")
                        .Returns(true);

            createCloneGit.Create("https://svn.com/project/svn", "users.txt", "projectName");

            processCaller.Received(1)
                         .Execute(@"git.exe",
                                   "svn clone \"https://svn.com/project/svn\" --authors-file=users.txt --no-metadata svnclone",
                                   "projectName");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNotInformedUrlSvnShouldThrowArgumentException() {
            createCloneGit.Create(string.Empty, "c:\\users.txt", "projectName");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNotInformedUserFilePathShouldThrowArgumentException() {
            createCloneGit.Create("https://svn.com/project/svn", string.Empty, "projectName");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNotInformedProjetcNameShouldThrowArgumentException() {
            createCloneGit.Create("https://svn.com/project/svn", "c:\\users.txt", string.Empty);
        }

        [Test]
        [ExpectedException(typeof(FileUsersNotFoundException))]
        public void ShouldThrowExceptionWhenFileUsersNotFound() {
            validateFile.Exist("c:\\users.txt")
                        .Returns(false);

            createCloneGit.Create("https://svn.com/project/svn", "c:\\users.txt", "projectName");
        }
    }
}
