using DreamTimeStudioZ.Recipes;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;

namespace TimeKeeper_v3.ViewModels
{
    public class ToDoItemsModel : INotifyPropertyChanged
    {
        private const string DATA_FILE_NAME = "PON.xml";
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsDataLoaded = false;
        private double notImportantDatePush = 8;
        
        //Constructor
        public ToDoItemsModel()
        {
            this.Items = new ObservableCollection<ToDoItemModel>();

        }//end constructor

        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<ToDoItemModel> Items
        {
            get;
            private set;
        }
        /*
         * Declaring 4 more OCollections to hold the 4 categories
         * Before refactor, check the copyItemToCategory function
         */

        public ObservableCollection<ToDoItemModel> Items_1
        {
            get;
            private set;
        }
        public ObservableCollection<ToDoItemModel> Items_2
        {
            get;
            private set;
        }
        public ObservableCollection<ToDoItemModel> Items_3
        {
            get;
            private set;
        }
        public ObservableCollection<ToDoItemModel> Items_4
        {
            get;
            private set;
        }

        public int[] GetItemCount() {
            int[] itemCount = { Items_1.Count, Items_2.Count, Items_3.Count, Items_4.Count};
            
            return itemCount;
        }
        public ToDoItemModel GetItem(string ID) {
            ToDoItemModel result = this.Items.Where(f => f.ID == ID).FirstOrDefault();
            return result;
        }//end GetItem

        public async Task CreateNewToDoItem(string nOte, string tItle, DateTime tDdate, int type)
        {
            ToDoItemModel result = new ToDoItemModel()
            {
                ID = DateTime.Now.Ticks.ToString(),
                Note = nOte,
                Title = tItle,
                tdDate = tDdate.Date,
                Type = type
            };
            this.Items.Add(result);
            //copyItemToCategory(result);
            checkSortItemsForCategory();
            await UpdateToDoItems();
            //return result;
        }//end CreateNewToDoItem

        public void EditToDoItem(string ID, string nOte, string tItle, DateTime tDdate, int type)
        {
            ToDoItemModel selectedItem = GetItem(ID);
            selectedItem.Note = nOte;
            selectedItem.Title = tItle;
            selectedItem.tdDate = tDdate;
            selectedItem.Type = type;
            checkSortItemsForCategory();
        }//end EditToDoItem

        public async Task RemoveToDoItem(ToDoItemModel toDoItemToRemove) {
            this.Items.Remove(toDoItemToRemove);
            await UpdateToDoItems();
            NotifyPropertyChanged("Items");
            checkSortItemsForCategory();
        }// end RemoveToDoItem(ToDoItemModel toDoItemToRemove)

        public async Task MarkDone(ToDoItemModel toDoItemToRemove)
        {
            this.Items.Remove(toDoItemToRemove);
            await UpdateToDoItems();
            NotifyPropertyChanged("Items");
            checkSortItemsForCategory();
        }// end RemoveToDoItem(ToDoItemModel toDoItemToRemove)

        public async Task UpdateToDoItems() {
            StorageFolder appFolder = IORecipes.GetAppStorageFolder();
            await IORecipes.DeleteFileInFolder(appFolder, DATA_FILE_NAME);

            string itemsAsXml = IORecipes.SerializeToString(this.Items);
            StorageFile dataFile = await IORecipes.CreateFileInFolder(appFolder, DATA_FILE_NAME);
            await IORecipes.WriteStringToFile(dataFile, itemsAsXml);
        }//end UpdateToDoItems() 


        public bool isDataLoaded
        {
            get;
            private set;
        }

        public async Task LoadData() {
            //move this to constructor!!!
            this.Items_1 = new ObservableCollection<ToDoItemModel>();
            this.Items_2 = new ObservableCollection<ToDoItemModel>();
            this.Items_3 = new ObservableCollection<ToDoItemModel>();
            this.Items_4 = new ObservableCollection<ToDoItemModel>();

            StorageFolder appFolder = IORecipes.GetAppStorageFolder();
            StorageFile dataFile = await IORecipes.GetFileInFolder(appFolder, DATA_FILE_NAME);

            if (dataFile != null)
            {
                if (!isDataLoaded) {
                    string itemsAsXml = await IORecipes.ReadStringFromFile(dataFile);
                    this.Items = IORecipes.SerializeFromString<ObservableCollection<ToDoItemModel>>(itemsAsXml);
                }
            }
            else
            {
                if (!isDataLoaded)
                {
                    this.Items = new ObservableCollection<ToDoItemModel>();
                    this.Items.Add(
                        new ToDoItemModel()
                        {
                            ID = DateTime.Now.Ticks.ToString(),
                            Note = "This is the First ToDo Description This is the First ToDo Description This is the First ToDo Description This is the First ToDo Description",
                            Title = "First Thing",
                            tdDate = DateTime.Now.AddDays(1),
                            Type = 1
                        });
                    this.Items.Add(
                        new ToDoItemModel()
                        {
                            ID = (DateTime.Now.Ticks+1).ToString(),
                            Note = "This is the Second ToDo Description",
                            Title = "Second Thing",
                            tdDate = DateTime.Now.AddDays(2),
                            Type = 3
                        });
                    this.Items.Add(
                        new ToDoItemModel()
                        {
                            ID = (DateTime.Now.Ticks+2).ToString(),
                            Note = "This is the Third ToDo Description",
                            Title = "Third Thing",
                            tdDate = DateTime.Now.AddDays(3),
                            Type = 1
                        });
                    this.Items.Add(
                        new ToDoItemModel()
                        {
                            ID = (DateTime.Now.Ticks + 2).ToString(),
                            Note = "This is the Third ToDo Description",
                            Title = "Third Thing",
                            tdDate = DateTime.Now.AddDays(4),
                            Type = 3
                        });
                }// end if (!isDataLoaded)
            }//end if (dataFile != null)
            this.isDataLoaded = true;
            NotifyPropertyChanged("Items");
            checkSortItemsForCategory();
        }// end LoadData()

        /*
         * This function does a search in the loaded ToDo...Items.title
         */
        public ToDoItemModel SearchItemsByTitle(string title) {
            ToDoItemModel foundItem = this.Items.Where(f => f.Title.Contains(title)).FirstOrDefault();
            return foundItem;
        }

        /*
         * This function does sort the items into right OCollection 
         */

        private void checkSortItemsForCategory() { 
            foreach(ToDoItemModel checkSortItem in this.Items){
                sortForDate(checkSortItem);
                //copyItemToCategory(checkSortItem);
            }
        }
        /*
         * The logic here is that if an item is more than in now() + settings bounder for soon
         * than add one to the base category, so push it to not soon
         * important soon = 1
         * importan not soon = 2
         * not important soon = 3
         * not important not soon = 4
         */
        private void sortForDate(ToDoItemModel sortable)
        {
            if (sortable.Type != 2 && sortable.Type != 4)
            {
                if (sortable.tdDate > DateTime.Now.AddDays(notImportantDatePush))
                {
                    sortable.Type += 1;
                }
            }
            copyItemToCategory(sortable);
        }

        private void copyItemToCategory(ToDoItemModel sortable) {
            switch (sortable.Type) { 
                case 1:
                    //check first is it in the list already 
                    if (!this.Items_1.Contains(sortable)) {
                        this.Items_1.Add(sortable);
                    }
                    NotifyPropertyChanged("Items_1");
                    break;
                case 2:
                    if (!this.Items_2.Contains(sortable))
                    {
                        this.Items_2.Add(sortable);
                    }
                    NotifyPropertyChanged("Items_2");
                    break;
                case 3:
                    if (!this.Items_3.Contains(sortable))
                    {
                        this.Items_3.Add(sortable);
                    }
                    NotifyPropertyChanged("Items_3");
                    break;
                case 4:
                    if (!this.Items_4.Contains(sortable))
                    {
                        this.Items_4.Add(sortable);
                    }
                    NotifyPropertyChanged("Items_4");
                    break;
            }
        }

        
    }
}
