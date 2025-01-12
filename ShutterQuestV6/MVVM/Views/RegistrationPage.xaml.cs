using Microsoft.Maui.Controls;

namespace ShutterQuest.Views
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            BindingContext = new RegistrationViewModel();
        }
    }
}
