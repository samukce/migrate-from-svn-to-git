﻿using System;
using System.IO;
using MigrateFromSvnToGit;
using MigrateFromSvnToGit.Core;
using NUnit.Framework;

namespace Test.Core {
    [TestFixture]
    public class ValidateFileTest {
        [Test]
        public void ShouldReturnTrueWhenFileExist() {
            var filename = Path.GetTempFileName();

            Assert.That(new ValidateFile().Exist(filename));
        }

        [Test]
        public void ShouldReturnFalseWhenFileDoesNotExist() {
            Assert.That(new ValidateFile().Exist("fileDoesNotExist.txt"), Is.False);
        }

        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenNotInformedPathShouldThrowArgumentException(string fullPath) {
            new ValidateFile().Exist(fullPath);
        }
    }
}
