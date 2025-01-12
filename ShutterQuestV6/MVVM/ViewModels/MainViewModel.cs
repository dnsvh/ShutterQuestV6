﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ShutterQuestV6;
using ShutterQuestV6.MVVM.Models;
using ShutterQuestV6.Services;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly DatabaseService _databaseService;

    private string _pointsDisplay;
    public string PointsDisplay
    {
        get => _pointsDisplay;
        set
        {
            _pointsDisplay = value;
            OnPropertyChanged();
        }
    }

    public MainViewModel()
    {
        _databaseService = new DatabaseService(Path.Combine(FileSystem.AppDataDirectory, "shutterquest.db3"));
        Task.Run(async () => await LoadUserPointsAsync());
    }

    private async Task LoadUserPointsAsync()
    {
        try
        {
            var userId = App.LoggedInUserId;

            var user = await _databaseService.GetByIdAsync<User>(userId);

            if (user != null)
            {
                var points = user.Points;

                PointsDisplay = $"{points} C";

                System.Diagnostics.Debug.WriteLine($"User Points: {PointsDisplay}");
            }
            else
            {
                PointsDisplay = "0 C"; 
            }
        }
        catch (Exception ex)
        {
            PointsDisplay = "Error fetching points";
            System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
