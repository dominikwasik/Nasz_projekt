using SQLite.Net;
using SQLite.Net.Platform.WinRT;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace LearnEnglish
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Browser : Page
    {


        private class tabela
        {
            public string pol { get; set; }
            public string ang { get; set; }
            public int zaliczone { get; set; }
            public int test { get; set; }
        }
        string kat = "";
        public Browser()
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
                List<tabela> words = conn.Query<tabela>("SELECT pol, ang from '" + kat + "'").ToList<tabela>();
                listWords.ItemsSource = words;
            }

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Nauka));
        }

        private async void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            using (SQLite.Net.SQLiteConnection conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\baza_slow3.sqlite"))
            {
                int numer = Convert.ToInt32(listWords.SelectedIndex);
                string angResult = ((LearnEnglish.Browser.tabela)listWords.SelectedItem).ang;
                conn.Execute(@"DELETE from '"+kat+"' WHERE ang='"+angResult+"'");
                await new Windows.UI.Popups.MessageDialog("Pomyślnie usunięto", "Usunięto").ShowAsync();
                this.Frame.Navigate(this.GetType());
            }
        }
    }
}
