using System;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace TwitterAPISearch
{

    /// <summary>
    /// This Class is specifically design to connect with optimized pooling to Microsoft Sql Server.
    /// </summary>
    public class MySqlConnection
    {
        public SqlConnection Connection;
        public SqlDataReader DataReader;
        public SqlCommand Command;
        private string mySqlConnectionString = "Network Library=DBMSSOCN;Data Source=win2008sql,1433;database=kderby2012;user ID=tlitterio;Password=P@ssword;";
        public void CloseConn()
        {
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Open)
                {
                    Connection.Close();
                }
                Connection.Dispose();
            }
        }
        public SqlConnection CreateConn()
        {
            if (Connection == null) { Connection = new SqlConnection(); };
            if (Connection.ConnectionString == string.Empty || Connection.ConnectionString == null)
            {
                try
                {
                    Connection.ConnectionString = "Min Pool Size=5;Max Pool Size=40;Connect Timeout=4;" + mySqlConnectionString + ";";
                    Connection.Open();
                }
                catch (Exception)
                {
                    if (Connection.State != ConnectionState.Closed)
                    {
                        Connection.Close();
                    }
                    Connection.ConnectionString = "Pooling=false;Connect Timeout=45;" + mySqlConnectionString + ";";
                    Connection.Open();
                }
                return Connection;
            }
            if (Connection.State != ConnectionState.Open)
            {
                try
                {
                    Connection.ConnectionString = "Min Pool Size=5;Max Pool Size=40;Connect Timeout=4;" + mySqlConnectionString + ";";
                    Connection.Open();
                }
                catch (Exception)
                {
                    if (Connection.State != ConnectionState.Closed)
                    {
                        Connection.Close();
                    }
                    Connection.ConnectionString = "Pooling=false;Connect Timeout=45;" + mySqlConnectionString + ";";
                    Connection.Open();
                }
            }
            return Connection;
        }
        public MySqlConnection()
        {

        }
        public void InsertCollection(List<string> listOfData)
        {
            //SqlBulkCopy();
            //cmdins.Parameters.Add('@name',tweets.User.ScreenName);
            //cmdins.Parameters.Add('@tweet',tweets.Text);
            //cmdins.Parameters.Add('@language',tweets.User.Language);
            //)
            //string sqlins = "INSERT INTO twitter (screename,tweet,language) VALUES (@name,@tweet,@Language)";
        }
        public void CreateInsertCommand(string strdata)
        {
            //Command.

        }
        public void InsertOneItem(string username, string tweet, DateTime published, int currStream, string tweetid)
        {
            string myInsertString = string.Format("INSERT INTO twitter (screename,tweet,datetime,stream,tweetid) VALUES (@username,@tweet,@published,@currStream,@tweetid)");
            SqlCommand cmdIns = new SqlCommand(myInsertString, Connection);
            cmdIns.Parameters.AddWithValue("@username", username);
            cmdIns.Parameters.AddWithValue("@tweet", tweet);
            cmdIns.Parameters.AddWithValue("@published", published);
            cmdIns.Parameters.AddWithValue("@currStream", currStream);
            cmdIns.Parameters.AddWithValue("@tweetid", tweetid);
            //cmdIns.Parameters.AddWithValue("@language", language);
            //cmdIns.Parameters.AddWithValue("@location", location);
            cmdIns.ExecuteNonQuery();
        }
    }
}