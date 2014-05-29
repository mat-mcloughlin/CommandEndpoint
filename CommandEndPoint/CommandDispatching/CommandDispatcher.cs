namespace CommandEndPoint.CommandDispatching
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;

    using Autofac;

    using CommandEndPoint.CommandDispatching.Exceptions;
    using CommandEndPoint.Domain;

    using FluentValidation;

    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext container;

        private readonly MethodInfo dispatchMethodInfo;

        public CommandDispatcher(IComponentContext container)
        {
            this.container = container;
            dispatchMethodInfo = typeof(CommandDispatcher).GetMethod("SendGeneric", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public async Task Send(Guid commandId, object command)
        {
            try
            {
                await (Task)this.dispatchMethodInfo.MakeGenericMethod(command.GetType()).Invoke(this, new[] { commandId, command });
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            }
        }
        
        private async Task SendGeneric<T>(Guid commandId, T command) where T : class
        {
            var validator = this.TryGetValidator<T>();
            var result = validator.Validate(command);

            if (!result.IsValid)
            {
                throw new CommandValidationException(result.Errors);
            }

            var handler = this.TryGetCommandHandler<T>();
            await Task.Factory.StartNew(() => handler.Execute(command));
        }

        private IValidator<T> TryGetValidator<T>()
        {
            var validators = this.container.Resolve<IList<IValidator<T>>>();
            this.CheckValidatorCount(validators);
            var handler = validators.First();
            return handler;
        }

        private ICommandHandler<T> TryGetCommandHandler<T>() where T : class
        {
            var handlers = this.container.Resolve<IList<ICommandHandler<T>>>();
            CheckHandlerCount(handlers);
            var handler = handlers.First();
            return handler;
        }

        private void CheckValidatorCount<T>(IList<IValidator<T>> validators)
        {
            if (validators.Count > 1)
            {
                throw new TooManyValidatorsException();
            }

            if (!validators.Any())
            {
                throw new NoValidatorRegisteredException();
            }
        }

        private static void CheckHandlerCount<T>(IList<ICommandHandler<T>> handlers)
        {
            if (handlers.Count > 1)
            {
                throw new TooManyCommandHandlersException();
            }

            if (!handlers.Any())
            {
                throw new NoCommandHandlerRegisteredException();
            }
        }
    }
}