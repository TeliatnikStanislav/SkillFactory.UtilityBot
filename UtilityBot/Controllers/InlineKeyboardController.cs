using System.Threading.Tasks;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _storage;

        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage storage)
        {
            _telegramClient = telegramBotClient;
            _storage = storage;
        }

        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обработка нажатия кнопки "Числа"
            if (callbackQuery.Data == "Числа")
            {
                var session = _storage.GetSession(callbackQuery.Message.Chat.Id);
                session.Task = "Числа";
                session.NumbersSelected = true;
                await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                    "Теперь вы можете вводить числа в чат.", cancellationToken: ct);
            }

            // Обработка нажатия кнопки "Знаки"
            else if (callbackQuery.Data == "Знаки")
            {
                var session = _storage.GetSession(callbackQuery.Message.Chat.Id);
                session.Task = "Знаки";
                session.NumbersSelected = false;
                await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                    "Теперь вы можете вводить текст для подсчета знаков.", cancellationToken: ct);
            }
        }
    }
}
