using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutterQuestV6.MVVM.Models
{
    using SQLite;

    [Table("Assignments")]
    public class Assignment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }
        public string Theme { get; set; }
        public int CreatedById { get; set; } // Foreign key to User

        [Ignore]
        public List<UserAssignment> UserAssignments { get; set; }
    }


}
