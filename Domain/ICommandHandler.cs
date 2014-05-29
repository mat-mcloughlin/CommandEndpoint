namespace Domain
{
    public interface ICommandHandler<in T>
    {
        void Execute(T command);
    }
}