namespace CommandEndPoint.Commands
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using CommandEndPoint.CommandBinding;
    using CommandEndPoint.CommandDispatching;

    using Newtonsoft.Json;

    public class CommandController : ApiController
    {
        private const string CommandHeader = "command";

        private readonly ICommandDispatcher commandDispatcher;

        private readonly ICommandTypeMap commandTypeMap;

        public CommandController(ICommandDispatcher commandDispatcher, ICommandTypeMap commandTypeMap)
        {
            this.commandDispatcher = commandDispatcher;
            this.commandTypeMap = commandTypeMap;
        }

        [Route("command/{commandId}")]
        public async Task<object> Put(Guid commandId)
        {
            var command = await this.BindToCommand();
            await this.commandDispatcher.Send(commandId, command);

            // pull in the dispatcher and handle errors here.
            return command;
        }

        private async Task<object> BindToCommand()
        {
            var requestBody = await this.Request.Content.ReadAsStringAsync();
            var commandTypeHeader = TryGetCommandTypeHeader();
            var commandType = TryGetCommandType(commandTypeHeader);

            var serialisedCommand = JsonConvert.DeserializeObject(requestBody, commandType);

            return serialisedCommand;
        }

        private  Type TryGetCommandType(string commandTypeHeader)
        {
            if (!this.commandTypeMap.ByName.ContainsKey(commandTypeHeader))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = string.Format("The command {0} doesn't exist.", commandTypeHeader) });
            }

            var commandType = this.commandTypeMap.ByName[commandTypeHeader];
            return commandType;
        }

        private string TryGetCommandTypeHeader()
        {
            try
            {
                var commandType = this.Request.Headers.GetValues(CommandHeader).FirstOrDefault();
                return commandType;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = string.Format("You need to include a {0} header in your request", CommandHeader) });
            }
        }
    }
}