using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;

namespace ShutterQuest.Views
{
    public partial class MainPage : Microsoft.Maui.Controls.TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>()
                .SetToolbarPlacement(ToolbarPlacement.Bottom);

            BindingContext = new MainViewModel();
        }
    }
}
