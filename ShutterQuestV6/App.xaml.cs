namespace ShutterQuestV6
{
    using Microsoft.Maui.Controls;
    using ShutterQuest.Views;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Start with the LoginPage
            MainPage = new NavigationPage(new LoginPage());
        }
    }
}