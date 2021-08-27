using Telegram.Bot.Types;

namespace Telegram.Bot.Commands
{
    public class CommandContext
    {
        public TelegramBotClient Client { get; set; }

        public Message Message { get; set; }
    }
}
