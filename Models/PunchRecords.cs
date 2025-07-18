using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PunchApiProject.Models
{
    [Table("PunchRecords")] // <== This line ensures EF matches the actual PostgreSQL table name
    public class PunchRecord
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime PunchIn { get; set; }
        public DateTime? PunchOut { get; set; }
    }
}
