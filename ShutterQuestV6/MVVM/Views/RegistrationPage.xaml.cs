using Microsoft.Maui.Controls;

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

