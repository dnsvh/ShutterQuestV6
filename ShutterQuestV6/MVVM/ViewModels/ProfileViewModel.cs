using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ShutterQuestV6.Services;
using ShutterQuestV6;
using ShutterQuestV6.MVVM.Models;


namespace ShutterQuest.MVVM.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _databaseService;

        public ProfileViewModel()
        {
            _databaseService = new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3"));
            LoadUserDataAsync();
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string DisplayNameOrUsername => string.IsNullOrEmpty(DisplayName) ? Username : DisplayName;

        private async void LoadUserDataAsync()
        {
            try
            {
                var userId = App.LoggedInUserId;
                var user = await _databaseService.GetByIdAsync<User>(userId);

                if (user != null)
                {
                    Username = user.Username;
                    Email = user.Email;
                    DisplayName = user.DisplayName;
                }
            }
            catch (Exception ex)
            {
                // Handle error (e.g., log it)
                System.Diagnostics.Debug.WriteLine($"Error loading user data: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
