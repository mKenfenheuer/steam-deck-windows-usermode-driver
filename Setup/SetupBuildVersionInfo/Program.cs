using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetupBuildVersionInfo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            switch (args[0])
            {
                case "write-git-info":
                    WriteGitInfo();
                    break;
            }
        }

        private static void WriteGitInfo()
        {
            string tag = GetGitTag();
            string hash = GetCommitHash();
            string isDirty = GetDirtyStatus();
        }

        private static string GetDirtyStatus()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "cmd.exe",
                    Arguments = "/c \"git diff --quiet || echo 'dirty'\""
                }
            };
            process.Start();
            process.WaitForExit();
            string dirty = process.StandardOutput.ReadToEnd().Trim();
            return dirty;
        }

        private static string GetGitTag()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "git",
                    Arguments = "describe --tags"
                }
            };
            process.Start();
            process.WaitForExit();
            string tag = process.StandardOutput.ReadToEnd().Trim();
            return tag;
        }

        private static string GetCommitHash()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    FileName = "git",
                    Arguments = "show --format=\"%h\" --no-patch"
                }
            };
            process.Start();
            process.WaitForExit();
            string hash = process.StandardOutput.ReadToEnd().Trim();
            return hash;
        }

        //git diff --quiet || echo 'dirty'
    }
}

/*
 * 
 * git describe --tags > git-info.txt
echo "-" >> git-info.txt
git show --format="%h" --no-patch >> git-info.txt
*/
