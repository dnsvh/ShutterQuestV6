using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using ShutterQuestV6.MVVM.Models;

namespace ShutterQuestV6.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        public async Task InitializeDatabaseAsync()
        {
            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<Post>();
            await _database.CreateTableAsync<Assignment>();
            await _database.CreateTableAsync<Review>();
            await _database.CreateTableAsync<UserAssignment>();
            await _database.CreateTableAsync<Membership>();

            await AddDefaultAssignmentsAsync();
        }

        private async Task AddDefaultAssignmentsAsync()
        {
            var existingAssignments = await _database.Table<Assignment>().ToListAsync();
            if (existingAssignments.Count > 0) return;

            var assignments = new List<Assignment>
            {
                new Assignment
                {
                    Title = "Explore Nature",
                    Description = "Capture the beauty of a natural landscape.",
                    OpenTime = DateTime.Now,
                    CloseTime = DateTime.Now.AddDays(7),
                    Theme = "Natuur",
                    CreatedById = 1 
                },
                new Assignment
                {
                    Title = "Culinary Creations",
                    Description = "Show off a delicious dish!",
                    OpenTime = DateTime.Now,
                    CloseTime = DateTime.Now.AddDays(7),
                    Theme = "Culinair",
                    CreatedById = 1
                },
                new Assignment
                {
                    Title = "Portrait Perfection",
                    Description = "Take a portrait that tells a story.",
                    OpenTime = DateTime.Now,
                    CloseTime = DateTime.Now.AddDays(7),
                    Theme = "Personen",
                    CreatedById = 1 
                },
                new Assignment
                {
                    Title = "Miscellaneous Masterpiece",
                    Description = "Submit any photo you love.",
                    OpenTime = DateTime.Now,
                    CloseTime = DateTime.Now.AddDays(7),
                    Theme = "Overige",
                    CreatedById = 1 
                }
            };

            await _database.InsertAllAsync(assignments);
        }

        public async Task AssignDefaultAssignmentToUser(string email, string assignmentTitle)
        {
            try
            {
                // Fetch the user by email
                var user = await _database.Table<User>().FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    System.Diagnostics.Debug.WriteLine($"User with email {email} not found.");
                    return;
                }

                // Fetch the assignment by title
                var assignment = await _database.Table<Assignment>().FirstOrDefaultAsync(a => a.Title == assignmentTitle);
                if (assignment == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Assignment with title '{assignmentTitle}' not found.");
                    return;
                }

                // Check if the user already has this assignment
                var existingUserAssignment = await _database.Table<UserAssignment>()
                    .FirstOrDefaultAsync(ua => ua.UserId == user.Id && ua.AssignmentId == assignment.Id);

                if (existingUserAssignment != null)
                {
                    System.Diagnostics.Debug.WriteLine($"User {email} already has the assignment '{assignmentTitle}'.");
                    return;
                }

                // Create a new UserAssignment entry
                var userAssignment = new UserAssignment
                {
                    UserId = user.Id,
                    AssignmentId = assignment.Id,
                    StartDate = DateTime.Now,
                    IsCompleted = false,
                    PointsEarned = 0
                };

                await _database.InsertAsync(userAssignment);

                System.Diagnostics.Debug.WriteLine($"Assigned assignment '{assignmentTitle}' to user '{email}'.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error assigning assignment to user: {ex.Message}");
            }
        }


        public async Task<int> InsertAsync<T>(T item) where T : new()
        {
            return await _database.InsertAsync(item);
        }

        public async Task<int> UpdateAsync<T>(T item) where T : new()
        {
            return await _database.UpdateAsync(item);
        }

        public async Task<List<T>> GetAllAsync<T>() where T : new()
        {
            return await _database.Table<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id) where T : new()
        {
            return await _database.FindAsync<T>(id);
        }

        public async Task<int> DeleteAsync<T>(T item) where T : new()
        {
            return await _database.DeleteAsync(item);
        }
    }
}
