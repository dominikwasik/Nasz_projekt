using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public Nauka1()
        {
            this.InitializeComponent();
            inpEng.Visibility = Visibility.Visible;
            btndknow.IsEnabled = false;
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                var existing = conn.Query<tabela>(@"select * from Budynki where zaliczone=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
                txtPol.Text = existing.pol;

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
            btndknow.IsEnabled = false;
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                var existing = conn.Query<tabela>(@"select * from Budynki where zaliczone=0 ORDER BY RANDOM() LIMIT 1").FirstOrDefault();
                txtPol.Text = existing.pol;

            }
        }
        private void btndknow_Click(object sender, RoutedEventArgs e)
        {

            txtEng.Visibility = Visibility.Visible;
            btnknow.IsEnabled = false;


        }


        private async void inpEng_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key.Equals(Windows.System.VirtualKey.Enter))
            {

            
            if (inpEng.Text == txtEng.Text)
            {
                    inpEng.Visibility = Visibility.Collapsed;
                    txtEng.Visibility = Visibility.Visible;
                    btnNext.Visibility = Visibility.Visible;
                    btnknow.Visibility = Visibility.Collapsed;
                }
            else
            {
                await new Windows.UI.Popups.MessageDialog("Niestety, to zła odpowiedź", "Zła odpowiedź").ShowAsync();
            }
            }
        }
    }
}
