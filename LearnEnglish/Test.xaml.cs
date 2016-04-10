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
    public sealed partial class Test : Page
    {
        public Test()
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
                x += 1;
                txtCount.Text = x.ToString();
                if (rtnRight.IsChecked == true)
                {
                    score += 1;
                }

                if (score == 3)
                {

                    float procent = (score / x) * 100;

                    await new Windows.UI.Popups.MessageDialog("Na " + x + " pytań, udzeliłeś " + score + " poprawnych odpowiedzi, w tym teście uzyskałeś " + procent + " procent", "Wynik").ShowAsync();
                }
            }
            else
            {
                await new Windows.UI.Popups.MessageDialog("Musisz wybrać przynajmniej jedną odpowiedź.", "Błąd").ShowAsync();
            }
            
        }
    }
}
