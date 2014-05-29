namespace Domain.Queries
{
    using FluentValidation;

    public class CreateQueryValidator : AbstractValidator<CreateQuery>
    {
        public CreateQueryValidator()
        {
            RuleFor(q => q.QueryId).NotEmpty();
            RuleFor(q => q.Name).NotEmpty();
            RuleFor(q => q.Description).NotEmpty();
        }
    }
}
