using NServiceBus;
using NServiceBus.Persistence.Sql;

namespace SqlUpgradeRepro;

public class MyMessage : ICommand
{
}

public class MyRepository
{
    public ISqlStorageSession Session { get; }

    public MyRepository(ISqlStorageSession storageSession)
    {
        this.Session = storageSession;
    }
}

public class MyMessageHandler : IHandleMessages<MyMessage>
{
    private MyRepository repository;

    public MyMessageHandler(MyRepository repository)
    {
        this.repository = repository;
    }
    
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Message received.");
        Console.WriteLine(repository.Session.Connection != null);
        return Task.CompletedTask;
    }
}