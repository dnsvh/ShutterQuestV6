using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using MvvmHelpers;
using ShutterQuestV6.MVVM.Models;
using ShutterQuestV6.Services;
using ShutterQuest.Views;

namespace ShutterQuestV6.MVVM.ViewModels
{
    public class ChallengeViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public ChallengeViewModel()
        {
            _databaseService = new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3"));
            LoadAssignmentsAsync();
            LoadUpcomingAssignmentAsync();
            FilterByThemeCommand = new Command<string>(FilterByTheme);
            EnterAssignmentCommand = new Command<int>(EnterAssignmentAsync);
            NavigateMembershipActionCommand = new Command(NavigateBasedOnMembership);
            NavigateToInspirationCommand = new Command(NavigateToInspiration);
        }

        // Properties for upcoming assignment
        private Assignment _upcomingAssignment;
        public Assignment UpcomingAssignment
        {
            get => _upcomingAssignment;
            set
            {
                _upcomingAssignment = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasUpcomingAssignment));
                OnPropertyChanged(nameof(HasNoUpcomingAssignment));
            }
        }

        public bool HasUpcomingAssignment => UpcomingAssignment != null;
        public bool HasNoUpcomingAssignment => !HasUpcomingAssignment;

        // All Assignments and Filtered Assignments
        public ObservableCollection<Assignment> AllAssignments { get; set; } = new ObservableCollection<Assignment>();
        public ObservableCollection<Assignment> FilteredAssignments { get; set; } = new ObservableCollection<Assignment>();

        public bool HasFilteredAssignments => FilteredAssignments.Any();
        public bool HasNoFilteredAssignments => !HasFilteredAssignments;

        // Commands
        public ICommand FilterByThemeCommand { get; }
        public ICommand EnterAssignmentCommand { get; }
        public ICommand NavigateMembershipActionCommand { get; }
        public ICommand NavigateToInspirationCommand { get; }

        private async void LoadAssignmentsAsync()
        {
            try
            {
                var assignments = await _databaseService.GetAllAsync<Assignment>();
                AllAssignments = new ObservableCollection<Assignment>(assignments);
                FilteredAssignments = new ObservableCollection<Assignment>(AllAssignments);

                OnPropertyChanged(nameof(AllAssignments));
                OnPropertyChanged(nameof(FilteredAssignments));
                OnPropertyChanged(nameof(HasFilteredAssignments));
                OnPropertyChanged(nameof(HasNoFilteredAssignments));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading assignments: {ex.Message}");
            }
        }

        private async void LoadUpcomingAssignmentAsync()
        {
            try
            {
                var userAssignments = await _databaseService.GetAllAsync<UserAssignment>();
                var currentUser = App.LoggedInUserId;

                // Get the user's active assignment
                var activeUserAssignment = userAssignments.FirstOrDefault(ua => ua.UserId == currentUser && !ua.IsCompleted);

                if (activeUserAssignment != null)
                {
                    // Fetch the assignment details
                    UpcomingAssignment = await _databaseService.GetByIdAsync<Assignment>(activeUserAssignment.AssignmentId);
                    System.Diagnostics.Debug.WriteLine($"Upcoming assignment loaded: {UpcomingAssignment?.Title}");
                }
                else
                {
                    UpcomingAssignment = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading upcoming assignment: {ex.Message}");
                UpcomingAssignment = null;
            }
            finally
            {
                OnPropertyChanged(nameof(UpcomingAssignment));
                OnPropertyChanged(nameof(HasUpcomingAssignment));
                OnPropertyChanged(nameof(HasNoUpcomingAssignment));
            }
        }

        private void FilterByTheme(string theme)
        {
            FilteredAssignments = string.IsNullOrEmpty(theme)
                ? new ObservableCollection<Assignment>(AllAssignments)
                : new ObservableCollection<Assignment>(AllAssignments.Where(a => a.Theme.Equals(theme, StringComparison.OrdinalIgnoreCase)));

            OnPropertyChanged(nameof(FilteredAssignments));
            OnPropertyChanged(nameof(HasFilteredAssignments));
            OnPropertyChanged(nameof(HasNoFilteredAssignments));
        }

        private async void EnterAssignmentAsync(int assignmentId)
        {
            try
            {
                var assignment = AllAssignments.FirstOrDefault(a => a.Id == assignmentId);
                if (assignment != null)
                {
                    var userAssignment = new UserAssignment
                    {
                        UserId = App.LoggedInUserId,
                        AssignmentId = assignment.Id,
                        StartDate = DateTime.Now,
                        IsCompleted = false,
                        PointsEarned = 0
                    };

                    await _databaseService.InsertAsync(userAssignment);

                    AllAssignments.Remove(assignment);
                    FilterByTheme(assignment.Theme); // Reapply filter after update
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error entering assignment: {ex.Message}");
            }
        }

        private async void NavigateBasedOnMembership()
        {
            try
            {
                var currentUser = await _databaseService.GetByIdAsync<User>(App.LoggedInUserId);
                if (currentUser == null) return;

                var memberships = await _databaseService.GetAllAsync<Membership>();
                var activeMembership = memberships.FirstOrDefault(m => m.UserId == currentUser.Id && m.IsActive);

                if (activeMembership != null)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new SettingsPage());
                }
                else
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new ShopPage());
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in NavigateBasedOnMembership: {ex.Message}");
            }
        }

        private async void NavigateToInspiration()
        {
            try
            {
                // Default category for inspiration
                string defaultCategory = "nature";
                await Application.Current.MainPage.Navigation.PushAsync(new InspirationPage(defaultCategory));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error navigating to InspirationPage: {ex.Message}");
            }
        }




    }
}
