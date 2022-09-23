using NServiceBus;
using NServiceBus.Settings;

namespace SqlUpgradeRepro;

public class MyMessageHandler : IHandleMessages<MyMessage>
{
    private MyRepository repository;
    private readonly string endpointName;

    public MyMessageHandler(MyRepository repository, ReadOnlySettings settings)
    {
        this.repository = repository;
        this.endpointName = settings.EndpointName();
    }
    
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine($"{endpointName} - Message received.");
        Console.WriteLine(repository.Session.Connection != null);
        return Task.CompletedTask;
    }
}