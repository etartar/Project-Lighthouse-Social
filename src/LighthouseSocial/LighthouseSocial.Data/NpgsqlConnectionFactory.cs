﻿using Npgsql;
using System.Data;

namespace LighthouseSocial.Data;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connStr;

    public NpgsqlConnectionFactory(string connStr)
    {
        _connStr = connStr;
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connStr);
    }
}
