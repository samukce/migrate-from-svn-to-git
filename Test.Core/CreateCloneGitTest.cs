using System;
using NSubstitute;
using NUnit.Framework;
using SvnToGit.Core;
using SvnToGit.Core.Exceptions;
using SvnToGit.Core.Interfaces;

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
            validateFile.Exist("projectName\\users.txt")
                        .Returns(true);

            validateFile.Exist("projectName\\svnclone\\perl.exe.stackdump")
                        .Returns(false);

            createCloneGit.Create("https://svn.com/project/svn", "users.txt", "projectName");

            processCaller.Received(1)
                         .ExecuteSync(@"git.exe",
                                   "svn clone \"https://svn.com/project/svn\" --authors-file=users.txt --no-metadata svnclone",
                                   "projectName");
        }

        [Test]
        public void ShouldToValidateFileUserProjectFolder() {
            validateFile.Exist("projectName\\users.txt")
                        .Returns(true);

            createCloneGit.Create("https://svn.com/project/svn", "users.txt", "projectName");

            processCaller.Received(1)
                         .ExecuteSync(@"git.exe",
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

        [Test]
        [ExpectedException(typeof(CloneErrorException))]
        public void ShouldThrowExceptionWhenErrorFileFound() {
            validateFile.Exist("projectName\\users.txt")
                        .Returns(true);

            validateFile.Exist("projectName\\svnclone\\perl.exe.stackdump")
                        .Returns(true);

            createCloneGit.Create("https://svn.com/project/svn", "users.txt", "projectName");
        }

        [Test]
        public void ShouldToDeleteSvnCloneFolderWhenCallCreateClone() {
            createCloneGit = Substitute.ForPartsOf<CreateCloneGit>(processCaller, validateFile);

            validateFile.Exist("projectName\\users.txt")
                        .Returns(true);

            validateFile.Exist("projectName\\svnclone\\perl.exe.stackdump")
                        .Returns(false);

            createCloneGit.Create("https://svn.com/project/svn", "users.txt", "projectName");

            createCloneGit.Received(1)
                          .CreateEmptyFolder("svnclone");
        }


        [Test]
        public void ShouldFirstCallCreateFolderAndThanCloneFromSvn() {
            createCloneGit = Substitute.ForPartsOf<CreateCloneGit>(processCaller, validateFile);

            validateFile.Exist("projectName\\users.txt")
                        .Returns(true);

            validateFile.Exist("projectName\\svnclone\\perl.exe.stackdump")
                        .Returns(false);

            createCloneGit.Create("https://svn.com/project/svn", "users.txt", "projectName");

            Received.InOrder(() => {
                createCloneGit.CreateEmptyFolder("svnclone");
                processCaller.ExecuteSync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
            });
        }
    }
}
