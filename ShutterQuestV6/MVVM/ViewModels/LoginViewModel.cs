using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using MvvmHelpers;
using Microsoft.Maui.Storage;
using ShutterQuest.Views;
using ShutterQuestV6.MVVM.Models;
using ShutterQuestV6.Services;

namespace ShutterQuestV6.MVVM.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public string Email { get; set; }
        public string Password { get; set; }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToRegisterCommand { get; }

        public LoginViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;

            LoginCommand = new Command(async () => await LoginAsync());
            NavigateToRegisterCommand = new Command(() =>
            {
                Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage(_databaseService));
            });
        }

        private async Task LoginAsync()
        {
            try
            {
                var users = await _databaseService.GetAllAsync<User>();
                var user = users.FirstOrDefault(u => u.Email == Email && u.Password == Password);

                if (user != null)
                {
                    App.LoggedInUserId = user.Id;

                    await SecureStorage.Default.SetAsync("loggedInUserId", user.Id.ToString());

                    (Application.Current as App)?.SetMainPage();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
