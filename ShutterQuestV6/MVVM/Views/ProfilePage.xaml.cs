using Microsoft.Maui.Controls;
using ShutterQuest.MVVM.ViewModels;

namespace ShutterQuest.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel();
        }
    }
}
