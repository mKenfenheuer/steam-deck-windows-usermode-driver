using SWICD.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SWICD.ViewModels
{
    internal class QuestionWindowViewModel
    {
        public string QuestionText { get; set; }

        public Window Window { get; set; }

        public CommandHandler OnYesClick => new CommandHandler((obj) => OnYesClicked());

        public CommandHandler OnNoClick => new CommandHandler((obj) => OnNoClicked());

        public bool Result { get; internal set; }

        private void OnYesClicked()
        {
            Result = true;
            Window?.Close();
        }
        private void OnNoClicked()
        {
            Result = false;
            Window?.Close();
        }
    }
}
