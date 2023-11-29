using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class NumbersController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _storage;

        public NumbersController(ITelegramBotClient telegramBotClient, IStorage storage)
        {
            _telegramClient = telegramBotClient;
            _storage = storage;
        }

        public async Task Handle(Message message, System.Threading.CancellationToken ct)
        {
            if (message.Text == null)
                return;

            var session = _storage.GetSession(message.Chat.Id);

            if (session.Task == "Числа" && session.NumbersSelected)
            {
                if (TryParseNumbers(message.Text, out var numbers))
                {
                    var sum = numbers.Sum();
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"Сумма чисел: {sum}", cancellationToken: ct);
                }
            }
        }

        private bool TryParseNumbers(string input, out int[] numbers)
        {
            numbers = null;

            var splitInput = input.Split(' ');

            if (splitInput.Length == 0)
                return false;

            numbers = new int[splitInput.Length];
            for (var i = 0; i < splitInput.Length; i++)
            {
                if (!int.TryParse(splitInput[i], out var parsedNumber))
                {
                    return false;
                }

                numbers[i] = parsedNumber;
            }

            return true;
        }
    }
}
