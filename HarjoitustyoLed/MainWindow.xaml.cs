using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HarjoitustyoLed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage ledRed0 = new BitmapImage(new Uri(@"Pictures/Led_Red0.png", UriKind.Relative));
        BitmapImage ledRed1 = new BitmapImage(new Uri(@"Pictures/Led_Red1.png", UriKind.Relative));
        BitmapImage ledRedNC = new BitmapImage(new Uri(@"Pictures/Led_RedNC.png", UriKind.Relative));
        BitmapImage ledBlue0 = new BitmapImage(new Uri(@"Pictures/Led_Blue0.png", UriKind.Relative));
        BitmapImage ledBlue1 = new BitmapImage(new Uri(@"Pictures/Led_Blue1.png", UriKind.Relative));
        BitmapImage ledBlueNC = new BitmapImage(new Uri(@"Pictures/Led_BlueNC.png", UriKind.Relative));
        private LedControl ledControl;
        private Led redLed;
        private Led blueLed;

        public MainWindow()
        {
            InitializeComponent();
            redLed = new Led(26, "punainen");
            blueLed = new Led(19, "sininen");
            ledControl = new LedControl();
            InitializeLedControl();
            LoadSequences();
        }

        private async void InitializeLedControl()
        {
            if (await ledControl.getStatus(redLed) != -1)
            {
                await ledControl.setStatus(redLed, 0);
                PicRedLed.Source = ledRed0;
            }
            else
            {
                MessageBox.Show("Punaiseen lediin ei saatu yhteyttä!", "Virhe");
            }

            if (await ledControl.getStatus(blueLed) != -1)
            {
                await ledControl.setStatus(blueLed, 0);
                PicBlueLed.Source = ledBlue0;
            }
            else
            {
                MessageBox.Show("Siniseen lediin ei saatu yhteyttä!", "Virhe");
            }

            // Jos kannassa ei ole vielä yhtään sekvenssiä, lisätään yksi

            populateSequence();
            //deleteAll();

        }

        private void LoadSequences()
        {
            // Ladataan sekvenssit tietokannasta
            try
            {
                using (var loadSequences = new SequenceContext())
                {
                    // Sekvenssien latausta ei tehdä, jos huomataan, ettei niitä ole yhtään.
                    if (loadSequences.LedSequences.Count() > 0)
                    {
                        SequencesComboBox.ItemsSource = loadSequences.LedSequences.ToList();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void BlueLedButton_Click(object sender, RoutedEventArgs e)
        {
            await ledControl.swapStatus(blueLed);
            setBluePic();
        }

        private async void RedLedButton_Click(object sender, RoutedEventArgs e)
        {
            await ledControl.swapStatus(redLed);
            setRedPic();
        }

        private async void setBluePic()
        {
            if (await ledControl.getStatus(blueLed) == 1)
            {
                PicBlueLed.Source = ledBlue1;
            }
            else if (await ledControl.getStatus(blueLed) == 0)
            {
                PicBlueLed.Source = ledBlue0;
            }
            else
            {
                PicBlueLed.Source = ledBlueNC;
            }
        }

        private async void setRedPic()
        {
            if (await ledControl.getStatus(redLed) == 1)
            {
                PicRedLed.Source = ledRed1;
            }
            else if (await ledControl.getStatus(redLed) == 0)
            {
                PicRedLed.Source = ledRed0;
            }
            else
            {
                PicRedLed.Source = ledRedNC;
            }
        }

        private void populateSequence()
        {
            try
            {
                // Jos tietokannassa ei ole yhtään sekvenssiä, lisätään sinne yksi
                var populateSecuence = new SequenceContext();

                if (populateSecuence.LedSequences.Count() == 0)
                {
                    var sekvenssi = new LedSequence
                    {
                        Name = "Sekunnin jumppa"
                    };
                    populateSecuence.Add(sekvenssi);

                    var aika1 = new TimeRow
                    {
                        Time = 1000,
                        LedSequence = sekvenssi
                    };
                    populateSecuence.Add(aika1);

                    var ledit = new LedRow
                    {
                        PinId = redLed.pinId,
                        Status = 0,
                        TimeRow = aika1
                    };
                    populateSecuence.Add(ledit);
                    ledit = new LedRow
                    {
                        PinId = blueLed.pinId,
                        Status = 1,
                        TimeRow = aika1
                    };
                    populateSecuence.Add(ledit);

                    var aika2 = new TimeRow
                    {
                        Time = 1000,
                        LedSequence = sekvenssi
                    };
                    populateSecuence.Add(aika2);

                    var ledit2 = new LedRow
                    {
                        PinId = redLed.pinId,
                        Status = 1,
                        TimeRow = aika2
                    };
                    populateSecuence.Add(ledit2);
                    ledit2 = new LedRow
                    {
                        PinId = blueLed.pinId,
                        Status = 0,
                        TimeRow = aika2
                    };
                    populateSecuence.Add(ledit2);

                    populateSecuence.SaveChanges();
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Tietoja ei voitu lisätä tietokantaan", "Virhe");
            }
        }

        private void deleteAll()
        {
            using (var deleteSequences = new SequenceContext())
            {
                deleteSequences.LedSequences.RemoveRange(deleteSequences.LedSequences);
                deleteSequences.TimeRows.RemoveRange(deleteSequences.TimeRows);
                deleteSequences.LedRows.RemoveRange(deleteSequences.LedRows);
                deleteSequences.SaveChanges();
            }
        }

        private void SequencesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ladataan sekvenssit tietokannasta
            try
            {
                var selectedSequence = this.SequencesComboBox.SelectedItem as LedSequence;
                using (var loadSequences = new SequenceContext())
                {
                    //MessageBox.Show($"Valitsit sekvenssin {selectedSequence.Name}, jonka id on {selectedSequence.Id}");
                    SequenceNameTextBox.Text = selectedSequence.Name;
                    var secuenceId = selectedSequence.Id;

                    var ledSequenceList = loadSequences.LedSequences
                        .Include(TimeRow => TimeRow.TimeRows)
                        .Where(c => c.Id.Equals(secuenceId))
                        .ToList();

                    var ledSequenceArray = loadSequences.LedSequences
                        .Include(TimeRow => TimeRow.TimeRows)
                        .Where(c => c.Id.Equals(secuenceId))
                        .ToArray();

                    var timeId = ledSequenceList[0].Id;

                    var ledSequence = loadSequences.LedRows
                        .Include(LedRow => LedRow.TimeRow)
                        .Include(LedSequence => LedSequence.TimeRow)
                        .ToList();

                    var uusi2 = loadSequences.TimeRows
                        .Include(c => c.LedRows)
                        .ToArray();

                  
                        
                    

                    var uusi =
                        from timer in loadSequences.TimeRows
                        join leds in loadSequences.LedRows on timer.Id equals leds.TimeRow.Id
                        join sekvenssi in loadSequences.LedSequences on timer.LedSequence.Id equals sekvenssi.Id
                        where sekvenssi.Id.Equals(secuenceId)
                        select sekvenssi;

                    foreach (var item in uusi)
                    {
                        var testi = item.TimeRows;
                    }

                    var query = (
                        from timer in loadSequences.TimeRows
                        join leds in loadSequences.LedRows on timer.Id equals leds.TimeRow.Id
                        join sekvenssi in loadSequences.LedSequences on timer.LedSequence.Id equals sekvenssi.Id
                        where sekvenssi.Id.Equals(secuenceId)
                        select new
                        {
                            sequencedId = timer.LedSequence.Id,
                            sequenceName = timer.LedSequence.Name,
                            timeRowId = timer.Id,
                            time = timer.Time,
                            ledRowId = leds.Id,
                            ledPinId = leds.PinId,
                            ledStatus = leds.Status
                        }).ToList();

                    //ShowSequenceDetails(ledSequenceArray);
                    //ShowSequenceDetails(ledSequenceList);
                    //ShowSequenceDetails();

                    //SequenceDetailListBox.ItemsSource = query;

                    TimeRowDetails(uusi2);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }
        }

        private void TimeRowDetails(TimeRow[] leds)
        {
            List<string> detailsList = new List<string>();
            string detailsString = "";
            string ledstring = "";

            foreach (var time in leds)
            {
                detailsString = "";
                ledstring = "";
                detailsString = time.Time + " ms\t";

                foreach (var ledstatus in time.LedRows)
                {
                    var pin = ledstatus.PinId;
                    var status = ledstatus.Status;
                    ledstring += $" pinId: {pin}, status: {status}";
                }

                detailsString += ledstring;

                detailsList.Add(detailsString);
            }

            SequenceDetailListBox.ItemsSource = detailsList;
        }
    

 
        private void ShowSequenceDetails(HarjoitustyoLed.LedSequence[] leds)
        {
            List<string> detailsList = new List<string>();
            string detailsString = "";
            foreach (var time in leds[0].TimeRows)
            {
                detailsString = time.Time + " ms\t";

                string test = time.LedRows[0].Id.ToString();


                detailsList.Add(detailsString);
            }
            SequenceDetailListBox.ItemsSource = detailsList;
        }
        private void ShowSequenceDetails(List<HarjoitustyoLed.LedSequence> leds)
        {
            List<string> detailsList = new List<string>();
            string detailsString = "";
            string ledstring = "";

            foreach (var times in leds[0].TimeRows)
            {
                detailsString = "";
                ledstring = "";
                
                detailsString += times.Time + "ms";

                foreach (var ledstatus in times.LedRows)
                {
                    var pin = ledstatus.PinId;
                    var status = ledstatus.Status;
                    ledstring += $" pinId: {pin}, status: {status}";
                }

                //detailsString += ledstring;

                detailsList.Add(detailsString);
            }

            SequenceDetailListBox.ItemsSource = detailsList;
        }
    }
}
