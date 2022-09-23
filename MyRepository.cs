using NServiceBus.Persistence.Sql;

namespace SqlUpgradeRepro;

public class MyRepository
{
    public ISqlStorageSession Session { get; }

    public MyRepository(ISqlStorageSession storageSession)
    {
        this.Session = storageSession;
    }
}