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
        private PlayList newList;
        public SequenceWindow()
        {
            InitializeComponent();
            newList = new PlayList();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Tallennetaan uusi sekvenssi
            if (SequenceEditListBox.Items.Count == 0 || seqcuenceNameTextBox.Text == "")
            {
                MessageBox.Show("Laita sekvenssiin vähintään yksi rivi ja anna sekvenssille nimi", "Virhe");
            }
            else
            {
                try
                {
                    using (var saveSequence = new SequenceContext())
                    {
                        // Luodaan uusi ledisekvenssi
                        var sekvenssi = new LedSequence
                        {
                            Name = seqcuenceNameTextBox.Text
                        };
                        saveSequence.Add(sekvenssi);

                        // Käydään listan rivit läpi
                        foreach (var playRow in newList.PlayListRows)
                        {
                            var aika1 = new TimeRow
                            {
                                Time = playRow.Time,
                                LedSequence = sekvenssi
                            };
                            saveSequence.Add(aika1);

                            var ledit = new LedRow
                            {
                                PinId = playRow.PinId1,
                                Status = playRow.Status1,
                                TimeRow = aika1
                            };
                            saveSequence.Add(ledit);
                            ledit = new LedRow
                            {
                                PinId = playRow.PinId2,
                                Status = playRow.Status2,
                                TimeRow = aika1
                            };
                            saveSequence.Add(ledit);
                        }

                        // Tallennetaan uudet tiedot tietokantaan
                        saveSequence.SaveChanges();
                        MessageBox.Show("Tiedot tallennettu tietokantaan.", "Onnistui");
                        this.Close();
                    }
                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        private void AddSequenceRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (timeTextBox.Text == "")
            {
                MessageBox.Show("Lisää riville kesto (ms)", "Virhe");
            }
            else
            {
                // Lisätään uusi rivi listaan
                PlayListRow newRow = new PlayListRow();
                int status1 = 0;
                int status2 = 0;

                // Luetaan radiobuttonit
                bool red0 = (bool)redRadio0.IsChecked;
                status1 = red0 ? 0 : 1;
                bool blue0 = (bool)blueRadio0.IsChecked;
                status2 = blue0 ? 0 : 1;

                newRow.Time = int.Parse(timeTextBox.Text);
                newRow.PinId1 = 26;
                newRow.Status1 = status1;
                newRow.PinId2 = 19;
                newRow.Status2 = status2;
                newList.addRow(newRow);
                SequenceEditListBox.ItemsSource = newList.PlayListRows;
                SequenceEditListBox.Items.Refresh();
            }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
