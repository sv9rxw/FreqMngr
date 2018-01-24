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
            Service = new DbService(@"C:\Users\Dirty Harry\Source\Repos\FreqMngr\FreqMngr.WPF\FreqMngr\FreqDB.mdf");
            Service.Connect();
            var mlkia = Service.GetAllGroups();
            TestPrintGroups(mlkia);
        }

        private void TestPrintGroups(IEnumerable<Group> groupList)
        {
            foreach(Group group in groupList)            
                Console.WriteLine(group.ToString());            
        }

        private DbService Service { get; set; } = null;

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
