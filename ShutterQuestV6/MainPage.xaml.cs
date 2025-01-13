using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Microsoft.Maui.Devices;

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

            CurrentPageChanged += MainPage_CurrentPageChanged;
        }

        private void MainPage_CurrentPageChanged(object sender, EventArgs e)
        {
            try
            {

                Vibration.Vibrate(20); 
            }
            catch (FeatureNotSupportedException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Vibration not supported: {ex.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error triggering haptics: {ex.Message}");
            }
        }
    }
}
