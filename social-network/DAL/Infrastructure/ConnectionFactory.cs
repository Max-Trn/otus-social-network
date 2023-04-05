using System.Data;
using Npgsql;
using social_network.Configuration;

namespace social_network.DAL.Infrastructure;

public class ConnectionFactory
{
    public IDbConnection MasterConnection { get; }
    public IDbConnection ReplicaConnection { get; }

    public ConnectionFactory(ApplicationConfiguration config)
    {
        MasterConnection = new NpgsqlConnection(config.ConnectionString);
        ReplicaConnection = new NpgsqlConnection(config.ConnectionStringReplica);
    }
}