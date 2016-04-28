using System;
using System.IO;
using NSubstitute;
using NUnit.Framework;
using SvnToGit.Core;
using SvnToGit.Core.Exceptions;
using SvnToGit.Core.Interfaces;

namespace Test.Core {
    [TestFixture]
    public class MigrateSourceToGitTest {
        private MigrationOrchestrator migrationOrchestrator;
        private ICreateCloneGit createCloneGit;
        private ICreateBareGit createBareGit;
        private IOpenFolder openFolder;

        private const string PathProjectName = "ProjectPath";
        private string FileNameUserFake { get { return Path.GetTempFileName(); } }

        [SetUp]
        public void Setup() {
            createCloneGit = Substitute.For<ICreateCloneGit>();
            createBareGit = Substitute.For<ICreateBareGit>();
            openFolder = Substitute.For<IOpenFolder>();

            if (Directory.Exists(PathProjectName))
                Directory.Delete(PathProjectName, true);

            migrationOrchestrator = new MigrationOrchestrator(createCloneGit, createBareGit, openFolder);
        }

        [Test]
        public void WhenToMigrateShouldCloneSvnToGit() {
            migrationOrchestrator.Migrate(string.Empty, FileNameUserFake, PathProjectName);

            createCloneGit.ReceivedWithAnyArgs(1)
                          .Create(string.Empty, string.Empty, string.Empty);
        }

        [Test]
        public void WhenToMigrateShouldCreateBare() {
            migrationOrchestrator.Migrate(string.Empty, FileNameUserFake, PathProjectName);

            createBareGit.ReceivedWithAnyArgs(1)
                         .Create(string.Empty);
        }

        [Test]
        public void ShouldFirstCallCreateCloneFromSvn() {
            migrationOrchestrator.Migrate(string.Empty, FileNameUserFake, PathProjectName);

            Received.InOrder(() => {
                createCloneGit.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
                createBareGit.Create(Arg.Any<string>());
            });
        }

        [Test]
        public void AbortIfErrorWhenCreateCloneGit() {
            createCloneGit.WhenForAnyArgs(c => c.Create(string.Empty, string.Empty, string.Empty))
                          .Throw<Exception>();

            Assert.Throws<Exception>(() => migrationOrchestrator.Migrate(string.Empty, FileNameUserFake, PathProjectName));
        }

        [Test]
        public void AbortIfErrorWhenCreateBareGit() {
            createBareGit.WhenForAnyArgs(c => c.Create(string.Empty))
                         .Throw<Exception>();

            Assert.Throws<Exception>(() => migrationOrchestrator.Migrate(string.Empty, FileNameUserFake, PathProjectName));
        }

        [Test]
        public void ShouldCreateFolderByProjectName() {
            migrationOrchestrator.Migrate(string.Empty, FileNameUserFake, PathProjectName);

            Assert.That(Directory.Exists(PathProjectName));
        }

        [Test]
        public void ShouldThrowExceptionIfFolderAlreadyExist() {
            Directory.CreateDirectory(PathProjectName);

            Assert.Throws<ProjectFolderAlreadyExistsException>(() => migrationOrchestrator.Migrate(string.Empty, FileNameUserFake, PathProjectName));
        }

        [Test]
        public void ShouldCopyUsersFileForFolderOfProject() {
            var filenameUsers = Path.GetTempFileName();

            migrationOrchestrator.Migrate(string.Empty, filenameUsers, PathProjectName);

            Assert.That(File.Exists(Path.Combine(PathProjectName, Path.GetFileName(filenameUsers))));
        }

        [Test]
        public void ShouldUsePathFromFolderProjectWhenCreateClone() {
            var filenameUsers = FileNameUserFake;

            migrationOrchestrator.Migrate(string.Empty, filenameUsers, PathProjectName);

            var newPathUsersFile = Path.GetFileName(filenameUsers);

            createCloneGit.Received(1)
                          .Create(Arg.Any<string>(), newPathUsersFile, Arg.Any<string>());
        }

        [Test]
        public void AbortIfUsersFileNotInformed() {
            Assert.Throws<ArgumentException>(() => migrationOrchestrator.Migrate(string.Empty, string.Empty, PathProjectName));
        }

        [Test]
        public void ShouldOpenFolderWhenConcluded() {
            migrationOrchestrator.Migrate(string.Empty, FileNameUserFake, PathProjectName);

            Received.InOrder(() => {
                createCloneGit.Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
                createBareGit.Create(Arg.Any<string>());
                openFolder.Folder(PathProjectName);
            });
        }

        [Test]
        public void RetryCloneGitTwoTimesIfCloneErrorWhenCreateCloneGitAndRetry() {
            createCloneGit.WhenForAnyArgs(c => c.Create(string.Empty, string.Empty, string.Empty))
                          .Throw<CloneErrorException>();

            migrationOrchestrator.Migrate(string.Empty, FileNameUserFake, PathProjectName, 1);

            createCloneGit.Received(2)
                          .Create(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Test]
        public void NotRetryCloneIfNotErrorInClone() {
            var filenameUsers = FileNameUserFake;

            migrationOrchestrator.Migrate(string.Empty, filenameUsers, PathProjectName, 1);

            var newPathUsersFile = Path.GetFileName(filenameUsers);

            createCloneGit.Received(1)
                          .Create(Arg.Any<string>(), newPathUsersFile, Arg.Any<string>());
        }
    }
}
