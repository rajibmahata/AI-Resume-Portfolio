using System.ComponentModel.DataAnnotations;

namespace AIResumePortfolio.Models
{
    public class Feedback
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
