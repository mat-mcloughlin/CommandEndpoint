namespace CommandEndPoint.CommandDispatching.Exceptions
{
    using System;
    using System.Collections.Generic;

    using FluentValidation.Results;

    public class CommandValidationException : Exception
    {
        public CommandValidationException(IList<ValidationFailure> errors)
            : base("Command failed validation.")
        {
            this.Errors = errors;
        }

        public IList<ValidationFailure> Errors { get; private set; }
    }
}