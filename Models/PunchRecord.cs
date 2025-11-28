using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PunchApiProject.Models
{
    [Table("PunchRecords")]
    public class PunchRecord
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; } = null!;

        [Required]
        public DateTime ActionDateTime { get; set; }

        [Required]
        [StringLength(20)]
        public string ActionType { get; set; } = string.Empty; // "PunchIn" or "PunchOut"
    }
}