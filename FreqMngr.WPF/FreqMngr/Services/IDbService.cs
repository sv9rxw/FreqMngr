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

        bool InsertGroup(Group group);
        Task<bool> InsertGroupAsync(Group group);

        bool UpdateGroup(Group group);
        Task<bool> UpdateGroypAsync(Group group);

        bool DeleteGroup(Group group);
        Task<bool> DeleteGroupAsync(Group group);

        List<Group> GetAllGroups();        
        List<Freq> GetFreqs(Group group);
        Task<List<Freq>> GetAllDescendantFreqsAsync(Group group);
        List<Group> GetGroupsTree();
        Task<List<Group>> GetGroupsTreeAsync();
        bool Connect();
        void Disconnect();

        Task<bool> UpdateFreqAsync(Freq freq);
        bool UpdateFreq(Freq freq);
        bool InsertFreq(Freq freq);
        Task<bool> InsertFreqAsync(Freq freq);
    }
}
