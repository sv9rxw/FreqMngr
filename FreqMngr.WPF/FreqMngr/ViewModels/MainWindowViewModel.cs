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
using System.Diagnostics;

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
        
        private IDbService Service { get; set; }

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

        /// <summary>
        /// List of all available Modulations to be used in Combo Boxes
        /// </summary>
        private ObservableCollection<String> _Modulations = new ObservableCollection<String>();
        public ObservableCollection<String> Modulations
        {
            get { return _Modulations; }
            set
            {
                if (value == _Modulations)
                    return;

                _Modulations = value;
                OnPropertyChanged(nameof(Modulations));
            }
        }

        /// <summary>
        /// List of all Groups in Database to be displayed in TreeView
        /// </summary>
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

        private Freq _ActiveFreq = null;
        public Freq ActiveFreq
        {
            get
            {
                return _ActiveFreq;
            }
            set
            {
                if (value == _ActiveFreq)
                    return;

                _ActiveFreq = value;
                OnPropertyChanged(nameof(ActiveFreq));
            }
        }

        private List<Freq> _SelectedFreqs = new List<Freq>();
        public List<Freq> SelectedFreqs
        {
            get { return _SelectedFreqs; }
            set
            {
                if (value == null)
                    Debug.WriteLine("MainWindowViewModel: Set SelectedFreqs = null");
                else
                    Debug.WriteLine("MainWindowViewModel: Set SelectedFreqs count = " + value.Count.ToString());
                _SelectedFreqs = value;
                OnPropertyChanged(nameof(SelectedFreqs));
            }
        }

        private List<Freq> _FreqsClipboard = null;

        #region Commands
        public RelayCommand LoadDatabaseCommand { get; set; }
        private RelayCommand CloseDatabaseCommand { get; set; }
        public RelayCommand FreqsSelectionChangedCommand { get; set; }
        public RelayCommand SaveFreqCommand { get; set; }
        public RelayCommand NewFreqCommand { get; set; }
        public RelayCommand CutFreqsCommand { get; set; }
        #endregion

        #region Constructor and Event
        public MainWindowViewModel()
        {
            if (IsInDesignMode)
                Service = new DbServiceMock("designmode");
            else
            {
                String folderPath = System.IO.Directory.GetCurrentDirectory();
                //_DbFilePath = folderPath + @"\FreqDB.mdf";
                //_DbFilePath = @"..\..\FreqDB.mdf";
                _DbFilePath = @"C:\Users\Dirty Harry\Source\Repos\FreqMngr\FreqMngr.WPF\FreqMngr\FreqDb.mdf";
                System.Windows.MessageBox.Show(_DbFilePath);
                Service = new DbService(_DbFilePath);
            }

            this.PropertyChanged += MainWindowViewModel_PropertyChanged;

            LoadDatabaseCommand = new RelayCommand((item) => { LoadDatabase(); }, (item) => { return true; });

            FreqsSelectionChangedCommand = new RelayCommand((item) =>
            {
                _SelectedFreqs.Clear();

                IList<object> list = (IList<object>)item;
                if (list != null)
                {
                    foreach (object obj in list)
                    {
                        if (obj != null)
                        {
                            if (obj is Freq)
                            {
                                _SelectedFreqs.Add((obj as Freq));
                            }
                        }
                    }
                }                    
            });

            SaveFreqCommand = new RelayCommand((item) => { SaveActiveFreq(); }, (item) => { return CanSaveActiveFreq(); });
            CloseDatabaseCommand = new RelayCommand((item) => { CloseDatabase(); });
            CutFreqsCommand = new RelayCommand((item) => { CutFreqs(); }, (item) => { return CanCutFreqs(); });
            NewFreqCommand = new RelayCommand((item) => { NewFreq(); }, (item => { return true; }));
        }

        private async void MainWindowViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e != null && e.PropertyName != null)
            {
                if (e.PropertyName == nameof(ActiveGroup))
                {
                    IsBusy = true;
                    _Freqs.Clear();
                    List<Freq> freqs = await Service.GetAllDescendantFreqsAsync(_ActiveGroup);
                    foreach (Freq freq in freqs)
                        _Freqs.Add(freq);
                    IsBusy = false;
                }
            }
        }
        #endregion


        #region Db Load and Close Methods
        private async void LoadDatabase()
        {
            IsBusy = true;
            Service.Connect();

            //Load modulations
            _Modulations.Clear();
            List<String> modList = await Service.GetModulationsAsync();
            foreach (String mod in modList)
                _Modulations.Add(mod);

            //Load groups
            _Groups.Clear();
            List<Group> groupList = await Service.GetGroupsTreeAsync();
            foreach (Group group in groupList)
                _Groups.Add(group);

            IsBusy = false;
        }

        private void CloseDatabase()
        {
            Debug.WriteLine("Closing Database");
            Service.Disconnect();
        }

        #endregion


        #region Cut, Copy and Paste Methods
        private bool CanCutFreqs()
        {
            if (SelectedFreqs != null && SelectedFreqs.Count > 0)
                return true;
            return false;
        }
                                     
        private void CutFreqs()
        {
            if (SelectedFreqs != null && SelectedFreqs.Count>0) 
            {
                _FreqsClipboard = new List<Freq>();
                foreach (Freq freq in SelectedFreqs)
                {
                    _FreqsClipboard.Add(freq);
                }
            }
        }

        #endregion

        #region New, Edit and Delete Methods
        private void NewFreq()
        {
            Debug.WriteLine(nameof(NewFreq));
        }

        private bool CanSaveActiveFreq()
        {
            return true;
        }

        private async void SaveActiveFreq()
        {
            //TODO: call DbService save freq with ActiveFreq as parameter
            if (_ActiveFreq != null)
                await Service.UpdateFreqAsync(_ActiveFreq);
        }
        #endregion





    }
}
