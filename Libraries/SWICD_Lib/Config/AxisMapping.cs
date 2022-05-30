using System;

namespace SWICD_Lib.Config
{
    public class AxisMapping
    {
        public EmulatedAxisConfig LeftStickX { get; set; } = new EmulatedAxisConfig(EmulatedAxis.LeftStickX, HardwareButton.BtnLStickTouch);
        public EmulatedAxisConfig LeftStickY { get; set; } = new EmulatedAxisConfig(EmulatedAxis.LeftStickY, HardwareButton.BtnLStickTouch);
        public EmulatedAxisConfig RightStickX { get; set; } = new EmulatedAxisConfig(EmulatedAxis.RightStickX, HardwareButton.BtnRStickTouch);
        public EmulatedAxisConfig RightStickY { get; set; } = new EmulatedAxisConfig(EmulatedAxis.RightStickY, HardwareButton.BtnRStickTouch);
        public EmulatedAxisConfig LeftPadX { get; set; } = new EmulatedAxisConfig(EmulatedAxis.LeftStickX, HardwareButton.BtnLPadTouch);
        public EmulatedAxisConfig LeftPadY { get; set; } = new EmulatedAxisConfig(EmulatedAxis.LeftStickY, HardwareButton.BtnLPadTouch);
        public EmulatedAxisConfig RightPadX { get; set; } = new EmulatedAxisConfig(EmulatedAxis.RightStickX, HardwareButton.BtnRPadTouch);
        public EmulatedAxisConfig RightPadY { get; set; } = new EmulatedAxisConfig(EmulatedAxis.RightStickY, HardwareButton.BtnRPadTouch);
        public EmulatedAxisConfig RightPadPressure { get; set; } = new EmulatedAxisConfig(EmulatedAxis.None);
        public EmulatedAxisConfig LeftPadPressure { get; set; } = new EmulatedAxisConfig(EmulatedAxis.None);
        public EmulatedAxisConfig L2 { get; set; } = new EmulatedAxisConfig(EmulatedAxis.LT);
        public EmulatedAxisConfig R2 { get; set; } = new EmulatedAxisConfig(EmulatedAxis.RT);
        public EmulatedAxisConfig GyroAccelX { get; set; } = new EmulatedAxisConfig(EmulatedAxis.None);
        public EmulatedAxisConfig GyroAccelY { get; set; } = new EmulatedAxisConfig(EmulatedAxis.None);
        public EmulatedAxisConfig GyroAccelZ { get; set; } = new EmulatedAxisConfig(EmulatedAxis.None);
        public EmulatedAxisConfig GyroRoll { get; set; } = new EmulatedAxisConfig(EmulatedAxis.None);
        public EmulatedAxisConfig GyroPitch { get; set; } = new EmulatedAxisConfig(EmulatedAxis.None);
        public EmulatedAxisConfig GyroYaw { get; set; } = new EmulatedAxisConfig(EmulatedAxis.None);

        internal string ToString(string executable = null)
        {
            string config = $"[axes]\r\n";
            if (executable != null)
            {
                config = $"[axes,{executable}]\r\n";
            }
            config += $"LeftStickX={LeftStickX}\r\n";
            config += $"LeftStickY={LeftStickY}\r\n";
            config += $"RightStickX={RightStickX}\r\n";
            config += $"RightStickY={RightStickY}\r\n";
            config += $"LeftPadX={LeftPadX}\r\n";
            config += $"LeftPadY={LeftPadY}\r\n";
            config += $"RightPadX={RightPadX}\r\n";
            config += $"RightPadY={RightPadY}\r\n";
            config += $"RightPadPressure={RightPadPressure}\r\n";
            config += $"LeftPadPressure={LeftPadPressure}\r\n";
            config += $"L2={L2}\r\n";
            config += $"R2={R2}\r\n";
            config += $"GyroAccelX={GyroAccelX}\r\n";
            config += $"GyroAccelY={GyroAccelY}\r\n";
            config += $"GyroAccelZ={GyroAccelZ}\r\n";
            config += $"GyroRoll={GyroRoll}\r\n";
            config += $"GyroPitch={GyroPitch}\r\n";
            config += $"GyroYaw={GyroYaw}\r\n";
            return config;
        }}
}