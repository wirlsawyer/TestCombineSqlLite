using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;//for SQLite
using System.Diagnostics;
using System.Data.SqlClient;

namespace GoogleMapCrawler
{
    class SQLliteHelp
    {
        private static SQLliteHelp _instance;
        private String _dbName;
        private String _tableName;
        private SQLiteConnection _sqlite_connect = null;
        private SQLiteTransaction _sqlite_trans = null;
        private SQLliteHelp() { }

        public static SQLliteHelp Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new SQLliteHelp();
                return _instance;
            }
        }


        public void Init(String pathDB)
        {
            //Application.StartupPath + @"\myData.db"
            String dbName = Path.GetFileName(pathDB);
            if (! File.Exists(pathDB))
            {
                SQLiteConnection.CreateFile(dbName);
            }           
        }

        public void CreateTable(String pathDB)
        {            
            SQLiteConnection sqlite_connect = new SQLiteConnection(String.Format("Data source={0}", pathDB));
            //建立資料庫連線

            sqlite_connect.Open();// Open
            SQLiteCommand sqlite_cmd = sqlite_connect.CreateCommand(); //create command

            String strSQL = "CREATE TABLE IF NOT EXISTS myData (";
            strSQL += "id VARCHAR(32) PRIMARY KEY, ";         
            strSQL += "name string, ";
            strSQL += "tel string, ";
            strSQL += "address string, ";
            strSQL += "lat string, ";
            strSQL += "lon string, ";
            strSQL += "website string, ";
            strSQL += "star string, ";
            strSQL += "cluster string, ";
            strSQL += "desp string, ";
            strSQL += "params string ";           
            strSQL += ");";

            sqlite_cmd.CommandText = strSQL;
            //create table header
            //INTEGER PRIMARY KEY AUTOINCREMENT=>auto increase index
            try
            {
                sqlite_cmd.ExecuteNonQuery(); //using behind every write cmd
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            sqlite_connect.Close();
        }

        public SQLiteConnection Open(String pathDB)
        {
            _sqlite_connect = new SQLiteConnection(String.Format("Data source={0}", pathDB));
            //建立資料庫連線

            _sqlite_connect.Open();// Open

            return _sqlite_connect;
        }

        public void Close()
        {
            _sqlite_connect.Close();
            _sqlite_connect = null;
        }

        public void BeginTransaction()
        {
            _sqlite_trans = _sqlite_connect.BeginTransaction();
        }

        public void Commit()
        {
            _sqlite_trans.Commit();
        }

        public void AddAndUpdate(MapInfo info)
        {
            SQLiteCommand sqlite_cmd = _sqlite_connect.CreateCommand(); //create command

            String strSQL = String.Format("INSERT OR REPLACE INTO {0} (id, name, tel, address, lat, lon, website, star, cluster, desp, params)", "myData");
            strSQL = String.Format("{0} values (\"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\", \"{10}\", \"{11}\" );", strSQL, info.ID, info.Name, info.Tel, info.Address, info.Latitude, info.Longitude, info.WebSite, info.Star, info.Cluster, info.Desp, info.Params);

            //Debug.WriteLine("strSQL=" + strSQL);
            sqlite_cmd.CommandText = strSQL;

            try
            {
                int iResult = sqlite_cmd.ExecuteNonQuery(); //using behind every write cmd
                if (iResult == 0)
                {
                    //Debug.WriteLine("strSQL=" + strSQL);
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void AddAndUpdate(MapInfo info, String pathDB)
        {
            SQLiteConnection sqlite_connect = new SQLiteConnection(String.Format("Data source={0}", pathDB));
            //建立資料庫連線

            sqlite_connect.Open();// Open
            SQLiteCommand sqlite_cmd = sqlite_connect.CreateCommand(); //create command

            String strSQL = String.Format("INSERT OR REPLACE INTO {0} (id, name, tel, address, lat, lon, website, star, cluster, desp, params)", "myData");
            strSQL = String.Format("{0} values (\"{1}\", \"{2}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\", \"{9}\", \"{10}\", \"{11}\" );", strSQL, info.ID, info.Name, info.Tel, info.Address, info.Latitude, info.Longitude, info.WebSite, info.Star, info.Cluster, info.Desp, info.Params);

            //Debug.WriteLine("strSQL=" + strSQL);
            sqlite_cmd.CommandText = strSQL;

            try
            {
                int iResult = sqlite_cmd.ExecuteNonQuery(); //using behind every write cmd
                if (iResult == 0)
                {
                    //Debug.WriteLine("strSQL=" + strSQL);
                }
            }
            catch (SqlException ex)
            {
                Debug.WriteLine(ex.ToString());
            }


           
            sqlite_connect.Close();
        }


        public List<MapInfo> GetAllData(String path)
        {
            List<MapInfo> result = new List<MapInfo>();


            SQLiteConnection sqlite_connect = new SQLiteConnection(String.Format("Data source={0}", path));
            //建立資料庫連線

            sqlite_connect.Open();// Open

            String SQL = "SELECT * FROM myData";

            SQLiteCommand command = new SQLiteCommand(SQL, sqlite_connect);

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                MapInfo info = new MapInfo();
                info.ID = reader.GetString(0);
                info.Name = reader.GetString(1);
                info.Tel = reader.GetString(2);
                info.Address = reader.GetString(3);
                info.Latitude = reader.GetValue(4).ToString();
                info.Longitude = reader.GetValue(5).ToString();
                info.WebSite = reader.GetString(6);
                info.Star = reader.GetValue(7).ToString();
                info.Cluster = reader.GetString(8);
                info.Desp = reader.GetString(9);
                info.Params = reader.GetString(10);
                result.Add(info);
                //Console.WriteLine(String.Format("{0} {1} {2}", reader.GetString(0), reader.GetString(1), reader.GetString(2)));
            }
            sqlite_connect.Close();

            return result;        
        }
    }
}
