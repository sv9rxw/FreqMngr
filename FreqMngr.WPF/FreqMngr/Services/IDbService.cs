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
        bool Connected { get; }

        bool Connect();
        void Disconnect();

        List<String> GetModulations();
        Task<List<String>> GetModulationsAsync();

        List<Group> GetAllGroups();
        List<Group> GetGroupsTree();
        Task<List<Group>> GetGroupsTreeAsync();
        bool InsertGroup(Group group);
        Task<bool> InsertGroupAsync(Group group);
        bool UpdateGroup(Group group);
        Task<bool> UpdateGroypAsync(Group group);
        bool DeleteGroup(Group group);
        Task<bool> DeleteGroupAsync(Group group);
        
        List<Freq> GetFreqs(SearchFilter filter);
        Task<List<Freq>> GetAllDescendantFreqsAsync(Group group);
        Task<bool> UpdateFreqAsync(Freq freq);
        bool UpdateFreq(Freq freq);
        bool InsertFreq(Freq freq);
        Task<bool> InsertFreqAsync(Freq freq);
        bool DeleteFreq(Freq freq);
        Task<bool> DeleteFreqAsync(Freq freq);

        List<Freq> SearchFreqs(String term);
        Task<List<Freq>> SearchFreqsAsync(String term);
    }
}
