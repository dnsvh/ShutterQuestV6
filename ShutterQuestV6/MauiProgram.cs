using Microsoft.Extensions.DependencyInjection;
using ShutterQuestV6;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3");
        var databaseService = new DatabaseService(dbPath);

        // Initialize the database
        Task.Run(async () => await databaseService.InitializeDatabaseAsync()).Wait();

        builder.Services.AddSingleton(databaseService);

        return builder.Build();
    }
}
