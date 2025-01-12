using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using ShutterQuest.Views;
using ShutterQuestV6.Services;

namespace ShutterQuestV6.MVVM.ViewModels
{
    public class SettingsViewModel
    {
        public ICommand LogOutCommand { get; }

        public SettingsViewModel()
        {
            LogOutCommand = new Command(async () => await LogOutAsync());
        }

        private async Task LogOutAsync()
        {
            SecureStorage.Default.Remove("loggedInUserId");

            var databaseService = new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3"));
            Application.Current.MainPage = new NavigationPage(new LoginPage(databaseService));
        }
    }
}
