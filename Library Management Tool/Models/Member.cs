namespace Library_Management_Tool.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long Mobile { get; set; }
        public DateTime Dob { get; set; }
        public string Status { get; set; } = "Active";
        public DateTime AdmissionDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public ICollection<Issue> Issues{ get; set; }
    }
}
