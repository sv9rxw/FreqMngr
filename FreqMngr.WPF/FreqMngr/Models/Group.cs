using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FreqMngr.Models
{
    public class Group :  INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        private Group _Parent = null;
        public Group Parent
        {
            get
            {
                return _Parent;
            }
            set
            {
                if (value == _Parent)
                    return;

                _Parent = value;
                OnPropertyChanged(nameof(Parent));
            }
        }

        private int _Id;

        private int _ParentId;

        private String _Name = null;
        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (value == _Name)
                    return;

                _Name = value;
                OnPropertyChanged(nameof(Name));
            }                
        }


        public Group(String name)
            : this()
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("name");
            _Name = name;
        }

        public Group(int id, String name, int parentId) 
            :this(name)
        {
            _Id = id;
            _ParentId = parentId;
        }

        public Group(String name, Group parent)
            : this(name)
        {
            _Parent = parent;
        }

        public Group()
        {
            this._Children = new ObservableCollection<Group>();
        }

        private ObservableCollection<Group> _Children = null;

        public ObservableCollection<Group> Children       
        {
            get
            {
                return _Children;
            }
            set
            {
                if (value == _Children)
                    return;

                _Children = value;
                OnPropertyChanged(nameof(Children));
            }
        }                   
        

        public override string ToString()
        {
            return ("[" + _Id.ToString() + ", " + Name + ", " + _ParentId.ToString() + "]");
        }
    }
}
