using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management_Tool.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public int TotalCopies { get; set; }
        public int Availability { get; set; }
        public int PublisherId { get; set; }
        public DateTime AddedDate { get; set; }
        public string ImageURL { get; set; }
    }
}
