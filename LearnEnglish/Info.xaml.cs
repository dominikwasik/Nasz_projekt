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
    public sealed partial class Info : Page
    {


        public Info()
        {
            this.InitializeComponent();
        }


        //rozwijanie lewego panelu
        private void btnShowPane_Click1(object sender, RoutedEventArgs e)
        {
            MySplitView1.IsPaneOpen = !MySplitView1.IsPaneOpen;
        }

        private void btnStartLearn_Click1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Nauka));
        }

        private void btnStartTest_Click1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Test));
        }

        private void btnAbout_Click1(object sender, RoutedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged1(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack1_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void nauka_show_Click(object sender, RoutedEventArgs e)
        {
            test_text.Visibility = Visibility.Collapsed;
            nauka_text.Visibility = Visibility.Visible;
            more_text.Visibility = Visibility.Collapsed;
        }

        private void test_show_Click(object sender, RoutedEventArgs e)
        {
            test_text.Visibility = Visibility.Visible;
            nauka_text.Visibility = Visibility.Collapsed;
            more_text.Visibility = Visibility.Collapsed;
        }

        private void cos_show_Click(object sender, RoutedEventArgs e)
        {
            test_text.Visibility = Visibility.Collapsed;
            nauka_text.Visibility = Visibility.Collapsed;
            more_text.Visibility = Visibility.Visible;
        }

        private void textBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
