namespace Domain
{
    using EasyNetQ;
    using EasyNetQ.NonGeneric;


    using NEventStore;

    public static class DelegateDipatcher
    {
        public static void DispatchCommit(IBus bus, ICommit commit)
        {
            foreach (var @event in commit.Events)
            {
                var eventType = @event.Body.GetType();
                bus.Publish(eventType, @event.Body);
            }
        }
    }
}