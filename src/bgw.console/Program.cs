using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace bgw.console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            const string defaultFirstCommandString = "-c \"git ";
            const string defaultLastCommandString = "\"";

            var path = Directory.GetCurrentDirectory();

            var unixPath = new UnixPath(path).path;
            
            var bashExePath = Path.Combine(Environment.SystemDirectory, "bash.exe");

            var currentDirectoryCommand = $"cd {unixPath}";
            var gitCommand = new StringBuilder();
            gitCommand.Append(defaultFirstCommandString);
            
            foreach (var arg in args)
            {
                gitCommand.Append(arg);
                gitCommand.Append(" ");
            }

            gitCommand.Append(defaultLastCommandString);

            var process = new Process
            {
                StartInfo =
                {
                    FileName = bashExePath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = false,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    Arguments = currentDirectoryCommand
                }
            };
            process.Start();

            process.StartInfo.Arguments = gitCommand.ToString();
            process.Start();
            
            var results = process.StandardOutput.ReadToEnd();
            var error = process.StandardError;
            process.WaitForExit();
            process.Close();
            
            Console.WriteLine(results);
            
            while (error.Peek() >= 0)
            {
                Console.WriteLine(error.ReadLine());
            }
        }
    }
}