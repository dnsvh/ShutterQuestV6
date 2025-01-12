using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutterQuestV6.MVVM.Models
{
    using SQLite;

    [Table("Users")]
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Points { get; set; }
        public bool IsAdmin { get; set; }

        [Ignore]
        public Membership Membership { get; set; } // Navigation, not stored!

        [Ignore]
        public List<UserAssignment> UserAssignments { get; set; } // Navigation, also not stored!
    }

}
