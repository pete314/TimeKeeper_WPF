using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TimeKeeper_v3.Resources;
using TimeKeeper_v3.ViewModels;
using Microsoft.Xna.Framework.GamerServices;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;

namespace TimeKeeper_v3
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ToDoItemModel lastSelectedToDoItem;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the LongListSelector control to the sample data
            DataContext = App.ViewModel;

            // Set the orientation to landscap and portrait
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        // Load data for the ViewModel Items
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            /*if (!App.ViewModel.IsDataLoaded)
            {
                await App.ViewModel.LoadData();
            }*/
            string strItemIndex;
            if (NavigationContext.QueryString.ContainsKey("pivIndex"))
            {
                strItemIndex = NavigationContext.QueryString["pivIndex"].ToString();
                pivotControl.SelectedIndex = Convert.ToInt32(strItemIndex);
                
            }
        }

        // Handle selection changed on LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToDoItemModel selectedToDoItem;

            if (sender.Equals(MainLongListSelector)) { 
               selectedToDoItem = MainLongListSelector.SelectedItem as ToDoItemModel;
            }else if (sender.Equals(MainLongListSelector2)) { 
               selectedToDoItem = MainLongListSelector2.SelectedItem as ToDoItemModel;
            }else if (sender.Equals(MainLongListSelector3)) { 
               selectedToDoItem = MainLongListSelector3.SelectedItem as ToDoItemModel;
            }else if (sender.Equals(MainLongListSelector4)) { 
               selectedToDoItem = MainLongListSelector4.SelectedItem as ToDoItemModel;
            }else { 
               selectedToDoItem = MainLongListSelector5.SelectedItem as ToDoItemModel;
            }

            lastSelectedToDoItem = selectedToDoItem;
            if (lastSelectedToDoItem != null)
            {
                NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + selectedToDoItem.ID, UriKind.Relative));
            }
            //MainLongListSelector.SelectedItem = null;
            
            /*// If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // Navigate to the new page
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + (MainLongListSelector.SelectedItem as ToDoItemModel).ID, UriKind.Relative));

            // Reset selected item to null (no selection)
            MainLongListSelector.SelectedItem = null;
             * */
        }
        /*
        private void MainLongListSelector_Tapped(object sender, System.Windows.Input){
            MainLongListSelector_SelectionChanged(null, null);
        }

        */

        //handle appbar button clicks
        private void Convert_Click(object sender, EventArgs e)
        {
            
            //appBar button elements
            var addBtn = this.ApplicationBar.Buttons[0];
            var searchBtn = this.ApplicationBar.Buttons[1];

            //check which button was clicked
            if (sender.Equals(addBtn))
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

        private void trySearch(string searchTerm)
        {

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

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            Guide.BeginShowMessageBox(
                "Please confirm", 
                "Are you sure you want to delete this Item?",
                new string[] {"Yes", "No"}, 0,
                MessageBoxIcon.Warning,
                new AsyncCallback(OnMessageBoxAction),
                null );
        }

        private void OnMessageBoxAction(IAsyncResult ar)
        {
            int? selectedButton = Guide.EndShowMessageBox(ar);
            switch (selectedButton) { 
                case 0:
                    Deployment.Current.Dispatcher.BeginInvoke(
                                                        () => DeletePart2());
                    break;
                default:
                    break;
            }
        }

        private async Task DeletePart2()
        {
            await App.ViewModel.RemoveToDoItem(lastSelectedToDoItem);
        }


        /*
         * Exporting to excel
         */
        private async void LaunchExcel()
        {
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("myExcelFile.xls", CreationCollisionOption.ReplaceExisting);
            using (var s = await file.OpenAsync(FileAccessMode.ReadWrite))
            using (var dw = new DataWriter(s))
            {
                dw.WriteString("Export file created");
            }

            await Launcher.LaunchFileAsync(
                await ApplicationData.Current.LocalFolder.GetFileAsync("myExcelFile.xls"));
        }
        /*
         * This is the search feature for the main page
         * Re-jig the main search textbox or-else spin out to separat page!?
         * 
        private void SearchToDoModelItems(bool startSearch = true, string searchArg = null) {
            if (startSearch)
                SearchBoxMainPage.Visibility = System.Windows.Visibility.Visible;
            else {
                SearchBoxMainPage.Visibility = System.Windows.Visibility.Collapsed;
                MessageBox.Show("Nothing found with title" + searchArg);
            }
        }

        private void SearchBoxMainPage_TextChanged(object sender, KeyEventArgs e)
        {
            var isEnterKey =
                            e.Key == System.Windows.Input.Key.Enter ||
                            e.PlatformKeyCode == 10;
            if (isEnterKey)
            {
                var searchResult = App.ViewModel.SearchItemsByTitle(SearchBoxMainPage.Text);
                if (searchResult.Title.Equals(SearchBoxMainPage.Text, StringComparison.InvariantCultureIgnoreCase))
                {

                    NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + searchResult.ID, UriKind.Relative));
                }
                else
                {
                    SearchToDoModelItems(false, SearchBoxMainPage.Text);
                }
            }

        }
        */
        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}