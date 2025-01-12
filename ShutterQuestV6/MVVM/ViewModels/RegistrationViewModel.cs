using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using MvvmHelpers;
using ShutterQuestV6.MVVM.Models;
using ShutterQuestV6.Services;

namespace ShutterQuestV6.MVVM.ViewModels
{
    public class RegistrationViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public ICommand RegisterCommand { get; }

        public RegistrationViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            RegisterCommand = new Command(async () => await RegisterAsync());
        }

        private async Task RegisterAsync()
        {
            try
            {
                var newUser = new User
                {
                    Username = Username,
                    Email = Email,
                    Password = Password,
                    Points = 5,
                    IsAdmin = false
                };

                await _databaseService.InsertAsync(newUser);

                await SecureStorage.SetAsync("loggedInUserId", newUser.Id.ToString());
                await Application.Current.MainPage.DisplayAlert("Success", "User registered successfully", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
