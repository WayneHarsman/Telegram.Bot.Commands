using Telegram.Bot.Types;

namespace Telegram.Bot.Commands
{
    public interface ICommandContext
    {
        public TelegramBotClient Client { get; set; }
        
        public string ChatId { get; set; }
        public User From { get; set; }
    }
}
