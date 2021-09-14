using Telegram.Bot.Types;

namespace Telegram.Bot.Commands.DefaultCommandContexts
{
    public class MessageCommandContext : ICommandContext
    {
        public TelegramBotClient Client { get; set; }
        public string ChatId { get; set; }
        public User From { get; set; }

        public MessageCommandContext(TelegramBotClient client, Message msg)
        {
            Client = client;
            ChatId = msg.Chat.Id.ToString();
            From = msg.From;
        }
    }
}