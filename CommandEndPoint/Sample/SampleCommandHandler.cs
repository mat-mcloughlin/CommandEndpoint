namespace CommandEndPoint.Sample
{
    using System.Threading;

    using CommandEndPoint.Domain;

    public class SampleCommandHandler : ICommandHandler<SampleCommand>
    {
        public void Execute(SampleCommand command)
        {
            Thread.Sleep(5000);
        }
    }
}