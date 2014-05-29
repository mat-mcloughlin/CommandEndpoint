namespace CommandEndPoint.CommandBinding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using CommandEndPoint.Domain;

    public class CommandTypeMap : ICommandTypeMap
    {
        private readonly IDictionary<string, Type> commandTypes;

        public CommandTypeMap()
        {
            this.commandTypes = new Dictionary<string, Type>();

            var types = typeof(ICommandHandler<>).Assembly.GetTypes().Where(t => IsAssignableToGenericType(t, typeof(ICommandHandler<>)) && t.IsInterface == false);

            foreach (var commandType in types.Select(GetCommandFromInterface))
            {
                this.commandTypes.Add(commandType.Name, commandType);
            }

        }

        public IDictionary<string, Type> ByName
        {
            get
            {
                return this.commandTypes;
            }
        }

        private static Type GetCommandFromInterface(Type type)
        {
            return type.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)).GetGenericArguments()[0];
        }

        public static bool IsAssignableToGenericType(Type type, Type genericType)
        {
            var interfaceTypes = type.GetInterfaces();

            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
            {
                return true;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            var baseType = type.BaseType;

            return baseType != null && IsAssignableToGenericType(baseType, genericType);
        }
    }
}