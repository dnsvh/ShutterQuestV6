using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutterQuestV6.MVVM.Models
{
    using SQLite;

    [Table("Reviews")]
    public class Review
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ReviewerId { get; set; } // Foreign key to User
        public int PostId { get; set; } // Foreign key to Post

        public int Rating { get; set; }
        public string Content { get; set; }
        public DateTime ReviewedDate { get; set; }
    }

}
