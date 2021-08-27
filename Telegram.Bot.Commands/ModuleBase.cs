using Telegram.Bot.Types;

namespace Telegram.Bot.Commands
{
    /// <summary>
    ///     Provides a base class for custom command module to inherit from
    /// </summary>
    public class ModuleBase
    {
        public CommandContext Context { get; set; }

        /// <summary>
        /// Reply with basic text message
        /// </summary>
        /// <param name="message">Text message that will be sent</param>
        public void ReplyAsync(string message)
        {
            Context.Client.SendTextMessageAsync(Context.Message.Chat, message);

        }
    }
}
