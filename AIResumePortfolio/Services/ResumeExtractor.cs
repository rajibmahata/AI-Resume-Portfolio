using System.Text.RegularExpressions;

namespace AIResumePortfolio.Services
{
    public class ResumeSections
    {
        public string PersonalInfo { get; set; }
        public string Summary { get; set; }
        public string Experience { get; set; }
        public string Education { get; set; }
        public string Skills { get; set; }
        public string Projects { get; set; }
        public string Recommendations { get; set; }
    }
    public class ResumeExtractor
    {
        public static ResumeSections ExtractSections(string resumeText)
        {
            var sections = new ResumeSections
            {
                PersonalInfo = ExtractSection(resumeText, "Personal Details"),
                Summary = ExtractSection(resumeText, "Experience Summary"),
                Experience = ExtractSection(resumeText, "Experience Summary", includeFollowingHeader: true),
                Education = ExtractSection(resumeText, "Education"),
                Skills = ExtractSection(resumeText, "Skills"),
                Projects = ExtractSection(resumeText, "Project"),
                Recommendations = ExtractSection(resumeText, "Recommendations")
            };

            return sections;
        }

        private static string ExtractSection(string text, string sectionName, bool includeFollowingHeader = false)
        {
            string pattern;
            if (includeFollowingHeader)
            {
                pattern = $@"(?<={sectionName}\s*\n)(.*?)(?=\n\s*\b(?:Personal Details|Experience Summary|Education|Skills|Project|Recommendations)\b|$)";
            }
            else
            {
                pattern = $@"(?<={sectionName}\s*\n)(.*?)(?=\n\s*\b(?:Personal Details|Experience Summary|Education|Skills|Project|Recommendations)\b)";
            }

            var match = Regex.Match(text, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            return match.Success ? match.Value.Trim() : string.Empty;
        }
    }
}
