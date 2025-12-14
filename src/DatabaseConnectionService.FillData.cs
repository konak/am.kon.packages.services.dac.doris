using System;
using System.Data;
using am.kon.packages.dac.doris;
using am.kon.packages.dac.primitives;
using am.kon.packages.dac.primitives.Exceptions;
using MySql.Data.MySqlClient;

namespace am.kon.packages.services.dac.doris;

public partial class DatabaseConnectionService
{
    public void FillData<T>(T dataOut, string sql, IDataParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        _defaultDatabase.FillData(dataOut, sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    public void FillData<T>(T dataOut, string sql, MySqlParameter[] parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        _defaultDatabase.FillData(dataOut, sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    [Obsolete("Use FillData<T>(..., DacDorisParameters ...) instead.", false)]
    public void FillData<T>(T dataOut, string sql, DacSqlParameters parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        _defaultDatabase.FillData(dataOut, sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }

    public void FillData<T>(T dataOut, string sql, DacDorisParameters parameters, CommandType commandType = CommandType.Text, bool throwDbException = true,
        bool throwGenericException = true, bool throwSystemException = true, int startRecord = 0, int maxRecords = 0)
    {
        _defaultDatabase.FillData(dataOut, sql, parameters, commandType, throwDbException, throwGenericException, throwSystemException, startRecord, maxRecords);
    }
}
