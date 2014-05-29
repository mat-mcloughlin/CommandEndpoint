namespace Domain.Queries
{
    using System;

    using CommonDomain.Core;

    public class Query : AggregateBase
    {
        private Guid id;

        private string description;

        private string name;

        private Query(Guid id)
        {
            this.id = id;
        }

        public Query(Guid id, string name, string description)
        {
            this.Id = id;
            RaiseEvent(new QueryCreatedEvent(name, description));
        }

        private void Apply(QueryCreatedEvent @event)
        {
            this.name = @event.Name;
            this.description = @event.Description;
        }
    }
}
