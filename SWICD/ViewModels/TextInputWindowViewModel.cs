using SWICD.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SWICD.ViewModels
{
    internal class TextInputWindowViewModel
    {
        public string QuestionText { get; set; }

        public Window Window { get; set; }

        public CommandHandler OnYesClick => new CommandHandler((obj) => OnYesClicked());

        public CommandHandler OnNoClick => new CommandHandler((obj) => OnNoClicked());

        public string Result { get; set; }

        private void OnYesClicked()
        {
            Window?.Close();
        }
        private void OnNoClicked()
        {
            Window?.Close();
        }
    }
}
