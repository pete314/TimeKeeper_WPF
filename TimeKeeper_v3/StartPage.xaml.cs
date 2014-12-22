using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Threading.Tasks;

namespace TimeKeeper_v3
{
    public partial class StartPage : PhoneApplicationPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            //&& !(App.Current as App).dataWasLoaded
            if (!App.ViewModel.IsDataLoaded)
            {
                await App.ViewModel.LoadData();
            }
            if (App.ViewModel.isDataLoaded)
            {
                importantSoonCntText.Text = App.ViewModel.Items_1.Count.ToString();
                importantNotSoonCntText.Text = App.ViewModel.Items_2.Count.ToString();
                notImportantSoonCntText.Text = App.ViewModel.Items_3.Count.ToString();
                notImportantNotSoonCntText.Text = App.ViewModel.Items_4.Count.ToString();
            }
        }
        private void Convert_Click(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (sender.Equals(spImportantSoon))
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml?pivIndex=" + 0, UriKind.RelativeOrAbsolute));
            }
            else if (sender.Equals(spImportantNotSoon))
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml?pivIndex=" + 1, UriKind.RelativeOrAbsolute));
            }
            else if (sender.Equals(spNotImportantSoon))
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml?pivIndex=" + 2, UriKind.RelativeOrAbsolute));
            }
            else if (sender.Equals(spNotImportantNotSoon))
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml?pivIndex=" + 3, UriKind.RelativeOrAbsolute));
            }
        }

        //Handle navigation bar icon click
        private void Convert_Icon_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Works");
        }//end Convert_click

        //handle appbar button clicks
    }
}