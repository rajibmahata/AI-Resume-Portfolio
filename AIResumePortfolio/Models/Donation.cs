using System.ComponentModel.DataAnnotations;

namespace AIResumePortfolio.Models
{
    public class Donation
    {
        [Key]
        public int Id { get; set; }
        public string DonorName { get; set; }
        public decimal Amount { get; set; }
        public DateTime DonationDate { get; set; }
    }
}
