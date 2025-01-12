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
        public ICommand RegisterCommand { get; }

        public LoginViewModel()
        {
            _databaseService = new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3"));
            LoginCommand = new Command(async () => await LoginAsync());
            RegisterCommand = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new RegistrationPage()));
        }

        private async Task LoginAsync()
        {
            var user = (await _databaseService.GetAllAsync<User>())
                .FirstOrDefault(u => u.Email == Email && u.Password == Password);

            if (user != null)
            {
                // Navigate to the home page on successful login
                await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
            }
        }
    }

}
