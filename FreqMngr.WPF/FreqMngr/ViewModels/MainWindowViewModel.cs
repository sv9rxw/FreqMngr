using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreqMngr.Models;
using FreqMngr.Services;

namespace FreqMngr.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {


            if (IsInDesignMode)
                Service = new DbServiceMock("designmode");
            else
            {
                String folderPath = System.IO.Directory.GetCurrentDirectory();
                Service = new DbService(folderPath + @"\FreqDB.mdf");
            }

            Service.Connect();
            _Groups = (ObservableCollection<Group>)Service.GetGroupsTree();
            
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

        private ObservableCollection<Group> _Groups = null;
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


        private ObservableCollection<Freq> _Freqs;
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
