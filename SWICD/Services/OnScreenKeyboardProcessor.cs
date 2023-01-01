using neptune_hidapi.net;
using System;
using SWICD.Config;
using SWICD.HVDK;
using SWICD.OSK;
using Osklib;

namespace SWICD.Services
{
    internal class OnScreenKeyboardProcessor
    {
        private bool _isOnScreenKeyboardEnabled = false;
        private bool _isOskOpend = false;

        public const byte OnscreenkeyboardOpacity = 127;
        public const int OverlabPercentage = 55;

        private NeptuneControllerInputState _lastState = null;
        private OnScreenKeyboardWatcher _onScreenKeyboardWatcher = null;
        private Overlay _overlay = null;

        public OnScreenKeyboardProcessor()
        {
            TouchInjector.InitializeTouchInjection();
        }

        public void SetOnScreenState(bool enable)
        {
            if(enable)
            {
                if(_isOnScreenKeyboardEnabled == false)
                    EnableWatching();
            }
            else
            {
                if(_isOnScreenKeyboardEnabled)
                    DisableWatching();
            }
        }

        private void KeyboardOpened(object sender, EventArgs e)
        {
            if(_isOnScreenKeyboardEnabled)
            {
                //lock lizard mouse here if you want when osk is opened
                _isOskOpend = true;
                _overlay.Show();
            }
        }

        private void KeyboardClosed(object sender, EventArgs e)
        {
            if (_isOnScreenKeyboardEnabled)
            {
                //unlock lizard mouse here if you want
                _isOskOpend = false;
                _overlay.Hide();
            }
        }

        private void ParametersChanged(object sender, EventArgs e)
        {
            if (_isOnScreenKeyboardEnabled && _onScreenKeyboardWatcher.Location.Right - _onScreenKeyboardWatcher.Location.Left > 0)
                _overlay.SetRect(_onScreenKeyboardWatcher.Location.Left, _onScreenKeyboardWatcher.Location.Top, _onScreenKeyboardWatcher.Location.Right - _onScreenKeyboardWatcher.Location.Left, _onScreenKeyboardWatcher.Location.Bottom - _onScreenKeyboardWatcher.Location.Top);
        }

        private void EnableWatching()
        {
            //Initialize

            OnScreenKeyboard.Show();

            if (_onScreenKeyboardWatcher != null)
                _onScreenKeyboardWatcher.Dispose();
            if (_overlay != null)
                _overlay.Dispose();

            _onScreenKeyboardWatcher = new OnScreenKeyboardWatcher();
            _onScreenKeyboardWatcher.KeyboardOpened += KeyboardOpened;
            _onScreenKeyboardWatcher.KeyboardClosed += KeyboardClosed;
            _onScreenKeyboardWatcher.ParametersChanged += ParametersChanged;

            _overlay = new Overlay();
            _overlay.OverlabPercentage = OverlabPercentage;
            _overlay.OskTransparent = OnscreenkeyboardOpacity;
            bool result = _overlay.Init();
            _overlay.SetRect(_onScreenKeyboardWatcher.Location.Left, _onScreenKeyboardWatcher.Location.Top, _onScreenKeyboardWatcher.Location.Right - _onScreenKeyboardWatcher.Location.Left, _onScreenKeyboardWatcher.Location.Bottom - _onScreenKeyboardWatcher.Location.Top);
            
            if (result == false)
            {
                LoggingService.LogError("OnScreenKeyboard Init Failed");
                return;
            }
            _isOnScreenKeyboardEnabled = true;

            if(OnScreenKeyboard.IsOpened())
                OnScreenKeyboard.Close();
        }
        private void DisableWatching()
        {
            if (_onScreenKeyboardWatcher != null)
                _onScreenKeyboardWatcher.Dispose();
            if (_overlay != null)
                _overlay.Dispose();

            _onScreenKeyboardWatcher = null;
            _overlay = null;

            _isOnScreenKeyboardEnabled = false;
        }

        public void ToggleOnScreenKeyboard()
        {
            if (OnScreenKeyboard.IsOpened())
            {
                OnScreenKeyboard.Close();
            }
            else
            {
                OnScreenKeyboard.Show();
            }
        }

        public void Log(object s, LogArgs e)
        {
            LoggingService.LogInformation(e.Msg);
        }

        public void MapTouchPadInput(ControllerConfig config, NeptuneControllerInputState input)
        {
            if (_isOnScreenKeyboardEnabled == false || config.ProfileSettings.OnScreenKeyboardEnabled == false)
                return;

            if (_isOskOpend)
            {
                _overlay.SetLeftPadPos(input.AxesState[NeptuneControllerAxis.LeftPadX], input.AxesState[NeptuneControllerAxis.LeftPadY]);
                _overlay.SetRightPadPos(input.AxesState[NeptuneControllerAxis.RightPadX], input.AxesState[NeptuneControllerAxis.RightPadY]);
                _overlay.ProcessTouch(input.ButtonState[NeptuneControllerButton.BtnLPadPress], input.ButtonState[NeptuneControllerButton.BtnRPadPress]);
            }
            else
            {
                _overlay.EventTouch.Clear();
            }

            _lastState = input;
        }
    }
}

