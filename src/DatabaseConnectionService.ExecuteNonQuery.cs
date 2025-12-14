using System;
using System.Data;
using am.kon.packages.dac.doris;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Exceptions;
using MySql.Data.MySqlClient;

namespace am.kon.packages.services.dac.doris;

public partial class DatabaseConnectionService
{
    public Task<int> ExecuteNonQueryAsync(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text)
    {
        return _defaultDatabase.ExecuteNonQueryAsync(sql, parameters, commandType);
    }

    public Task<int> ExecuteNonQueryAsync(string sql, MySqlParameter[] parameters, CommandType commandType = CommandType.Text)
    {
        return _defaultDatabase.ExecuteNonQueryAsync(sql, parameters, commandType);
    }

    [Obsolete("Use ExecuteNonQueryAsync(string sql, DacDorisParameters parameters, ...) instead.", false)]
    public Task<int> ExecuteNonQueryAsync(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text)
    {
        return _defaultDatabase.ExecuteNonQueryAsync(sql, parameters, commandType);
    }

    public Task<int> ExecuteNonQueryAsync(string sql, DacDorisParameters parameters, CommandType commandType = CommandType.Text)
    {
        return _defaultDatabase.ExecuteNonQueryAsync(sql, parameters, commandType);
    }
}
