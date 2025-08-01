using System.ComponentModel.DataAnnotations;

namespace PunchApiProject.Models
{
    public class Employee
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public ICollection<LoginRecord> Logins { get; set; } = new List<LoginRecord>();

        public ICollection<PunchRecord> PunchRecords { get; set; } = new List<PunchRecord>();
    }
}
