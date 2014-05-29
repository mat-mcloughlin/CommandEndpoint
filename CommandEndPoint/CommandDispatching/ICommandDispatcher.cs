namespace CommandEndPoint.CommandDispatching
{
    using System;
    using System.Threading.Tasks;

    public interface ICommandDispatcher
    {
        Task Send(Guid commandId, object command);
    }
}