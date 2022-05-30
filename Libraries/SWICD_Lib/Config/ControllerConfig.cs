namespace SWICD_Lib.Config
{
    public class ControllerConfig
    {
        public ButtonMapping ButtonMapping { get; set; } = new ButtonMapping();
        public AxisMapping AxisMapping { get; set; } = new AxisMapping();

        public string ToString(string executable = null)
        {
            return ButtonMapping.ToString(executable) + "\r\n" + AxisMapping.ToString(executable);
        }
    }
}