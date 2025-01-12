using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutterQuestV6.MVVM.Models
{
    using SQLite;

    [Table("Posts")]
    public class Post
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int AuthorId { get; set; } // Foreign key to User
        public int AssignmentId { get; set; } // Foreign key to Assignment

        public string Description { get; set; }
        public DateTime PostedDate { get; set; }
        public string Image { get; set; }
        public decimal OverallRating { get; set; }

        [Ignore] // Will not be stored in database !
        public List<Review> ReviewList { get; set; }
    }

}
