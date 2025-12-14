using System;
using System.Data;
using am.kon.packages.dac.doris;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Exceptions;
using MySql.Data.MySqlClient;

namespace am.kon.packages.services.dac.doris;

public partial class DatabaseConnectionService
{
    public Task<object> ExecuteScalarAsync(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text)
    {
        return _defaultDatabase.ExecuteScalarAsync(sql, parameters, commandType);
    }

    public Task<object> ExecuteScalarAsync(string sql, MySqlParameter[] parameters, CommandType commandType = CommandType.Text)
    {
        return _defaultDatabase.ExecuteScalarAsync(sql, parameters, commandType);
    }

    [Obsolete("Use ExecuteScalarAsync(string sql, DacDorisParameters parameters, ...) instead.", false)]
    public Task<object> ExecuteScalarAsync(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text)
    {
        return _defaultDatabase.ExecuteScalarAsync(sql, parameters, commandType);
    }

    public Task<object> ExecuteScalarAsync(string sql, DacDorisParameters parameters, CommandType commandType = CommandType.Text)
    {
        return _defaultDatabase.ExecuteScalarAsync(sql, parameters, commandType);
    }
}
