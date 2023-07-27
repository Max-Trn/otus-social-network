using System.Data;
using Npgsql;
using dialogs_api.Configuration;

namespace dialogs_api.DAL.Infrastructure;

public class ConnectionFactory
{
    public IDbConnection MasterConnection { get; }

    public ConnectionFactory(ApplicationConfiguration config)
    {
        MasterConnection = new NpgsqlConnection(config.ConnectionString);
    }
}