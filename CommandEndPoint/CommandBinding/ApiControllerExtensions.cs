namespace CommandEndPoint.CommandBinding
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Newtonsoft.Json;

    public static class ApiControllerExtensions
    {
        private const string CommandHeader = "command";

        public async static Task<object> BindToCommand(this ApiController apiController, Guid commandId)
        {
            var requestBody = await apiController.Request.Content.ReadAsStringAsync();
            var commandTypeHeader = TryGetCommandTypeHeader(apiController);
            var commandType = TryGetCommandType(commandTypeHeader);

            var serialisedCommand = JsonConvert.DeserializeObject(requestBody, commandType);

            TrySetIdProperty(commandId, serialisedCommand);

            return serialisedCommand;
        }

        private static void TrySetIdProperty(Guid commandId, object serialisedCommand)
        {
            var property = serialisedCommand.GetType().GetProperty("Id");

            if (property == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = string.Format("The command does not contain an Id") });

            }

            property.SetValue(serialisedCommand, commandId);
        }

        private static Type TryGetCommandType(string commandTypeHeader)
        {
            if (!CommandTypeMap.ByName.ContainsKey(commandTypeHeader))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = string.Format("The command {0} doesn't exist.", commandTypeHeader) });
            }

            var commandType = CommandTypeMap.ByName[commandTypeHeader];
            return commandType;
        }

        private static string TryGetCommandTypeHeader(ApiController apiController)
        {
            try
            {
                var commandType = apiController.Request.Headers.GetValues(CommandHeader).FirstOrDefault();
                return commandType;
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError) { ReasonPhrase = string.Format("You need to include a {0} header in your request", CommandHeader) });
            }
        }
    }
}