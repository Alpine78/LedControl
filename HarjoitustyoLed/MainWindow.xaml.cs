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
            asetaAlkutila();
        }

        private async void asetaAlkutila()
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

                ledit = new LedRow
                {
                    PinId = redLed.pinId,
                    Status = 1,
                    TimeRow = aika2
                };
                populateSecuence.Add(ledit);
                ledit = new LedRow
                {
                    PinId = blueLed.pinId,
                    Status = 0,
                    TimeRow = aika2
                };
                populateSecuence.Add(ledit);


                populateSecuence.SaveChanges();

            }
        }
    }
}
