﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite.Net;
using Windows.UI.Popups;
using Windows.Media.SpeechSynthesis;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LearnEnglish
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Test2_1 : Page
    {
        static Random _random = new Random();
        string kat = "";
        string koniec = "";
        int ilosc = 0;
        string rodzaj = "";
        string wynik = "";
        string[] odpowiedziAng = new string[4];
        string[] odpowiedziPol = new string[4];
        int good_answer = 0;
        int bad_answer = 0;
        tabela zmienna;


        //klasa odpowiadająca za czytanie słów
        private async void readText(string text)
        {

            var voices = SpeechSynthesizer.AllVoices;
            //nowy obiekt speechsynthesizer
            SpeechSynthesizer speech = new SpeechSynthesizer();
            //wybranie głosu angielskiego i sprawdzenie czy taki jest zainstalowany
            try
            {
                speech.Voice = voices.First(x => x.Gender == VoiceGender.Male && x.Language.Contains("pl-PL"));
                SpeechSynthesisStream stream = await speech.SynthesizeTextToStreamAsync(text);
                mediaSound.SetSource(stream, stream.ContentType);
            }
            catch (Exception)
            {
                await new Windows.UI.Popups.MessageDialog("Aby móc poprawnie odsłuchiwać wyrazy, musisz zainstalować język angielski na swoim urządzeniu. Wybierz opcje POMOC z menu po lewej aby dowiedzieć się jak to zrobić.", "Brak komponentu").ShowAsync();
            }

        }

        //metoda sprawdzająca czy słówka w kategorii są już w 100% opanowane
        private async void sprawdzenie()
        {
            //sprawdzenie jeśli ktoś wybrał że chce zaliczać z 0 odpowiedzi
            if (ilosc == 0)
            {
                readText("Ojojoj, nieładnie tak oszukiwać. Wierzę że potrafisz rozwiązać test składający się z więcej niż ZERO odpowiedzi.");
                await new Windows.UI.Popups.MessageDialog("Niestety, ale ten błąd też przewidzieliśmy.", "Błąd").ShowAsync();
                Frame.GoBack();
            }


            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                //licznik pokazujący ile słów pozostało do zaliczenia z dalej kategorii
                var count = conn.ExecuteScalar<int>("SELECT Count(*) FROM '" + kat + "' where test=0");
                txtCount.Text = count.ToString();

                var existing = conn.Query<tabela>(@"select * from '" + kat + "' where test=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
                //zmienna gdzie przechowywana jest prawidłowa odpowiedz
                try
                {
                    wynik = existing.ang;
                }
                catch (Exception ex)
                {
                    koniec = "koniec";
                }
            }

        }

        //metoda wyswietlająca napis jeśli wszystkie słówka z danej kategorii będą zaliczone
        private async void wysw()
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
                "Wygląda na to że znasz już wszystkie słowa z tej kategorii, kliknij powrót aby wrócić i wybrać inną kategorie", "Wybierz inną kategorie");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Powrót", new UICommandInvokedHandler(CommandHandler)));
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Zakończ", new UICommandInvokedHandler(CommandHandler)));

            await dialog.ShowAsync();

        }
        private async void wysw1()
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
                "Żadne słówka z tej kategorii nie są oznaczone jako znane, wybierz opcje 'Wszystkie słówka' z poprzedniego menu.", "Brak słówek do wyświetlenia");

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Powrót", new UICommandInvokedHandler(CommandHandler)));
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Zakończ", new UICommandInvokedHandler(CommandHandler)));

            await dialog.ShowAsync();

        }

        //obsługa przycisków z popup
        private void CommandHandler(IUICommand command)
        {
            var commandlabel = command.Label;
            switch (commandlabel)
            {
                case "Powrót":
                    Frame.GoBack();
                    break;
                case "Zakończ":
                    Application.Current.Exit();
                    break;
                default:
                    break;
            }
        }

        static void Shuffle<T>(T[] array)
        {
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(_random.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

        public Test2_1()
        {
            this.InitializeComponent();

            //pobranie zmiennych z okna Test2 które są zachowane w pamięci
            kat = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Kategorie"].ToString();
            ilosc = Convert.ToInt32(Windows.Storage.ApplicationData.Current.LocalSettings.Values["Ilosc"]);
            rodzaj = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Rodzaj"].ToString();

            //wywolanie metody sprawdzenia czy słówka są opanowane
            sprawdzenie();
            if (koniec == "koniec")
            {
                goto koniec;
            }

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




            txtCategory.Text = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Kategorie"].ToString();


            //wyswietlenie aktualnego progresu
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                //licznik pokazujący ile słów pozostało do zaliczenia z danej kategorii
                var pozostale = conn.ExecuteScalar<int>("SELECT Count(*) FROM '" + kat + "' where test=0");
                var count = conn.ExecuteScalar<int>("SELECT Count(*) FROM '" + kat + "'");
                var count1 = conn.ExecuteScalar<int>("SELECT Count(*) FROM '" + kat + "' where test=1");
                txtCount.Text = pozostale.ToString();
                //progressbar pokazujący postęp
                prgProgress.Maximum = ilosc;
               // prgBad.Maximum = ilosc;
                prgGood.Maximum = ilosc;
            }

            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                //licznik pokazujący ile słów pozostało do zaliczenia z dalej kategorii
                var count = conn.ExecuteScalar<int>("SELECT Count(*) FROM '" + kat + "' where test=0");
                txtCount.Text = count.ToString();
                
                if (rodzaj=="Tylko znane")
                {
                    //jeśli wybrano tylko znane słówka, to będzie losowało z zaliczone=1;
                    zmienna = conn.Query<tabela>(@"select * from '" + kat + "' where zaliczone=1 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
                    //warunek jeśli wykryto że nie ma żadnych słówek znanych, wtedy przehodzi do etykiety koniec
                    if (zmienna==null)
                    {
                        koniec = "koniec";
                        goto koniec;
                    }
                }
                //jeśli wybrano wszystkie słówka to będzie losowało z tych które w teście nie zostały jeszcze zaliczone
                else
                {
                    zmienna = conn.Query<tabela>(@"select * from '" + kat + "' where test=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
                }
                var existing = zmienna;
                //zmienna gdzie przechowywana jest prawidłowa odpowiedz
                wynik = existing.ang;
                //Wrzuca odp z wynikiem do tablicy
                odpowiedziAng[0] = existing.ang;
                // Ładuje słowo polskie to textboxa w panelu testu
                txtPol.Text = existing.pol;

                //petla gdzie laduje pozostale randomowe odpowiedzi do tablicy odpowiedzi
                for (int i = 1; i < 4; i++)
                {
                    existing = conn.Query<tabela>(@"select * from '" + kat + "'where test=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
                    odpowiedziAng[i] = existing.ang;

                }

            }
            //wymieszanie odpowiedzi
            Shuffle(odpowiedziAng);
            // wstawienie odpowiedzi do radiobuttonów
            ans1.Content = odpowiedziAng[0];
            ans2.Content = odpowiedziAng[1];
            ans3.Content = odpowiedziAng[2];
            ans4.Content = odpowiedziAng[3];

            //ta etykieta stanie się tylko wtedy gdy nie będzie słówek do wyświetlenia
        koniec: if (koniec == "koniec")
            {
                if (zmienna==null)
                {
                    wysw1();
                }
                else
                {
                    wysw();
                }
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
            Frame.Navigate(typeof(MainPage));
        }

        private void btnStartLearn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Nauka));
        }


        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {

            prgProgress.Value += 1;
            if (prgProgress.Value == prgProgress.Maximum)
            {
                int score = Convert.ToInt32(prgProgress.Maximum);
                float your_score = good_answer;
                float procent = (your_score / score) * 100;
                await new Windows.UI.Popups.MessageDialog("Ukończyłeś test. Na "+score+" pytań, udzieliłeś "+your_score+" poprawnych odpowiedzi.", "Wynik testu = "+procent+"%").ShowAsync();
            }

            else
            {

            if (rbtnStackPanel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true))
            {
                    //zwiekszenie wartosci progressbar po każdym pytaniu


                if (ans1.IsChecked == true)
                {
                    if (wynik == ans1.Content.ToString())
                    {
                        using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
                        {
                            conn.Execute(@"UPDATE '" + kat + "' SET test = 1  WHERE ang='" + wynik + "'");
                        }
                        good_answer += 1;
                    }
                    else bad_answer += 1;
                }
                if (ans2.IsChecked == true)
                {
                    if (wynik == ans2.Content.ToString())
                    {
                        using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
                        {
                            conn.Execute(@"UPDATE '" + kat + "' SET test = 1  WHERE ang='" + wynik + "'");
                        }
                        good_answer += 1;
                    }
                    else bad_answer += 1;
                }
                if (ans3.IsChecked == true)
                {
                    if (wynik == ans3.Content.ToString())
                    {
                        using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
                        {
                            conn.Execute(@"UPDATE '" + kat + "' SET test = 1  WHERE ang='" + wynik + "'");
                        }
                        good_answer += 1;
                    }
                    else bad_answer += 1;
                }
                if (ans4.IsChecked == true)
                {
                        if (wynik == ans4.Content.ToString())
                        {
                            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
                            {
                                conn.Execute(@"UPDATE '" + kat + "' SET test = 1  WHERE ang='" + wynik + "'");
                            }
                            good_answer += 1;
                        }
                        else bad_answer += 1;
                }
            }

            else
            {
                    await new Windows.UI.Popups.MessageDialog("Musisz wybrać przynajmniej jedną odpowiedź.", "Błąd").ShowAsync();
                    goto brak_wyboru;
            }


                prgGood.Value = good_answer;

                //kolejne wybranie słowa po wcisnięciu przycisku next
                using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                //licznik pokazujący ile słów pozostało do zaliczenia z danej kategorii
                var pozostale = conn.ExecuteScalar<int>("SELECT Count(*) FROM '" + kat + "' where test=0");
                var count = conn.ExecuteScalar<int>("SELECT Count(*) FROM '" + kat + "'");
                var count1 = conn.ExecuteScalar<int>("SELECT Count(*) FROM '" + kat + "' where test=1");
                txtCount.Text = pozostale.ToString();

                   // tabela zmienna;
                    if (rodzaj == "Tylko znane")
                    {
                        zmienna = conn.Query<tabela>(@"select * from '" + kat + "' where zaliczone=1 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
                    }
                    else
                    {
                        zmienna = conn.Query<tabela>(@"select * from '" + kat + "' where test=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
                    }
                    var existing = zmienna;
                    //zmienna gdzie przechowywana jest prawidłowa odpowiedz
                    wynik = existing.ang;
                    //Wrzuca odp z wynikiem do tablicy
                    odpowiedziAng[0] = existing.ang;
                    // Ładuje słowo polskie to textboxa w panelu testu
                    txtPol.Text = existing.pol;
                    // prgProgress.Value = progress;

                //petla gdzie laduje pozostale randomowe odpowiedzi do tablicy odpowiedzi
                for (int i = 1; i < 4; i++)
                {
                    existing = conn.Query<tabela>(@"select * from '" + kat + "'where test=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
                    odpowiedziAng[i] = existing.ang;
                }
            }
            //wymieszanie odpowiedzi
            Shuffle(odpowiedziAng);
            // wstawienie odpowiedzi do radiobuttonów
            ans1.Content = odpowiedziAng[0];
            ans2.Content = odpowiedziAng[1];
            ans3.Content = odpowiedziAng[2];
            ans4.Content = odpowiedziAng[3];
            brak_wyboru:;

        }
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Info));
        }
    }
}
