using Microsoft.Maui.Controls;
using ShutterQuestV6.MVVM.ViewModels;

namespace ShutterQuest.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }
    }
}
