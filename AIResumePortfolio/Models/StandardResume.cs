namespace AIResumePortfolio.Models
{
    public class StandardResume
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public ContactInfo Contact { get; set; }
        public List<string> Skills { get; set; }
        public List<Experience> Experience { get; set; }
       // public List<Education> Education { get; set; }
    }
}
