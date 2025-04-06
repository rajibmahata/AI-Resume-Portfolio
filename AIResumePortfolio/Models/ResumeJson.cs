namespace AIResumePortfolio.Models
{
    public class ResumeJson
    {
        public PersonalInfo personalInfo { get; set; }
        public string summary { get; set; }
        public List<Experiencejson> experience { get; set; }
        public List<Education> education { get; set; }
        public List<string> skills { get; set; }
        public List<Project> projects { get; set; }
        public List<string> recommendations { get; set; }
    }

    public class PersonalInfo
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
    }

    public class Experiencejson
    {
        public string company { get; set; }
        public string role { get; set; }
        public string duration { get; set; }
        public string description { get; set; }
    }

    public class Education
    {
        public string institution { get; set; }
        public string degree { get; set; }
        public string duration { get; set; }
    }

    public class Project
    {
        public string title { get; set; }
        public string description { get; set; }
    }
}
