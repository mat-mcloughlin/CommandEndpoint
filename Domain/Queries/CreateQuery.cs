namespace Domain.Queries
{
    using System;

    public class CreateQuery
    {
        public CreateQuery(Guid queryId, string name, string description)
        {
            this.QueryId = queryId;
            this.Name = name;
            this.Description = description;
        }

        public Guid QueryId { get; private set; }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
