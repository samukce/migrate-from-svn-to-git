﻿using System;
using System.IO;
using Castle.Core;
using SvnToGit.Core.Interfaces;

namespace SvnToGit.Core {
    [CastleComponent("SvnToGit.Core.ValidateFile", typeof(IValidateFile), Lifestyle = LifestyleType.Singleton)]
    public class ValidateFile : IValidateFile {
        public bool Exist(string fullPath) {
            if (string.IsNullOrWhiteSpace(fullPath))
                throw new ArgumentException("fullPath");

            return File.Exists(fullPath);
        }
    }
}
