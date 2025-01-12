using System.Collections.Generic;
using SQLite;

namespace ShutterQuestV6.MVVM.Models
{
    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Username { get; set; }

        public string DisplayName { get; set; }

        [Unique]
        public string Email { get; set; }

        public string Password { get; set; }

        public int Points { get; set; } = 5; // Default to 5

        public bool IsAdmin { get; set; }

        [Ignore]
        public Membership Membership { get; set; }

        [Ignore]
        public List<UserAssignment> UserAssignments { get; set; }
    }
}
