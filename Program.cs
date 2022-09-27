// See https://aka.ms/new-console-template for more information

using NServiceBus;
using NServiceBus.SimpleInjector;
using SimpleInjector;
using SimpleInjector.Lifestyles;
using Microsoft.Data.SqlClient;
using SqlUpgradeRepro;

Console.WriteLine("Hello, World!");

var container = new Container();
container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
container.Options.AllowOverridingRegistrations = true;
container.Options.AutoWirePropertiesImplicitly();
container.Register<MyRepository>(Lifestyle.Scoped);

var endpointConfiguration = new EndpointConfiguration("SqlUpgradeRepro");
endpointConfiguration.EnableInstallers();

var transport = endpointConfiguration.UseTransport<LearningTransport>();
transport.StorageDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".learningtransport"));

var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
var connection = @"Server=localhost,1433;Initial Catalog=SqlUpgradeRepro;User Id=sa;Password=yourStrong(!)Password;encrypt=false";
persistence.SqlDialect<SqlDialect.MsSqlServer>();
persistence.ConnectionBuilder(
    connectionBuilder: () =>
    {
        return new SqlConnection(connection);
    });

endpointConfiguration.UseContainer<SimpleInjectorBuilder>(
    customizations =>
    {
        customizations.UseExistingContainer(container);
    });

SqlHelper.EnsureDatabaseExists(connection);

var endpointInstance = await Endpoint.Start(endpointConfiguration);

container.Verify();

await endpointInstance.SendLocal(new MyMessage());

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await endpointInstance.Stop()
    .ConfigureAwait(false);