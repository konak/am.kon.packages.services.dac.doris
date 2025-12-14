using System;
using System.Data;
using am.kon.packages.dac.doris;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Exceptions;
using MySql.Data.MySqlClient;

namespace am.kon.packages.services.dac.doris;

public partial class DatabaseConnectionService
{
    public Task<IDataReader> ExecuteReaderAsync(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true)
    {
        return _defaultDatabase.ExecuteReaderAsync(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException);
    }

    public Task<IDataReader> ExecuteReaderAsync(string sql, MySqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true)
    {
        return _defaultDatabase.ExecuteReaderAsync(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException);
    }

    [Obsolete("Use ExecuteReaderAsync(string sql, DacDorisParameters parameters, ...) instead.", false)]
    public Task<IDataReader> ExecuteReaderAsync(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true)
    {
        return _defaultDatabase.ExecuteReaderAsync(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException);
    }

    public Task<IDataReader> ExecuteReaderAsync(string sql, DacDorisParameters parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true)
    {
        return _defaultDatabase.ExecuteReaderAsync(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException);
    }
}
