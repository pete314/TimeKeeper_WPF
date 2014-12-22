using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper_v3.ViewModels
{
    public class ToDoItemModel :INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _id;

        public string ID {
            get { return _id; }
            set {
                if (value != _id) {
                    _id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        private string _modDate;

        public string ModifiedDate
        {
            get { return _modDate; }
            set
            {
                if (value != _modDate)
                {
                    _modDate = value;
                    NotifyPropertyChanged("ModifiedDate");
                }
            }
        }

        private string _note;

        public string Note
        {
            get { return _note; }
            set
            {
                if (value != _note)
                {
                    _note = value;

                    string[] lines = _note.Split('\n');
                    if (lines.Length > 0) {
                        FirstLine = lines[0];
                    }
                    else
                        FirstLine = "(empty)";

                    NotifyPropertyChanged("Note");
                }
                
            }
        }

        private string _fistLine;

        public string FirstLine
        {
            get { return _fistLine; }
            set
            {
                if (value != _fistLine)
                {
                    _fistLine = value;
                    NotifyPropertyChanged("FirstLine");
                }
                else
                    _fistLine = "(empty)";
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                if (value != _title)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }

        private DateTime _date;
        public DateTime tdDate
        {
            get { return _date; }
            set
            {
                if (value != _date)
                {
                    _date = value;
                    NotifyPropertyChanged("tdDate");
                }
            }
        }

        private int _type;
        public int Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    _type = value;
                    NotifyPropertyChanged("Type");
                }
            }
        }
    }
}
