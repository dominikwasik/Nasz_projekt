using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LearnEnglish
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Nauka1 : Page
    {

        string vAng = "";
        public Nauka1()
        {
            
            this.InitializeComponent();

            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                var existing = conn.Query<tabela>(@"select * from Budynki where zaliczone=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
                txtPol.Text = existing.pol;
                vAng = existing.ang;
            }

        }

        //klasa odpowiadająca za czytanie słów
        private async void readText(string text)
        {

            var voices = SpeechSynthesizer.AllVoices;
            //nowy obiekt speechsynthesizer
            SpeechSynthesizer speech = new SpeechSynthesizer();
            //wybranie głosu angielskiego i sprawdzenie czy taki jest zainstalowany
            try
            {
                speech.Voice = voices.First(x => x.Gender == VoiceGender.Male && x.Language.Contains("en-US"));
                SpeechSynthesisStream stream = await speech.SynthesizeTextToStreamAsync(text);
                mediaSound.SetSource(stream, stream.ContentType);
            }
            catch (Exception)
            {
                await new Windows.UI.Popups.MessageDialog("Aby móc poprawnie odsłuchiwać wyrazy, musisz zainstalować język angielski na swoim urządzeniu. Wybierz opcje POMOC z menu po lewej aby dowiedzieć się jak to zrobić.", "Brak komponentu").ShowAsync();
            }


        }
        private class tabela
        {
            public string pol { get; set; }
            public string ang { get; set; }
            public int zaliczone { get; set; }
            public int test { get; set; }
        }


            private void btnShowPane_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
            txtLogo.Visibility = Visibility.Visible;
            if (MySplitView.IsPaneOpen == true)
            {
                txtLogo.Visibility = Visibility.Visible;

            }
            else
            {
                txtLogo.Visibility = Visibility.Collapsed;
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Nauka));
        }

        //jeśli znam, wyswietlenie okna do wpisania słowa
        private void btnknow_Click(object sender, RoutedEventArgs e)
        {
            inpEng.Visibility = Visibility.Visible;
           // btndknow.IsEnabled = false;

            
        }
        private void btndknow_Click(object sender, RoutedEventArgs e)
        {
            txtEng.Text = vAng;
            txtEng.Visibility = Visibility.Visible;
            inpEng.Visibility = Visibility.Collapsed;
            btnknow.IsEnabled = false;
            btndknow.IsEnabled = false;
            btnNext.Visibility = Visibility.Visible;


        }


        private async void inpEng_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.Equals(Windows.System.VirtualKey.Enter))
            {

            
            if (inpEng.Text == vAng)
            {
                    txtEng.Text = vAng;
                    inpEng.Visibility = Visibility.Collapsed;
                    txtEng.Visibility = Visibility.Visible;
                    btnNext.Visibility = Visibility.Visible;
                    btnknow.IsEnabled = false;
                    btndknow.IsEnabled = false;
                }
            else
            {
                await new Windows.UI.Popups.MessageDialog("Niestety, to zła odpowiedź", "Zła odpowiedź").ShowAsync();
            }
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            txtEng.Visibility = Visibility.Collapsed;
            btnknow.IsEnabled = true;
            btndknow.IsEnabled = true;
            btnNext.Visibility = Visibility.Collapsed;
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                var existing = conn.Query<tabela>(@"select * from Budynki where zaliczone=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
                txtPol.Text = existing.pol;
                vAng = existing.ang;


            }
        }

        private async void btnSpeak_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                readText(vAng);
            }
            catch (Exception)
            {
                await new Windows.UI.Popups.MessageDialog("Aby móc poprawnie odsłuchiwać wyrazy, musisz zainstalować język angielski na swoim urządzeniu. Wybierz opcje POMOC z menu po lewej aby dowiedzieć się jak to zrobić.", "Brak komponentu").ShowAsync();
            }
            
        }
    }
}
