using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telegram.Bot.Commands.Builders;

namespace Telegram.Bot.Commands
{
    /// <summary>
    /// Main class that provides methods for configuration and invocation of commands
    /// </summary>
    public class CommandService
    {
        private readonly CommandMapper _mapper = new CommandMapper();

        public void AddModulesAsync(Assembly assembly, IServiceProvider services)
        {
            var searchResult = ModuleBuilder.Search(assembly);
            ModuleBuilder.Build(searchResult, services, _mapper);
        }

        public void ExecuteAsync(CommandContext context, string command, IServiceProvider services)
        {
            var commandMethod = _mapper.GetCommandMethod(command);

            if (commandMethod == null)
            {
                return;
            }

            var moduleInstance = _mapper.GetModuleInstance(command);

            var moduleType = _mapper.GetModule(command);

            //Some serious shitcode happening here
            //TODO: consider injecting context as a command method argument
            moduleType.GetProperty("Context").SetValue(moduleInstance, context);


            commandMethod.Invoke(moduleInstance, null);
        }
    }
}
