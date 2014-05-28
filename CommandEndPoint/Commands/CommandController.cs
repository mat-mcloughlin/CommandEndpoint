namespace CommandEndPoint.Commands
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;

    using CommandEndPoint.CommandBinding;

    public class CommandController : ApiController
    {
        [Route("command/{commandId}")]
        public async Task<object> Put(Guid commandId)
        {
            var command = await this.BindToCommand(commandId);

            // pull in the dispatcher and handle errors here.
            return command;
        }
    }
}