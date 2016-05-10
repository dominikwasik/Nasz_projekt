using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
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
        string kat = "";
        string vAng = "";
        public Nauka1()
        {
            
            this.InitializeComponent();
            kat = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Kategoria"].ToString();

            //konwersja stringa z polskimi znakami na bez polskich znaków
            StringBuilder sb = new StringBuilder(kat);

            sb.Replace('ą', 'a')

              .Replace('ć', 'c')

              .Replace('ę', 'e')

              .Replace('ł', 'l')

              .Replace('ń', 'n')

              .Replace('ó', 'o')

              .Replace('ś', 's')

              .Replace('ż', 'z')

              .Replace('ź', 'z')

              .Replace('Ą', 'A')

              .Replace('Ć', 'C')

              .Replace('Ę', 'E')

              .Replace('Ł', 'L')

              .Replace('Ń', 'N')

              .Replace('Ó', 'O')

              .Replace('Ś', 'S')

              .Replace('Ż', 'Z')

              .Replace('Ź', 'Z');



            kat = sb.ToString();

            
            //załadowanie z ustawień nazwy wybranej kategorii
            txtCategory.Text = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Kategoria"].ToString();

            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                var existing = conn.Query<tabela>(@"select * from '"+kat+"' where zaliczone=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
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

        //klasa odpowiadająca za szukanie zdań
        private async void searchText(string searchWord)
        {
            //Wyświetlanie przykładowych zdań:
            string urlAddress = "http://sentence.yourdictionary.com/" + searchWord;
            HttpClient client = new HttpClient();
            // string searchHere = await client.GetStringAsync(urlAddress);
            string searchHere = "";

            try
            {
                searchHere = await client.GetStringAsync(urlAddress);
            }
            catch (Exception)
            {
                txtExample.Text = "Brak dostepu do internetu lub nie znaleziono przykładu";
            }


            string searchForThis = vAng; //przechwycenie tekstu z textboxa którego ma szukać

            //wzór wyszukania tekstu przy pomocy biblioteki regex
            var pattern = String.Format("[^\\.]*\\b{0}\\b[^\\.]*", searchForThis);

            //użycie drugi raz wzoru do ostatecznego wyczyszczenia znaczników html.
            var pattern1 = String.Format("[^\\>]*\\b{0}\\b[^\\.]*", searchForThis);

            //jeśli znalazło dopasowanie
            if (Regex.IsMatch(searchHere, pattern))
            {
                //pętla wyświetlająca wszystkie dopasowania
                /*
                foreach (var matchedItem in Regex.Matches(searchWithinThis, pattern))
                {

                } */
                //wyświetlanie odpowiedniego dopasowania

                string result = Regex.Matches(searchHere, pattern)[18].ToString();
                //wycięcie znaczników html
                string noHTML = Regex.Replace(result, @"<[^>]+>|&nbsp;", "").Trim();
                string noHTMLNormalised = Regex.Replace(noHTML, @"\s{2,}", " ");

                //ostateczne czyszczenie znaczników html, jesli jeszcze zawierają tagi
                if (noHTMLNormalised.Contains(">"))
                {
                    noHTMLNormalised = Regex.Match(noHTMLNormalised, pattern1).ToString();

                    txtExample.Text = noHTMLNormalised;
                }
                else txtExample.Text = noHTMLNormalised;


                //zrobić kolejne przejście w którym będzie zaczynało tekst od znaku >
            }
            else txtExample.Text = "Nie znaleziono przykładu";

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
            inpEng.Text = "";
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

            //Wyświetlanie przykładowych zdań:
            searchText(vAng);



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

                    using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
                    {
                        conn.Execute(@"UPDATE '" + kat + "' SET zaliczone = 1  WHERE ang='" + vAng + "'");
                    }

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
            txtExample.Text = "Tutaj będzie przykładowe zdanie.";
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                var existing = conn.Query<tabela>(@"select * from '" + kat + "' where zaliczone=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
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
