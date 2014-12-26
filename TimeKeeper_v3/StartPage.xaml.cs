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
using System.Windows.Input;
using System.Collections.ObjectModel;
using TimeKeeper_v3.ViewModels;

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
            //appBar hidden element variables
            var aboutBtn = this.ApplicationBar.MenuItems[0];

            //appBar button elements
            var addBtn = this.ApplicationBar.Buttons[0];
            var searchBtn = this.ApplicationBar.Buttons[1];

            //check which button was clicked
            if (sender.Equals(aboutBtn))
            {
                NavigationService.Navigate(new Uri("/AboutPage.xaml", UriKind.Relative));
            }
            else if (sender.Equals(addBtn))
            {
                //Drop user to empty editorPage
                NavigationService.Navigate(new Uri("/editorPage.xaml", UriKind.Relative));
            }
            else if (sender.Equals(searchBtn))
            {
                //SearchToDoModelItems();
                if (search_TextBox.Visibility == Visibility.Collapsed)
                    search_TextBox.Visibility = Visibility.Visible;
                else
                    search_TextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void startSearch_enter(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                trySearch(search_TextBox.Text);
            }
        }//end Convert_click

        private void trySearch(string searchTerm) {
            
            /*
            var ravSearchResults = App.ViewModel.Items.Where(x => x.Title == searchTerm);
            ObservableCollection<ToDoItemModel> searchResults = new ObservableCollection<ToDoItemModel>(ravSearchResults);
            //searchResults.Count();
            */

            ToDoItemModel result = App.ViewModel.SearchItemsByTitle(searchTerm);
            if (result == null)
                MessageBox.Show("Nothing found!");
            else
                NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + result.ID, UriKind.Relative));
        }

        private void searchTB_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        //handle appbar button clicks
    }
}