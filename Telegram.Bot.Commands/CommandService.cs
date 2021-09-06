using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Commands.Builders;

namespace Telegram.Bot.Commands
{
    /// <summary>
    /// Main class that provides methods for configuration and invocation of commands
    /// </summary>
    public class CommandService
    {
        private ILogger<CommandService> _logger;
        private readonly CommandMapper _mapper;

        public CommandService(ILogger<CommandService> logger = null)
        {
            _logger = logger;
            _mapper = new CommandMapper();
        }

        /// <summary>
        /// Initializes and maps user commands
        /// </summary>
        /// <param name="assembly">Assembly that contains custom built command modules</param>
        /// <param name="provider">Service provider that contains dependencies required for module initialization</param>
        public void AddModulesAsync(Assembly assembly, IServiceProvider provider)
        {
            var searchResult = ModuleBuilder.Search(assembly);
            ModuleBuilder.Build(searchResult, provider, _mapper);
        }

        // public void AddModule<T>() 
        //     where T : ModuleBase
        // {
        //     
        // }

        public void ExecuteAsync(CommandContext context, string command)
        {
            //TODO: check if commands is registered
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

            
            //TODO: add pre-execution event and post-execution event invocation
            commandMethod.Invoke(moduleInstance, null);
        }
    }
}
