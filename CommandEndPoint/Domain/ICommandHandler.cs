namespace CommandEndPoint.Domain
{
    public interface ICommandHandler<in T>
    {
        void Execute(T command);
    }
}