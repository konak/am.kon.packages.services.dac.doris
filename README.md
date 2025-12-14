# am.kon.packages.services.dac.doris

`am.kon.packages.services.dac.doris` wraps the Doris DAC provider in an injectable service so you can resolve database connections via ASP.NET Core (or generic host) dependency injection. The API mirrors the other provider services, making it easy to swap databases without changing repository logic.

## Installation

```bash
 dotnet add package am.kon.packages.services.dac.doris
```

## Configuration

Add the DAC sections to `appsettings.json` (or another configuration source):

```json
{
  "am.kon.dac": {
    "DefaultConnection": "Primary"
  },
  "ConnectionStrings": {
    "Primary": "Server=localhost;Port=9030;Database=app;Uid=root;Pwd=change-me;",
    "Archive": "Server=localhost;Port=9030;Database=archive;Uid=root;Pwd=another-secret;"
  }
}
```

Register options and the service during startup:

```csharp
using am.kon.packages.dac.primitives.Config;
using am.kon.packages.services.dac.doris;
using am.kon.packages.services.dac.doris.Config;

services.Configure<DacConfig>(configuration.GetSection(DacConfig.SectionDefaultName));
services.Configure<ConnectionStringsConfig>(configuration.GetSection(ConnectionStringsConfig.SectionDefaultName));
services.AddSingleton<DatabaseConnectionService>();
```

## Working with the service

```csharp
public sealed class CustomerRepository
{
    private readonly DatabaseConnectionService _connections;

    public CustomerRepository(DatabaseConnectionService connections) => _connections = connections;

    public async Task<int> DeactivateAsync(long customerId)
    {
        var parameters = new DacDorisParameters()
            .AddItem("@CustomerId", customerId);

        return await _connections.ExecuteNonQueryAsync(
            sql: "UPDATE customers SET is_active = 0 WHERE customer_id = @CustomerId",
            parameters: parameters.ToArray());
    }
}
```

All provider methods are exposed—`ExecuteReaderAsync`, `ExecuteScalarAsync`, `FillData`, `GetDataSet`, etc.—with overloads for `IDataParameter[]`, `MySqlParameter[]`, and `DacDorisParameters`.

## Transactional work

Use the underlying provider exposed via `DefaultDatabase` to orchestrate transactional batches. The helper commits when the delegate completes and rolls back automatically on any exception.

```csharp
using MySql.Data.MySqlClient;

var database = connections.DefaultDatabase;

await database.ExecuteTransactionalSQLBatchAsync(async transaction =>
{
    var conn = (MySqlConnection)transaction.Connection;
    var tx = (MySqlTransaction)transaction;

    using var debit = new MySqlCommand("UPDATE accounts SET balance = balance - @amount WHERE id = @source", conn, tx);
    debit.Parameters.AddWithValue("@amount", amount);
    debit.Parameters.AddWithValue("@source", sourceAccount);
    await debit.ExecuteNonQueryAsync();

    using var credit = new MySqlCommand("UPDATE accounts SET balance = balance + @amount WHERE id = @target", conn, tx);
    credit.Parameters.AddWithValue("@amount", amount);
    credit.Parameters.AddWithValue("@target", targetAccount);
    await credit.ExecuteNonQueryAsync();

    return true;
});
// The DAC issues CommitAsync for you; throwing cancels the transaction and triggers RollbackAsync.
```

For non-transactional batches with a single open connection, call `ExecuteSQLBatchAsync` instead.

### Transactional updates across databases

Coordinate changes across two or more named databases with `TransactionScope`. Calling `scope.Complete()` commits the distributed transaction; skipping it cancels all pending work.

```csharp
using System.Transactions;
using MySql.Data.MySqlClient;

using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

await connections.DefaultDatabase.ExecuteTransactionalSQLBatchAsync(async tx =>
{
    var cmd = (MySqlCommand)tx.Connection.CreateCommand();
    cmd.Transaction = (MySqlTransaction)tx;
    cmd.CommandText = "UPDATE orders SET status = 'archived' WHERE order_id = @id";
    cmd.Parameters.AddWithValue("@id", orderId);
    await cmd.ExecuteNonQueryAsync();
    return 0;
});

await connections["Archive"].ExecuteTransactionalSQLBatchAsync(async tx =>
{
    var cmd = (MySqlCommand)tx.Connection.CreateCommand();
    cmd.Transaction = (MySqlTransaction)tx;
    cmd.CommandText = "INSERT INTO archived_orders(order_id, archived_at) VALUES(@id, NOW())";
    cmd.Parameters.AddWithValue("@id", orderId);
    await cmd.ExecuteNonQueryAsync();
    return 0;
});

scope.Complete(); // Remove this line to cancel the cross-database transaction.
```

## Deriving specialised services

Derive from `DatabaseConnectionService` when you want domain-specific helpers while keeping DI registration simple:

```csharp
public sealed class ReportingConnectionService : DatabaseConnectionService
{
    public ReportingConnectionService(
        ILogger<DatabaseConnectionService> logger,
        IConfiguration configuration,
        IOptions<DacConfig> dacConfig,
        IOptions<ConnectionStringsConfig> connectionStrings)
        : base(logger, configuration, dacConfig, connectionStrings) { }

    public Task<DataTable> LoadSnapshotAsync(int year, int month)
    {
        var parameters = new DacDorisParameters()
            .AddItem("@year", year)
            .AddItem("@month", month);

        return Task.FromResult(DefaultDatabase.GetDataTable(
            sql: "SELECT * FROM reporting.monthly_snapshot WHERE year = @year AND month = @month",
            parameters: parameters.ToArray()));
    }
}
```

Register the derived type alongside the base service if consumers rely on the shared indexer and lifecycle hooks.

## Lifecycle hooks

- `Start()` currently returns a completed task for symmetry with hosted services.
- `Stop()` cancels the internal `CancellationTokenSource`, signalling any long-running operations to finish promptly.

Call `Stop()` from your host shutdown path, or wrap the service in an `IHostedService` adapter to do it automatically.

For lower-level access without dependency injection, rely on [`am.kon.packages.dac.doris`](../am.kon.packages.dac.doris/README.md) directly.
