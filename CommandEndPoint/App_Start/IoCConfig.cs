namespace CommandEndPoint
{
    using System.Reflection;
    using System.Web.Http;

    using Autofac;
    using Autofac.Integration.WebApi;

    using CommandEndPoint.CommandBinding;
    using CommandEndPoint.CommandDispatching;
    using CommandEndPoint.Domain;

    using FluentValidation;

    public static class IoCConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(typeof(ICommandHandler<>).Assembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(typeof(ICommandHandler<>).Assembly).AsClosedTypesOf(typeof(IValidator<>));
            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();
            builder.RegisterType<CommandTypeMap>().As<ICommandTypeMap>();
            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}