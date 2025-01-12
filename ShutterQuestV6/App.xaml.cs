namespace ShutterQuestV6
{
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Controls.PlatformConfiguration;
    using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

    public partial class App : Microsoft.Maui.Controls.Application
    {
        public static int LoggedInUserId { get; set; }

        public App()
        {
            InitializeComponent();
            SetMainPage();
        }

        public void SetMainPage()
        {
            var mainPage = new ShutterQuest.Views.MainPage();

            // Ensure the TabbedPage toolbar is at the bottom for Android
            mainPage.On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            mainPage.On<Android>().SetIsSwipePagingEnabled(false);

            MainPage = mainPage;
        }
    }
}
