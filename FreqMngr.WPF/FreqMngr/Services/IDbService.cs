using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FreqMngr.Models;

namespace FreqMngr.Services
{
    interface IDbService
    {
        IEnumerable<Group> GetAllGroups();
        IEnumerable<Freq> GetFreqs(Group group);
        IEnumerable<Group> GetGroupsTree();
        bool Connect();
    }
}
