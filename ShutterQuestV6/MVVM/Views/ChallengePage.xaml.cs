using Microsoft.Maui.Controls;
using ShutterQuestV6.MVVM.ViewModels;

namespace ShutterQuest.Views
{
    public partial class ChallengePage : ContentPage
    {
        public ChallengePage()
        {
            InitializeComponent();
            BindingContext = new ChallengeViewModel();
        }
    }
}
