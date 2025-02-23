using Microsoft.Data.SqlClient;
using System.Data;

namespace RAG.AI.Domain.SeedWork;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection(string connectionString);
}

public interface ISqlServerDbConnectionFactory : IDbConnectionFactory
{

}

public interface IPostgresDbConnectionFactory : IDbConnectionFactory
{

}

public class SqlServerDbConnectionFactory : ISqlServerDbConnectionFactory
{

    public IDbConnection CreateConnection(string connectionString)
    {
        return new SqlConnection(connectionString);
    }
}





