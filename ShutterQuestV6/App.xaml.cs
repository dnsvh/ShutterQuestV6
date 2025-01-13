using System.IO;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using ShutterQuest.Views;
using ShutterQuestV6.MVVM.Models;
using ShutterQuestV6.Services;

namespace ShutterQuestV6
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        public static int LoggedInUserId { get; set; } = 0;
        private readonly DatabaseService _databaseService;

        public App()
        {
            InitializeComponent();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3");
            _databaseService = new DatabaseService(dbPath);

            InitializeApp();
        }

        private async void InitializeApp()
        {
            try
            {
                await _databaseService.InitializeDatabaseAsync();


                var userIdString = await SecureStorage.Default.GetAsync("loggedInUserId");

                if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
                {
                    var user = await _databaseService.GetByIdAsync<User>(userId);
                    if (user != null)
                    {
                        LoggedInUserId = userId;
                        System.Diagnostics.Debug.WriteLine($"LoggedInUserId set to: {LoggedInUserId}");
                        SetMainPage();
                        return;
                    }
                }

                MainPage = new NavigationPage(new LoginPage(_databaseService));
            }
            catch (Exception ex)
            {
                MainPage = new ContentPage
                {
                    Content = new Label
                    {
                        Text = $"Initialization failed: {ex.Message}",
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    }
                };
            }
        }

        public void SetMainPage()
        {
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Colors.Black,
                BarTextColor = Colors.White
            };
        }

        public void Logout()
        {
            SecureStorage.Default.Remove("loggedInUserId");
            LoggedInUserId = 0;
            MainPage = new NavigationPage(new LoginPage(_databaseService));
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            if (MainPage == null)
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3");
                var databaseService = new DatabaseService(dbPath);
                MainPage = new NavigationPage(new LoginPage(databaseService));
            }

            return new Window(MainPage);
        }
    }
}
