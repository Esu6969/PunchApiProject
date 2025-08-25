namespace PunchApiProject.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public ICollection<PunchRecord> PunchRecords { get; set; } = new List<PunchRecord>();
        public ICollection<LoginRecord> Logins { get; set; } = new List<LoginRecord>();
    }
}