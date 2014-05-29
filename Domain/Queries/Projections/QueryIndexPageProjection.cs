namespace Domain.Queries.Projections
{
    using EasyNetQ.AutoSubscribe;

    public class QueryIndexPageProjection : IConsume<QueryCreatedEvent>
    {
        public void Consume(QueryCreatedEvent @event)
        {
            
        }
    }
}
