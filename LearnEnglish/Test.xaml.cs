using System;
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
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LearnEnglish
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Test : Page
    {
        static Random _random = new Random();
        string kat = "";
        string wynik = "";
        string[] odpowiedziAng = new string[4];
        string[] odpowiedziPol = new string[4];

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

        public Test()
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
            
            


            txtCategory.Text = Windows.Storage.ApplicationData.Current.LocalSettings.Values["Kategoria"].ToString();

            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                //licznik pokazujący ile słów pozostało do zaliczenia z dalej kategorii
                var count = conn.ExecuteScalar<int>("SELECT Count(*) FROM '" + kat + "' where test=0");
                txtCount.Text = count.ToString();

                var existing = conn.Query<tabela>(@"select * from '" + kat + "' where test=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
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

        //licznik do testu
        private float x = 0;

        //licznik punktów
        private float score = 0;
        private async void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (rbtnStackPanel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true))
            {

                if (ans1.IsChecked == true)
                {
                    if (wynik == ans1.Content.ToString())
                    {
                        using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
                        {
                            conn.Execute(@"UPDATE '" + kat + "' SET test = 1  WHERE ang='" + wynik + "'");
                        }
                    }
                }
                    if (ans2.IsChecked == true)
                    {
                        if (wynik == ans2.Content.ToString())
                        {
                            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
                            {
                                conn.Execute(@"UPDATE '" + kat + "' SET test = 1  WHERE ang='" + wynik + "'");
                            }
                        }
                    }
                    if (ans3.IsChecked == true)
                    {
                        if (wynik == ans3.Content.ToString())
                        {
                            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
                            {
                                conn.Execute(@"UPDATE '" + kat + "' SET test = 1  WHERE ang='" + wynik + "'");
                            }
                        }
                    }
                    if (ans4.IsChecked == true)
                    {
                        if (wynik == ans4.Content.ToString())
                        {
                            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
                            {
                                conn.Execute(@"UPDATE '" + kat + "' SET test = 1  WHERE ang='" + wynik + "'");
                            }
                        }
                    }
                }
               // x += 1;
               // txtCount.Text = x.ToString();
                //if (rtnRight.IsChecked == true)
                //{
                //    score += 1;
               // }

               // if (score == 3)
             //   {
             //
              //      float procent = (score / x) * 100;
            //
                   // await new Windows.UI.Popups.MessageDialog("Na " + x + " pytań, udzeliłeś " + score + " poprawnych odpowiedzi, w tym teście uzyskałeś " + procent + " procent", "Wynik").ShowAsync();
               // }
            
            else
            {
                await new Windows.UI.Popups.MessageDialog("Musisz wybrać przynajmniej jedną odpowiedź.", "Błąd").ShowAsync();
            }

            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                //licznik pokazujący ile słów pozostało do zaliczenia z dalej kategorii
                var count = conn.ExecuteScalar<int>("SELECT Count(*) FROM '" + kat + "' where test=0");
                txtCount.Text = count.ToString();

                var existing = conn.Query<tabela>(@"select * from '" + kat + "' where test=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
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

        }
    }
}
