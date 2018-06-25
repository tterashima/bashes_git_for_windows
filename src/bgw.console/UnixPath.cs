using System;
using System.Text;
using System.Text.RegularExpressions;

namespace bgw.console
{
    public class UnixPath
    {
        private const string WindowsPathAnchor = "^(?<driveLetter>.?):(?<directoryPath>.*?)$";
        private const string WindowsPathSeparator = "\\\\";
        private const string UnixPathSeparator = "/";
        private string driveLetter;
        private readonly string directoryPath;
        public readonly string path;
        
        public UnixPath(string windowsPath)
        {
            var pathRegex = new Regex(WindowsPathAnchor);
            var match = pathRegex.Match(windowsPath);
            driveLetter = match.Groups["driveLetter"].Value.ToLower();
            directoryPath = Regex.Replace(match.Groups["directoryPath"].Value, WindowsPathSeparator, UnixPathSeparator);
            var pathBuilder = new StringBuilder();
            pathBuilder.Append($"/mnt/{driveLetter}{directoryPath}");
            path = pathBuilder.ToString();
        }
    }
}