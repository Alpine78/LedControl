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
using System.Text.RegularExpressions;

namespace HarjoitustyoLed
{
    /// <summary>
    /// Interaction logic for SequenceWindow.xaml
    /// </summary>
    public partial class SequenceWindow : Window
    {
        public SequenceWindow()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Tallennetaan uusi sekvenssi
            try
            {
                using (var saveSequence = new SequenceContext())
                {

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void AddSequenceRowButton_Click(object sender, RoutedEventArgs e)
        {
            // Lisätään uusi rivi listaan
            int time = 0;
            int pinId1 = 0;
            int status1 = 0;
            int pinId2 = 0;
            int status2 = 0;

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
