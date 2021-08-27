using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telegram.Bot.Commands.Attributes;

namespace Telegram.Bot.Commands
{
    public class CommandMapper
    {
        private Dictionary<TypeInfo, List<string>> _Map = new Dictionary<TypeInfo, List<string>>();
        private Dictionary<TypeInfo, object> _RegisteredModules = new Dictionary<TypeInfo, object>();
        private Dictionary<string, MethodInfo> _Commands = new Dictionary<string, MethodInfo>();

        public TypeInfo GetModule(string command)
        {
            var moduleType = _Map.First(x => x.Value.Contains(command)).Key;

            return moduleType;
        }
        public object GetModuleInstance(string command)
        {
            var moduleType = GetModule(command);
            return _RegisteredModules[moduleType];
        }

        public MethodInfo GetCommandMethod(string command)
        {
            if (!_Commands.ContainsKey(command))
            {
                return null;
            }
            return _Commands[command];
        }

        public void MapModule(object moduleInstance)
        {
            TypeInfo module = moduleInstance.GetType().GetTypeInfo();

            if (_RegisteredModules.ContainsKey(module))
            {
                return;
            }

            //TODO: add null check
            var commands = module.DeclaredMethods
                .Where(x =>
                {
                    return x.GetCustomAttributes().Any(attribute => attribute.GetType() == typeof(CommandAttribute));
                }).ToList();

            //If no command was found abort mapping
            if (!commands.Any()) {return;}


            //If we got that far then not only module wasn't registered before, but it also contains some commands
            _RegisteredModules.Add(module, moduleInstance);

            var commandsNames = commands
                .Select<MethodInfo, string>(x => x.GetCustomAttribute<CommandAttribute>().CommandName)
                .ToList();

            foreach (var command in commands)
            {
                var name = command.GetCustomAttribute<CommandAttribute>().CommandName;
                _Commands.Add(name, command);
            }

            _Map.Add(module, commandsNames);
        }


    }
}
