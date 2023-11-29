using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class PunctuationController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _storage;

        public PunctuationController(ITelegramBotClient telegramBotClient, IStorage storage)
        {
            _telegramClient = telegramBotClient;
            _storage = storage;
        }

        public async Task Handle(Message message, CancellationToken ct)
        {
            var session = _storage.GetSession(message.Chat.Id);

            // Проверка состояния
            if (session.Task == "Знаки" && !session.NumbersSelected)
            {
                int arrayLength = message.Text?.Length ?? 0;

                await _telegramClient.SendTextMessageAsync(
                    message.Chat.Id,
                    $"Количество знаков: {arrayLength}",
                    cancellationToken: ct);
            }
        }
    }
}
