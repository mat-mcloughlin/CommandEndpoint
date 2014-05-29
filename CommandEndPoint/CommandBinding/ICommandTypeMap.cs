namespace CommandEndPoint.CommandBinding
{
    using System;
    using System.Collections.Generic;

    public interface ICommandTypeMap
    {
        IDictionary<string, Type> ByName { get; }
    }
}