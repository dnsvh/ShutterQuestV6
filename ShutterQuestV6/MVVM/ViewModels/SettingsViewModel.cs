using System.Windows.Input;
using Microsoft.Maui.Controls;
using ShutterQuestV6;

public class SettingsViewModel
{
    public ICommand LogOutCommand { get; }

    public SettingsViewModel()
    {
        LogOutCommand = new Command(LogOut);
    }

    private void LogOut()
    {
        // Reset logged-in user ID
        App.LoggedInUserId = 0;

        // Navigate back to the LoginPage
        var databaseService = new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3"));
        Application.Current.MainPage = new NavigationPage(new ShutterQuest.Views.LoginPage(databaseService));
    }
}
