using System;
using CefSharp;
using System.IO;
using System.Net;

namespace FrontEnd.BrowserForm {
    public class LocalSchemeHandler : IResourceHandler {
        private string mimeType;
        private MemoryStream stream;

        public bool ProcessRequestAsync(IRequest request, ICallback callback) {
            var uri = new Uri(request.Url);
            var file = uri.Authority + uri.AbsolutePath;

            if (!File.Exists(file))
                return false;

            var bytes = File.ReadAllBytes(file);

            stream = new MemoryStream(bytes);

            switch (Path.GetExtension(file)) {
                case ".html":
                    mimeType = "text/html";
                    break;
                case ".js":
                    mimeType = "text/javascript";
                    break;
                case ".png":
                    mimeType = "image/png";
                    break;
                case ".appcache":
                case ".manifest":
                    mimeType = "text/cache-manifest";
                    break;
                case ".css":
                    mimeType = "text/css";
                    break;
                default:
                    mimeType = "application/octet-stream";
                    break;
            }

            callback.Continue();
            return true;
        }

        public Stream GetResponse(IResponse response, out long responseLength, out string redirectUrl) {
            responseLength = stream.Length;
            redirectUrl = null;

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusText = "OK";
            response.MimeType = mimeType;

            return stream;
        }
    }
}