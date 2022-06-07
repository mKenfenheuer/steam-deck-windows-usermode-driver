using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWICD.Model
{
    internal class LogEntryModel
    {
        public LogLevel LogLevel { get; internal set; }
        public string Message { get; internal set; }
        public DateTime Time { get; internal set; }
        public System.Windows.Media.SolidColorBrush LogTextColorBrush => new System.Windows.Media.SolidColorBrush(FromDrawingColor(LogTextColor));
        public Color LogTextColor
        {
            get
            {
                switch (LogLevel)
                {
                    case LogLevel.Error:
                        return Color.Red;
                    case LogLevel.Warning:
                        return Color.Yellow;
                    case LogLevel.Debug:
                        return Color.Green;
                    case LogLevel.Information:
                        return Color.White;
                    case LogLevel.Critical:
                        return Color.DarkRed;
                    default:
                        return Color.White;
                }
            }
        }
        public string LogLevelText => $"[{LogLevel}]";
        public string TimeText => $"[{Time}]";

        private System.Windows.Media.Color FromDrawingColor(Color color) => System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
    }
}
