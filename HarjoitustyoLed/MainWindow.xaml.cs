using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
using System.Timers;
using System.Threading;

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
        private bool playing;

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
            await setBluePic();
        }

        private async void RedLedButton_Click(object sender, RoutedEventArgs e)
        {
            await ledControl.swapStatus(redLed);
            await setRedPic();
        }

        private async Task setBluePic()
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

        private async Task setRedPic()
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
                    int sequenceId = selectedSequence.Id;

                    /*
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
                        */



                    var uusi2 = loadSequences.TimeRows
                        .Include(c => c.LedRows)
                        .ToArray();

                    //.Where(d => d.LedSequence.Id.Equals(sequenceId))


                    var query = (
                       from timerows in loadSequences.TimeRows
                       join ledrows in loadSequences.LedRows on timerows.Id equals ledrows.TimeRow.Id
                       where timerows.LedSequence.Id == sequenceId
                       select new { TimeRow = timerows, LedRow = ledrows }).ToList();

                    /*
                    var query = (                      
                       from sequencerows in loadSequences.LedSequences
                       join timerows in loadSequences.TimeRows on sequencerows.Id equals timerows.LedSequence.Id
                       join ledrows in loadSequences.LedRows on timerows.Id equals ledrows.TimeRow.Id
                       where sequencerows.Id == sequenceId
                       select new { LedSequence = sequencerows, TimeRow = timerows, LedRow = ledrows }).ToList();
                       */


                    /*
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
                    */

                    /*
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
                        */

                    var joinquery = (
                        from timer in loadSequences.TimeRows
                        join leds in loadSequences.LedRows on timer.Id equals leds.TimeRow.Id
                        join sekvenssi in loadSequences.LedSequences on timer.LedSequence.Id equals sekvenssi.Id
                        where sekvenssi.Id.Equals(sequenceId)
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

                    var joinqueryArray = (
                        from timer in loadSequences.TimeRows
                        join leds in loadSequences.LedRows on timer.Id equals leds.TimeRow.Id
                        join sekvenssi in loadSequences.LedSequences on timer.LedSequence.Id equals sekvenssi.Id
                        where sekvenssi.Id.Equals(sequenceId)
                        select new
                        {
                            sequencedId = timer.LedSequence.Id,
                            sequenceName = timer.LedSequence.Name,
                            timeRowId = timer.Id,
                            time = timer.Time,
                            ledRowId = leds.Id,
                            ledPinId = leds.PinId,
                            ledStatus = leds.Status
                        }).ToArray();

                    // TimeRowDetails(uusi2);
                    // Tämä on melkein ok, älä poista. Pitää saada vain yhden id:n tiedot 

                    //TimeRowDetailsFromQyeryArray(joinqueryArray);
                    //TimeRowDetailsFromQyery(joinquery);

                    // Tekisin tämän kasauksen omassa funktiossa, mutta en osaa välittää tätä tyyppiä ulos.
                    List<string> detailsList = new List<string>();
                    string detailsString = "";
                    foreach (var time in joinquery)
                    {
                        detailsString = "";
                        detailsString = "AikaId " + time.timeRowId + "\t";
                        detailsString += "Aika " + time.time + " ms\t";
                        detailsString += "pidId " + time.ledPinId + "\t";
                        detailsString += "status " + time.ledStatus + "\t";
                        detailsList.Add(detailsString);
                    }

                    SequenceDetailListBox.ItemsSource = detailsList;
                    SequenceDetailListBox.Items.Refresh();

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
            SequenceDetailListBox.Items.Refresh();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Play-nappia painettu, toistetaan sekvenssi
            var selectedSequence = this.SequencesComboBox.SelectedItem as LedSequence;

            if (selectedSequence != null)
            {
                PlayList uusiLista = new PlayList();
                try
                {
                    using (var loadSequences = new SequenceContext())
                    {
                        int time = 0;
                        int pinId1 = 0;
                        int status1 = 0;
                        int pinId2 = 0;
                        int status2 = 0;

                        SequenceNameTextBox.Text = selectedSequence.Name;
                        var sequenceId = selectedSequence.Id;

                        /*
                        var query = loadSequences.TimeRows
                            .Include(c => c.LedRows)
                            .ToArray();
                        */

                        var query = (
                            from timer in loadSequences.TimeRows
                            join leds in loadSequences.LedRows on timer.Id equals leds.TimeRow.Id
                            join sekvenssi in loadSequences.LedSequences on timer.LedSequence.Id equals sekvenssi.Id
                            where sekvenssi.Id.Equals(sequenceId)
                            select new
                            {
                                sequencedId = timer.LedSequence.Id,
                                sequenceName = timer.LedSequence.Name,
                                timeRowId = timer.Id,
                                time = timer.Time,
                                ledRowId = leds.Id,
                                ledPinId = leds.PinId,
                                ledStatus = leds.Status
                            }).ToArray();

                        PlayListRow playListRow = new PlayListRow();

                        int count = 0;

                        foreach (var sequence in query)
                        {
                            if (count % 2 == 0)
                            {
                                time = sequence.time;
                                pinId1 = sequence.ledPinId;
                                status1 = sequence.ledStatus;
                            }
                            else
                            {
                                pinId2 = sequence.ledPinId;
                                status2 = sequence.ledStatus;
                                PlayListRow listanrivi = new PlayListRow(time, pinId1, status1, pinId2, status2);
                                uusiLista.addRow(listanrivi);
                            }
                            count++;
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                PlaySequence(uusiLista);
            }
        }

        private async void PlaySequence(PlayList playList)
        {
            playing = true;
            playButton.Visibility = System.Windows.Visibility.Collapsed;
            stopButton.Visibility = System.Windows.Visibility.Visible;

            while (playing)
            {
                foreach (var row in playList.PlayListRows)
                {
                    await ledControl.setStatus(redLed, row.Status1);
                    if (row.Status1 == 0) PicRedLed.Source = ledRed0;
                    else PicRedLed.Source = ledRed1;
                    await ledControl.setStatus(blueLed, row.Status2);
                    if (row.Status2 == 1) PicBlueLed.Source = ledBlue0;
                    else PicBlueLed.Source = ledBlue1;
                    Thread.Sleep(row.Time);
                }

            }

            // Laitetaan kuvat oikein lopputilanteen mukaisesti
            await setBluePic();
            await setRedPic();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            playing = false;
            playButton.Visibility = Visibility.Visible;
            stopButton.Visibility = Visibility.Collapsed;
        }

        private void NewSeqcuenceButton_Click(object sender, RoutedEventArgs e)
        {
            SequenceWindow window = new SequenceWindow();
            var result = window.ShowDialog();
            SequencesComboBox.Items.Refresh();
            SequenceDetailListBox.Items.Refresh();
        }
    }
}
