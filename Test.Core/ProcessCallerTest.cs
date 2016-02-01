using System;
using System.IO;
using Core;
using Core.Exceptions;
using Core.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Test.Core {
    [TestFixture]
    public class ProcessCallerTest {
        private const string DirectoryNameTest = "teste";
        private ProcessCaller processCaller;
        private ILogger logger;

        [SetUp]
        public void Setup() {
            if (Directory.Exists(DirectoryNameTest))
                Directory.Delete(DirectoryNameTest, true);

            logger = Substitute.For<ILogger>();
            processCaller = new ProcessCaller(logger);
        }

        [Test]
        public void ShouldCreateTheTestDirectoryUsingCmdWindow() {
            processCaller.Execute("cmd.exe", "/c md " + DirectoryNameTest);

            Assert.That(Directory.Exists(DirectoryNameTest));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNotInformedFileNameShouldThrowArgumentException() {
            processCaller.Execute(string.Empty, "/c md " + DirectoryNameTest);
        }

        [Test]
        [ExpectedException(typeof(ExecuteFileNotFoundException))]
        public void ShouldThrowExecuteFileNotFoundExceptionWhenFileNotFound() {
            processCaller.Execute("nothing.exe", string.Empty);
        }

        [Test]
        public void ShouldThrowEventWithOutputFromProcess() {
            processCaller = new ProcessCaller(logger);

            processCaller.Execute("cmd.exe", "/c dir");

            logger.ReceivedWithAnyArgs()
                  .Info(null);
        }

        [Test]
        public void ShouldThrowEventWithOutputErrorFromProcess() {
            processCaller = new ProcessCaller(logger);

            processCaller.Execute("cmd.exe", "/c no_commande_xist");

            logger.ReceivedWithAnyArgs()
                  .Info(null);
        }
    }
}
