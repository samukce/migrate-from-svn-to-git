using System;
using System.IO;
using Core;
using Core.Exceptions;
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

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNotInformedFileNameShouldThrowArgumentException() {
            new ProcessCaller().Execute(string.Empty, "/c md " + DirectoryNameTest);
        }

        [Test]
        [ExpectedException(typeof(ExecuteFileNotFoundException))]
        public void ShouldThrowExecuteFileNotFoundExceptionWhenFileNotFound() {
            new ProcessCaller().Execute("nothing.exe", string.Empty);
        }
    }
}
