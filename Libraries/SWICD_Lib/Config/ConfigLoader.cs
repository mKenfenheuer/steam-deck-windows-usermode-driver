using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD_Lib.Config
{
    public class ConfigLoader
    {
        public static Configuration GetConfiguration(string file)
        {
            Configuration configuration = new Configuration();

            string[] lines = File.ReadAllLines(file);

            string section = "general";

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!line.StartsWith("#") && line.Length > 0) // Ignore comments
                {
                    if (line.StartsWith("["))
                    {
                        if (!line.Contains("]")) throw new Exception($"Malformed config. Expected \"]\" in line {i}");
                        section = line.Substring(1, line.IndexOf("]") - 1);
                        continue;
                    }

                    string[] parts = line.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                    if(parts.Length > 2)
                    {
                        parts = new string[]
                        {
                            line.Substring(0, line.IndexOf("=")),
                            line.Substring(line.IndexOf("=") +1)
                        };
                    }

                    if (parts.Length != 2)
                        throw new Exception($"Malformed config. Expected \"=\" in line {i}");

                    if (section == "general")
                    {
                        ProcessGeneralLine(parts[0].Trim(), parts[1].Trim(), ref configuration);
                    }
                    if (section == "actions")
                    {
                        ProcessActionsLine(parts[0].Trim(), parts[1].Trim(), ref configuration);
                    }

                    if (section == "buttons")
                    {
                        configuration.DefaultControllerConfig = ProcessButtonsLine(parts[0].Trim(), parts[1].Trim(), configuration.DefaultControllerConfig);
                        continue;
                    }

                    if (section == "axes")
                    {
                        configuration.DefaultControllerConfig = ProcessAxesLine(parts[0].Trim(), parts[1].Trim(), configuration.DefaultControllerConfig);
                        continue;
                    }

                    if (section == "profile")
                    {
                        configuration.DefaultControllerConfig = ProcessProfileLine(parts[0].Trim(), parts[1].Trim(), configuration.DefaultControllerConfig);
                        continue;
                    }

                    string executable = section.Substring(section.IndexOf(",") + 1);

                    if (section.StartsWith("buttons"))
                    {

                        configuration.PerProcessControllerConfig[executable] = ProcessButtonsLine(parts[0].Trim(), parts[1].Trim(),
                                                                    configuration.PerProcessControllerConfig.ContainsKey(executable) ?
                                                                    configuration.PerProcessControllerConfig[executable] : GetControllerConfigFromDefault(configuration, executable));
                    }

                    if (section.StartsWith("axes"))
                    {

                        configuration.PerProcessControllerConfig[executable] = ProcessAxesLine(parts[0].Trim(), parts[1].Trim(),
                                                                    configuration.PerProcessControllerConfig.ContainsKey(executable) ?
                                                                    configuration.PerProcessControllerConfig[executable] : GetControllerConfigFromDefault(configuration, executable));
                    }

                    if (section.StartsWith("profile"))
                    {

                        configuration.PerProcessControllerConfig[executable] = ProcessProfileLine(parts[0].Trim(), parts[1].Trim(),
                                                                    configuration.PerProcessControllerConfig.ContainsKey(executable) ?
                                                                    configuration.PerProcessControllerConfig[executable] : GetControllerConfigFromDefault(configuration, executable));
                    }
                }
            }
            return configuration;
        }

        private static ControllerConfig GetControllerConfigFromDefault(Configuration configuration, string executable)
        {
            var config = (ControllerConfig)configuration.DefaultControllerConfig.Clone();
            config.Executable = executable;
            return config;
        }

        public static void SaveConfiguration(Configuration config, string file)
        {
            string configText = "[general]\r\n";
            foreach (string executable in config.BlacklistedProcesses)
            {
                configText += $"blacklist={executable}\r\n";
            }
            foreach (string executable in config.WhitelistedProcesses)
            {
                configText += $"whitelist={executable}\r\n";
            }
            configText += $"mode={(config.OperationMode == OperationMode.Whitelist ? "whitelist" : "blacklist")}\r\n";

            configText += "\r\n";

            configText += config.DefaultControllerConfig.ToString();

            configText += "\r\n";

            configText += config.ButtonActions.ToString();

            configText += "\r\n";

            foreach (string executable in config.PerProcessControllerConfig.Keys)
                configText += config.PerProcessControllerConfig[executable].ToString() + "\r\n";

            File.WriteAllText(file, configText.Trim());
        }

        private static ControllerConfig ProcessAxesLine(string v1, string v2, ControllerConfig configuration)
        {
            HardwareAxis axis = (HardwareAxis)Enum.Parse(typeof(HardwareAxis), v1);
            configuration.AxisMapping[axis] = new EmulatedAxisConfig(v2);
            return configuration;
        }

        private static ControllerConfig ProcessProfileLine(string v1, string v2, ControllerConfig configuration)
        {
            if (v1 == "DisableLizardMode")
            {
                configuration.ProfileSettings.DisableLizardMode = v2.ToLower() == "true";
            }
            return configuration;
        }

        private static void ProcessActionsLine(string v1, string v2, ref Configuration configuration)
        {
            if (v1 == "OpenWindowsGameBar")
            {
                configuration.ButtonActions.OpenWindowsGameBar = (HardwareButton)Enum.Parse(typeof(HardwareButton), v2);
            }
        }

        private static ControllerConfig ProcessButtonsLine(string v1, string v2, ControllerConfig configuration)
        {
            HardwareButton hardwareButton = HardwareButton.None;
            EmulatedButton emulatedButton = EmulatedButton.None;

            Enum.TryParse(v1, out hardwareButton);
            Enum.TryParse(v2, out emulatedButton);

            configuration.ButtonMapping[hardwareButton] = emulatedButton;

            return configuration;
        }

        private static void ProcessGeneralLine(string key, string value, ref Configuration config)
        {
            switch (key)
            {
                case "blacklist":
                    config.BlacklistedProcesses.Add(value);
                    break;
                case "whitelist":
                    config.WhitelistedProcesses.Add(value);
                    break;
                case "mode":
                    if (value == "whitelist")
                    {
                        config.OperationMode = OperationMode.Whitelist;
                    }
                    else if (value == "blacklist")
                    {
                        config.OperationMode = OperationMode.Blacklist;
                    }
                    else
                    {
                        throw new Exception("Unexpected mode of operation!");
                    }
                    break;
            }
        }
    }
}
