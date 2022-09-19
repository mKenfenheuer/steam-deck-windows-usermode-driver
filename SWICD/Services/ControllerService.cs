using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using neptune_hidapi.net;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using SWICD.Config;
using System.IO;
using System.Diagnostics;
using SWICD.ViewModels;

namespace SWICD.Services
{
    internal class ControllerService
    {
        public static ControllerService Instance { get; private set; } = new ControllerService();
        public bool EmulationEnabled { get; private set; }
        public string DecisionExecutable { get; private set; }
        public bool LizardMouseEnabled { get => _neptuneController.LizardMouseEnabled; private set => _neptuneController.LizardMouseEnabled = value; }
        public bool LizardButtonsEnabled { get => _neptuneController.LizardButtonsEnabled; private set => _neptuneController.LizardButtonsEnabled = value; }
        public bool Started { get; private set; }
        public Configuration Configuration { get; internal set; }

        private KeyboardMouseInputMapper _keyboardMouseInputMapper = new KeyboardMouseInputMapper();
        internal KeyboardMouseInputMapper KeyboardMouseInputMapper => _keyboardMouseInputMapper;

        public bool FailedInit { get; private set; }

        private ButtonActionsProcessor _buttonActionsProcessor = new ButtonActionsProcessor();
        private NeptuneController _neptuneController;
        private ViGEmClient _viGEmClient;
        private ControllerConfig _currentControllerConfig = new ControllerConfig();
        private IXbox360Controller _emulatedController;
        private Task _checkProcessesTask;
        private bool _running = true;
        private ControllerConfig profile;

        public event EventHandler<bool> OnServiceStartStop;

        public ControllerService()
        {
            try
            {
                _neptuneController = new NeptuneController();
            } catch (Exception ex)
            {
                LoggingService.LogError($"Could not connect to Neptune controller.\r\n{ex}");
                FailedInit = true;
            }
            LoggingService.LogInformation($"Driver Version: {BuildVersionInfo.Version}");
            LoggingService.LogInformation($"Driver Build Time (UTC): {BuildVersionInfo.BuildTime}");
            ConfigLoader.TryMigrateConfiguration(Environment.SpecialFolder.MyDocuments, "SWICD", "app_config.conf");
            Configuration = ConfigLoader.GetConfiguration(Environment.SpecialFolder.MyDocuments, "SWICD", "app_config.json");
            LoggingService.LogInformation("Config Loaded.");
            LoggingService.LogInformation($"Executable specific profiles: {Configuration.PerProcessControllerConfig.Count}");
            LoggingService.LogInformation($"Mode: {Configuration.GenericSettings.OperationMode}");
            LoggingService.LogInformation($"Blacklisted processes: {Configuration.GenericSettings.BlacklistedProcesses.Count}");
            LoggingService.LogInformation($"Whitelisted processes: {Configuration.GenericSettings.WhitelistedProcesses.Count}");
            _neptuneController.OnControllerInputReceived = input => Task.Run(() => OnControllerInputReceived(input));
        }

        private void InitEmuController()
        {
            try
            {
                _viGEmClient = new ViGEmClient();
                _emulatedController = _viGEmClient.CreateXbox360Controller();
                _emulatedController.AutoSubmitReport = false;
                _emulatedController.FeedbackReceived += EmulatedController_FeedbackReceived;
            } catch(Exception ex)
            {
                LoggingService.LogError($"Could not connect to ViGEm Bus. Make sure its installed!\r\n{ex}");
                FailedInit = true;
            }
        }

        private void EnsureInitEmuController()
        {
            if (_viGEmClient == null && !FailedInit)
                InitEmuController();
        }

        private bool lastLeftHapticOn = false;
        private bool lastRightHapticOn = false;
        private void EmulatedController_FeedbackReceived(object sender, Xbox360FeedbackReceivedEventArgs e)
        {
            if (profile == null)
                return;

            byte amplitude = 0, period = 0;

            if(profile.ProfileSettings.HapticFeedbackEnabled)
            {
                amplitude = profile.ProfileSettings.HapticFeedbackAmplitude;
                period = profile.ProfileSettings.HapticFeedbackPeriod;
            }
            
            bool leftHaptic = e.LargeMotor > 0;
            bool rightHaptic = e.SmallMotor > 0;

            if (leftHaptic != lastLeftHapticOn)
                _ = _neptuneController.SetHaptic(1, (ushort)(leftHaptic ? amplitude : 0), (ushort)(leftHaptic ? period : 0), 0);


            if (rightHaptic != lastRightHapticOn)
                _ = _neptuneController.SetHaptic(0, (ushort)(rightHaptic ? amplitude : 0), (ushort)(rightHaptic ? period : 0), 0);

            lastLeftHapticOn = leftHaptic;
            lastRightHapticOn = rightHaptic;
        }

        public async Task SetHaptic(byte position, ushort amplitude, ushort period, ushort cunt)
        {
            await _neptuneController.SetHaptic(position, amplitude, period, cunt);
        }

        private async Task CheckProcessesLoop()
        {
            while (_running && !FailedInit)
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
                        DecisionExecutable = $"{process.ProcessName}.exe";
                        break;
                    }
                    if (Configuration.GenericSettings.OperationMode == OperationMode.Whitelist &&
                        Configuration.GenericSettings.WhitelistedProcesses.Contains($"{process.ProcessName}.exe"))
                    {

                        emulate = true;
                        DecisionExecutable = $"{process.ProcessName}.exe";
                        break;
                    }

                    if (Configuration.GenericSettings.OperationMode == OperationMode.Combined &&
                        Configuration.GenericSettings.BlacklistedProcesses.Contains($"{process.ProcessName}.exe"))
                    {
                        emulate = false;
                        DecisionExecutable = $"{process.ProcessName}.exe";
                        break;
                    }

                    if (Configuration.GenericSettings.OperationMode == OperationMode.Combined &&
                        Configuration.GenericSettings.WhitelistedProcesses.Contains($"{process.ProcessName}.exe"))
                    {
                        emulate = true;
                        DecisionExecutable = $"{process.ProcessName}.exe";
                    }
                }


                var _profile = Configuration.DefaultControllerConfig;

                foreach (var process in list.OrderBy(p => p.ProcessName))
                {
                    if (Configuration.PerProcessControllerConfig.ContainsKey($"{process.ProcessName}.exe"))
                    {
                        _profile = Configuration.PerProcessControllerConfig[$"{process.ProcessName}.exe"];
                        break;
                    }
                }

                profile = _profile;

                emulate = profile.ProfileSettings.GetInvertedEmulationEnabled(emulate);

                if (EmulationEnabled != emulate)
                {
                    EmulationEnabled = emulate;
                    LoggingService.LogDebug($"EmulationEnabled changed to: {EmulationEnabled}");
                }
                if (_currentControllerConfig != profile)
                {
                    _currentControllerConfig = profile;
                    var profileDisplay = profile.Executable ?? "Default Profile";
                    LoggingService.LogDebug($"Active profile changed to: {profileDisplay}");
                }
                if (LizardMouseEnabled != !_currentControllerConfig.ProfileSettings.ToggledDisableLizardMouse)
                {
                    LizardMouseEnabled = !_currentControllerConfig.ProfileSettings.ToggledDisableLizardMouse;
                    LoggingService.LogDebug($"LizardMouseEnabled changed to: {LizardMouseEnabled}");
                }
                if (LizardButtonsEnabled != !_currentControllerConfig.ProfileSettings.ToggledDisableLizardButtons)
                {
                    LizardButtonsEnabled = !_currentControllerConfig.ProfileSettings.ToggledDisableLizardButtons;
                    LoggingService.LogDebug($"LizardButtonsEnabled changed to: {LizardButtonsEnabled}");
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError($"Could not check for process changes: {ex}");
            }
        }


        private void OnControllerInputReceived(NeptuneControllerInputEventArgs e)
        {
            HandleInput(e.State);
        }

        public void Start()
        {
            EnsureInitEmuController();
            if (FailedInit)
            {
                OnServiceStartStop?.Invoke(this, false);
                return;
            }
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
            OnServiceStartStop?.Invoke(this, true);
            LoggingService.LogInformation("Driver started.");
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
        DateTime lastHapticUpdate = DateTime.UtcNow;
        private void HandleInput(NeptuneControllerInputState state)
        {
            if (FailedInit)
                return;

            _emulatedController.ResetReport();

            if (EmulationEnabled)
            {
                InputMapper.MapInput(_currentControllerConfig, state, _emulatedController);
                _keyboardMouseInputMapper.MapKeyboardInput(_currentControllerConfig, state);
            }

            if(!LizardButtonsEnabled)
            {
                _keyboardMouseInputMapper.MapMouseInput(_currentControllerConfig, state);
            }

            _buttonActionsProcessor.ProcessInput(Configuration.ButtonActions, _currentControllerConfig, state);

            _emulatedController.SubmitReport();

        }
    }
}
