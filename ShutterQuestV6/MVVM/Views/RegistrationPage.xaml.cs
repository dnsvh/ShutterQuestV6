using Microsoft.Maui.Controls;
using ShutterQuestV6.Services;
using ShutterQuestV6.MVVM.ViewModels;

namespace ShutterQuest.Views
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage(DatabaseService databaseService)
        {
            InitializeComponent();
            BindingContext = new RegistrationViewModel(databaseService);
        }
    }
}

