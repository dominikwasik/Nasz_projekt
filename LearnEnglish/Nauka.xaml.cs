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
    public sealed partial class Nauka : Page
    {
        public string item = "";
        public Nauka()
        {
            this.InitializeComponent();

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

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //utworzenie nowej zmiennej przechowującej ustawienia lokalne
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            //pobranie numeru zaznaczonego listview z kategorii
            int numer = Convert.ToInt32(listCategory.SelectedIndex);
            //sprawdzenie ile kategorii wybrano, jeśli żadnej, to wyswietli informacje
            if (listCategory.SelectedItems.Count == 0)
            {
                await new Windows.UI.Popups.MessageDialog("Wybierz kategorie z listy po lewej.", "Wybierz Kategorie").ShowAsync();
            }
            else
            {
                //pobranie nazwy kategorii
                string result = (listCategory.Items[numer] as ListViewItem).Content.ToString();
                //utworzenie nowej nazwy ustawień, która będzie przechowywała zaznaczoną kategorie w pamięci aplikacji
                localSettings.Values["Kategoria"] = result;
                // localSettings.Values.Remove("text");
                Frame.Navigate(typeof(Nauka1));
            }
        }

        private async void btnBrowse_Click(object sender, RoutedEventArgs e)
        {

            //utworzenie nowej zmiennej przechowującej ustawienia lokalne
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            //pobranie numeru zaznaczonego listview z kategorii
            int numer = Convert.ToInt32(listCategory.SelectedIndex);
            //sprawdzenie ile kategorii wybrano, jeśli żadnej, to wyswietli informacje
            if (listCategory.SelectedItems.Count == 0)
            {
                await new Windows.UI.Popups.MessageDialog("Wybierz kategorie z listy.", "Wybierz Kategorie").ShowAsync();
            }
            else
            {
                //pobranie nazwy kategorii
                string result = (listCategory.Items[numer] as ListViewItem).Content.ToString();
                //utworzenie nowej nazwy ustawień, która będzie przechowywała zaznaczoną kategorie w pamięci aplikacji
                localSettings.Values["Kategoria"] = result;
                // localSettings.Values.Remove("text");
                Frame.Navigate(typeof(Browser));
            }

        }

        private async void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //utworzenie nowej zmiennej przechowującej ustawienia lokalne
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            //pobranie numeru zaznaczonego listview z kategorii
            int numer = Convert.ToInt32(listCategory.SelectedIndex);
            //sprawdzenie ile kategorii wybrano, jeśli żadnej, to wyswietli informacje
            if (listCategory.SelectedItems.Count == 0)
            {
                await new Windows.UI.Popups.MessageDialog("Wybierz kategorie z listy.", "Wybierz Kategorie").ShowAsync();
            }
            else
            {
                //pobranie nazwy kategorii
                string result = (listCategory.Items[numer] as ListViewItem).Content.ToString();
                //utworzenie nowej nazwy ustawień, która będzie przechowywała zaznaczoną kategorie w pamięci aplikacji
                localSettings.Values["Kategoria"] = result;
                // localSettings.Values.Remove("text");
                Frame.Navigate(typeof(Test));
            }
        }
    }
}
