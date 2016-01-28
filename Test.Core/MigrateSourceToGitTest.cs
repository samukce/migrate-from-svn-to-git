using System;
using MigrateFromSvnToGit;
using NSubstitute;
using NUnit.Framework;

namespace Test.Core {
    [TestFixture]
    public class MigrateSourceToGitTest {
        [Test]
        public void WhenToMigrateShouldCloneSvnToGit() {
            var createCloneGit = Substitute.For<ICreateCloneGit>();

            var migrationOrchestrator = new MigrationOrchestrator(createCloneGit, Substitute.For<ICreateBareGit>());

            migrationOrchestrator.Migrate(string.Empty, string.Empty, string.Empty);

            createCloneGit.ReceivedWithAnyArgs(1)
                          .Create(string.Empty, string.Empty, string.Empty);
        }

        [Test]
        public void WhenToMigrateShouldCreateBare() {
            var createBareGit = Substitute.For<ICreateBareGit>();

            var migrationOrchestrator = new MigrationOrchestrator(Substitute.For<ICreateCloneGit>(), createBareGit);

            migrationOrchestrator.Migrate(string.Empty, string.Empty, string.Empty);

            createBareGit.ReceivedWithAnyArgs(1)
                         .Create(string.Empty);
        }

        [Test]
        public void ShouldFirstCallCreateCloneFromSvn() {
            var createBareGit = Substitute.For<ICreateBareGit>();
            var createCloneGit = Substitute.For<ICreateCloneGit>();

            var migrationOrchestrator = new MigrationOrchestrator(createCloneGit, createBareGit);

            migrationOrchestrator.Migrate(string.Empty, string.Empty, string.Empty);

            Received.InOrder(() => {
                createCloneGit.Create(string.Empty, string.Empty, string.Empty);
                createBareGit.Create(string.Empty);
            });
        }

        [Test]
        public void AbortIfErrorWhenCreateCloneGit() {
            var createBareGit = Substitute.For<ICreateBareGit>();
            var createCloneGit = Substitute.For<ICreateCloneGit>();

            createCloneGit.WhenForAnyArgs(c => c.Create(string.Empty, string.Empty, string.Empty))
                          .Throw<Exception>();

            var migrationOrchestrator = new MigrationOrchestrator(createCloneGit, createBareGit);

            Assert.Throws<Exception>(() => migrationOrchestrator.Migrate(string.Empty, string.Empty, string.Empty));
        }

        [Test]
        public void AbortIfErrorWhenCreateBareGit() {
            var createBareGit = Substitute.For<ICreateBareGit>();
            var createCloneGit = Substitute.For<ICreateCloneGit>();

            createBareGit.WhenForAnyArgs(c => c.Create(string.Empty))
                         .Throw<Exception>();

            var migrationOrchestrator = new MigrationOrchestrator(createCloneGit, createBareGit);

            Assert.Throws<Exception>(() => migrationOrchestrator.Migrate(string.Empty, string.Empty, string.Empty));
        }
    }
}
