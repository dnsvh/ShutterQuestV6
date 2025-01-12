using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShutterQuestV6.MVVM.Models
{
    using SQLite;

    [Table("Memberships")]
    public class Membership
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int UserId { get; set; } // Foreign key to User

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public string MembershipType { get; set; }
        public decimal MonthlyPrice { get; set; }
    }

}
