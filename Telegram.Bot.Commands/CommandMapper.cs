using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Commands.Attributes;

namespace Telegram.Bot.Commands
{
    public class CommandMapper
    {
        private Dictionary<TypeInfo, List<string>> _Map = new Dictionary<TypeInfo, List<string>>();
        private Dictionary<TypeInfo, object> _RegisteredModules = new Dictionary<TypeInfo, object>();
        private Dictionary<string, MethodInfo> _Commands = new Dictionary<string, MethodInfo>();

        private ILogger<CommandMapper> _logger = null;


        public CommandMapper(ILogger<CommandMapper> logger = null)
        {
            _logger = logger;
        }

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
            
            _logger?.LogDebug($"Mapping <{module.Name}> module");

            if (_RegisteredModules.ContainsKey(module))
            {
                _logger.LogDebug($"Module <{module.Name}> is already registered");
                return;
            }

            //TODO: add null check
            var commands = module.DeclaredMethods
                .Where(x =>
                {
                    return x.GetCustomAttributes().Any(attribute => attribute.GetType() == typeof(CommandAttribute));
                }).ToList();
            
            _logger?.LogDebug($"Mapper found {commands.Count} command(s).");

            //If no command was found abort mapping
            if (!commands.Any())
            {
                _logger?.LogDebug($"No commands was found in <{module.Name}> module. Module will not be initialized");
                return;
            }


            //If we got that far then not only module wasn't registered before, but it also contains some commands
            _RegisteredModules.Add(module, moduleInstance);
            _logger?.LogDebug($"Module <{module.Name}> is registered");

            var commandsNames = commands
                .Select<MethodInfo, string>(x => x.GetCustomAttribute<CommandAttribute>().CommandName)
                .ToList();

            foreach (var command in commands)
            {
                var name = command.GetCustomAttribute<CommandAttribute>().CommandName;
                _Commands.Add(name, command);
            }

            _Map.Add(module, commandsNames);
            _logger?.LogDebug($"Module <{module.Name}> has been registered");
        }


    }
}
