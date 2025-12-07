using System;
using System.Data;
using am.kon.packages.dac.doris;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Exceptions;
using MySql.Data.MySqlClient;

namespace am.kon.packages.services.dac.doris;

public partial class DatabaseConnectionService
{
    public DataTable GetDataTable(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        return _defaultDatabase.GetDataTable(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    public DataTable GetDataTable(string sql, MySqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        return _defaultDatabase.GetDataTable(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    [Obsolete("Use GetDataTable(..., DacDorisParameters ...) instead.", false)]
    public DataTable GetDataTable(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        return _defaultDatabase.GetDataTable(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    public DataTable GetDataTable(string sql, DacDorisParameters parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        return _defaultDatabase.GetDataTable(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }
}
