namespace CommandEndPoint
{
    using System.Reflection;
    using System.Web.Http;

    using Autofac;
    using Autofac.Integration.WebApi;

    using CommandEndPoint.CommandBinding;
    using CommandEndPoint.CommandDispatching;

    using CommonDomain.Core;
    using CommonDomain.Persistence;
    using CommonDomain.Persistence.EventStore;

    using Domain;

    using EasyNetQ;

    using FluentValidation;

    using NEventStore;
    using NEventStore.Dispatcher;
    using NEventStore.Persistence.Sql.SqlDialects;

    public static class IoCConfig
    {
        public static void RegisterDependencies(IBus bus)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(typeof(ICommandHandler<>).Assembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(typeof(ICommandHandler<>).Assembly).AsClosedTypesOf(typeof(IValidator<>));
            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();
            builder.RegisterType<CommandTypeMap>().As<ICommandTypeMap>();

            builder.Register(
                m =>
                {
                    var store = Wireup.Init()
                        .UsingSqlPersistence("EventStoreConnectionString")
                        .WithDialect(new MsSqlDialect())
                        .InitializeStorageEngine()
                        .UsingJsonSerialization()
                        .Compress()
                        .UsingAsynchronousDispatchScheduler()
                        .DispatchTo(new DelegateMessageDispatcher(commit => DelegateDipatcher.DispatchCommit(bus, commit)))
                        .Build();
                    return new EventStoreRepository(store, new AggregateFactory(), new ConflictDetector());
                }).As<IRepository>();

            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}