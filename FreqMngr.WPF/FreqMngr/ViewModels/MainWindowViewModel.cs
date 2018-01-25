using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreqMngr.Models;
using FreqMngr.Services;
using FreqMngr.Commands;
using System.Windows.Input;

namespace FreqMngr.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private String _DbFilePath = "FreqDB.mdf";
        public String DbFilePath
        {
            get
            {
                return _DbFilePath;
            }
            set
            {
                if (value == _DbFilePath)
                    return;

                _DbFilePath = value;
                OnPropertyChanged(nameof(DbFilePath));
            }
        }

        private bool _IsBusy = false;
        public bool IsBusy
        {
            get { return _IsBusy; }
            set
            {
                if (value == _IsBusy)
                    return;

                _IsBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public MainWindowViewModel()
        {
            if (IsInDesignMode)
                Service = new DbServiceMock("designmode");
            else
            {
                String folderPath = System.IO.Directory.GetCurrentDirectory();
                _DbFilePath = folderPath + @"\FreqDB.mdf";
                Service = new DbService(_DbFilePath);
            }

            this.PropertyChanged += MainWindowViewModel_PropertyChanged;
            
            //Service.Connect();
            //_Groups = (ObservableCollection<Group>)Service.GetGroupsTree();       
        }

        private RelayCommand _LoadDatabaseCommand;
        public RelayCommand LoadDatabaseCommand
        {
            get
            {
                return _LoadDatabaseCommand ?? (_LoadDatabaseCommand = new RelayCommand(
                    x =>
                    {
                        LoadDatabase();
                    }));
            }
        }
                

        private async void LoadDatabase()
        {
            IsBusy = true;
            Service.Connect();            
            _Groups.Clear();
            List<Group> groupList = await Service.GetGroupsTreeAsync();
            foreach (Group group in groupList)
                _Groups.Add(group);
            IsBusy = false;
        }


        private async void MainWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName != null)
            {
                if (e.PropertyName == nameof(ActiveGroup))
                {                    
                    IsBusy = true;
                    _Freqs.Clear();
                    List<Freq> freqs = await GetAllFreqsAsync(_ActiveGroup);
                    foreach (Freq freq in freqs)
                        _Freqs.Add(freq);
                    IsBusy = false;
                }
            }
        }


        private Task<List<Freq>> GetAllFreqsAsync(Group group)
        {
            return Task.Factory.StartNew(() =>
            {
                return (List<Freq>)GetAllFreqs(group);
            });
        }

        private IEnumerable<Freq> GetAllFreqs(Group group)
        {
            List<Freq> list = new List<Freq>();

            list = (List<Freq>)Service.GetFreqs(group);
            
            foreach(Group childGroup in group.Children)
            {
                List<Freq> childFreqList = (List<Freq>)GetAllFreqs(childGroup);
                list.AddRange(childFreqList);
            }
                        
            return list;                        
        }


        private void TestPrintGroups(IEnumerable<Group> groupList)
        {
            foreach(Group group in groupList)            
                Console.WriteLine(group.ToString());            
        }

        private IDbService _Service = null;
        private IDbService Service
        {
            get { return _Service; }
            set { _Service = value; }
        }

        private ObservableCollection<Group> _Groups = new ObservableCollection<Group>();
        public ObservableCollection<Group> Groups
        {
            get { return _Groups; }
            set
            {
                if (value == _Groups)
                    return;

                _Groups = value;
                OnPropertyChanged(nameof(Groups));
            }
        }


        private ObservableCollection<Freq> _Freqs = new ObservableCollection<Freq>();
        public ObservableCollection<Freq> Freqs
        {
            get
            {
                return _Freqs;
            }
            set
            {
                if (value == _Freqs)
                    return;

                _Freqs = value;
                OnPropertyChanged(nameof(Freqs));
            }
        }

        private Group _RootGroup = null;
        public Group RootGroup
        {
            get
            {
                return _RootGroup;
            }
            set
            {
                if (value == _RootGroup)
                    return;

                _RootGroup = value;
                OnPropertyChanged(nameof(RootGroup));
            }
        }

        private Group _ActiveGroup = null;
        public Group ActiveGroup
        {
            get { return _ActiveGroup; }
            set
            {
                if (value == _ActiveGroup)
                    return;

                _ActiveGroup = value;
                OnPropertyChanged(nameof(ActiveGroup));
            }
        }


    }
}
