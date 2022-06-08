using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using neptune_hidapi.net;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using SWICD_Lib.Config;
using System.IO;
using System.Diagnostics;

namespace SWICD.Services
{
    internal class ControllerService
    {
        public static ControllerService Instance { get; private set; } = new ControllerService();
        public bool EmulationEnabled { get; private set; }
        public bool LizardModeEnabled { get => _neptuneController.LizardModeEnabled; private set => _neptuneController.LizardModeEnabled = value; }
        public bool Started { get; private set; }
        public Configuration Configuration { get; internal set; }

        private NeptuneController _neptuneController = new NeptuneController();
        private ViGEmClient _viGEmClient;
        private ControllerConfig _currentControllerConfig = new ControllerConfig();
        private IXbox360Controller _emulatedController;
        private Task _checkProcessesTask;
        private bool _running = true;

        public event EventHandler<bool> OnServiceStartStop;

        public ControllerService()
        {
            if (File.Exists("app_config.conf"))
            {
                Configuration = ConfigLoader.GetConfiguration("app_config.conf");
                LoggingService.LogInformation("Config Loaded.");
                LoggingService.LogInformation($"Executable specific profiles: {Configuration.PerProcessControllerConfig.Count}");
                LoggingService.LogInformation($"Mode: {Configuration.GenericSettings.OperationMode}");
                LoggingService.LogInformation($"Blacklisted processes: {Configuration.GenericSettings.BlacklistedProcesses.Count}");
                LoggingService.LogInformation($"Whitelisted processes: {Configuration.GenericSettings.WhitelistedProcesses.Count}");
            }
            else
            {
                LoggingService.LogWarning("Could not load config. Creating default empty config.");
                Configuration = new Configuration();
                ConfigLoader.SaveConfiguration(Configuration, "app_config.conf");
                LoggingService.LogInformation($"Executable specific profiles: {Configuration.PerProcessControllerConfig.Count}");
                LoggingService.LogInformation($"Mode: {Configuration.GenericSettings.OperationMode}");
                LoggingService.LogInformation($"Blacklisted processes: {Configuration.GenericSettings.BlacklistedProcesses.Count}");
                LoggingService.LogInformation($"Whitelisted processes: {Configuration.GenericSettings.WhitelistedProcesses.Count}");
            }
            _neptuneController.OnControllerInputReceived += OnControllerInputReceived;
            _viGEmClient = new ViGEmClient();
            _emulatedController = _viGEmClient.CreateXbox360Controller();
            _emulatedController.FeedbackReceived += EmulatedController_FeedbackReceived;
        }

        private void EmulatedController_FeedbackReceived(object sender, Xbox360FeedbackReceivedEventArgs e)
        {

        }

        private async Task CheckProcessesLoop()
        {
            while (_running)
            {
                await CheckProcesses();
                await Task.Delay(1000);
            }
        }

        private async Task CheckProcesses()
        {
            try
            {
                var list = Process.GetProcesses();

                bool emulate = Configuration.GenericSettings.OperationMode == OperationMode.Blacklist ? true : false;

                foreach (var process in list)
                {
                    if (Configuration.GenericSettings.OperationMode == OperationMode.Blacklist &&
                        Configuration.GenericSettings.BlacklistedProcesses.Contains($"{process.ProcessName}.exe"))
                    {
                        emulate = false;
                        break;
                    }
                    if (Configuration.GenericSettings.OperationMode == OperationMode.Whitelist &&
                        Configuration.GenericSettings.WhitelistedProcesses.Contains($"{process.ProcessName}.exe"))
                    {

                        emulate = true;
                        break;
                    }

                    if (Configuration.GenericSettings.OperationMode == OperationMode.Combined &&
                        Configuration.GenericSettings.BlacklistedProcesses.Contains($"{process.ProcessName}.exe"))
                    {
                        emulate = false;
                        break;
                    }

                    if (Configuration.GenericSettings.OperationMode == OperationMode.Combined &&
                        Configuration.GenericSettings.WhitelistedProcesses.Contains($"{process.ProcessName}.exe"))
                    {
                        emulate = true;
                    }
                }


                var profile = Configuration.DefaultControllerConfig;

                foreach (var process in list.OrderBy(p => p.ProcessName))
                {
                    if (Configuration.PerProcessControllerConfig.ContainsKey(process.ProcessName))
                    {
                        profile = Configuration.PerProcessControllerConfig[process.ProcessName];
                        break;
                    }
                }

                if (EmulationEnabled != emulate)
                {
                    EmulationEnabled = emulate;
                    LoggingService.LogDebug($"EmulationEnabled changed to: {EmulationEnabled}");
                }
                _currentControllerConfig = profile;
                if (LizardModeEnabled != !_currentControllerConfig.ProfileSettings.DisableLizardMode)
                {
                    LizardModeEnabled = !_currentControllerConfig.ProfileSettings.DisableLizardMode;
                    LoggingService.LogDebug($"LizardModeEnabled changed to: {LizardModeEnabled}");
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError($"Could not check for process changes: {ex}");
            }
        }


        private void OnControllerInputReceived(object sender, NeptuneControllerInputEventArgs e)
        {
            _ = Task.Run(() => HandleInput(e.State));
        }

        public void Start()
        {
            _running = true;
            _checkProcessesTask = Task.Run(async () => await CheckProcessesLoop());
            _emulatedController.Connect();
            _currentControllerConfig = Configuration.DefaultControllerConfig;
            try
            {
                _neptuneController.Open();
            }
            catch (Exception ex)
            {
                LoggingService.LogError($"Could not open neptune controller: {ex}");
            }
            Started = true;
            LoggingService.LogInformation("Driver started.");
            OnServiceStartStop?.Invoke(this, true);
        }

        public void Stop()
        {
            _running = false;
            try
            {
                _neptuneController.Close();
            }
            catch (Exception ex)
            {
                LoggingService.LogError($"Could not close neptune controller: {ex}");
            }
            EmulationEnabled = false;
            Started = false;
            _emulatedController.Disconnect();
            LoggingService.LogInformation("Driver stopped.");
            OnServiceStartStop?.Invoke(this, false);
        }

        private void HandleInput(NeptuneControllerInputState state)
        {
            _emulatedController.ResetReport();

            if (EmulationEnabled)
                InputMapper.MapInput(_currentControllerConfig, state, _emulatedController);

            _emulatedController.SubmitReport();
        }
    }
}
