using System;
using MigrateFromSvnToGit;
using MigrateFromSvnToGit.Core;
using MigrateFromSvnToGit.Core.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Test.Core {
    [TestFixture]
    public class MigrateSourceToGitTest {
        private MigrationOrchestrator migrationOrchestrator;
        private ICreateCloneGit createCloneGit;
        private ICreateBareGit createBareGit;

        [SetUp]
        public void Setup() {
            createCloneGit = Substitute.For<ICreateCloneGit>();
            createBareGit = Substitute.For<ICreateBareGit>();

            migrationOrchestrator = new MigrationOrchestrator(createCloneGit, createBareGit);
        }

        [Test]
        public void WhenToMigrateShouldCloneSvnToGit() {
            migrationOrchestrator.Migrate(string.Empty, string.Empty, string.Empty);

            createCloneGit.ReceivedWithAnyArgs(1)
                          .Create(string.Empty, string.Empty, string.Empty);
        }

        [Test]
        public void WhenToMigrateShouldCreateBare() {
            migrationOrchestrator.Migrate(string.Empty, string.Empty, string.Empty);

            createBareGit.ReceivedWithAnyArgs(1)
                         .Create(string.Empty);
        }

        [Test]
        public void ShouldFirstCallCreateCloneFromSvn() {
            migrationOrchestrator.Migrate(string.Empty, string.Empty, string.Empty);

            Received.InOrder(() => {
                createCloneGit.Create(string.Empty, string.Empty, string.Empty);
                createBareGit.Create(string.Empty);
            });
        }

        [Test]
        public void AbortIfErrorWhenCreateCloneGit() {
            createCloneGit.WhenForAnyArgs(c => c.Create(string.Empty, string.Empty, string.Empty))
                          .Throw<Exception>();

            Assert.Throws<Exception>(() => migrationOrchestrator.Migrate(string.Empty, string.Empty, string.Empty));
        }

        [Test]
        public void AbortIfErrorWhenCreateBareGit() {
            createBareGit.WhenForAnyArgs(c => c.Create(string.Empty))
                         .Throw<Exception>();

            Assert.Throws<Exception>(() => migrationOrchestrator.Migrate(string.Empty, string.Empty, string.Empty));
        }
    }
}
