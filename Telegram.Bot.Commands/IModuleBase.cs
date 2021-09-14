namespace Telegram.Bot.Commands
{
    internal interface IModuleBase
    {
        void SetContext(ICommandContext context);
    }
}