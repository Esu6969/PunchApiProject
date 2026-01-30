using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PunchApiProject.Models
{
    public class EmployeeContacts
    {
        [Key] 
        public int EmployeeContactId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public string ContactNumber { get; set; } = string.Empty;

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}
