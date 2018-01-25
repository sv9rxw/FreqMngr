using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FreqMngr.Models;
using System.Collections.ObjectModel;

namespace FreqMngr.Services
{
    interface IDbService
    {
        IEnumerable<Group> GetAllGroups();
        void FillFreqs(ObservableCollection<Freq> list, Group group);
        IEnumerable<Freq> GetFreqs(Group group);
        IEnumerable<Group> GetGroupsTree();        
        bool Connect();
    }
}
