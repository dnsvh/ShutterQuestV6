using Microsoft.Maui.Controls;
using ShutterQuestV6.MVVM.ViewModels;
using ShutterQuestV6.Services;

namespace ShutterQuest.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(DatabaseService databaseService)
        {
            InitializeComponent();
            BindingContext = new LoginViewModel(databaseService);
        }
    }
}
