using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using MvvmHelpers;
using ShutterQuest.Views;
using ShutterQuestV6.MVVM.Models;

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
                // Navigate to RegistrationPage
                Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage(_databaseService));
            });
        }

        private async Task LoginAsync()
        {
            var users = await _databaseService.GetAllAsync<User>();
            var user = users.FirstOrDefault(u => u.Email == Email && u.Password == Password);

            if (user != null)
            {
                // Set the logged-in user ID
                App.LoggedInUserId = user.Id;

                // Set MainPage with bottom tabs
                (Application.Current as App)?.SetMainPage();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
            }
        }
    }




}
