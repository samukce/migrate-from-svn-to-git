using System;
using System.Diagnostics;
using Castle.Core;
using Core.Interfaces;

namespace Core {
    [CastleComponent("Core.OpenFolder", typeof(IOpenFolder), Lifestyle = LifestyleType.Singleton)]
    public class OpenFolder : IOpenFolder {
        public void Folder(string path) {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("path");

            Process.Start(path);
        }
    }
}
