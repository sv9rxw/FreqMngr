using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreqMngr.Services
{
    public class SearchFilter
    {
        public String SearchTerm { get; set; }
        public List<int> GroupIdList { get; set; }

        public SearchFilter(String searchTerm)
        {
            SearchTerm = searchTerm;
        }

        public SearchFilter(String searchTerm, List<int> groupIdList)
            :this(searchTerm)
        {
            GroupIdList = groupIdList;
        }
    }
}
