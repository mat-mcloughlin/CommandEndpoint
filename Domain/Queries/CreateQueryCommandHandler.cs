namespace Domain.Queries
{
    using System;

    using CommonDomain.Persistence;

    public class CreateQueryCommandHandler : ICommandHandler<CreateQuery>
    {
        private readonly IRepository repository;

        public CreateQueryCommandHandler(IRepository repository)
        {
            this.repository = repository;
        }

        public void Execute(CreateQuery command)
        {
            var query = new Query(command.QueryId, command.Name, command.Description);
            this.repository.Save(query, Guid.NewGuid());
        }
    }
}
