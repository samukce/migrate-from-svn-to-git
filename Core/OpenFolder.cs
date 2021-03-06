﻿using System;
using System.Diagnostics;
using Castle.Core;
using SvnToGit.Core.Interfaces;

namespace SvnToGit.Core {
    [CastleComponent("SvnToGit.Core.OpenFolder", typeof(IOpenFolder), Lifestyle = LifestyleType.Singleton)]
    public class OpenFolder : IOpenFolder {
        public void Folder(string path) {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("path");

            Process.Start(path);
        }
    }
}
