using MySql.Data.MySqlClient;
using System.Data;

namespace Neon.Database.Interfaces
{
    public interface IRegularQueryAdapter
    {
        void AddParameter(string name, object query);
        bool findsResult();
        int getInteger();
        DataRow getRow();
        string getString();
        DataTable getTable();
        void runFastQuery(string query);
        MySqlDataReader ExecuteReader();
        void RunQuery(string query);
        void SetQuery(string query);
    }
}