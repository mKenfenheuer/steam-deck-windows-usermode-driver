using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                    WriteGitInfo(args[1]);
                    break;
                case "delete-git-info":
                    File.Delete(args[1]);
                    break;
                case "write-assembly-info":
                    WriteAssemblyInfo(args[1]);
                    break;
                case "write-setup-info":
                    WriteSetupInfo(args[1]);
                    break;
                case "write-build-info":
                    WriteBuildVersionInfo(args[1]);
                    break;
            }
        }

        private static void WriteSetupInfo(string v)
        {
            string info = GetVersionInfo();
            string text = File.ReadAllText(v);
            string replaced = Regex.Replace(text, "(\"ProductVersion\" = \"8:)[^\\\"]+", $"$1{GetVersionInfo()}");
            File.WriteAllText(v, replaced);
        }

        private static void WriteAssemblyInfo(string v)
        {
            string info = GetVersionInfo();
            string text = File.ReadAllText(v);
            string replaced = Regex.Replace(text, "(AssemblyVersion\\(\")[^\\\"]+", $"AssemblyVersion(\"{GetAssemblyVersionInfo()}");
            replaced = Regex.Replace(replaced, "(AssemblyFileVersion\\(\")[^\\\"]+", $"AssemblyFileVersion(\"{GetAssemblyVersionInfo()}");
            File.WriteAllText(v, replaced);
        }

        private static void WriteBuildVersionInfo(string v)
        {
            string info = GetVersionInfo();
            string text = File.ReadAllText(v);
            string replaced = Regex.Replace(text, "(Version => \")[^\\\"]+", $"Version => \"{GetVersionInfo()}");
            replaced = Regex.Replace(replaced, "(BuildTime => new DateTime\\()[^\\)]+", $"BuildTime => new DateTime({DateTime.UtcNow.Ticks}");
            File.WriteAllText(v, replaced);
        }

        private static object GetAssemblyVersionInfo()
        {
            string tag = GetGitTag();
            tag = Regex.Replace(tag, "[^0-9]+([0-9\\.]*).*", "$1");
            return tag;
        }

        private static string GetVersionInfo()
        {
            string tag = GetGitTag();
            string hash = GetCommitHash();
            tag = Regex.Replace(tag, $"-?[0-9]+-g{hash}", "");
            string config = GetConfiguration();
            return $"{tag}-{hash}-{config}";
        }

        private static void WriteGitInfo(string file)
        {
            File.WriteAllText(file, GetVersionInfo());
        }

        private static string GetConfiguration()
        {
#if DEBUG
            return "debug";
#else
            return "release";
#endif

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
    }
}
