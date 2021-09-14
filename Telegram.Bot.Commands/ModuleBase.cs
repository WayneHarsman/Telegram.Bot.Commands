using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Commands
{

    public abstract class ModuleBase : ModuleBase<ICommandContext>{}

    /// <summary>
    /// Base class for a command module to inherit from
    /// </summary>
    /// <typeparam name="T">Type of context that implements <see cref="ICommandContext"/></typeparam>
    public abstract class ModuleBase<T> : IModuleBase
        where T : class, ICommandContext
    {
        public T Context { get; set; }

        /// <summary>
        /// Reply with basic text message
        /// </summary>
        /// <param name="message">Text message that will be sent</param>
        protected virtual async Task<Message> ReplyAsync(string message)
        {
            //TODO: FIX this mess
            return await Context.Client.SendTextMessageAsync(Context.ChatId, message);
        }

        void IModuleBase.SetContext(ICommandContext context)
        {
            var newContext = context as T;
            Context = newContext ??
                      throw new InvalidOperationException(
                          $"Invalid context type. <{typeof(T).Name}> expected. Got {context.GetType().Name}");
        }
    }
}
