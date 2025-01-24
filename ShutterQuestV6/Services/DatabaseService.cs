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
            await AddDefaultPostsAsync();


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
                    Description = "Fotografeer een mooi landschap in een bos!",
                    OpenTime = DateTime.Now,
                    CloseTime = DateTime.Now.AddDays(7),
                    Theme = "Natuur",
                    CreatedById = 1
                },
                new Assignment
                {
                    Title = "Culinary Creations",
                    Description = "Maak een foto van een zelfgemaakt gerecht.",
                    OpenTime = DateTime.Now,
                    CloseTime = DateTime.Now.AddDays(7),
                    Theme = "Culinair",
                    CreatedById = 1
                },
                new Assignment
                {
                    Title = "Portrait Perfection",
                    Description = "Maak een portret van een persoon die een verhaal vertelt.",
                    OpenTime = DateTime.Now,
                    CloseTime = DateTime.Now.AddDays(7),
                    Theme = "Personen",
                    CreatedById = 1
                },
                new Assignment
                {
                    Title = "Miscellaneous Masterpiece",
                    Description = "Eigen keuze, maak een mooie foto van wat dan ook!",
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

                var user = await _database.Table<User>().FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    System.Diagnostics.Debug.WriteLine($"User with email {email} not found.");
                    return;
                }

                var assignment = await _database.Table<Assignment>().FirstOrDefaultAsync(a => a.Title == assignmentTitle);
                if (assignment == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Assignment with title '{assignmentTitle}' not found.");
                    return;
                }
                var existingUserAssignment = await _database.Table<UserAssignment>()
    .FirstOrDefaultAsync(ua => ua.UserId == user.Id && ua.AssignmentId == assignment.Id);
                if (existingUserAssignment != null)
                {
                    System.Diagnostics.Debug.WriteLine($"User {email} already has the assignment '{assignmentTitle}'.");
                    return;
                }

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

        private async Task AddDefaultPostsAsync()
        {
            var existingPosts = await _database.Table<Post>().ToListAsync();
            if (existingPosts.Count > 0) return;

            var posts = new List<Post>
            {
                new Post
                {
                    AuthorId = 1,
                    AssignmentId = 1,
                    Description = "Mooie foto van de zonopkomst in de bergen.",
                    PostedDate = DateTime.Now.AddDays(-15),
                    Image = "sunrise_mountains.png",
                    OverallRating = 4.5m
                },
                new Post
                {
                    AuthorId = 2,
                    AssignmentId = 2,
                    Description = "Lekkere spaghetti!",
                    PostedDate = DateTime.Now.AddDays(-16),
                    Image = "spaghetti_plate.png",
                    OverallRating = 4.8m
                },
                new Post
                {
                    AuthorId = 3,
                    AssignmentId = 3,
                    Description = "Serieus portret van een oudere man.",
                    PostedDate = DateTime.Now.AddDays(-17),
                    Image = "elderly_portrait.png",
                    OverallRating = 4.9m
                },
                new Post
                {
                    AuthorId = 4,
                    AssignmentId = 4,
                    Description = "Een foto van de standsverlichting na zonsondergang!",
                    PostedDate = DateTime.Now.AddDays(-18),
                    Image = "city_lights.png",
                    OverallRating = 4.7m
                },
                new Post
                {
                    AuthorId = 5,
                    AssignmentId = 1,
                    Description = "Het bospad in de namiddag!.",
                    PostedDate = DateTime.Now.AddDays(-19),
                    Image = "forest_trail.png",
                    OverallRating = 4.6m
                }
            };

            await _database.InsertAllAsync(posts);
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