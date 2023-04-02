using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_Tool.Models
{
    public class Issue
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int MemberId { get; set; }
        public DateTime IssueDate { get; set; } = new DateTime(new DateTime().Year, new DateTime().Month, new DateTime().Day);
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } = null;
        public string Status { get; set; } = "Issued";
    }
}
