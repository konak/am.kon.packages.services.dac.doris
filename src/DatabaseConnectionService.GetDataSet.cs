using System;
using System.Data;
using am.kon.packages.dac.doris;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Exceptions;
using MySql.Data.MySqlClient;

namespace am.kon.packages.services.dac.doris;

public partial class DatabaseConnectionService
{
    public DataSet GetDataSet(string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        return _defaultDatabase.GetDataSet(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    public DataSet GetDataSet(string sql, MySqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        return _defaultDatabase.GetDataSet(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    [Obsolete("Use GetDataSet(..., DacDorisParameters ...) instead.", false)]
    public DataSet GetDataSet(string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        return _defaultDatabase.GetDataSet(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    public DataSet GetDataSet(string sql, DacDorisParameters parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        return _defaultDatabase.GetDataSet(sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }
}
