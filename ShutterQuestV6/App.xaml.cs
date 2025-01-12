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

            // Initialize the database service
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3");
            _databaseService = new DatabaseService(dbPath);

            // Set MainPage during initialization
            InitializeApp();
        }

        private async void InitializeApp()
        {
            try
            {
                // Attempt to fetch the logged-in user
                var userIdString = await SecureStorage.Default.GetAsync("loggedInUserId");

                if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
                {
                    var user = await _databaseService.GetByIdAsync<User>(userId);
                    if (user != null)
                    {
                        LoggedInUserId = userId;
                        SetMainPage();
                        return;
                    }
                }

                // If no user is logged in, set LoginPage
                MainPage = new NavigationPage(new LoginPage(_databaseService));
            }
            catch (Exception ex)
            {
                // Fallback error page
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

        // Set the main page for logged-in users
        public void SetMainPage()
        {
            MainPage = new NavigationPage(new MainPage());
        }

        public void Logout()
        {
            // Clear the stored user ID
            SecureStorage.Default.Remove("loggedInUserId");

            // Reset LoggedInUserId
            LoggedInUserId = 0;

            // Redirect to LoginPage
            MainPage = new NavigationPage(new LoginPage(_databaseService));
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            // Ensure a window is created with the correct MainPage
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
