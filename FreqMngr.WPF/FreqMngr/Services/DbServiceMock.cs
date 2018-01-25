using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreqMngr.Models;
using System.Collections.ObjectModel;

namespace FreqMngr.Services
{
    public class DbServiceMock : IDbService
    {
        public IEnumerable<Group> GetAllGroups()
        {
            ObservableCollection<Group> groupList = new ObservableCollection<Group>();
            groupList.Add(new Group(1, "All", 0));
            groupList.Add(new Group(2, "HF", 1));
            groupList.Add(new Group(3, "Mil-Gov", 2));
            groupList.Add(new Group(4, "DISA Mystic Star", 3));
            groupList.Add(new Group(5, "USAF HFGCS", 3));
            groupList.Add(new Group(6, "Number Stations", 3));
            groupList.Add(new Group(7, "Aero", 2));
            groupList.Add(new Group(8, "VOLMETs", 7));
            groupList.Add(new Group(9, "MWARA", 7));
            groupList.Add(new Group(10, "MWARA NAT-A", 9));
            return groupList;
        }

        public IEnumerable<Freq> GetFreqs(Group group)
        {
            throw new NotImplementedException();
        }

        public DbServiceMock(String path)
        {
            Console.WriteLine("Design-time DbServiceMock");
        }

        public bool Connect()
        {
            return true;
        }

        public IEnumerable<Group> GetGroupsTree()
        {
            ObservableCollection<Group> groupList = new ObservableCollection<Group>();
            Group root = new Group(1, "All", 0);
            groupList.Add(root);

            Group hf = new Group(2, "HF", 1);
            root.Children.Add(hf);

            Group milgov = new Group(3, "Mil-Gov", 2);
            hf.Children.Add(milgov);

            Group disa = new Group(4, "DISA Mystic Star", 3);
            milgov.Children.Add(disa);

            Group hfgcs = new Group(5, "USAF HFGCS", 3);
            milgov.Children.Add(hfgcs);

            Group number = new Group(6, "Number Stations", 3);
            milgov.Children.Add(number);

            Group aero = new Group(7, "Aero", 2);
            hf.Children.Add(aero);

            Group volmet = new Group(8, "VOLMETs", 7);
            aero.Children.Add(volmet);

            Group mwara = new Group(9, "MWARA", 7);
            aero.Children.Add(mwara);

            Group mwaranat = new Group(10, "MWARA NAT-A", 9);
            mwara.Children.Add(mwaranat);

            Group time = new Group(11, "Time", 2);
            hf.Children.Add(time);

            return groupList;
        }
    }
}
