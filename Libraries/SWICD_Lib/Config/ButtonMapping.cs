using System;

namespace SWICD_Lib.Config
{
    public class ButtonMapping
    {
        public EmulatedButton BtnX { get; set; } = EmulatedButton.BtnX;
        public EmulatedButton BtnY { get; set; } = EmulatedButton.BtnY;
        public EmulatedButton BtnA { get; set; } = EmulatedButton.BtnA;
        public EmulatedButton BtnB { get; set; } = EmulatedButton.BtnB;
        public EmulatedButton BtnMenu { get; set; } = EmulatedButton.BtnStart;
        public EmulatedButton BtnOptions { get; set; } = EmulatedButton.BtnBack;
        public EmulatedButton BtnSteam { get; set; } = EmulatedButton.BtnStart;
        public EmulatedButton BtnQuickAccess { get; set; } = EmulatedButton.None;
        public EmulatedButton BtnDpadUp { get; set; } = EmulatedButton.BtnDpadUp;
        public EmulatedButton BtnDpadLeft { get; set; } = EmulatedButton.BtnDpadUp;
        public EmulatedButton BtnDpadRight { get; set; } = EmulatedButton.BtnDpadRight;
        public EmulatedButton BtnDpadDown { get; set; } = EmulatedButton.BtnDpadDown;
        public EmulatedButton BtnL1 { get; set; } = EmulatedButton.BtnLB;
        public EmulatedButton BtnR1 { get; set; } = EmulatedButton.BtnRB;
        public EmulatedButton BtnL2 { get; set; } = EmulatedButton.None;
        public EmulatedButton BtnR2 { get; set; } = EmulatedButton.None;
        public EmulatedButton BtnL4 { get; set; } = EmulatedButton.BtnA;
        public EmulatedButton BtnR4 { get; set; } = EmulatedButton.BtnB;
        public EmulatedButton BtnL5 { get; set; } = EmulatedButton.BtnX;
        public EmulatedButton BtnR5 { get; set; } = EmulatedButton.BtnY;
        public EmulatedButton BtnRPadPress { get; set; } = EmulatedButton.BtnRS;
        public EmulatedButton BtnLPadPress { get; set; } = EmulatedButton.BtnLS;
        public EmulatedButton BtnRPadTouch { get; set; } = EmulatedButton.None;
        public EmulatedButton BtnLPadTouch { get; set; } = EmulatedButton.None;
        public EmulatedButton BtnRStickPress { get; set; } = EmulatedButton.BtnRS;
        public EmulatedButton BtnLStickPress { get; set; } = EmulatedButton.BtnLS;
        public EmulatedButton BtnRStickTouch { get; set; } = EmulatedButton.None;
        public EmulatedButton BtnLStickTouch { get; set; } = EmulatedButton.None;

        internal string ToString(string executable = null)
        {
            string config = $"[buttons]\r\n";
            if (executable != null)
            {
                config = $"[buttons,{executable}]\r\n";
            }
            config += $"BtnX={GetEmulatedButtonName(BtnX)}\r\n";
            config += $"BtnY={GetEmulatedButtonName(BtnY)}\r\n";
            config += $"BtnA={GetEmulatedButtonName(BtnA)}\r\n";
            config += $"BtnB={GetEmulatedButtonName(BtnB)}\r\n";
            config += $"BtnMenu={GetEmulatedButtonName(BtnMenu)}\r\n";
            config += $"BtnOptions={GetEmulatedButtonName(BtnOptions)}\r\n";
            config += $"BtnSteam={GetEmulatedButtonName(BtnSteam)}\r\n";
            config += $"BtnQuickAccess={GetEmulatedButtonName(BtnQuickAccess)}\r\n";
            config += $"BtnDpadUp={GetEmulatedButtonName(BtnDpadUp)}\r\n";
            config += $"BtnDpadLeft={GetEmulatedButtonName(BtnDpadLeft)}\r\n";
            config += $"BtnDpadRight={GetEmulatedButtonName(BtnDpadRight)}\r\n";
            config += $"BtnDpadDown={GetEmulatedButtonName(BtnDpadDown)}\r\n";
            config += $"BtnL1={GetEmulatedButtonName(BtnL1)}\r\n";
            config += $"BtnR1={GetEmulatedButtonName(BtnR1)}\r\n";
            config += $"BtnL2={GetEmulatedButtonName(BtnL2)}\r\n";
            config += $"BtnR2={GetEmulatedButtonName(BtnR2)}\r\n";
            config += $"BtnL4={GetEmulatedButtonName(BtnL4)}\r\n";
            config += $"BtnR4={GetEmulatedButtonName(BtnR4)}\r\n";
            config += $"BtnL5={GetEmulatedButtonName(BtnL5)}\r\n";
            config += $"BtnR5={GetEmulatedButtonName(BtnR5)}\r\n";
            config += $"BtnRPadPress={GetEmulatedButtonName(BtnRPadPress)}\r\n";
            config += $"BtnLPadPress={GetEmulatedButtonName(BtnLPadPress)}\r\n";
            config += $"BtnRPadTouch={GetEmulatedButtonName(BtnRPadTouch)}\r\n";
            config += $"BtnLPadTouch={GetEmulatedButtonName(BtnLPadTouch)}\r\n";
            config += $"BtnRStickPress={GetEmulatedButtonName(BtnRStickPress)}\r\n";
            config += $"BtnLStickPress={GetEmulatedButtonName(BtnLStickPress)}\r\n";
            config += $"BtnRStickTouch={GetEmulatedButtonName(BtnRStickTouch)}\r\n";
            config += $"BtnLStickTouch={GetEmulatedButtonName(BtnLStickTouch)}\r\n";
            return config;
        }
        internal string GetEmulatedButtonName(EmulatedButton value) => Enum.GetName(typeof(EmulatedButton), value);

    }
}