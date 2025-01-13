using Microsoft.Maui.Controls;
using ShutterQuestV6.MVVM.ViewModels;

namespace ShutterQuest.Views
{
    public partial class InspirationPage : ContentPage
    {
        public InspirationPage(string category)
        {
            InitializeComponent();
            BindingContext = new InspirationViewModel(category);
        }
    }
}
