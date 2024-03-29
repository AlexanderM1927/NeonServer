﻿using ConsoleWriter;
using MySql.Data.MySqlClient;
using Neon.Database.Interfaces;
using System;
using System.Data;

namespace Neon.Database.Adapter
{
    public class QueryAdapter : IRegularQueryAdapter
    {
        protected IDatabaseClient client;
        protected MySqlCommand command;


        public bool dbEnabled = true;
        public QueryAdapter(IDatabaseClient Client)
        {
            client = Client;
        }

        /*private static bool dbEnabled
        {
            get { return DatabaseManager.dbEnabled; }
        }*/

        public void AddParameter(string parameterName, object val)
        {
            command.Parameters.AddWithValue(parameterName, val);
        }

        public bool findsResult()
        {
            bool hasRows = false;
            try
            {
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    hasRows = reader.HasRows;
                }
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, command.CommandText);
            }

            return hasRows;
        }

        public MySqlDataReader ExecuteReader()
        {
            try
            {
                MySqlDataReader reader = command.ExecuteReader();
                return reader;
            }
            catch (MySqlException ex)
            {
                Writer.LogQueryError(ex, command.CommandText);

                return null;
            }
            finally
            {
                command.CommandText = string.Empty;
                command.Parameters.Clear();
            }
        }

        public int getInteger()
        {
            int result = 0;
            try
            {
                object obj2 = command.ExecuteScalar();
                if (obj2 != null)
                {
                    int.TryParse(obj2.ToString(), out result);
                }
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, command.CommandText);
            }

            return result;
        }

        public DataRow getRow()
        {
            DataRow row = null;
            try
            {
                DataSet dataSet = new DataSet();
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    adapter.Fill(dataSet);
                }
                if ((dataSet.Tables.Count > 0) && (dataSet.Tables[0].Rows.Count == 1))
                {
                    row = dataSet.Tables[0].Rows[0];
                }
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, command.CommandText);
            }

            return row;
        }

        public string getString()
        {
            string str = string.Empty;
            try
            {
                object obj2 = command.ExecuteScalar();
                if (obj2 != null)
                {
                    str = obj2.ToString();
                }
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, command.CommandText);
            }

            return str;
        }

        public DataTable getTable()
        {
            DataTable dataTable = new DataTable();
            if (!dbEnabled)
            {
                return dataTable;
            }

            try
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, command.CommandText);
            }

            return dataTable;
        }

        public void RunQuery(string query)
        {
            if (!dbEnabled)
            {
                return;
            }

            SetQuery(query);
            RunQuery();
        }

        public void SetQuery(string query)
        {
            command.Parameters.Clear();
            command.CommandText = query;
        }

        public void addParameter(string name, byte[] data)
        {
            command.Parameters.Add(new MySqlParameter(name, MySqlDbType.Blob, data.Length));
        }

        public long InsertQuery()
        {
            if (!dbEnabled)
            {
                return 0;
            }

            long lastInsertedId = 0L;
            try
            {
                command.ExecuteScalar();
                lastInsertedId = command.LastInsertedId;
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, command.CommandText);
            }
            return lastInsertedId;
        }

        public void runFastQuery(string query)
        {
            if (!dbEnabled)
            {
                return;
            }

            DateTime now = DateTime.Now;
            SetQuery(query);
            RunQuery();
            _ = DateTime.Now - now;
        }

        public void RunQuery()
        {
            if (!dbEnabled)
            {
                return;
            }

            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                Writer.LogQueryError(exception, command.CommandText);
            }
        }
    }
}