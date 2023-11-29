using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using System;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using UtilityBot.Controllers;

public class Bot : BackgroundService
{
    private ITelegramBotClient _telegramClient;
    private MessageController _messageController;
    private InlineKeyboardController _inlineKeyboardController;
    private NumbersController _numbersController;
    private PunctuationController _punctuationController;

    public Bot(
        ITelegramBotClient telegramClient,
        MessageController messageController,
        InlineKeyboardController inlineKeyboardController,
        NumbersController numbersController,
        PunctuationController punctuationController)
    {
        _messageController = messageController;
        _telegramClient = telegramClient;
        _inlineKeyboardController = inlineKeyboardController;
        _numbersController = numbersController;
        _punctuationController = punctuationController;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramClient.StartReceiving(
            async (botClient, update, ct) =>
            {
                await HandleUpdateAsync(botClient, update, ct);
            },
            HandleErrorAsync,
            new ReceiverOptions() { AllowedUpdates = { } },
            cancellationToken: stoppingToken);

        Console.WriteLine("Бот запущен");
    }

    async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
            return;
        }

        if (update.Type == UpdateType.Message)
        {
            switch (update.Message!.Type)
            {
                case MessageType.Text:
                    await _messageController.Handle(update.Message, cancellationToken);
                    await _numbersController.Handle(update.Message, cancellationToken);
                    await _punctuationController.Handle(update.Message, cancellationToken);
                    return;
                default:
                    await _telegramClient.SendTextMessageAsync(update.Message.From.Id, "Данный тип сообщений не поддерживается. Пожалуйста, отправьте текст.", cancellationToken: cancellationToken);
                    return;
            }
        }
    }

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        Console.WriteLine("Ожидаем 10 секунд перед повторным подключением.");
        Thread.Sleep(10000);

        return Task.CompletedTask;
    }
}
