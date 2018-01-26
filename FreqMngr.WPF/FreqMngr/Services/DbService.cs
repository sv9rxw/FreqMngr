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
using System.Threading;

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
        private static String TABLEFREQS_CLM_MODULATIONTYPE = "ModulationDetails";
        private static String TABLEFREQS_CLM_PROTOCOL = "Protocol";
        private static String TABLEFREQS_CLM_BANDWIDTH = "Bandwidth";
        private static String TABLEFREQS_CLM_COUNTRY = "Country";
        private static String TABLEFREQS_CLM_USER = "User";        
        private static String TABLEFREQS_CLM_DESCRIPTION = "Description";
        private static String TABLEFREQS_CLM_REFERENCES = "References";
        private static String TABLEFREQS_CLM_QSL = "QSL";
        private static String TABLEFREQS_CLM_COORDINATES = "Coordinates";

        private static int TABLEFREQS_CLM_IDX_ID = 0;
        private static int TABLEFREQS_CLM_IDX_NAME = 1;
        private static int TABLEFREQS_CLM_IDX_FREQUENCY = 2;
        private static int TABLEFREQS_CLM_IDX_PARENTID = 3;
        private static int TABLEFREQS_CLM_IDX_MODULATION = 4;
        private static int TABLEFREQS_CLM_IDX_MODULATIONTYPE = 5;
        private static int TABLEFREQS_CLM_IDX_PROTOCOL = 6;
        private static int TABLEFREQS_CLM_IDX_BANDWIDTH = 7;
        private static int TABLEFREQS_CLM_IDX_COUNTRY = 8;
        private static int TABLEFREQS_CLM_IDX_USER = 9;
        private static int TABLEFREQS_CLM_IDX_DESCRIPTION = 10;
        private static int TABLEFREQS_CLM_IDX_REFERENCES = 11;
        private static int TABLEFREQS_CLM_IDX_QSL = 12;
        private static int TABLEFREQS_CLM_IDX_COORDINATES = 13;



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
        
        public List<Group> GetAllGroups()
        {
            if (_Connected == false) throw new Exception("Not connected to any database");

            SqlCommand cmd = new SqlCommand("select * from TableGroups", _SqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();

            List<Group> groupList = new List<Group>();
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

            List<Group> allGroupsList = GetAllGroups();            
                                                            
            return root;
        }

        //private void LoadFreqs(List<Freq> list, Group group)
        //{
        //    if (_Connected == false) throw new Exception("Not connected to any database");

        //    SqlCommand cmd = new SqlCommand("select * from TableFreqs where ParentId = '" + group.Id + "'", _SqlConnection);
        //    SqlDataReader reader = cmd.ExecuteReader();


        //    while (reader.Read())
        //    {
        //        int freqId = (int)reader[TABLEFREQS_CLM_ID];
        //        double freqFrequency = (double)reader[TABLEFREQS_CLM_FREQUENCY];
        //        String freqName = (String)reader[TABLEFREQS_CLM_NAME];
        //        int parentId = (int)reader[TABLEFREQS_CLM_PARENTID];
        //        String freqModulation = (String)reader[TABLEFREQS_CLM_MODULATION];


        //        Freq freq = new Freq()
        //        {
        //            Frequency = freqFrequency,
        //            Name = freqName,
        //            Modulation = freqModulation
        //        };

        //        list.Add(freq);

        //        Debug.WriteLine("Found: " + freqId.ToString() + ", " + freqName);
        //    }

        //    reader.Close();

        //    foreach(Group childgroup in group.Children)
        //    {
        //        LoadFreqs(list, childgroup);
        //    }
        //}            

        private String SafeGetString(SqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return String.Empty;
        }

        public List<Freq> GetFreqs(Group group)
        {            
            List<Freq> freqList = new List<Freq>();
            if (_Connected == false) throw new Exception("Not connected to any database");

            SqlCommand cmd = new SqlCommand("select * from TableFreqs where ParentId = '" + group.Id + "'", _SqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                int freqId = (int)reader[TABLEFREQS_CLM_ID];
                double freqFrequency = (double)reader[TABLEFREQS_CLM_FREQUENCY];
                String freqName = (String)reader[TABLEFREQS_CLM_NAME];
                int parentId = reader[TABLEFREQS_CLM_PARENTID] as int? ?? default(int);
                String freqModulation = SafeGetString(reader, TABLEFREQS_CLM_IDX_MODULATION);
                String freqModulationType = SafeGetString(reader, TABLEFREQS_CLM_IDX_MODULATIONTYPE);
                String freqProtocol = SafeGetString(reader, TABLEFREQS_CLM_IDX_PROTOCOL);
                double freqBandwidth = reader[TABLEFREQS_CLM_BANDWIDTH] as double? ?? default(double);

                String freqCountry = SafeGetString(reader, TABLEFREQS_CLM_IDX_COUNTRY);
                String freqUser = SafeGetString(reader, TABLEFREQS_CLM_IDX_USER);
                String freqDescription = SafeGetString(reader, TABLEFREQS_CLM_IDX_DESCRIPTION);
                String freqReferences = SafeGetString(reader, TABLEFREQS_CLM_IDX_REFERENCES);
                String freqQSL = SafeGetString(reader, TABLEFREQS_CLM_IDX_QSL);
                String freqCoordinates = SafeGetString(reader, TABLEFREQS_CLM_IDX_COORDINATES);


                Freq freq = new Freq()
                {                    
                    Parent = group,
                    Frequency = freqFrequency,
                    Name = freqName,
                    Modulation = freqModulation,
                    ModulationType = freqModulationType,
                    Protocol = freqProtocol,
                    Bandwidth = freqBandwidth,
                    Country = freqCountry,
                    User = freqUser,
                    Description = freqDescription,
                    References = freqReferences,
                    QSL = freqQSL,
                    Coordinates = freqCoordinates
                };

                freqList.Add(freq);

                Debug.WriteLine("Found: " + freqId.ToString() + ", " + freqName);
            }

            reader.Close();
            return freqList;
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

        public List<Group> GetGroupsTree()
        {
            List<Group> groupList = GetAllGroups();

            List<Group> groupTree = new List<Group>();
                      
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

        public Task<List<Group>> GetGroupsTreeAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                List<Group> result = GetGroupsTree();                
                return result;
            });
        }

    }
}
