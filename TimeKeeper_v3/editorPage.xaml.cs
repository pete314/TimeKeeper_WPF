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
using TimeKeeper_v3.ViewModels;

namespace TimeKeeper_v3
{
    public partial class editorPage : PhoneApplicationPage
    {
        private string selectedIndex;
        private ToDoItemModel selectedTodoItem;
        public string PlacerHolderText { get; set; }
        private String selectedDate;

        public editorPage()
        {
            InitializeComponent();
            // Set the orientation to landscap and portrait
            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
        }
        // When page is navigated to set data context to selected item in list
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (DataContext == null)
            {
                //string selectedIndex = "";
                if (NavigationContext.QueryString.TryGetValue("selectedItem", out selectedIndex))
                {
                    DataContext = App.ViewModel.GetItem(selectedIndex);
                    selectedTodoItem = App.ViewModel.GetItem(selectedIndex);
                    //int index = int.Parse(selectedIndex);
                    //DataContext = App.ViewModel.Items[index];

                }
            }
        }

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (selectedTodoItem != null)
            {
                selectedTodoItem.Note = editorPageNote.Text.Replace("\r", "\n");
                await App.ViewModel.UpdateToDoItems();
            }
        }
        private void Convert_Click(object sender, EventArgs e) {
            var saveBtn = this.ApplicationBar.Buttons[0];
            int cType = SwitchContentType();

            if (sender.Equals(saveBtn))
            {
                MessageBox.Show("Saved!");
                //This is a edit ToDo element function call [the else is a new note]
                if (selectedIndex != null)
                {
                    App.ViewModel.EditToDoItem(selectedIndex, editorPageNote.Text, editorPageTitle.Text, (DateTime)editorPageDate.Value, cType);
                }
                else
                {
                    App.ViewModel.CreateNewToDoItem(editorPageNote.Text, editorPageTitle.Text, (DateTime)editorPageDate.Value, cType);
                }
                NavigationService.GoBack();
            }
        }

        private int SwitchContentType() {
            //get the selected item from the listpicker
            ListPickerItem selectedItem = (ListPickerItem)listPicker.SelectedItem;
            string cType = (string)selectedItem.Content;

            int iType = 0;
            switch (cType)
            {
                case "Important":
                    iType = 1;
                    break;
                case "Not Important":
                    iType = 2;
                    break;
            }
            /*switch (cType) { 
                case "Important [Soon]":
                    iType = 1;
                    break;
                case "Important [Not Soon]":
                    iType = 2;
                    break;
                case "Not Important [Soon]":
                    iType = 3;
                    break;
                case "Not Important [Not Soon]":
                    iType = 4;
                    break;
            }*/

            return iType;
        }

    }
}