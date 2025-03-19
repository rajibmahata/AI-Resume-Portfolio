using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AIResumePortfolio.AIAgents
{
    // AI Service: AiService.cs
    public class AiService
    {
        private readonly HttpClient _httpClient;
        private const string AiApiUrl = "https://api.openai.com/v1/completions";
        private const string ApiKey = "YOUR_OPENAI_API_KEY";

        public AiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Agent 1: Extract and format resume into market-standard JSON
        public async Task<ResumeJsonModel> GenerateStructuredResume(string resumeText)
        {
            var requestBody = new
            {
                model = "gpt-4-turbo",
                prompt = $"Format this resume text into a market-standard structured JSON: {resumeText}",
                max_tokens = 1000
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var response = await _httpClient.PostAsync(AiApiUrl, requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var structuredResume = JsonSerializer.Deserialize<ResumeJsonModel>(responseContent);

            return structuredResume;
        }

        // Agent 2: Generate modern HTML resume from JSON content
        public async Task<string> GenerateResumeHtml(ResumeJsonModel structuredResume)
        {
            var requestBody = new
            {
                model = "gpt-4-turbo",
                prompt = $"Convert this structured resume JSON into a professional, modern HTML page: {JsonSerializer.Serialize(structuredResume)}",
                max_tokens = 1500
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);

            var response = await _httpClient.PostAsync(AiApiUrl, requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }

    // Resume JSON Model
    public class ResumeJsonModel
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        public List<string> Skills { get; set; }
        public List<ExperienceModel> Experience { get; set; }
        public List<EducationModel> Education { get; set; }
        public List<ProjectModel> Projects { get; set; }
    }

    public class ExperienceModel
    {
        public string Company { get; set; }
        public string Role { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
    }

    public class EducationModel
    {
        public string Institution { get; set; }
        public string Degree { get; set; }
        public string Year { get; set; }
    }

    public class ProjectModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Technologies { get; set; }
    }

    // Resume Table Model
   

}
