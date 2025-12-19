using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using am.kon.packages.dac.doris;
using am.kon.packages.dac.primitives.Config;
using am.kon.packages.services.dac.doris.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace am.kon.packages.services.dac.doris;

/// <summary>
/// Service component to interact with Apache Doris databases (via the MySQL wire protocol) using the DAC abstractions.
/// </summary>
public partial class DatabaseConnectionService
{
    private readonly ILogger<DatabaseConnectionService> _logger;
    private readonly IConfiguration _configuration;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly CancellationToken _cancellationToken;
    private readonly SortedList<string, DataBase> _databaseConnections;
    private readonly DataBase _defaultDatabase;
    private readonly DacConfig _dacConfig;
    private readonly ConnectionStringsConfig _connectionStringsConfig;

    public DataBase DefaultDatabase => _defaultDatabase;

    public DatabaseConnectionService(
        ILogger<DatabaseConnectionService> logger,
        IConfiguration configuration,
        IOptions<DacConfig> dacConfigOptions,
        IOptions<ConnectionStringsConfig> connectionStringsOptions)
    {
        _logger = logger;
        _configuration = configuration;
        _dacConfig = dacConfigOptions.Value;
        _connectionStringsConfig = connectionStringsOptions.Value;

        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;

        _databaseConnections = new SortedList<string, DataBase>(_connectionStringsConfig.Count);

        foreach (KeyValuePair<string, string> record in _connectionStringsConfig)
        {
            _databaseConnections.Add(record.Key, new DataBase(record.Value, _cancellationToken));
        }

        if (_databaseConnections.TryGetValue(_dacConfig.DefaultConnection, out var defaultDb))
        {
            _defaultDatabase = defaultDb;
        }
        else
        {
            throw new KeyNotFoundException($"Default connection '{_dacConfig.DefaultConnection}' is not configured for the Doris DAC service.");
        }
    }

    public Task Start() => Task.CompletedTask;

    public Task Stop()
    {
        _cancellationTokenSource.Cancel();
        return Task.CompletedTask;
    }

    public DataBase? this[string key]
    {
        get
        {
            if (_databaseConnections.TryGetValue(key, out DataBase? database))
                return database;

            return null;
        }
    }
}
