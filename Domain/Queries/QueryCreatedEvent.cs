namespace Domain.Queries
{
    public class QueryCreatedEvent
    {
        public QueryCreatedEvent(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}