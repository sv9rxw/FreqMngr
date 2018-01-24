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
    }
}
