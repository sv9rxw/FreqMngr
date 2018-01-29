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
        List<String> GetModulations();
        Task<List<String>> GetModulationsAsync();
        List<Group> GetAllGroups();        
        List<Freq> GetFreqs(Group group);
        Task<List<Freq>> GetAllDescendantFreqsAsync(Group group);
        List<Group> GetGroupsTree();
        Task<List<Group>> GetGroupsTreeAsync();
        bool Connect();
        void Disconnect();

        Task<bool> UpdateFreqAsync(Freq freq);
    }
}
