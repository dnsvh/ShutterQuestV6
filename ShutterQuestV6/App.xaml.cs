namespace ShutterQuestV6
{
    using Microsoft.Maui.Controls;
    using ShutterQuest.Views;

    public partial class App : Application
    {
        private readonly DatabaseService _databaseService;

        public App(DatabaseService databaseService)
        {
            InitializeComponent();
            _databaseService = databaseService;

            MainPage = new NavigationPage(new LoginPage(_databaseService));
        }
    }
}