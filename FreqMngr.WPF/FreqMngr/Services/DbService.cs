using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;

using System.Diagnostics;

using FreqMngr.Models;
using System.Collections.ObjectModel;
using System.Threading;

namespace FreqMngr.Services
{
    public class DbService : IDbService
    {
        private static String TABLEMODULATIONS_CLM_NAME = "Name";
        private static int TABLEMODULATIONS_CLM_IDX_NAME = 0;
        private static String TABLEMODULATIONS_PARAM_NAME = "pName";

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
        private static String TABLEFREQS_CLM_SERVICE = "Service";        
        private static String TABLEFREQS_CLM_DESCRIPTION = "Description";
        private static String TABLEFREQS_CLM_URLs = "URLs";
        private static String TABLEFREQS_CLM_QSL = "QSL";
        private static String TABLEFREQS_CLM_COORDINATES = "Coordinates";

        private static String TABLEFREQS_PARAM_ID = "@pId";
        private static String TABLEFREQS_PARAM_FREQUENCY = "@pFrequency";
        private static String TABLEFREQS_PARAM_NAME = "@pName";
        private static String TABLEFREQS_PARAM_PARENTID = "@pParentId";
        private static String TABLEFREQS_PARAM_MODULATION = "@pModulation";
        private static String TABLEFREQS_PARAM_MODULATIONTYPE = "@pModulationType";
        private static String TABLEFREQS_PARAM_PROTOCOL = "@pProtocol";
        private static String TABLEFREQS_PARAM_BANDWIDTH = "@pBandwidth";
        private static String TABLEFREQS_PARAM_COUNTRY = "@pCountry";
        private static String TABLEFREQS_PARAM_SERVICE = "@pService";
        private static String TABLEFREQS_PARAM_DESCRIPTION = "@pDescription";
        private static String TABLEFREQS_PARAM_URLS = "@pURLs";
        private static String TABLEFREQS_PARAM_QSL = "@pQSL";
        private static String TABLEFREQS_PARAM_COORDINATES = "@pCoordinates";

        private static int TABLEFREQS_CLM_IDX_ID = 0;
        private static int TABLEFREQS_CLM_IDX_NAME = 1;
        private static int TABLEFREQS_CLM_IDX_FREQUENCY = 2;
        private static int TABLEFREQS_CLM_IDX_PARENTID = 3;
        private static int TABLEFREQS_CLM_IDX_MODULATION = 4;
        private static int TABLEFREQS_CLM_IDX_MODULATIONTYPE = 5;
        private static int TABLEFREQS_CLM_IDX_PROTOCOL = 6;
        private static int TABLEFREQS_CLM_IDX_BANDWIDTH = 7;
        private static int TABLEFREQS_CLM_IDX_COUNTRY = 8;
        private static int TABLEFREQS_CLM_IDX_SERVICE = 9;
        private static int TABLEFREQS_CLM_IDX_DESCRIPTION = 10;
        private static int TABLEFREQS_CLM_IDX_URLS = 11;
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

        public void Disconnect()
        {
            if (_SqlConnection != null)
                _SqlConnection.Close();
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
                String freqUser = SafeGetString(reader, TABLEFREQS_CLM_IDX_SERVICE);
                String freqDescription = SafeGetString(reader, TABLEFREQS_CLM_IDX_DESCRIPTION);
                String freqURLs = SafeGetString(reader, TABLEFREQS_CLM_IDX_URLS);
                String freqQSL = SafeGetString(reader, TABLEFREQS_CLM_IDX_QSL);
                String freqCoordinates = SafeGetString(reader, TABLEFREQS_CLM_IDX_COORDINATES);


                Freq freq = new Freq()
                {                  
                    Id = freqId,  
                    Parent = group,
                    Frequency = freqFrequency,
                    Name = freqName,
                    Modulation = freqModulation,
                    ModulationType = freqModulationType,
                    Protocol = freqProtocol,
                    Bandwidth = freqBandwidth,
                    Country = freqCountry,
                    Service = freqUser,
                    Description = freqDescription,
                    URLs = freqURLs,
                    QSL = freqQSL,
                    Coordinates = freqCoordinates
                };

                freqList.Add(freq);

                Debug.WriteLine("Found: " + freqId.ToString() + ", " + freqName);
            }

            reader.Close();
            return freqList;
        }

        private IEnumerable<Freq> GetAllFreqs(Group group)
        {
            List<Freq> list = new List<Freq>();

            list = (List<Freq>)GetFreqs(group);

            foreach (Group childGroup in group.Children)
            {
                List<Freq> childFreqList = (List<Freq>)GetAllFreqs(childGroup);
                list.AddRange(childFreqList);
            }

            return list;
        }

        public Task<List<Freq>> GetAllDescendantFreqsAsync(Group group)
        {
            return Task.Factory.StartNew(() => { return (List<Freq>)GetAllFreqs(group); });
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

        public bool UpdateFreq(Freq freq)
        {
            //TODO : create save freq SQL query
            //String updateQuery = "UPDATE TableFreqs SET Frequency = '2222',Name = 'Malakia Updated' WHERE Id = 1";
            //String updateQuery = "UPDATE TableFreqs(Name) VALUES (@pName) WHERE Id = @pId ;";            
            //String updateQuery = "UPDATE TableFreqs SET Name = @pName WHERE Id = @pId";
            //String updateQuery = "UPDATE TableFreqs SET Frequency = @pFrequency,Name = @pName WHERE Id = @pId";

            String updateQuery = "UPDATE TableFreqs SET " +
                TABLEFREQS_CLM_FREQUENCY + " = " + TABLEFREQS_PARAM_FREQUENCY + "," +
                TABLEFREQS_CLM_NAME + " = " + TABLEFREQS_PARAM_NAME + "," +
                TABLEFREQS_CLM_PARENTID + " = " + TABLEFREQS_PARAM_PARENTID + "," +
                TABLEFREQS_CLM_MODULATION + " = " + TABLEFREQS_PARAM_MODULATION + "," +
                TABLEFREQS_CLM_MODULATIONTYPE + " = " + TABLEFREQS_PARAM_MODULATIONTYPE + "," +
                TABLEFREQS_CLM_PROTOCOL + " = " + TABLEFREQS_PARAM_PROTOCOL + "," +
                TABLEFREQS_CLM_BANDWIDTH + " = " + TABLEFREQS_PARAM_BANDWIDTH + "," +
                TABLEFREQS_CLM_COUNTRY + " = " + TABLEFREQS_PARAM_COUNTRY + "," + 
                TABLEFREQS_CLM_SERVICE + " = " + TABLEFREQS_PARAM_SERVICE +  "," +               
                TABLEFREQS_CLM_DESCRIPTION + " = " + TABLEFREQS_PARAM_DESCRIPTION + "," +
                TABLEFREQS_CLM_URLs + " = " + TABLEFREQS_PARAM_URLS + "," +
                TABLEFREQS_CLM_QSL + " = " + TABLEFREQS_PARAM_QSL + "," +
                TABLEFREQS_CLM_COORDINATES + " = " + TABLEFREQS_PARAM_COORDINATES +
                " WHERE " + 
                TABLEFREQS_CLM_ID + " = " + TABLEFREQS_PARAM_ID;


            SqlCommand cmd = new SqlCommand(updateQuery, _SqlConnection);
            cmd.Parameters.Add(TABLEFREQS_PARAM_ID, SqlDbType.Int).Value = freq.Id;
            cmd.Parameters.Add(TABLEFREQS_PARAM_FREQUENCY, SqlDbType.Float).Value = freq.Frequency;
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_NAME, freq.Name);
            cmd.Parameters.Add(TABLEFREQS_PARAM_PARENTID, SqlDbType.Int).Value = freq.Parent.Id;
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_MODULATION, freq.Modulation);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_MODULATIONTYPE, freq.ModulationType);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_PROTOCOL, freq.Protocol);
            cmd.Parameters.Add(TABLEFREQS_PARAM_BANDWIDTH, SqlDbType.Float).Value = freq.Bandwidth;
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_COUNTRY, freq.Country);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_SERVICE, freq.Service);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_DESCRIPTION, freq.Description);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_URLS, freq.URLs);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_QSL, freq.QSL);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_COORDINATES, freq.Coordinates);

            Debug.WriteLine("CommandText = " + cmd.CommandText);

            int rows = 0;
            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch(Exception expt)
            {
                Debug.WriteLine("Rows affected = " + rows.ToString() + " Expt: " + expt.Message);
                return false;
            }                        
            
            Debug.WriteLine("Rows affected = " + rows.ToString());            
            return true;
        }

        public Task<bool> UpdateFreqAsync(Freq freq)
        {
            return Task.Factory.StartNew(() =>{ return UpdateFreq(freq); });
        }

        public bool InsertFreq(Freq freq)
        {
            if (freq == null)
                return false;

            //"INSERT INTO dbo.SMS_PW (id,username,password,email) VALUES (@id,@username,@password, @email)";

            String insertQuery = "INSERT INTO TableFreqs (" +
                TABLEFREQS_CLM_FREQUENCY + "," +
                TABLEFREQS_CLM_NAME + "," +
                TABLEFREQS_CLM_PARENTID + "," +
                TABLEFREQS_CLM_MODULATION + "," +
                TABLEFREQS_CLM_MODULATIONTYPE + "," +
                TABLEFREQS_CLM_PROTOCOL + "," +
                TABLEFREQS_CLM_BANDWIDTH + "," +
                TABLEFREQS_CLM_COUNTRY + "," +
                TABLEFREQS_CLM_SERVICE + "," +
                TABLEFREQS_CLM_DESCRIPTION + "," +
                TABLEFREQS_CLM_URLs + "," +
                TABLEFREQS_CLM_QSL + "," +
                TABLEFREQS_CLM_COORDINATES + ")" +
                " VALUES (" +
                TABLEFREQS_PARAM_FREQUENCY + "," +
                TABLEFREQS_PARAM_NAME + "," +
                TABLEFREQS_PARAM_PARENTID + "," +
                TABLEFREQS_PARAM_MODULATION + "," +
                TABLEFREQS_PARAM_MODULATIONTYPE + "," +
                TABLEFREQS_PARAM_PROTOCOL + "," +
                TABLEFREQS_PARAM_BANDWIDTH + "," +
                TABLEFREQS_PARAM_COUNTRY + "," +
                TABLEFREQS_PARAM_SERVICE + "," +
                TABLEFREQS_PARAM_DESCRIPTION + "," +
                TABLEFREQS_PARAM_URLS + "," +
                TABLEFREQS_PARAM_QSL + "," +
                TABLEFREQS_PARAM_COORDINATES + ")";

            SqlCommand cmd = new SqlCommand(insertQuery, _SqlConnection);            
            cmd.Parameters.Add(TABLEFREQS_PARAM_FREQUENCY, SqlDbType.Float).Value = freq.Frequency;
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_NAME, freq.Name);
            cmd.Parameters.Add(TABLEFREQS_PARAM_PARENTID, SqlDbType.Int).Value = freq.Parent.Id;
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_MODULATION, freq.Modulation);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_MODULATIONTYPE, freq.ModulationType);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_PROTOCOL, freq.Protocol);
            cmd.Parameters.Add(TABLEFREQS_PARAM_BANDWIDTH, SqlDbType.Float).Value = freq.Bandwidth;
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_COUNTRY, freq.Country);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_SERVICE, freq.Service);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_DESCRIPTION, freq.Description);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_URLS, freq.URLs);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_QSL, freq.QSL);
            cmd.Parameters.AddWithValue(TABLEFREQS_PARAM_COORDINATES, freq.Coordinates);

            int rows = 0;
            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception expt)
            {
                Debug.WriteLine("Rows affected = " + rows.ToString() + " Expt: " + expt.Message);
                return false;
            }

            Debug.WriteLine("Rows affected = " + rows.ToString());
            return true;
        }

        public Task<bool> InsertFreqAsync(Freq freq)
        {
            return Task.Factory.StartNew(() => { return InsertFreq(freq); });
        }

        public List<string> GetModulations()
        {
            List<String> modList = new List<String>();
            if (_Connected == false) throw new Exception("Not connected to any database");

            SqlCommand cmd = new SqlCommand("select * from TableModulations", _SqlConnection);
            SqlDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                String mod = (String)reader[TABLEMODULATIONS_CLM_NAME];
                modList.Add(mod);
            }
            reader.Close();
            return modList;
        }

        public Task<List<String>> GetModulationsAsync()
        {
            return Task.Factory.StartNew(() => { return GetModulations(); });
        }


    }
}
