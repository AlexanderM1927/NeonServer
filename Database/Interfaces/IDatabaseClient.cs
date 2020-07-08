using MySql.Data.MySqlClient;
using System;

namespace Neon.Database.Interfaces
{
    public interface IDatabaseClient : IDisposable
    {
        void connect();
        void disconnect();
        IQueryAdapter GetQueryReactor();
        MySqlCommand createNewCommand();
        void reportDone();
    }
}