namespace CommandEndPoint
{
    using System.Web.Http;

    using Domain;
    using Domain.Queries;

    using EasyNetQ;

    public class WebApiApplication : System.Web.HttpApplication
    {
        private static IBus bus;

        protected void Application_Start()
        {
            bus = RabbitHutch.CreateBus("host=localhost");
            GlobalConfiguration.Configure(WebApiConfig.Register);
            IoCConfig.RegisterDependencies(bus);

            QueueSubscriber.Subscribe(bus);
        }

        protected void Application_End()
        {
            bus.Dispose();
        }
    }
}

