using Microsoft.Maui.Controls;
using ShutterQuestV6.MVVM.ViewModels;

namespace ShutterQuest.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}
