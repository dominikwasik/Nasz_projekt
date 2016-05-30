using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Reflection;
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
    public sealed partial class Test2 : Page
    {
        private class tabela
        {
            public string name { get; set; }
        }

        public object PobierzWartosc(object wartosc, string propertyName)
        {
            return wartosc.GetType().GetProperties()
               .Single(pi => pi.Name == propertyName)
               .GetValue(wartosc, null);
        }


        public Test2()

        {
            this.InitializeComponent();


            //wyswietlenie w comboboxie wszystkich kategorii
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))

            {
                var words = conn.Query<tabela>("select name from sqlite_master where type='table'").ToList<tabela>();
                cmbCategory.ItemsSource = words;

            }

            for (int i = 0; i < 21; i++)
            {
                cmbIlosc.Items.Add(i);
            }



        }

        private void btnShowPane_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void btnStartLearn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Nauka));
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCategory.SelectedIndex<0||cmbRodzaj.SelectedIndex<0||cmbIlosc.SelectedIndex<0)
            {
                await new Windows.UI.Popups.MessageDialog("Wybierz wszystkie opcje do testu", "Błąd").ShowAsync();
            }
            else
            {

                string kategoria = PobierzWartosc(cmbCategory.Items[cmbCategory.SelectedIndex], "name").ToString();
                string ilosc = cmbIlosc.Items[cmbIlosc.SelectedIndex].ToString();
                string rodzaj = (cmbRodzaj.Items[cmbRodzaj.SelectedIndex] as ComboBoxItem).Content.ToString();
                Windows.Storage.ApplicationDataContainer zmiennedoTestu = Windows.Storage.ApplicationData.Current.LocalSettings;
                zmiennedoTestu.Values["Kategorie"] = kategoria;
                zmiennedoTestu.Values["Ilosc"] = ilosc;
                zmiennedoTestu.Values["Rodzaj"] = rodzaj;
                await new Windows.UI.Popups.MessageDialog("Wybrałeś kategorie: "+kategoria+" oraz "+rodzaj+" słówka, test będzie się składał z "+ilosc+" pytań", "Potwierdź").ShowAsync();
                Frame.Navigate(typeof(Test2_1));
            }
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Info));
        }
    }
}
