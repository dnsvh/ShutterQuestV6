using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutterQuestV6.MVVM.Models
{
    using SQLite;

    [Table("UserAssignments")]
    public class UserAssignment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; } // Foreign key to User
        public int AssignmentId { get; set; } // Foreign key to Assignment

        public DateTime StartDate { get; set; }
        public bool IsCompleted { get; set; }
        public int PointsEarned { get; set; }
    }


}
