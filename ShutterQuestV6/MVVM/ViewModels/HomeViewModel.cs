using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using MvvmHelpers;
using ShutterQuestV6.MVVM.Models;
using ShutterQuestV6.Services;

namespace ShutterQuestV6.MVVM.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        private string _welcomeMessage;
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Post> FeedPosts { get; set; } = new ObservableCollection<Post>();

        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand { get; }

        public HomeViewModel()
        {
            _databaseService = new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3"));
            RefreshCommand = new Command(async () => await RefreshFeedAsync());
            Task.Run(async () => await InitializeAsync());
        }

        private async Task InitializeAsync()
        {
            await LoadWelcomeMessageAsync();
            await LoadFeedPostsAsync();
        }

        private async Task LoadWelcomeMessageAsync()
        {
            try
            {
                var userId = App.LoggedInUserId;
                var user = await _databaseService.GetByIdAsync<User>(userId);

                if (user != null)
                {
                    WelcomeMessage = $"Welcome back, {user.DisplayName}!";
                }
                else
                {
                    WelcomeMessage = "Welcome back!";
                }
            }
            catch (Exception ex)
            {
                WelcomeMessage = "Welcome back!";
                System.Diagnostics.Debug.WriteLine($"Error loading welcome message: {ex.Message}");
            }
        }

        private async Task LoadFeedPostsAsync()
        {
            try
            {
                var posts = await _databaseService.GetAllAsync<Post>();
                FeedPosts.Clear();

                foreach (var post in posts)
                {
                    FeedPosts.Add(post);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading feed posts: {ex.Message}");
            }
        }

        private async Task RefreshFeedAsync()
        {
            IsRefreshing = true;

            // Refresh feed posts
            await LoadFeedPostsAsync();

            IsRefreshing = false;
        }
    }
}
