using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;

using System.Diagnostics;

using FreqMngr.Models;
using System.Collections.ObjectModel;

namespace FreqMngr.Services
{
    public class DbService : IDbService
    {
        private static String TABLEGROUPS_CLM_ID = "Id";
        private static String TABLEGROUPS_CLM_NAME = "Name";
        private static String TABLEGROUPS_CLM_PARENTID = "ParentId";

        private static String TABLEFREQS_CLM_ID = "Id";
        private static String TABLEFREQS_CLM_FREQUENCY = "Frequency";
        private static String TABLEFREQS_CLM_NAME = "Name";        
        private static String TABLEFREQS_CLM_PARENTID = "ParentId";
        private static String TABLEFREQS_CLM_MODULATION = "Modulation";
        

        private String _MdfDbFilePath;
        private String _ConnectionString = null;
        private SqlConnection _SqlConnection = null;


        private bool _Connected = false;
        public bool Connected
        {
            get
            {
                return _Connected;                        
            }
        }

        public DbService(String mdfDbFilePath)
        {
            if (String.IsNullOrWhiteSpace(mdfDbFilePath)) throw new ArgumentException("mdfDbFilePath");
            _MdfDbFilePath = mdfDbFilePath;

            //TODO: construct connection string            
            _ConnectionString = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = \"" + _MdfDbFilePath +"\"; Integrated Security = True";
        }

        public bool Connect()
        {
            //TODO: connect to MDF file database using SQL client stuff
            _SqlConnection = new SqlConnection(_ConnectionString);
            _SqlConnection.Open();
            _Connected = true;
            Debug.WriteLine("Connected");

            return true;
        }

        
        public IEnumerable<Group> GetAllGroups()
        {
            if (_Connected == false) throw new Exception("Not connected to any database");

            SqlCommand cmd = new SqlCommand("select * from TableGroups", _SqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();

            ObservableCollection<Group> groupList = new ObservableCollection<Group>();
            while (reader.Read())
            {
                int groupId = (int)reader[TABLEGROUPS_CLM_ID];
                String groupName = (String)reader[TABLEGROUPS_CLM_NAME];
                int groupParentId = reader[TABLEGROUPS_CLM_PARENTID] as int? ?? default(int);                
                Group group = new Group(groupId, groupName, groupParentId);                
                groupList.Add(group);
                Console.WriteLine(group.ToString());
            }
            reader.Close();
            return groupList;
        }

        public Group GetRootGroup()
        {
            if (_Connected==false) throw new Exception("Not connected to any database");
            Group root = null;

            IEnumerable<Group> allGroupsList = GetAllGroups();            
                                                            
            return root;
        }
        
        private void LoadFreqs(ObservableCollection<Freq> list, Group group)
        {
            if (_Connected == false) throw new Exception("Not connected to any database");

            SqlCommand cmd = new SqlCommand("select * from TableFreqs where ParentId = '" + group.Id + "'", _SqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            

            while (reader.Read())
            {
                int freqId = (int)reader[TABLEFREQS_CLM_ID];
                double freqFrequency = (double)reader[TABLEFREQS_CLM_FREQUENCY];
                String freqName = (String)reader[TABLEFREQS_CLM_NAME];
                int parentId = (int)reader[TABLEFREQS_CLM_PARENTID];
                String freqModulation = (String)reader[TABLEFREQS_CLM_MODULATION];


                Freq freq = new Freq()
                {
                    Frequency = freqFrequency.ToString(),
                    Name = freqName,
                    Modulation = freqModulation
                };

                list.Add(freq);

                Debug.WriteLine("Found: " + freqId.ToString() + ", " + freqName);
            }

            reader.Close();

            foreach(Group childgroup in group.Children)
            {
                LoadFreqs(list, childgroup);
            }
        }            
        
        public IEnumerable<Freq> GetFreqs(Group group)
        {            
            ObservableCollection<Freq> freqList = new ObservableCollection<Freq>();
            //Load frequencies of this group and its children recursively
            LoadFreqs(freqList, group);         
            return freqList;
        }

        public void FillFreqs(ObservableCollection<Freq> list, Group group)
        {
            if (list == null)
                throw new NullReferenceException(nameof(FillFreqs) + ":list");

            if (group == null)
                return;

            if (_Connected == false)
                throw new Exception("Not connected to any database");

            SqlCommand cmd = new SqlCommand("select * from TableFreqs where ParentId = '" + group.Id + "'", _SqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                int freqId = (int)reader[TABLEFREQS_CLM_ID];
                double freqFrequency = (double)reader[TABLEFREQS_CLM_FREQUENCY];
                String freqName = (String)reader[TABLEFREQS_CLM_NAME];
                int parentId = (int)reader[TABLEFREQS_CLM_PARENTID];
                String freqModulation = (String)reader[TABLEFREQS_CLM_MODULATION];


                Freq freq = new Freq()
                {
                    Frequency = freqFrequency.ToString(),
                    Name = freqName,
                    Modulation = freqModulation
                };

                list.Add(freq);

                Debug.WriteLine("Found: " + freqId.ToString() + ", " + freqName);
            }

            reader.Close();
        }

        private void LoadChildren(IEnumerable<Group> groupList, Group parent)
        {
            foreach(Group group in groupList)
            {
                if (group.ParentId == parent.Id)
                {
                    parent.Children.Add(group);
                    group.Parent = parent;
                    LoadChildren(groupList, group);
                }
            }
        }

        public IEnumerable<Group> GetGroupsTree()
        {
            IEnumerable<Group> groupList = GetAllGroups();

            ObservableCollection<Group> groupTree = new ObservableCollection<Group>();
                      
            // Get root group
            foreach(Group group in groupList)
            {
                if (group.Id==1)
                {
                    Debug.WriteLine("Found root group: " + group.ToString());
                    groupTree.Add(group);
                    LoadChildren(groupList, group);
                }
            }            

            return groupTree;
        }
    }
}
