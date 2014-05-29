namespace Domain
{
    using Domain.Queries.Projections;

    using EasyNetQ;
    using EasyNetQ.AutoSubscribe;

    public static class QueueSubscriber
    {
        public static void Subscribe(IBus bus)
        {
            var subscriber = new AutoSubscriber(bus, "CommandEndPoint");
            subscriber.Subscribe(typeof(QueryIndexPageProjection).Assembly);
        }
    }
}
