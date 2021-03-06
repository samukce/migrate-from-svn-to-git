﻿using System;
using System.IO;
using NSubstitute;
using NUnit.Framework;
using SvnToGit.Core;
using SvnToGit.Core.Exceptions;
using SvnToGit.Core.Interfaces;

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
            processCaller.ExecuteSync("cmd.exe", "/c md " + DirectoryNameTest, string.Empty);

            Assert.That(Directory.Exists(DirectoryNameTest));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNotInformedFileNameShouldThrowArgumentException() {
            processCaller.ExecuteSync(string.Empty, "/c md " + DirectoryNameTest, string.Empty);
        }

        [Test]
        [ExpectedException(typeof(ExecuteFileNotFoundException))]
        public void ShouldThrowExecuteFileNotFoundExceptionWhenFileNotFound() {
            processCaller.ExecuteSync("nothing.exe", string.Empty, string.Empty);
        }

        [Test]
        public void ShouldThrowEventWithOutputFromProcess() {
            processCaller = new ProcessCaller(logger);

            processCaller.ExecuteSync("cmd.exe", "/c dir", string.Empty);

            logger.ReceivedWithAnyArgs()
                  .Info(null);
        }

        [Test]
        public void ShouldThrowEventWithOutputErrorFromProcess() {
            processCaller = new ProcessCaller(logger);

            processCaller.ExecuteSync("cmd.exe", "/c no_commande_xist", string.Empty);

            logger.ReceivedWithAnyArgs()
                  .Error(null);
        }

        [Test]
        public void ShouldConsiderRootpath() {
            const string rootPath = "rootpath";

            if (Directory.Exists(rootPath))
                Directory.Delete(rootPath, true);
            Directory.CreateDirectory(rootPath);

            processCaller.ExecuteSync("cmd.exe", "/c md " + DirectoryNameTest, rootPath);

            Assert.That(Directory.Exists(Path.Combine(rootPath, DirectoryNameTest)));
        }
    }
}
