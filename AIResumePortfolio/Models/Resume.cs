using System.ComponentModel.DataAnnotations;

namespace AIResumePortfolio.Models
{
    public class Resume
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ParsedJson { get; set; } // JSON from AI analysis
        public string PortfolioHtml { get; set; }

        public string MAC { get; set; }
        public string IPAddress { get; set; }

        public string ParsedTextContent { get; set; }
    }
}
