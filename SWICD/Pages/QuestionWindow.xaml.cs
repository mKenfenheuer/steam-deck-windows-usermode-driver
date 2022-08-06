using SWICD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SWICD.Pages
{
    /// <summary>
    /// Interaktionslogik für QuestionWindow.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {
        public QuestionWindow(string questionText, string[] answers)
        {
            InitializeComponent();
            DataContext = new QuestionWindowViewModel()
            {
                QuestionText = questionText,
                AnswerTexts = answers
            };
        }        
    }
}
